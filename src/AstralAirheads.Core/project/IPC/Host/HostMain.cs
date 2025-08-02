using System;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using AstralAirheads.Logging;
using AstralAirheads.Util.CLI;
using AstralAirheads.Validation;

namespace AstralAirheads.IPC.Host;

/// <summary>
/// Contains the entry point of the pipe IPC client process to get started on
/// starting/initializing pipe servers.
/// </summary>
public sealed class HostMain
{
    /// <summary>
    /// Invoked every time after we have successfully connected to a pipe server since
    /// this acts as the program's main loop.
    /// </summary>
    public static event EventHandler<HostLoopEventArgs>? HostLoop;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Default timeout for pipe operations in milliseconds.
    /// </summary>
    private const int DefaultTimeoutMs = 30000; // 30 seconds

    /// <summary>
    /// Sends a JSON message to the server with timeout and cancellation support.
    /// </summary>
    /// <param name="stream">The value of the client stream.</param>
    /// <param name="message">The value of the message.</param>
    /// <param name="timeoutMs">Timeout in milliseconds. Default is 30 seconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public static async Task SendMessageAsync(
        NamedPipeClientStream stream, 
        PipeMessage message, 
        int timeoutMs = DefaultTimeoutMs,
        CancellationToken cancellationToken = default)
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
            // Use a single StreamWriter instance and don't dispose it immediately
            // to avoid closing the underlying stream
            var json = JsonSerializer.Serialize(message, message.GetType(), JsonOptions);
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
    /// Reads a JSON message from the server with timeout and cancellation support.
    /// </summary>
    /// <param name="stream">The client stream to read from.</param>
    /// <param name="timeoutMs">Timeout in milliseconds. Default is 30 seconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The deserialized message.</returns>
    public static async Task<PipeMessage> ReadMessageAsync(
        NamedPipeClientStream stream,
        int timeoutMs = DefaultTimeoutMs,
        CancellationToken cancellationToken = default)
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
                throw new InvalidOperationException("Received empty or null message from server.");

            var message = JsonSerializer.Deserialize<PipeMessage>(json, JsonOptions);
            Requires.NotNull(message);
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
    /// Reads a JSON message from a string (legacy method for backward compatibility).
    /// </summary>
    /// <param name="json">The value of the JSON string.</param>
    public static PipeMessage ReadMessage(string json)
    {
        Requires.NotNull(json);

        var message = JsonSerializer.Deserialize<PipeMessage>(json, JsonOptions);
        Requires.NotNull(message);

        return message;
    }

    /// <summary>
    /// This is the actual entry point of the pipe client host.
    /// This is awaitable so you either use <c>.GetAwaiter().GetResult()</c> or actually try
    /// to await this method.
    /// </summary>
    /// <param name="args">The value of the command-line arguments.</param>
    /// <param name="cancellationToken">Cancellation token for graceful shutdown.</param>
    /// <returns>The exit-code of the entry point.</returns>
    public static async Task<int> StartAsync(string[] args, CancellationToken cancellationToken = default)
    {
        var exitCode = 0;
        using var logger = new Logger(LoggerSettings.Default);
        var cliParser = new CommandLineParser('/', args);

        try
        {
            var pipeName = cliParser.GetParmValue("pipeName");
            if (string.IsNullOrEmpty(pipeName))
                throw new NullReferenceException("A pipe name is required to connect to the server! " +
                    "Specify an pipe name with the \"/pipeName\" parameter.");

            using var clientStream = 
                new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);

            using var connectTimeoutCts = new CancellationTokenSource(DefaultTimeoutMs);
            using var connectCombinedCts = CancellationTokenSource.CreateLinkedTokenSource(
                connectTimeoutCts.Token, cancellationToken);

            try
            {
                await clientStream.ConnectAsync(connectCombinedCts.Token);
            }
            catch (OperationCanceledException) when (connectTimeoutCts.Token.IsCancellationRequested)
            {
                throw new TimeoutException($"Failed to connect to pipe '{pipeName}' within {DefaultTimeoutMs}ms");
            }

            logger.LogInfo("Successfully connected to pipe: {0}", pipeName);

            while (clientStream.IsConnected && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    HostLoop?.Invoke(null, new HostLoopEventArgs(pipeName!, clientStream));
                    
                    await Task.Delay(10, cancellationToken);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    logger.LogInfo("Host loop cancelled by user request.");
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogErr("Error in host loop: {0}", ex.Message);
                    if (ex is ObjectDisposedException || ex is InvalidOperationException)
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogFatal("A fatal error occurred in the client host: \"{0}\"", ex.Message);
            exitCode = ex.HResult;
        }

        return exitCode;
    }
}
