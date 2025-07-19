using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Matt.Logging;

public class Logger(bool closeWriterOnDispose = false) : ILogger
{
    private bool _disposed;

    public TextWriter OutputWriter => Console.Out;

    public void LogDebug(string message) => Log(MessageLevel.Debug, message);
    public void LogDebug([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args) =>
        Log(MessageLevel.Debug, format, args);

    public void LogInfo(string message) => Log(message);
    public void LogInfo([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args) =>
        Log(format, args);

    public void LogWarn(string message) => Log(MessageLevel.Warn, message);
    public void LogWarn([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args) =>
        Log(MessageLevel.Warn, format, args);

    public void LogErr(string message) => Log(MessageLevel.Err, message);
    public void LogErr([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args) =>
        Log(MessageLevel.Err, format, args);

    public void LogCrit(string message) => Log(MessageLevel.Crit, message);
    public void LogCrit([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args) =>
        Log(MessageLevel.Crit, format, args);

    public void LogFatal(string message) => Log(MessageLevel.Fatal, message);
    public void LogFatal([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args) =>
        Log(MessageLevel.Fatal, format, args);

    public void Log(string message) => Log(MessageLevel.Info, message);

    public void Log([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args) =>
        Log(MessageLevel.Info, format, args);

    private string FormatMessage(MessageLevel level, DateTime currentTime, string fileName, int lineNumber, string message)
    {
        var sb = new StringBuilder();

        var levelStr = level switch
        {
            MessageLevel.Debug => "DEBUG",
            MessageLevel.Info => "INFORMATION",
            MessageLevel.Warn => "WARNING",
            MessageLevel.Err => "ERROR",
            MessageLevel.Crit => "CRITICAL",
            MessageLevel.Fatal => "FATAL",
            _ => "INFO",
        };

        // log level
        sb.Append(levelStr);

        sb.Append(" - ");

        // date time
        sb.Append($"{currentTime:HH-mm-ss-MM-dd-yyyy}");

        sb.Append(" - ");

        // file info
        sb.Append($"{fileName}:{lineNumber}");

        sb.Append(" - ");

        // main message
        sb.Append(message);

        return sb.ToString();
    }

    public void Log(MessageLevel level, string message)
    {
        var st = new StackTrace(true); // 'true' enables file info
        StackFrame? frame = null;

        for (int i = 1; i < st.FrameCount; i++)
        {
            var f = st.GetFrame(i);
            var method = f?.GetMethod();
            if (method?.DeclaringType != typeof(Logger))
            {
                frame = f;
                break;
            }
        }

        var file = Path.GetFileName(frame?.GetFileName()) ?? "unknown file";
        var line = frame?.GetFileLineNumber() ?? 0;

        if (level == MessageLevel.Debug)
            Debug.WriteLine(FormatMessage(level, DateTime.Now,
            file, line, message));
        else
            OutputWriter.WriteLine(FormatMessage(level, DateTime.Now,
            file, line, message));
    }

    public void Log([StringSyntax(StringSyntaxAttribute.CompositeFormat)] MessageLevel level, string format, params object?[] args)
    {
        var st = new StackTrace(true); // 'true' enables file info
        StackFrame? frame = null;

        for (int i = 1; i < st.FrameCount; i++)
        {
            var f = st.GetFrame(i);
            var method = f?.GetMethod();
            if (method?.DeclaringType != typeof(Logger))
            {
                frame = f;
                break;
            }
        }

        var file = Path.GetFileName(frame?.GetFileName()) ?? "unknown file";
        var line = frame?.GetFileLineNumber() ?? 0;

        if (level == MessageLevel.Debug)
            Debug.WriteLine(FormatMessage(level, DateTime.Now,
            file, line, string.Format(format, args)));
        else
            OutputWriter.WriteLine(FormatMessage(level, DateTime.Now,
            file, line, string.Format(format, args)));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (closeWriterOnDispose)
                    OutputWriter.Close();
            }
            
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
