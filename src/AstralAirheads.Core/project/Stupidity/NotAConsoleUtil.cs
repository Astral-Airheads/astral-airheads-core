using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace AstralAirheads.Stupidity;

/// <summary>
/// A random Console utility, for fun. :D
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
[DebuggerStepThrough]
public sealed class NotAConsoleUtil
{
    /// <summary>
    /// This is utterly fucking retarded LMAO.
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="text"></param>
    public static void WriteLine(int delay, string text)
    {
        foreach (var i in text)
        {
            Console.Write(i);
            Thread.Sleep(delay);
        }
    }

    /// <summary>
    /// This is utterly fucking retarded LMAO.
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public static void WriteLine(int delay, string format, params object?[] args)
    {
        foreach (var i in string.Format(format, args))
        {
            Console.Write(i);
            Thread.Sleep(delay);
        }
    }
}
