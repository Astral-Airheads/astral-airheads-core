// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.IO;

namespace AstralAirheads.Logging;

/// <summary>
/// Defines a logging interface that provides methods for writing log messages at different severity levels.
/// </summary>
/// <remarks>
/// This interface provides a contract for logging implementations that support multiple message levels
/// and various formatting options. It extends <see cref="IDisposable"/> to allow proper resource cleanup.
/// </remarks>
public interface ILogger : IDisposable
{
    /// <summary>
    /// Gets the minimum logging level.
    /// </summary>
    /// <value>The actual message level.</value>
    MessageLevel MinimumLevel { get; }

    /// <summary>
    /// Gets the text writer used for standard output messages.
    /// </summary>
    /// <value>The text writer for standard output.</value>
    TextWriter OutputWriter { get; }

    /// <summary>
    /// Gets the text writer used for error output messages.
    /// </summary>
    /// <value>The text writer for error output.</value>
    TextWriter ErrWriter { get; }

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
    void Log(string message);

    /// <summary>
    /// Logs an informational message with a single argument.
    /// </summary>
    /// <param name="format">The format string.</param>
    /// <param name="arg0">The first argument to format.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    void Log(string format, object? arg0);

    /// <summary>
    /// Logs an informational message with two arguments.
    /// </summary>
    /// <param name="format">The format string.</param>
    /// <param name="arg0">The first argument to format.</param>
    /// <param name="arg1">The second argument to format.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    void Log(string format, object? arg0, object? arg1);

    /// <summary>
    /// Logs an informational message with three arguments.
    /// </summary>
    /// <param name="format">The format string.</param>
    /// <param name="arg0">The first argument to format.</param>
    /// <param name="arg1">The second argument to format.</param>
    /// <param name="arg2">The third argument to format.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    void Log(string format, object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Logs an informational message with multiple arguments.
    /// </summary>
    /// <param name="format">The format string.</param>
    /// <param name="args">The arguments to format.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    void Log(string format, params object?[] args);

    /// <summary>
    /// Logs a message at the specified level.
    /// </summary>
    /// <param name="level">The message level.</param>
    /// <param name="message">The message to log.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
    void Log(MessageLevel level, string message);

    /// <summary>
    /// Logs a message at the specified level with a single argument.
    /// </summary>
    /// <param name="level">The message level.</param>
    /// <param name="format">The format string.</param>
    /// <param name="arg0">The first argument to format.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    void Log(MessageLevel level, string format, object? arg0);

    /// <summary>
    /// Logs a message at the specified level with two arguments.
    /// </summary>
    /// <param name="level">The message level.</param>
    /// <param name="format">The format string.</param>
    /// <param name="arg0">The first argument to format.</param>
    /// <param name="arg1">The second argument to format.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    void Log(MessageLevel level, string format, object? arg0, object? arg1);

    /// <summary>
    /// Logs a message at the specified level with three arguments.
    /// </summary>
    /// <param name="level">The message level.</param>
    /// <param name="format">The format string.</param>
    /// <param name="arg0">The first argument to format.</param>
    /// <param name="arg1">The second argument to format.</param>
    /// <param name="arg2">The third argument to format.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    void Log(MessageLevel level, string format, object? arg0, object? arg1, object? arg2);

    /// <summary>
    /// Logs a message at the specified level with multiple arguments.
    /// </summary>
    /// <param name="level">The message level.</param>
    /// <param name="format">The format string.</param>
    /// <param name="args">The arguments to format.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    void Log(MessageLevel level, string format, params object?[] args);
}
