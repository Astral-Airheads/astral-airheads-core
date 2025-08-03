// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

namespace AstralAirheads.Logging;

/// <summary>
/// Stores configuration and stuff for proper logging, these properties
/// can be changed at any time.
/// </summary>
/// <param name="stdout">The value of the destination for normal messages. (info to warning)</param>
/// <param name="stderr">The value of the destination for error messages. (error to fatal)</param>
/// <param name="closeWriterOnDispose">Specifies whether to close the main output writer upon disposing the logger.</param>
/// <param name="minLevel">The value of the minimum logger level.</param>
/// <param name="logFileName">The value of the output file name.</param>
/// <param name="enableColors">Specifies whether to enable log message colors.</param>
public class LoggerSettings(LoggerDestination stdout, LoggerDestination stderr, 
    bool closeWriterOnDispose,
    MessageLevel minLevel,
    string logFileName, bool enableColors)
{
    /// <summary>
    /// Default configuration for the logger.
    /// </summary>
    public static readonly LoggerSettings Default = 
        new(LoggerDestination.Default, LoggerDestination.Error, 
            false, MessageLevel.Info, "debug.log", true);

    /// <summary>
    /// The file name of the logger output.
    /// </summary>
    public string LogFileName { get; set; } = logFileName;

    /// <summary>
    /// The main destination for normal messages. (info to warning)
    /// </summary>
    public LoggerDestination StdOut { get; set; } = stdout;

    /// <summary>
    /// The main destination for error messages. (error to fatal)
    /// </summary>
    public LoggerDestination StdErr { get; set; } = stderr;

    /// <summary>
    /// Specifies whether to close the output writer upon disposing the logger.
	/// The only exception to this is when the destination is redirected to a log file.
    /// </summary>
    public bool ShouldCloseWriterOnDispose { get; set; } = closeWriterOnDispose;

	/// <summary>
	/// Specifies whether to close the error writer upon disposing the logger.
	/// The only exception to this is when the destination is redirected to a log file.
	/// </summary>
	public bool ShouldCloseErrWriterOnDispose { get; set; } = closeWriterOnDispose;

	/// <summary>
	/// Specifies whether to enable color logging or not.
	/// </summary>
	public bool Colors { get; set; } = enableColors;

    /// <summary>
    /// The logger's minimum message level to be displayed.
    /// </summary>
    public MessageLevel MinimumLevel { get; set; } = minLevel;
}
