#if NET40
namespace System.Threading.Tasks;

/// <summary>
/// .NET 4.0에서 Task.Run을 제공하는 확장 클래스입니다.
/// .NET 4.5 이상에서는 Task.Run이 기본 제공됩니다.
/// </summary>
internal static class TaskEx
{
    extension(Task)
    {
        /// <summary>
        /// 지정된 작업을 스레드 풀에서 실행하도록 큐에 넣고 해당 작업을 나타내는 Task 개체를 반환합니다.
        /// </summary>
        /// <param name="action">실행할 작업입니다.</param>
        /// <returns>스레드 풀에서 실행할 작업을 나타내는 Task입니다.</returns>
        public static Task Run(Action action)
        {
            return Run(action, CancellationToken.None);
        }

        /// <summary>
        /// 지정된 작업을 스레드 풀에서 실행하도록 큐에 넣고 해당 작업을 나타내는 Task 개체를 반환합니다.
        /// </summary>
        /// <param name="action">실행할 작업입니다.</param>
        /// <param name="cancellationToken">작업 취소에 사용할 수 있는 CancellationToken입니다.</param>
        /// <returns>스레드 풀에서 실행할 작업을 나타내는 Task입니다.</returns>
        public static Task Run(Action action, CancellationToken cancellationToken)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var tcs = new TaskCompletionSource<object?>();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            ThreadPool.QueueUserWorkItem(_ =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    tcs.TrySetCanceled();
                    return;
                }

                try
                {
                    action();
                    tcs.TrySetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// 지정된 작업을 스레드 풀에서 실행하도록 큐에 넣고 함수에서 반환된 값을 포함하는 Task를 반환합니다.
        /// </summary>
        /// <typeparam name="TResult">반환된 Task의 결과 형식입니다.</typeparam>
        /// <param name="function">실행할 함수입니다.</param>
        /// <returns>스레드 풀에서 실행할 작업을 나타내는 Task입니다.</returns>
        public static Task<TResult> Run<TResult>(Func<TResult> function)
        {
            return Run(function, CancellationToken.None);
        }

        /// <summary>
        /// 지정된 작업을 스레드 풀에서 실행하도록 큐에 넣고 함수에서 반환된 값을 포함하는 Task를 반환합니다.
        /// </summary>
        /// <typeparam name="TResult">반환된 Task의 결과 형식입니다.</typeparam>
        /// <param name="function">실행할 함수입니다.</param>
        /// <param name="cancellationToken">작업 취소에 사용할 수 있는 CancellationToken입니다.</param>
        /// <returns>스레드 풀에서 실행할 작업을 나타내는 Task입니다.</returns>
        public static Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var tcs = new TaskCompletionSource<TResult>();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            ThreadPool.QueueUserWorkItem(_ =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    tcs.TrySetCanceled();
                    return;
                }

                try
                {
                    var result = function();
                    tcs.TrySetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }
    }
}
#endif
