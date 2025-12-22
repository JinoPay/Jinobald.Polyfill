#if NET35 || NET40
namespace System
{
    public class ProgressEventArgs<T> : EventArgs
    {
        public ProgressEventArgs(T result)
        {
            Result = result;
        }

        public T Result { get; }
    }

    public class Progress<T> : IProgress<T>
    {
        private readonly SynchronizationContext synchronizationContext;

        public Progress()
        {
            synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
        }

        public event EventHandler<ProgressEventArgs<T>> ProgressChanged;

        public void Report(T value)
        {
            OnProgressChanged(value);
        }

        protected virtual void OnProgressChanged(T value)
        {
            EventHandler<ProgressEventArgs<T>> handler = ProgressChanged;
            if (handler != null)
            {
                var args = new ProgressEventArgs<T>(value);
                if (synchronizationContext != null && synchronizationContext != SynchronizationContext.Current)
                {
                    synchronizationContext.Post(s => handler(this, (ProgressEventArgs<T>)s), args);
                }
                else
                {
                    handler(this, args);
                }
            }
        }
    }
}
#endif
