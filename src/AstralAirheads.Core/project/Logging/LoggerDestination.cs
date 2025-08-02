using System;

namespace AstralAirheads.Logging;

/// <summary>
/// Specifies the output of the logger.
/// </summary>
[Flags]
public enum LoggerDestination
{
    /// <summary>
    /// Outputs messages into the debug output.
    /// </summary>
    Debug = 0,

    /// <summary>
    /// Outputs messages into the application's console output.
    /// </summary>
    Console,

    /// <summary>
    /// Outputs messages into the specified *.log file.
    /// </summary>
    File,

    /// <summary>
    /// Outputs messages into the application's error output. (stderr)
    /// </summary>
    Error,

    /// <summary>
    /// Default value.
    /// </summary>
    Default = Console
}
