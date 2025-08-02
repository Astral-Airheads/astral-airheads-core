// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
#if NET8_0_OR_GREATER
using System.Runtime.Versioning;
#endif
using System.Threading;
using System.Threading.Tasks;

namespace AstralAirheads.IPC;

/// <summary>
/// Represents a pipe instance.
/// Note: This class uses Windows-specific named pipes and should only be used on Windows platforms.
/// </summary>
#if NET8_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
public class PipeInstance(string exePath, string pipeName, int maxNumInstances = 1) : IDisposable
{
    private bool _disposed;
    private NamedPipeServerStream _serverStream = 
        new(pipeName, PipeDirection.InOut, maxNumInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
    
    private Process? _clientProcess;

    /// <summary>
    /// Default timeout for pipe operations in milliseconds.
    /// </summary>
    private const int DefaultTimeoutMs = 30000;

    /// <summary>
    /// Gets the <seealso cref="NamedPipeServerStream"/> of this instance.
    /// </summary>
    public NamedPipeServerStream ServerStream => _serverStream;

    private bool _hasInitialized;

    /// <summary>
    /// Checks if the instance has been initialized.
    /// </summary>
    public bool HasInitialized => _hasInitialized;

    /// <summary>
    /// Gets the client process if it was started by this instance.
    /// </summary>
    public Process? ClientProcess => _clientProcess;

    /// <summary>
    /// Starts this instance where it also starts a client process to be communicating with the
    /// server (current assembly).
    /// </summary>
    /// <param name="timeoutMs">Timeout for connection in milliseconds. Default is 30 seconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see langword="true"/> on successful initialization.</returns>
    public async Task<bool> StartAsync(int timeoutMs = DefaultTimeoutMs, CancellationToken cancellationToken = default)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(PipeInstance));

        if (!exePath.EndsWith(".exe"))
            return false;

        if (!File.Exists(exePath))
            return false;

        try
        {
            _clientProcess = new Process()
            {
                StartInfo = new ProcessStartInfo(exePath)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                },
                EnableRaisingEvents = true
            };

            if (!_clientProcess.Start())
                return false;

            using var timeoutCts = new CancellationTokenSource(timeoutMs);
            using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
                timeoutCts.Token, cancellationToken);

            try
            {
                await _serverStream.WaitForConnectionAsync(combinedCts.Token);
            }
            catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
            {
                if (_clientProcess != null && !_clientProcess.HasExited)
                {
                    try
                    {
                        _clientProcess.Kill();
                    }
                    catch (Exception)
                    {
                    }
                }
                throw new TimeoutException($"Failed to connect to client process within {timeoutMs}ms");
            }

            _hasInitialized = true;
            return true;
        }
        catch (Exception)
        {
            if (_clientProcess != null && !_clientProcess.HasExited)
            {
                try
                {
                    _clientProcess.Kill();
                }
                catch (Exception)
                {
                }
            }
            throw;
        }
    }

    /// <summary>
    /// Sends a message to the connected client with timeout support.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="timeoutMs">Timeout in milliseconds. Default is 30 seconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task SendMessageAsync(string message, int timeoutMs = DefaultTimeoutMs, CancellationToken cancellationToken = default)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(PipeInstance));

        if (!_hasInitialized || !_serverStream.IsConnected)
            throw new InvalidOperationException("Pipe is not connected.");

        using var timeoutCts = new CancellationTokenSource(timeoutMs);
        using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
            timeoutCts.Token, cancellationToken);

        try
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(message + Environment.NewLine);
            await _serverStream.WriteAsync(buffer, 0, buffer.Length, combinedCts.Token);
            await _serverStream.FlushAsync(combinedCts.Token);
        }
        catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
        {
            throw new TimeoutException($"Send operation timed out after {timeoutMs}ms");
        }
    }

    /// <summary>
    /// Reads a message from the connected client with timeout support.
    /// </summary>
    /// <param name="timeoutMs">Timeout in milliseconds. Default is 30 seconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The received message.</returns>
    public async Task<string> ReadMessageAsync(int timeoutMs = DefaultTimeoutMs, CancellationToken cancellationToken = default)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(PipeInstance));

        if (!_hasInitialized || !_serverStream.IsConnected)
            throw new InvalidOperationException("Pipe is not connected.");

        using var timeoutCts = new CancellationTokenSource(timeoutMs);
        using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
            timeoutCts.Token, cancellationToken);

        try
        {
            using var reader = new StreamReader(_serverStream, System.Text.Encoding.UTF8, false, 1024, true);
            var message = await reader.ReadLineAsync();
            
            if (string.IsNullOrEmpty(message))
                throw new InvalidOperationException("Received empty or null message from client.");
                
            return message;
        }
        catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
        {
            throw new TimeoutException($"Read operation timed out after {timeoutMs}ms");
        }
    }

    /// <summary>
    /// Disposes the unmanaged resources used by the pipe instance and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _serverStream?.Dispose();
                
                if (_clientProcess != null && !_clientProcess.HasExited)
                {
                    try
                    {
                        _clientProcess.Kill();
                        _clientProcess.WaitForExit(5000);
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        _clientProcess?.Dispose();
                    }
                }
            }

            _hasInitialized = false;
            _disposed = true;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
