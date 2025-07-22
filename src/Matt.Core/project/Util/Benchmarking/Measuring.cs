using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Matt.Util.Benchmarking;

/// <summary>
/// 
/// </summary>
public sealed class Measuring
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="action"></param>
    /// <param name="milliseconds"></param>
    public static void MeasureExecutionTime(Action action, out long milliseconds)
    {
        var sw = new Stopwatch();
        
        sw.Start();
        
        action.Invoke();

        sw.Stop();

        milliseconds = sw.ElapsedMilliseconds;
    }
}
