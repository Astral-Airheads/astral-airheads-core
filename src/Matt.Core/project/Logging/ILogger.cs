using System;
using System.IO;

namespace Matt.Logging;

public interface ILogger : IDisposable
{
    TextWriter OutputWriter { get; }

    void Log(string message);
    void Log(string format, params object?[] args);

    void Log(MessageLevel level, string message);
    void Log(MessageLevel level, string format, params object?[] args);
}
