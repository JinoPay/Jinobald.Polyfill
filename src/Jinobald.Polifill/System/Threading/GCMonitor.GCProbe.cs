using System;
using System.Diagnostics;
using System.Threading;

namespace System.Threading;

public static partial class GCMonitor
{
    [DebuggerNonUserCode]
    private sealed class GCProbe
#if NET35 || NET462 ||NET472 ||NET48
        : System.Runtime.ConstrainedExecution.CriticalFinalizerObject
#endif
    {
        ~GCProbe()
        {
            try
            {
                // Empty
            }
            finally
            {
                try
                {
                    var check = Volatile.Read(ref _status);
                    if (check == _statusReady)
                    {
                        GC.ReRegisterForFinalize(this);
                        Internal.Invoke();
                    }
                }
                catch (Exception exception)
                {
                    // Catch them all - there shouldn't be exceptions here, yet we really don't want them
                    _ = exception;
                }
            }
        }
    }
}