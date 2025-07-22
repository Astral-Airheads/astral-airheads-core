using System;
using System.Threading;

namespace Matt.Stupidity;

/// <summary>
/// A random Console utility, for fun. :D
/// </summary>
public sealed class NotAConsoleUtil
{
    public static void WriteLine(int delay, string text)
    {
        foreach (var i in text)
        {
            Console.Write(i);
            Thread.Sleep(delay);
        }
    }


    public static void WriteLine(int delay, string format, params object?[] args)
    {
        foreach (var i in string.Format(format, args))
        {
            Console.Write(i);
            Thread.Sleep(delay);
        }
    }
}
