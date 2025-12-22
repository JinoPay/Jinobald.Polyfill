using System.Runtime.CompilerServices;

#if NET35
namespace System.Threading.Tasks;

/// <summary>
///     비동기 작업을 나타냅니다.
/// </summary>
public class Task : IDisposable
{
    private readonly CancellationToken _cancellationToken;
    private readonly List<Action<Task>> _continuations = new();
    private readonly object _lock = new();
    private volatile Exception? _exception;
    private volatile ManualResetEvent? _completionEvent;
    private volatile TaskStatus _status;

    /// <summary>
    ///     새 Task 인스턴스를 초기화합니다.
    /// </summary>
    public Task()
    {
        _status = TaskStatus.Created;
        _cancellationToken = CancellationToken.None;
    }

    internal Task(CancellationToken cancellationToken)
    {
        _status = TaskStatus.Created;
        _cancellationToken = cancellationToken;
    }

    /// <summary>
    ///     이미 성공적으로 완료된 작업을 가져옵니다.
    /// </summary>
    public static Task CompletedTask => FromResult<object?>(null);

    /// <summary>
    ///     이 Task를 실패하게 만든 예외를 가져옵니다.
    /// </summary>
    public AggregateException? Exception
    {
        get
        {
            if (_exception == null)
            {
                return null;
            }

            return new AggregateException(_exception);
        }
    }

    /// <summary>
    ///     이 Task가 취소되었는지 여부를 가져옵니다.
    /// </summary>
    public bool IsCanceled => _status == TaskStatus.Canceled;

    /// <summary>
    ///     이 Task가 완료되었는지 여부를 가져옵니다.
    /// </summary>
    public bool IsCompleted
    {
        get
        {
            TaskStatus status = _status;
            return status == TaskStatus.RanToCompletion ||
                   status == TaskStatus.Faulted ||
                   status == TaskStatus.Canceled;
        }
    }

    /// <summary>
    ///     이 Task가 성공적으로 완료되었는지 여부를 가져옵니다.
    /// </summary>
    public bool IsCompletedSuccessfully => _status == TaskStatus.RanToCompletion;

    /// <summary>
    ///     이 Task가 실패했는지 여부를 가져옵니다.
    /// </summary>
    public bool IsFaulted => _status == TaskStatus.Faulted;

    /// <summary>
    ///     이 Task의 TaskStatus를 가져옵니다.
    /// </summary>
    public TaskStatus Status => _status;

