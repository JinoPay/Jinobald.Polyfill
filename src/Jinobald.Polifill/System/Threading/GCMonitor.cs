using System.Diagnostics;

namespace System.Threading;

[DebuggerNonUserCode]
public static partial class GCMonitor
{
    private static int _status = _statusNotReady;
    private const int _statusNotReady = -2;
    private const int _statusPending = -1;
    private const int _statusReady = 0;

    public static bool FinalizingForUnload
    {
        get
        {
#if NET35 || NET462 ||NET472 ||NET48
            return AppDomain.CurrentDomain.IsFinalizingForUnload();
#else
            return false;
#endif
        }
    }

    public static event EventHandler Collected
    {
        add
        {
            try
            {
                Initialize();
                Internal.CollectedEventHandlers.Add(value);
            }
            catch (NullReferenceException) when (value == null)
            {
                // Empty
            }
        }

        remove
        {
            if (Volatile.Read(ref _status) != _statusReady) return;

            try
            {
                Internal.CollectedEventHandlers.Remove(value);
            }
            catch (NullReferenceException) when (value == null)
            {
                // Empty
            }
        }
    }

    private static void Initialize()
    {
        switch (Interlocked.CompareExchange(ref _status, _statusPending, _statusNotReady))
        {
            case _statusNotReady:
                GC.KeepAlive(new GCProbe());
                Volatile.Write(ref _status, _statusReady);
                break;

            case _statusPending:
                ThreadingHelper.SpinWaitUntil(ref _status, _statusReady);
                break;
        }
    }
}

#if NET35 || NET462 ||NET472 ||NET48

public static partial class GCMonitor
{
    private const int _statusFinished = 1;

    static GCMonitor()
    {
        var currentAppDomain = AppDomain.CurrentDomain;
        currentAppDomain.ProcessExit += ReportApplicationDomainExit;
        currentAppDomain.DomainUnload += ReportApplicationDomainExit;
    }

    private static void ReportApplicationDomainExit(object? sender, EventArgs e)
    {
        Volatile.Write(ref _status, _statusFinished);
    }
}

#endif