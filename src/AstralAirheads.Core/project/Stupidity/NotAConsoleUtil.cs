// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

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
    public static void Write(int delay, string text)
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
    public static void Write(int delay, string format, params object?[] args)
    {
        foreach (var i in string.Format(format, args))
        {
            Console.Write(i);
            Thread.Sleep(delay);
        }
    }
    /// <summary>
    /// This is utterly fucking retarded LMAO.
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="text"></param>
    public static void WriteLine(int delay, string text)
    {
        foreach (var i in text + "\n")
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
        foreach (var i in string.Format(format + "\n", args))
        {
            Console.Write(i);
            Thread.Sleep(delay);
        }
    }
}