    /// <summary>
    ///     Task 클래스의 현재 인스턴스에서 사용하는 모든 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     제공된 Task 개체 중 하나가 실행을 완료할 때까지 기다립니다.
    /// </summary>
    public static int WaitAny(params Task[] tasks)
    {
        if (tasks == null)
        {
            throw new ArgumentNullException(nameof(tasks));
        }

        if (tasks.Length == 0)
        {
            throw new ArgumentException("Task 배열이 비어 있습니다", nameof(tasks));
        }

        var handles = new WaitHandle[tasks.Length];
        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i] == null)
            {
                throw new ArgumentException("Task 배열에 null 요소가 포함되어 있습니다", nameof(tasks));
            }

            handles[i] = tasks[i].GetCompletionEvent();
        }

        return WaitHandle.WaitAny(handles);
    }

    /// <summary>
    ///     시간 지연 후에 완료되는 Task를 만듭니다.
    /// </summary>
    public static Task Delay(int millisecondsDelay)
    {
        return Delay(millisecondsDelay, CancellationToken.None);
    }

    /// <summary>
    ///     시간 지연 후에 완료되는 Task를 만듭니다.
    /// </summary>
    public static Task Delay(int millisecondsDelay, CancellationToken cancellationToken)
    {
        if (millisecondsDelay < -1)
        {
            throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));
        }

        var task = new Task(cancellationToken);

        if (cancellationToken.IsCancellationRequested)
        {
            task.SetCanceled();
            return task;
        }

        if (millisecondsDelay == 0)
        {
            task.SetResult();
            return task;
        }

        Timer? timer = null;
        CancellationTokenRegistration registration = default;

        timer = new Timer(_ =>
        {
            try
            {
                registration.Dispose();
                task.SetResult();
            }
            finally
            {
                timer?.Dispose();
            }
        }, null, millisecondsDelay, Timeout.Infinite);

        if (cancellationToken.CanBeCanceled)
        {
            registration = cancellationToken.Register(() =>
            {
                try
                {
                    task.SetCanceled();
                }
                finally
                {
                    timer?.Dispose();
                }
            });
        }

        return task;
    }

    /// <summary>
    ///     지정된 CancellationToken으로 취소로 인해 완료된 Task를 만듭니다.
    /// </summary>
    public static Task FromCanceled(CancellationToken cancellationToken)
    {
        var task = new Task(cancellationToken);
        task.SetCanceled();
        return task;
    }

    /// <summary>
    ///     지정된 CancellationToken으로 취소로 인해 완료된 Task를 만듭니다.
    /// </summary>
    /// <typeparam name="TResult">Task의 결과 형식입니다.</typeparam>
    /// <param name="cancellationToken">취소에 사용된 CancellationToken입니다.</param>
    /// <returns>취소로 완료된 Task입니다.</returns>
    public static Task<TResult> FromCanceled<TResult>(CancellationToken cancellationToken)
    {
        var task = new Task<TResult>(cancellationToken);
        task.SetCanceled();
        return task;
    }

    /// <summary>
    ///     지정된 예외와 함께 예외적으로 완료된 Task를 만듭니다.
    /// </summary>
    public static Task FromException(Exception exception)
    {
        var task = new Task(CancellationToken.None);
        task.SetException(exception);
        return task;
    }

    /// <summary>
    ///     지정된 작업을 스레드 풀에서 실행하도록 큐에 넣고 해당 작업을 나타내는 Task 개체를 반환합니다.
    /// </summary>
    public static Task Run(Action action)
    {
        return Run(action, CancellationToken.None);
    }

    /// <summary>
    ///     지정된 작업을 스레드 풀에서 실행하도록 큐에 넣고 해당 작업을 나타내는 Task 개체를 반환합니다.
    /// </summary>
    public static Task Run(Action action, CancellationToken cancellationToken)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var task = new Task(cancellationToken);
        task.Start(() => action());
        return task;
    }

    /// <summary>
    ///     제공된 모든 작업이 완료되면 완료되는 작업을 만듭니다.
    /// </summary>
    public static Task WhenAll(params Task[] tasks)
    {
        if (tasks == null)
        {
            throw new ArgumentNullException(nameof(tasks));
        }

        return Run(() =>
        {
            foreach (var task in tasks)
            {
                if (task == null)
                {
                    throw new ArgumentException("Task 배열에 null 요소가 포함되어 있습니다", nameof(tasks));
                }

                task.Wait();
            }
        });
    }

    /// <summary>
    ///     제공된 작업 중 하나가 완료되면 완료되는 작업을 만듭니다.
    /// </summary>
    public static Task<Task> WhenAny(params Task[] tasks)
    {
        if (tasks == null)
        {
            throw new ArgumentNullException(nameof(tasks));
        }

        if (tasks.Length == 0)
        {
            throw new ArgumentException("Task 배열이 비어 있습니다", nameof(tasks));
        }

        return Run(() =>
        {
            int index = WaitAny(tasks);
            return tasks[index];
        });
    }

    /// <summary>
    ///     제공된 모든 작업이 완료되면 완료되는 작업을 만듭니다.
    /// </summary>
    public static Task<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks)
    {
        if (tasks == null)
        {
            throw new ArgumentNullException(nameof(tasks));
        }

        return Run(() =>
        {
            var results = new TResult[tasks.Length];
            for (int i = 0; i < tasks.Length; i++)
            {
                if (tasks[i] == null)
                {
                    throw new ArgumentException("Task 배열에 null 요소가 포함되어 있습니다", nameof(tasks));
                }

                results[i] = tasks[i].Result;
            }

            return results;
        });
    }

    /// <summary>
    ///     지정된 예외와 함께 예외적으로 완료된 Task를 만듭니다.
    /// </summary>
    public static Task<TResult> FromException<TResult>(Exception exception)
    {
        var task = new Task<TResult>(CancellationToken.None);
        task.SetException(exception);
        return task;
    }

    /// <summary>
    ///     지정된 결과로 성공적으로 완료된 Task를 만듭니다.
    /// </summary>
    public static Task<TResult> FromResult<TResult>(TResult result)
    {
        var task = new Task<TResult>(CancellationToken.None);
        task.SetResult(result);
        return task;
    }

    /// <summary>
    ///     지정된 작업을 스레드 풀에서 실행하도록 큐에 넣고 함수에서 반환된 Task의 프록시를 반환합니다.
    /// </summary>
    public static Task<TResult> Run<TResult>(Func<TResult> function)
    {
        return Run(function, CancellationToken.None);
    }

    /// <summary>
    ///     지정된 작업을 스레드 풀에서 실행하도록 큐에 넣고 함수에서 반환된 Task의 프록시를 반환합니다.
    /// </summary>
    public static Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken)
    {
        if (function == null)
        {
            throw new ArgumentNullException(nameof(function));
        }

        var task = new Task<TResult>(cancellationToken);
        task.Start(() => function());
        return task;
    }

    /// <summary>
    ///     제공된 모든 Task 개체가 실행을 완료할 때까지 기다립니다.
    /// </summary>
    public static void WaitAll(params Task[] tasks)
    {
        if (tasks == null)
        {
            throw new ArgumentNullException(nameof(tasks));
        }

        foreach (var task in tasks)
        {
            if (task == null)
            {
                throw new ArgumentException("Task 배열에 null 요소가 포함되어 있습니다", nameof(tasks));
            }

            task.Wait();
        }
    }

    /// <summary>
    ///     지정된 시간 간격 내에 Task가 실행을 완료할 때까지 기다립니다.
    /// </summary>
    public bool Wait(int millisecondsTimeout)
    {
        return Wait(millisecondsTimeout, CancellationToken.None);
    }

    /// <summary>
    ///     지정된 시간 간격 내에 Task가 실행을 완료할 때까지 기다립니다.
    /// </summary>
    public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
    {
        if (IsCompleted)
        {
            if (_exception != null)
            {
                throw new AggregateException(_exception);
            }

            return true;
        }

        ManualResetEvent completionEvent = GetCompletionEvent();

        WaitHandle[] waitHandles = cancellationToken.CanBeCanceled
            ? new[] { completionEvent, cancellationToken.WaitHandle }
            : new[] { completionEvent };

        int index = WaitHandle.WaitAny(waitHandles, millisecondsTimeout);

        if (index == WaitHandle.WaitTimeout)
        {
            return false;
        }

        if (cancellationToken.CanBeCanceled && index == 1)
        {
            throw new OperationCanceledException("작업이 취소되었습니다.");
        }

        if (_exception != null)
        {
            throw new AggregateException(_exception);
        }

        return true;
    }

    /// <summary>
    ///     대상 Task가 완료될 때 실행되는 연속 작업을 만듭니다.
    /// </summary>
    public Task ContinueWith(Action<Task> continuationAction)
    {
        if (continuationAction == null)
        {
            throw new ArgumentNullException(nameof(continuationAction));
        }

        var continuationTask = new Task();

        Action<Task> continuation = t =>
        {
            try
            {
                continuationAction(t);
                continuationTask.SetResult();
            }
            catch (Exception ex)
            {
                continuationTask.SetException(ex);
            }
        };

        lock (_lock)
        {
            if (IsCompleted)
            {
                ThreadPool.QueueUserWorkItem(_ => continuation(this));
            }
            else
            {
                _continuations.Add(continuation);
            }
        }

        return continuationTask;
    }

    /// <summary>
    ///     대상 Task가 완료될 때 실행되는 연속 작업을 만듭니다.
    /// </summary>
    public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction)
    {
        if (continuationFunction == null)
        {
            throw new ArgumentNullException(nameof(continuationFunction));
        }

        var continuationTask = new Task<TResult>();

        Action<Task> continuation = t =>
        {
            try
            {
                TResult? result = continuationFunction(t);
                continuationTask.SetResult(result);
            }
            catch (Exception ex)
            {
                continuationTask.SetException(ex);
            }
        };

        lock (_lock)
        {
            if (IsCompleted)
            {
                ThreadPool.QueueUserWorkItem(_ => continuation(this));
            }
            else
            {
                _continuations.Add(continuation);
            }
        }

        return continuationTask;
    }

