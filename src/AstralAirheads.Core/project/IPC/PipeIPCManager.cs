// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#if NET8_0_OR_GREATER
using System.Runtime.Versioning;
#endif
using AstralAirheads.Validation;

// @imstilljustmatt:
// i needed to disable this since .NET Framework v4.7.2 doesn't have
// ObjectDisposedException.ThrowIf.
#pragma warning disable CA1513 // Use ObjectDisposedException throw helper.

namespace AstralAirheads.IPC;

/// <summary>
/// Manages pipe instances.
/// </summary>
#if NET8_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
public class PipeIPCManager : IDisposable
{
    private readonly List<PipeInstance> _pipes = [];
    private bool _disposed;

    /// <summary>
    /// Default timeout for pipe operations in milliseconds.
    /// </summary>
    private const int DefaultTimeoutMs = 30000; // 30 seconds

    /// <summary>
    /// Adds an <seealso cref="PipeInstance"/> to the manager.
    /// </summary>
    /// <param name="instance">The value of the new instance to be added.</param>
    /// <param name="startOnAdded">Specifies when to initialize the instance if it hasn't already.</param>
    /// <param name="timeoutMs">Timeout for starting the instance in milliseconds. Default is 30 seconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see langword="true"/> if the instance was successfully added and started.</returns>
    public async Task<bool> AddAsync(
        PipeInstance instance, 
        bool startOnAdded = false, 
        int timeoutMs = DefaultTimeoutMs,
        CancellationToken cancellationToken = default)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(PipeIPCManager));

        if (_pipes.Contains(instance))
            return false;

        Requires.NotNull(instance);
        _pipes.Add(instance);

        if (startOnAdded && !instance.HasInitialized)
        {
            try
            {
                return await instance.StartAsync(timeoutMs, cancellationToken);
            }
            catch (Exception)
            {
                _pipes.Remove(instance);
                throw;
            }
        }
        
        return true;
    }

    /// <summary>
    /// Removes a pipe instance from the manager and disposes it.
    /// </summary>
    /// <param name="instance">The instance to remove.</param>
    /// <returns><see langword="true"/> if the instance was found and removed.</returns>
    public bool Remove(PipeInstance instance)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(PipeIPCManager));

        if (instance == null)
            return false;

        var removed = _pipes.Remove(instance);
        if (removed)
        {
            instance.Dispose();
        }
        
        return removed;
    }

    /// <summary>
    /// Gets all managed pipe instances.
    /// </summary>
    /// <returns>A read-only collection of pipe instances.</returns>
    public IReadOnlyList<PipeInstance> GetInstances()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(PipeIPCManager));

        return _pipes.AsReadOnly();
    }

    /// <summary>
    /// Sends a message to all connected pipe instances.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="timeoutMs">Timeout for each send operation in milliseconds. Default is 30 seconds.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of instances that successfully received the message.</returns>
    public async Task<int> BroadcastMessageAsync(
        string message, 
        int timeoutMs = DefaultTimeoutMs,
        CancellationToken cancellationToken = default)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(PipeIPCManager));

        var successCount = 0;
        var tasks = new List<Task<bool>>();

        foreach (var pipe in _pipes)
        {
            if (pipe.HasInitialized)
            {
                tasks.Add(SendMessageToInstanceAsync(pipe, message, timeoutMs, cancellationToken));
            }
        }

        try
        {
            var results = await Task.WhenAll(tasks);
            successCount = results.Length;
        }
        catch (Exception)
        {
            // Count successful sends before the exception
            foreach (var task in tasks)
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    successCount++;
            }
        }

        return successCount;
    }

    private async Task<bool> SendMessageToInstanceAsync(
        PipeInstance pipe, 
        string message, 
        int timeoutMs,
        CancellationToken cancellationToken)
    {
        try
        {
            await pipe.SendMessageAsync(message, timeoutMs, cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the pipe instances or this manager and optionally releases the managed resources.
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            for (int i = 0; i < _pipes.Count; i++)
            {
                var pipe = _pipes[i];
                if (pipe != null && pipe.HasInitialized)
                {
                    try
                    {
                        pipe.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            _pipes.Clear();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
