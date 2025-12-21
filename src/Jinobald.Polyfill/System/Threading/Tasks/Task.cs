#if NET35
namespace System.Threading.Tasks;

using System;
using System.Collections.Generic;

/// <summary>
/// 비동기 작업을 나타냅니다.
/// </summary>
public class Task : IDisposable
{
    private readonly object _lock = new object();
    private volatile TaskStatus _status;
    private volatile Exception? _exception;
    private volatile ManualResetEvent? _completionEvent;
    private readonly List<Action<Task>> _continuations = new List<Action<Task>>();
    private readonly CancellationToken _cancellationToken;

    /// <summary>
    /// 이 Task의 TaskStatus를 가져옵니다.
    /// </summary>
    public TaskStatus Status
    {
        get { return _status; }
    }

    /// <summary>
    /// 이 Task가 완료되었는지 여부를 가져옵니다.
    /// </summary>
    public bool IsCompleted
    {
        get
        {
            var status = _status;
            return status == TaskStatus.RanToCompletion ||
                   status == TaskStatus.Faulted ||
                   status == TaskStatus.Canceled;
        }
    }

    /// <summary>
    /// 이 Task가 성공적으로 완료되었는지 여부를 가져옵니다.
    /// </summary>
    public bool IsCompletedSuccessfully
    {
        get { return _status == TaskStatus.RanToCompletion; }
    }

    /// <summary>
    /// 이 Task가 취소되었는지 여부를 가져옵니다.
    /// </summary>
    public bool IsCanceled
    {
        get { return _status == TaskStatus.Canceled; }
    }

    /// <summary>
    /// 이 Task가 실패했는지 여부를 가져옵니다.
    /// </summary>
    public bool IsFaulted
    {
        get { return _status == TaskStatus.Faulted; }
    }

    /// <summary>
    /// 이 Task를 실패하게 만든 예외를 가져옵니다.
    /// </summary>
    public AggregateException? Exception
    {
        get
        {
            if (_exception == null)
                return null;
            return new AggregateException(_exception);
        }
    }

    /// <summary>
    /// 이미 성공적으로 완료된 작업을 가져옵니다.
    /// </summary>
    public static Task CompletedTask
    {
        get { return FromResult<object?>(null); }
    }

    /// <summary>
    /// 새 Task 인스턴스를 초기화합니다.
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
    /// 지정된 작업을 스레드 풀에서 실행하도록 큐에 넣고 해당 작업을 나타내는 Task 개체를 반환합니다.
    /// </summary>
    public static Task Run(Action action)
    {
        return Run(action, CancellationToken.None);
    }

