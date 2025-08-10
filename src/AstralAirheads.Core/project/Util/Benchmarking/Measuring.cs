// Copyright (c) 2025 Matthew for the Astral Airheads, all rights reserved.
// Licensed under the MIT/X11 license, license terms are applied here.

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AstralAirheads.Util.Benchmarking;

/// <summary>
/// Basic performance measuring utility.
/// </summary>
public sealed class Measuring
{
    /// <summary>
    /// Measures execution time in milliseconds.
    /// </summary>
    /// <param name="action">The value of the action to execute.</param>
    /// <param name="milliseconds">The outputted estimated time (in milliseconds).</param>
    public static void MeasureExecutionTime(Action action, out long milliseconds)
    {
        var sw = new Stopwatch();
        
        sw.Start();
        
        action.Invoke();

        sw.Stop();

        milliseconds = sw.ElapsedMilliseconds;
    }
}
