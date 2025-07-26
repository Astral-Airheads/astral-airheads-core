namespace AstralAirheads.Logging;

/// <summary>
/// Defines the severity levels for log messages.
/// </summary>
/// <remarks>
/// The levels are ordered from least severe (Debug) to most severe (Fatal).
/// Each level represents a different degree of importance or urgency for the logged message.
/// </remarks>
public enum MessageLevel
{
    /// <summary>
    /// Debug level messages used for detailed diagnostic information.
    /// </summary>
    /// <remarks>
    /// These messages are typically only useful during development and debugging.
    /// They provide detailed information about the internal state of the application.
    /// </remarks>
    Debug = 0,

    /// <summary>
    /// Information level messages for general application flow.
    /// </summary>
    /// <remarks>
    /// These messages provide general information about the application's operation
    /// and are useful for monitoring normal application behavior.
    /// </remarks>
    Info,

    /// <summary>
    /// Warning level messages for potentially harmful situations.
    /// </summary>
    /// <remarks>
    /// These messages indicate situations that might be problematic but don't
    /// necessarily indicate an error or failure.
    /// </remarks>
    Warn,

    /// <summary>
    /// Error level messages for error events that might still allow the application to continue running.
    /// </summary>
    /// <remarks>
    /// These messages indicate errors that have occurred but don't necessarily
    /// prevent the application from continuing its operation.
    /// </remarks>
    Err,

    /// <summary>
    /// Critical level messages for critical events that may prevent the application from functioning properly.
    /// </summary>
    /// <remarks>
    /// These messages indicate serious problems that may affect the application's
    /// ability to function correctly.
    /// </remarks>
    Crit,

    /// <summary>
    /// Fatal level messages for fatal events that will lead to application termination.
    /// </summary>
    /// <remarks>
    /// These messages indicate critical errors that will cause the application
    /// to terminate or become unusable.
    /// </remarks>
    Fatal
}
