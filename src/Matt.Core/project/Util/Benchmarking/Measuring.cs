using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Matt.Util.Benchmarking;

public sealed class Measuring
{
    public static void MeasureExecutionTime(Action action, out long milliseconds)
    {
        var sw = new Stopwatch();
        
        sw.Start();
        
        action.Invoke();

        sw.Stop();

        milliseconds = sw.ElapsedMilliseconds;
    }
}
