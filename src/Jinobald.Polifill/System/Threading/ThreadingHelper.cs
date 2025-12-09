// Needed for NET40

using System.Diagnostics;

namespace System.Threading;

[DebuggerNonUserCode]
public static partial class ThreadingHelper
{
    internal const int SleepCountHint = 10;

    internal static long Milliseconds(long ticks)
    {
        return ticks / TimeSpan.TicksPerMillisecond;
    }

    internal static long TicksNow()
    {
        return DateTime.Now.Ticks;
    }
}