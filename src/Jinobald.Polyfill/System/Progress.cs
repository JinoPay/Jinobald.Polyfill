#if NET35 || NET40
namespace System
{
    using System.Threading;

    public class ProgressEventArgs<T> : EventArgs
    {
        public ProgressEventArgs(T result)
        {
            this.Result = result;
        }

        public T Result { get; }
    }

    public class Progress<T> : IProgress<T>
    {
        private SynchronizationContext synchronizationContext;

        public event EventHandler<ProgressEventArgs<T>> ProgressChanged;

        public Progress()
        {
            this.synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
        }

        public void Report(T value)
        {
            this.OnProgressChanged(value);
        }

        protected virtual void OnProgressChanged(T value)
        {
            EventHandler<ProgressEventArgs<T>> handler = this.ProgressChanged;
            if (handler != null)
            {
                var args = new ProgressEventArgs<T>(value);
                if (this.synchronizationContext != null && this.synchronizationContext != SynchronizationContext.Current)
                {
                    this.synchronizationContext.Post(s => handler(this, (ProgressEventArgs<T>)s), args);
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
