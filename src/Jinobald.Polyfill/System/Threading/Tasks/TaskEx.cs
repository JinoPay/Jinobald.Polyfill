#if NET40 || NET45 || NET451 || NET452
namespace System.Threading.Tasks;

/// <summary>
/// .NET 4.0/4.5에서 Task 관련 확장 메서드를 제공하는 확장 클래스입니다.
/// </summary>
internal static class TaskEx
{
#if NET40
    extension(CancellationTokenSource cts)
    {
        /// <summary>
        /// 지정된 시간 범위 후에 이 CancellationTokenSource에 대한 취소 작업을 예약합니다.
        /// </summary>
        /// <param name="millisecondsDelay">취소되기 전까지 기다릴 시간(밀리초)입니다.</param>
        public void CancelAfter(int millisecondsDelay)
        {
            if (millisecondsDelay < -1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            if (millisecondsDelay == 0)
            {
                cts.Cancel();
                return;
            }

            Timer? timer = null;
            timer = new Timer(_ =>
            {
                try
                {
                    cts.Cancel();
                }
                catch (ObjectDisposedException)
                {
                    // CancellationTokenSource가 이미 dispose된 경우 무시
                }
                timer?.Dispose();
            }, null, millisecondsDelay, Timeout.Infinite);
        }

        /// <summary>
        /// 지정된 시간 범위 후에 이 CancellationTokenSource에 대한 취소 작업을 예약합니다.
        /// </summary>
        /// <param name="delay">취소되기 전까지 기다릴 시간입니다.</param>
        public void CancelAfter(TimeSpan delay)
        {
            long totalMilliseconds = (long)delay.TotalMilliseconds;
            if (totalMilliseconds < -1 || totalMilliseconds > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(delay));

            cts.CancelAfter((int)totalMilliseconds);
        }
    }
#endif

    extension(Task)
    {
#if NET40
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

        /// <summary>
        /// 시간 지연 후에 완료되는 Task를 만듭니다.
        /// </summary>
        /// <param name="millisecondsDelay">반환된 Task가 완료되기 전에 대기할 시간(밀리초)입니다.</param>
        /// <returns>지정된 시간 지연 후에 완료되는 Task입니다.</returns>
        public static Task Delay(int millisecondsDelay)
        {
            return Delay(millisecondsDelay, CancellationToken.None);
        }

        /// <summary>
        /// 시간 지연 후에 완료되는 Task를 만듭니다.
        /// </summary>
        /// <param name="millisecondsDelay">반환된 Task가 완료되기 전에 대기할 시간(밀리초)입니다.</param>
        /// <param name="cancellationToken">지연 취소에 사용할 수 있는 CancellationToken입니다.</param>
        /// <returns>지정된 시간 지연 후에 완료되는 Task입니다.</returns>
        public static Task Delay(int millisecondsDelay, CancellationToken cancellationToken)
        {
            if (millisecondsDelay < -1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            var tcs = new TaskCompletionSource<object?>();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            if (millisecondsDelay == 0)
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            Timer? timer = null;
            timer = new Timer(_ =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    tcs.TrySetResult(null);
                }
                timer?.Dispose();
            }, null, millisecondsDelay, Timeout.Infinite);

            return tcs.Task;
        }

        /// <summary>
        /// 지정된 결과로 성공적으로 완료된 Task를 만듭니다.
        /// </summary>
        /// <typeparam name="TResult">Task에 저장할 결과 형식입니다.</typeparam>
        /// <param name="result">완료된 Task에 저장할 결과입니다.</param>
        /// <returns>지정된 결과로 완료된 Task입니다.</returns>
        public static Task<TResult> FromResult<TResult>(TResult result)
        {
            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetResult(result);
            return tcs.Task;
        }

