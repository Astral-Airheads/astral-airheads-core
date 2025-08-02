using System;
using System.IO;
using System.IO.Pipes;
#if NET8_0_OR_GREATER
using System.Runtime.Versioning;
#endif
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AstralAirheads.Validation;

namespace AstralAirheads.IPC;

/// <summary>
/// Utility class providing helper methods for safe IPC communication.
/// Note: This class uses Windows-specific named pipes and should only be used on Windows platforms.
/// </summary>
#if NET8_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
public static class IPCUtilities
{
    /// <summary>
    /// Default timeout for IPC operations in milliseconds.
    /// </summary>
    public const int DefaultTimeoutMs = 30000; // 30 seconds

    /// <summary>
    /// Default JSON serialization options for IPC messages.
    /// </summary>
    public static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Safely sends a JSON message to a pipe stream with timeout and error handling.
    /// </summary>
    /// <typeparam name="T">The type of message to send.</typeparam>
    /// <param name="stream">The pipe stream to send to.</param>
    /// <param name="message">The message to send.</param>
    /// <param name="timeoutMs">Timeout in milliseconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="jsonOptions">JSON serialization options.</param>
    public static async Task SendJsonMessageAsync<T>(
        PipeStream stream,
        T message,
        int timeoutMs = DefaultTimeoutMs,
        CancellationToken cancellationToken = default,
        JsonSerializerOptions? jsonOptions = null)
    {
        Requires.NotNull(stream);
        Requires.NotNull(message);

        if (!stream.IsConnected)
            throw new InvalidOperationException("Stream is not connected.");

        using var timeoutCts = new CancellationTokenSource(timeoutMs);
        using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
            timeoutCts.Token, cancellationToken);

        try
        {
            var options = jsonOptions ?? DefaultJsonOptions;
            var json = JsonSerializer.Serialize(message, typeof(T), options);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json + Environment.NewLine);
            
            await stream.WriteAsync(buffer, 0, buffer.Length, combinedCts.Token);
            await stream.FlushAsync(combinedCts.Token);
        }
        catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
        {
            throw new TimeoutException($"Send operation timed out after {timeoutMs}ms");
        }
        catch (ObjectDisposedException)
        {
            throw new InvalidOperationException("Stream has been disposed.");
        }
    }

    /// <summary>
    /// Safely reads a JSON message from a pipe stream with timeout and error handling.
    /// </summary>
    /// <typeparam name="T">The expected type of message.</typeparam>
    /// <param name="stream">The pipe stream to read from.</param>
    /// <param name="timeoutMs">Timeout in milliseconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="jsonOptions">JSON serialization options.</param>
    /// <returns>The deserialized message.</returns>
    public static async Task<T> ReadJsonMessageAsync<T>(
        PipeStream stream,
        int timeoutMs = DefaultTimeoutMs,
        CancellationToken cancellationToken = default,
        JsonSerializerOptions? jsonOptions = null)
    {
        Requires.NotNull(stream);

        if (!stream.IsConnected)
            throw new InvalidOperationException("Stream is not connected.");

        using var timeoutCts = new CancellationTokenSource(timeoutMs);
        using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
            timeoutCts.Token, cancellationToken);

        try
        {
            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8, false, 1024, true);
            var json = await reader.ReadLineAsync();
            
            if (string.IsNullOrEmpty(json))
                throw new InvalidOperationException("Received empty or null message from stream.");

            var options = jsonOptions ?? DefaultJsonOptions;
            var message = JsonSerializer.Deserialize<T>(json, options);
            
            if (message == null)
                throw new InvalidOperationException("Failed to deserialize message from JSON.");
                
            return message;
        }
        catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
        {
            throw new TimeoutException($"Read operation timed out after {timeoutMs}ms");
        }
        catch (ObjectDisposedException)
        {
            throw new InvalidOperationException("Stream has been disposed.");
        }
    }

    /// <summary>
    /// Performs a request-response pattern with timeout and error handling.
    /// </summary>
    /// <typeparam name="TRequest">The request message type.</typeparam>
    /// <typeparam name="TResponse">The response message type.</typeparam>
    /// <param name="stream">The pipe stream for communication.</param>
    /// <param name="request">The request message.</param>
    /// <param name="timeoutMs">Timeout for the entire operation in milliseconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="jsonOptions">JSON serialization options.</param>
    /// <returns>The response message.</returns>
    public static async Task<TResponse> RequestResponseAsync<TRequest, TResponse>(
        PipeStream stream,
        TRequest request,
        int timeoutMs = DefaultTimeoutMs,
        CancellationToken cancellationToken = default,
        JsonSerializerOptions? jsonOptions = null)
    {
        Requires.NotNull(stream);
        Requires.NotNull(request);

        if (!stream.IsConnected)
            throw new InvalidOperationException("Stream is not connected.");

        using var timeoutCts = new CancellationTokenSource(timeoutMs);
        using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
            timeoutCts.Token, cancellationToken);

        try
        {
            // Send the request
            await SendJsonMessageAsync(stream, request, timeoutMs, combinedCts.Token, jsonOptions);
            
            // Read the response
            return await ReadJsonMessageAsync<TResponse>(stream, timeoutMs, combinedCts.Token, jsonOptions);
        }
        catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
        {
            throw new TimeoutException($"Request-response operation timed out after {timeoutMs}ms");
        }
    }

    /// <summary>
    /// Safely connects to a named pipe with timeout and error handling.
    /// </summary>
    /// <param name="pipeName">The name of the pipe to connect to.</param>
    /// <param name="timeoutMs">Connection timeout in milliseconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The connected client stream.</returns>
    public static async Task<NamedPipeClientStream> ConnectToPipeAsync(
        string pipeName,
        int timeoutMs = DefaultTimeoutMs,
        CancellationToken cancellationToken = default)
    {
        Requires.NotNullOrEmpty(pipeName);

        var clientStream = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
        
        using var timeoutCts = new CancellationTokenSource(timeoutMs);
        using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
            timeoutCts.Token, cancellationToken);

        try
        {
            await clientStream.ConnectAsync(combinedCts.Token);
            return clientStream;
        }
        catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
        {
            clientStream.Dispose();
            throw new TimeoutException($"Failed to connect to pipe '{pipeName}' within {timeoutMs}ms");
        }
        catch (Exception)
        {
            clientStream.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Creates a named pipe server with proper configuration for IPC.
    /// Note: This method uses Windows-specific named pipes.
    /// </summary>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="maxInstances">Maximum number of pipe instances.</param>
    /// <returns>The configured server stream.</returns>
    public static NamedPipeServerStream CreatePipeServer(string pipeName, int maxInstances = 1)
    {
        Requires.NotNullOrEmpty(pipeName);
        
        return new NamedPipeServerStream(
            pipeName, 
            PipeDirection.InOut, 
            maxInstances, 
            PipeTransmissionMode.Message, 
            PipeOptions.Asynchronous);
    }

    /// <summary>
    /// Validates that a pipe stream is in a valid state for communication.
    /// </summary>
    /// <param name="stream">The stream to validate.</param>
    /// <param name="name">The name of the stream parameter for error messages.</param>
    public static void ValidateStream(PipeStream stream, string name = "stream")
    {
        if (stream == null)
            throw new ArgumentNullException(name);
            
        // Note: PipeStream doesn't have IsDisposed property in older .NET versions
        // We'll check IsConnected instead which is more reliable for our use case
            
        if (!stream.IsConnected)
            throw new InvalidOperationException($"{name} is not connected.");
    }
} 