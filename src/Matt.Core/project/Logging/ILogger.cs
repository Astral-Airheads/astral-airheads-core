using System;
using System.IO;

namespace Matt.Logging;

public interface ILogger : IDisposable
{
    TextWriter OutputWriter { get; }
    TextWriter ErrWriter { get; }

    void Log(string message);
    void Log(string format, object? arg0);
    void Log(string format, object? arg0, object? arg1);
    void Log(string format, object? arg0, object? arg1, object? arg2);
    void Log(string format, params object?[] args);

    void Log(MessageLevel level, string message);
    void Log(MessageLevel level, string format, object? arg0);
    void Log(MessageLevel level, string format, object? arg0, object? arg1);
    void Log(MessageLevel level, string format, object? arg0, object? arg1, object? arg2);
    void Log(MessageLevel level, string format, params object?[] args);
}