    /// <summary>
    /// 지정된 작업을 스레드 풀에서 실행하도록 큐에 넣고 해당 작업을 나타내는 Task 개체를 반환합니다.
    /// </summary>
    public static Task Run(Action action, CancellationToken cancellationToken)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        var task = new Task(cancellationToken);
        task.Start(() => action());
        return task;
    }

    /// <summary>
    /// 지정된 작업을 스레드 풀에서 실행하도록 큐에 넣고 함수에서 반환된 Task의 프록시를 반환합니다.
    /// </summary>
    public static Task<TResult> Run<TResult>(Func<TResult> function)
    {
        return Run(function, CancellationToken.None);
    }

    /// <summary>
    /// 지정된 작업을 스레드 풀에서 실행하도록 큐에 넣고 함수에서 반환된 Task의 프록시를 반환합니다.
    /// </summary>
    public static Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken)
    {
        if (function == null)
            throw new ArgumentNullException(nameof(function));

        var task = new Task<TResult>(cancellationToken);
        task.Start(() => function());
        return task;
    }

    /// <summary>
    /// 지정된 결과로 성공적으로 완료된 Task를 만듭니다.
    /// </summary>
    public static Task<TResult> FromResult<TResult>(TResult result)
    {
        var task = new Task<TResult>(CancellationToken.None);
        task.SetResult(result);
        return task;
    }

    /// <summary>
    /// 지정된 CancellationToken으로 취소로 인해 완료된 Task를 만듭니다.
    /// </summary>
    public static Task FromCanceled(CancellationToken cancellationToken)
    {
        var task = new Task(cancellationToken);
        task.SetCanceled();
        return task;
    }

    /// <summary>
    /// 지정된 예외와 함께 예외적으로 완료된 Task를 만듭니다.
    /// </summary>
    public static Task FromException(Exception exception)
    {
        var task = new Task(CancellationToken.None);
        task.SetException(exception);
        return task;
    }

    /// <summary>
    /// 지정된 예외와 함께 예외적으로 완료된 Task를 만듭니다.
    /// </summary>
    public static Task<TResult> FromException<TResult>(Exception exception)
    {
        var task = new Task<TResult>(CancellationToken.None);
        task.SetException(exception);
        return task;
    }

    /// <summary>
    /// 대상 Task가 완료될 때 실행되는 연속 작업을 만듭니다.
    /// </summary>
    public Task ContinueWith(Action<Task> continuationAction)
    {
        if (continuationAction == null)
            throw new ArgumentNullException(nameof(continuationAction));

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
    /// 대상 Task가 완료될 때 실행되는 연속 작업을 만듭니다.
    /// </summary>
    public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction)
    {
        if (continuationFunction == null)
            throw new ArgumentNullException(nameof(continuationFunction));

        var continuationTask = new Task<TResult>();

        Action<Task> continuation = t =>
        {
            try
            {
                var result = continuationFunction(t);
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

    /// <summary>
    /// Task가 실행을 완료할 때까지 기다립니다.
    /// </summary>
    public void Wait()
    {
        Wait(Timeout.Infinite, CancellationToken.None);
    }

    /// <summary>
    /// 지정된 시간 간격 내에 Task가 실행을 완료할 때까지 기다립니다.
    /// </summary>
    public bool Wait(int millisecondsTimeout)
    {
        return Wait(millisecondsTimeout, CancellationToken.None);
    }

    /// <summary>
    /// Task가 실행을 완료할 때까지 기다립니다.
    /// </summary>
    public void Wait(CancellationToken cancellationToken)
    {
        Wait(Timeout.Infinite, cancellationToken);
    }

    /// <summary>
    /// 지정된 시간 간격 내에 Task가 실행을 완료할 때까지 기다립니다.
    /// </summary>
    public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
    {
        if (IsCompleted)
        {
            if (_exception != null)
                throw new AggregateException(_exception);
            return true;
        }

        var completionEvent = GetCompletionEvent();

        var waitHandles = cancellationToken.CanBeCanceled
            ? new[] { completionEvent, cancellationToken.WaitHandle }
            : new[] { completionEvent };

        var index = WaitHandle.WaitAny(waitHandles, millisecondsTimeout);

        if (index == WaitHandle.WaitTimeout)
            return false;

        if (cancellationToken.CanBeCanceled && index == 1)
            throw new OperationCanceledException("The operation was canceled.");

        if (_exception != null)
            throw new AggregateException(_exception);

        return true;
    }

    /// <summary>
    /// 제공된 모든 Task 개체가 실행을 완료할 때까지 기다립니다.
    /// </summary>
    public static void WaitAll(params Task[] tasks)
    {
        if (tasks == null)
            throw new ArgumentNullException(nameof(tasks));

        foreach (var task in tasks)
        {
            if (task == null)
                throw new ArgumentException("Task array contains null element", nameof(tasks));
            task.Wait();
        }
    }

    /// <summary>
    /// 제공된 Task 개체 중 하나가 실행을 완료할 때까지 기다립니다.
    /// </summary>
    public static int WaitAny(params Task[] tasks)
    {
        if (tasks == null)
            throw new ArgumentNullException(nameof(tasks));
        if (tasks.Length == 0)
            throw new ArgumentException("Task array is empty", nameof(tasks));

        var handles = new WaitHandle[tasks.Length];
        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i] == null)
                throw new ArgumentException("Task array contains null element", nameof(tasks));
            handles[i] = tasks[i].GetCompletionEvent();
        }

        return WaitHandle.WaitAny(handles);
    }

    /// <summary>
    /// 제공된 모든 작업이 완료되면 완료되는 작업을 만듭니다.
    /// </summary>
    public static Task WhenAll(params Task[] tasks)
    {
        if (tasks == null)
            throw new ArgumentNullException(nameof(tasks));

        return Run(() =>
        {
            foreach (var task in tasks)
            {
                if (task == null)
                    throw new ArgumentException("Task array contains null element", nameof(tasks));
                task.Wait();
            }
        });
    }

    /// <summary>
    /// 제공된 모든 작업이 완료되면 완료되는 작업을 만듭니다.
    /// </summary>
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
                    throw new ArgumentException("Task array contains null element", nameof(tasks));
                results[i] = tasks[i].Result;
            }
            return results;
        });
    }

    /// <summary>
    /// 제공된 작업 중 하나가 완료되면 완료되는 작업을 만듭니다.
    /// </summary>
    public static Task<Task> WhenAny(params Task[] tasks)
    {
        if (tasks == null)
            throw new ArgumentNullException(nameof(tasks));
        if (tasks.Length == 0)
            throw new ArgumentException("Task array is empty", nameof(tasks));

        return Run(() =>
        {
            var index = WaitAny(tasks);
            return tasks[index];
        });
    }

    /// <summary>
    /// 시간 지연 후에 완료되는 Task를 만듭니다.
    /// </summary>
    public static Task Delay(int millisecondsDelay)
    {
        return Delay(millisecondsDelay, CancellationToken.None);
    }

    /// <summary>
    /// 시간 지연 후에 완료되는 Task를 만듭니다.
    /// </summary>
    public static Task Delay(int millisecondsDelay, CancellationToken cancellationToken)
    {
        if (millisecondsDelay < -1)
            throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

        var task = new Task(cancellationToken);

        if (millisecondsDelay == 0)
        {
            task.SetResult();
            return task;
        }

        var timer = new Timer(_ =>
        {
            if (cancellationToken.IsCancellationRequested)
                task.SetCanceled();
            else
                task.SetResult();
        }, null, millisecondsDelay, Timeout.Infinite);

        task.ContinueWith(_ => timer.Dispose());

        return task;
    }

    internal void Start(Action action)
    {
        lock (_lock)
        {
            if (_status != TaskStatus.Created)
                throw new InvalidOperationException("Task already started");

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
    /// Task를 성공적으로 완료된 것으로 표시합니다.
    /// </summary>
    public void SetResult()
    {
        lock (_lock)
        {
            if (IsCompleted)
                return;

            _status = TaskStatus.RanToCompletion;
            SignalCompletion();
        }
    }

    /// <summary>
    /// Task를 지정된 예외로 실패한 것으로 표시합니다.
    /// </summary>
    public void SetException(Exception exception)
    {
        lock (_lock)
        {
            if (IsCompleted)
                return;

            _exception = exception;
            _status = TaskStatus.Faulted;
            SignalCompletion();
        }
    }

    internal void SetCanceled()
    {
        lock (_lock)
        {
            if (IsCompleted)
                return;

            _status = TaskStatus.Canceled;
            SignalCompletion();
        }
    }

    private void SignalCompletion()
    {
        if (_completionEvent != null)
        {
            _completionEvent.Set();
        }

        var continuations = _continuations.ToArray();
        _continuations.Clear();

        foreach (var continuation in continuations)
        {
            var cont = continuation;
            ThreadPool.QueueUserWorkItem(_ => cont(this));
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

    /// <summary>
    /// Task 클래스의 현재 인스턴스에서 사용하는 모든 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
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

#if NET35 || NET40
    /// <summary>
    /// 이 Task를 await하는 데 사용되는 awaiter를 가져옵니다.
    /// </summary>
    public Runtime.CompilerServices.TaskAwaiter GetAwaiter()
    {
        return new Runtime.CompilerServices.TaskAwaiter(this);
    }
#endif
}

/// <summary>
/// 값을 반환할 수 있는 비동기 작업을 나타냅니다.
/// </summary>
public class Task<TResult> : Task
{
    private TResult _result = default!;

    /// <summary>
    /// 이 Task의 결과 값을 가져옵니다.
    /// </summary>
    public TResult Result
    {
        get
        {
            Wait();
            return _result;
        }
    }

    internal Task() : base()
    {
    }

    internal Task(CancellationToken cancellationToken) : base(cancellationToken)
    {
    }

    internal void Start(Func<TResult> function)
    {
        base.Start(() =>
        {
            _result = function();
        });
    }

    /// <summary>
    /// Task를 지정된 결과로 성공적으로 완료된 것으로 표시합니다.
    /// </summary>
    public void SetResult(TResult result)
    {
        _result = result;
        base.SetResult();
    }

#if NET35 || NET40
    /// <summary>
    /// 이 Task를 await하는 데 사용되는 awaiter를 가져옵니다.
    /// </summary>
    public new Runtime.CompilerServices.TaskAwaiter<TResult> GetAwaiter()
    {
        return new Runtime.CompilerServices.TaskAwaiter<TResult>(this);
    }
#endif
}
#endif
