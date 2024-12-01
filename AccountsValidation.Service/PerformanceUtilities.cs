using System.Diagnostics;

namespace AccountsValidation.Service;

public static class PerformanceUtilities
{
    public static TimeSpan WithMeasuredTime(Action action)
    {
        var stopwatch = Stopwatch.StartNew();

        action.Invoke();

        stopwatch.Stop();

        return stopwatch.Elapsed;
    }
}
