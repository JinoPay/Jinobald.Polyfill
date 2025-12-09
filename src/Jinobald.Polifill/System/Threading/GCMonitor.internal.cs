// Needed for Workaround

using System.Collections.ThreadSafe;
using System.Core;

namespace System.Threading;
#if NET35 || NET462 ||NET472 ||NET48

public static partial class GCMonitor
{
    private static partial class Internal
    {
        private static readonly WaitCallback _work = _ => RaiseCollected();

        public static void Invoke()
        {
            ThreadPool.QueueUserWorkItem(_work);
        }
    }
}

#else
    public static partial class GCMonitor
    {
        private static partial class Internal
        {
            private static readonly Action _work = RaiseCollected;

            public static void Invoke()
            {
                System.Threading.Tasks.Task.Run(_work);
            }
        }
    }
#endif

public static partial class GCMonitor
{
    private static partial class Internal
    {
        public static WeakDelegateCollection CollectedEventHandlers { get; } =
            new(false, false);

        private static void RaiseCollected()
        {
            var check = Volatile.Read(ref _status);
            if (check != _statusReady) return;

            try
            {
                CollectedEventHandlers.RemoveDeadItems();
                const object? sender = null;
                CollectedEventHandlers.Invoke(ActionHelper.GetNoopAction<Exception>(),
                    DelegateCollectionInvokeOptions.None, sender, EventArgs.Empty);
            }
            catch (Exception exception)
            {
                // Catch them all
                _ = exception;
            }

            Volatile.Write(ref _status, _statusReady);
        }
    }
}