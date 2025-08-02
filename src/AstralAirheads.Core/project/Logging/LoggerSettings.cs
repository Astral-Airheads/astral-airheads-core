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
public class LoggerSettings(LoggerDestination stdout, LoggerDestination stderr, 
    bool closeWriterOnDispose,
    MessageLevel minLevel,
    string logFileName)
{
    /// <summary>
    /// Default configuration for the logger.
    /// </summary>
    public static LoggerSettings Default => 
        new(LoggerDestination.Default, LoggerDestination.Error, 
            false, MessageLevel.Info, "debug.log");

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
    /// </summary>
    public bool ShouldCloseWriterOnDispose { get; set; } = closeWriterOnDispose;

    /// <summary>
    /// The logger's minimum message level to be displayed.
    /// </summary>
    public MessageLevel MinimumLevel { get; set; } = minLevel;
}