        /// <summary>
        /// 제공된 모든 작업이 완료되면 완료되는 작업을 만듭니다.
        /// </summary>
        /// <param name="tasks">완료를 기다릴 Task 배열입니다.</param>
        /// <returns>모든 Task가 완료되면 완료되는 Task입니다.</returns>
        public static Task WhenAll(params Task[] tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            if (tasks.Length == 0)
            {
                var emptyTcs = new TaskCompletionSource<object?>();
                emptyTcs.SetResult(null);
                return emptyTcs.Task;
            }

            return Run(() =>
            {
                foreach (var task in tasks)
                {
                    if (task == null)
                        throw new ArgumentException("Task 배열에 null 요소가 포함되어 있습니다", nameof(tasks));
                    task.Wait();
                }
            });
        }

        /// <summary>
        /// 제공된 모든 작업이 완료되면 완료되는 작업을 만듭니다.
        /// </summary>
        /// <typeparam name="TResult">완료된 Task의 결과 형식입니다.</typeparam>
        /// <param name="tasks">완료를 기다릴 Task 배열입니다.</param>
        /// <returns>모든 Task가 완료되면 완료되는 Task입니다.</returns>
        public static Task<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            return Run(() =>
            {
                var results = new TResult[tasks.Length];
                for (int i = 0; i < tasks.Length; i++)
                {
                    if (tasks[i] == null)
                        throw new ArgumentException("Task 배열에 null 요소가 포함되어 있습니다", nameof(tasks));
                    results[i] = tasks[i].Result;
                }
                return results;
            });
        }

        /// <summary>
        /// 제공된 작업 중 하나가 완료되면 완료되는 작업을 만듭니다.
        /// </summary>
        /// <param name="tasks">완료를 기다릴 Task 배열입니다.</param>
        /// <returns>첫 번째 Task가 완료되면 완료되는 Task입니다.</returns>
        public static Task<Task> WhenAny(params Task[] tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            if (tasks.Length == 0)
                throw new ArgumentException("Task 배열이 비어 있습니다", nameof(tasks));

            return Run(() =>
            {
                int index = Task.WaitAny(tasks);
                return tasks[index];
            });
        }
#endif

        /// <summary>
        /// 지정된 예외와 함께 예외적으로 완료된 Task를 만듭니다.
        /// </summary>
        /// <param name="exception">Task를 완료할 예외입니다.</param>
        /// <returns>지정된 예외로 완료된 Task입니다.</returns>
        public static Task FromException(Exception exception)
        {
            var tcs = new TaskCompletionSource<object?>();
            tcs.SetException(exception);
            return tcs.Task;
        }

        /// <summary>
        /// 지정된 예외와 함께 예외적으로 완료된 Task를 만듭니다.
        /// </summary>
        /// <typeparam name="TResult">Task의 결과 형식입니다.</typeparam>
        /// <param name="exception">Task를 완료할 예외입니다.</param>
        /// <returns>지정된 예외로 완료된 Task입니다.</returns>
        public static Task<TResult> FromException<TResult>(Exception exception)
        {
            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetException(exception);
            return tcs.Task;
        }

        /// <summary>
        /// 지정된 CancellationToken으로 취소로 인해 완료된 Task를 만듭니다.
        /// </summary>
        /// <param name="cancellationToken">취소에 사용된 CancellationToken입니다.</param>
        /// <returns>취소로 완료된 Task입니다.</returns>
        public static Task FromCanceled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object?>();
            tcs.SetCanceled();
            return tcs.Task;
        }

        /// <summary>
        /// 지정된 CancellationToken으로 취소로 인해 완료된 Task를 만듭니다.
        /// </summary>
        /// <typeparam name="TResult">Task의 결과 형식입니다.</typeparam>
        /// <param name="cancellationToken">취소에 사용된 CancellationToken입니다.</param>
        /// <returns>취소로 완료된 Task입니다.</returns>
        public static Task<TResult> FromCanceled<TResult>(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetCanceled();
            return tcs.Task;
        }
    }
}
#endif