#if NET35 || NET40
    /// <summary>
    ///     이 Task를 await하는 데 사용되는 awaiter를 가져옵니다.
    /// </summary>
    public TaskAwaiter GetAwaiter()
    {
        return new TaskAwaiter(this);
    }
#endif

    /// <summary>
    ///     Task를 지정된 예외로 실패한 것으로 표시합니다.
    /// </summary>
    public void SetException(Exception exception)
    {
        lock (_lock)
        {
            if (IsCompleted)
            {
                return;
            }

            _exception = exception;
            _status = TaskStatus.Faulted;
            SignalCompletion();
        }
    }

    /// <summary>
    ///     Task를 성공적으로 완료된 것으로 표시합니다.
    /// </summary>
    public void SetResult()
    {
        lock (_lock)
        {
            if (IsCompleted)
            {
                return;
            }

            _status = TaskStatus.RanToCompletion;
            SignalCompletion();
        }
    }

    /// <summary>
    ///     Task가 실행을 완료할 때까지 기다립니다.
    /// </summary>
    public void Wait()
    {
        Wait(Timeout.Infinite, CancellationToken.None);
    }

    /// <summary>
    ///     Task가 실행을 완료할 때까지 기다립니다.
    /// </summary>
    public void Wait(CancellationToken cancellationToken)
    {
        Wait(Timeout.Infinite, cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_completionEvent != null)
            {
                _completionEvent.Close();
                _completionEvent = null;
            }
        }
    }

    private ManualResetEvent GetCompletionEvent()
    {
        lock (_lock)
        {
            if (_completionEvent == null)
            {
                _completionEvent = new ManualResetEvent(IsCompleted);
            }

            return _completionEvent;
        }
    }

    private void SignalCompletion()
    {
        if (_completionEvent != null)
        {
            _completionEvent.Set();
        }

        Action<Task>[] continuations = _continuations.ToArray();
        _continuations.Clear();

        foreach (Action<Task> continuation in continuations)
        {
            Action<Task> cont = continuation;
            ThreadPool.QueueUserWorkItem(_ => cont(this));
        }
    }

    internal void Start(Action action)
    {
        lock (_lock)
        {
            if (_status != TaskStatus.Created)
            {
                throw new InvalidOperationException("Task가 이미 시작되었습니다");
            }

            _status = TaskStatus.WaitingToRun;
        }

        ThreadPool.QueueUserWorkItem(_ =>
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                SetCanceled();
                return;
            }

            lock (_lock)
            {
                _status = TaskStatus.Running;
            }

            try
            {
                action();
                SetResult();
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        });
    }

    /// <summary>
    ///     Task를 취소된 것으로 표시합니다.
    /// </summary>
    public void SetCanceled()
    {
        lock (_lock)
        {
            if (IsCompleted)
            {
                return;
            }

            _status = TaskStatus.Canceled;
            SignalCompletion();
        }
    }
}

/// <summary>
///     값을 반환할 수 있는 비동기 작업을 나타냅니다.
/// </summary>
public class Task<TResult> : Task
{
    private TResult _result = default!;

    internal Task() : base()
    {
    }

    internal Task(CancellationToken cancellationToken) : base(cancellationToken)
    {
    }

    /// <summary>
    ///     이 Task의 결과 값을 가져옵니다.
    /// </summary>
    public TResult Result
    {
        get
        {
            Wait();
            return _result;
        }
    }

    /// <summary>
    ///     대상 Task가 완료될 때 실행되는 연속 작업을 만듭니다.
    /// </summary>
    public Task ContinueWith(Action<Task<TResult>> continuationAction)
    {
        if (continuationAction == null)
        {
            throw new ArgumentNullException(nameof(continuationAction));
        }

        return base.ContinueWith(t => continuationAction((Task<TResult>)t));
    }

    /// <summary>
    ///     대상 Task가 완료될 때 실행되는 연속 작업을 만듭니다.
    /// </summary>
    public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction)
    {
        if (continuationFunction == null)
        {
            throw new ArgumentNullException(nameof(continuationFunction));
        }

        return base.ContinueWith(t => continuationFunction((Task<TResult>)t));
    }

#if NET35 || NET40
    /// <summary>
    ///     이 Task를 await하는 데 사용되는 awaiter를 가져옵니다.
    /// </summary>
    public new TaskAwaiter<TResult> GetAwaiter()
    {
        return new TaskAwaiter<TResult>(this);
    }
#endif

    /// <summary>
    ///     Task를 지정된 결과로 성공적으로 완료된 것으로 표시합니다.
    /// </summary>
    public void SetResult(TResult result)
    {
        _result = result;
        base.SetResult();
    }

    internal void Start(Func<TResult> function)
    {
        base.Start(() =>
        {
            _result = function();
        });
    }
}
#endif
