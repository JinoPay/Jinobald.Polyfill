#if NET35
namespace System.Threading.Tasks;

/// <summary>
///     수동으로 제어할 수 있는 Task의 producer 측을 나타냅니다.
/// </summary>
/// <typeparam name="TResult">이 TaskCompletionSource와 연결된 Task의 결과 형식입니다.</typeparam>
public class TaskCompletionSource<TResult>
{
    private readonly object _lock = new();
    private readonly Task<TResult> _task;

    /// <summary>
    ///     TaskCompletionSource 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public TaskCompletionSource()
    {
        _task = new Task<TResult>(CancellationToken.None);
    }

    /// <summary>
    ///     지정된 상태로 TaskCompletionSource 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="state">기본 Task에 연결할 상태 개체입니다.</param>
    public TaskCompletionSource(object? state)
    {
        _task = new Task<TResult>(CancellationToken.None);
    }

    /// <summary>
    ///     지정된 생성 옵션으로 TaskCompletionSource 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="creationOptions">기본 Task를 만드는 데 사용할 옵션입니다.</param>
    public TaskCompletionSource(TaskCreationOptions creationOptions)
    {
        _task = new Task<TResult>(CancellationToken.None);
    }

    /// <summary>
    ///     지정된 상태와 생성 옵션으로 TaskCompletionSource 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="state">기본 Task에 연결할 상태 개체입니다.</param>
    /// <param name="creationOptions">기본 Task를 만드는 데 사용할 옵션입니다.</param>
    public TaskCompletionSource(object? state, TaskCreationOptions creationOptions)
    {
        _task = new Task<TResult>(CancellationToken.None);
    }

    /// <summary>
    ///     이 TaskCompletionSource에 의해 만들어진 Task를 가져옵니다.
    /// </summary>
    public Task<TResult> Task => _task;

    /// <summary>
    ///     기본 Task를 Canceled 상태로 전환합니다.
    /// </summary>
    /// <exception cref="InvalidOperationException">기본 Task가 이미 완료 상태 중 하나에 있는 경우.</exception>
    public void SetCanceled()
    {
        if (!TrySetCanceled())
        {
            throw new InvalidOperationException("Task가 이미 완료되었습니다.");
        }
    }

    /// <summary>
    ///     기본 Task를 지정된 CancellationToken을 사용하여 Canceled 상태로 전환합니다.
    /// </summary>
    /// <param name="cancellationToken">취소에 사용된 CancellationToken입니다.</param>
    /// <exception cref="InvalidOperationException">기본 Task가 이미 완료 상태 중 하나에 있는 경우.</exception>
    public void SetCanceled(CancellationToken cancellationToken)
    {
        if (!TrySetCanceled(cancellationToken))
        {
            throw new InvalidOperationException("Task가 이미 완료되었습니다.");
        }
    }

    /// <summary>
    ///     기본 Task를 지정된 예외와 함께 Faulted 상태로 전환합니다.
    /// </summary>
    /// <param name="exception">Task에 바인딩할 예외입니다.</param>
    /// <exception cref="ArgumentNullException">exception이 null인 경우.</exception>
    /// <exception cref="InvalidOperationException">기본 Task가 이미 완료 상태 중 하나에 있는 경우.</exception>
    public void SetException(Exception exception)
    {
        if (!TrySetException(exception))
        {
            throw new InvalidOperationException("Task가 이미 완료되었습니다.");
        }
    }

    /// <summary>
    ///     기본 Task를 지정된 예외 컬렉션과 함께 Faulted 상태로 전환합니다.
    /// </summary>
    /// <param name="exceptions">Task에 바인딩할 예외 컬렉션입니다.</param>
    /// <exception cref="ArgumentNullException">exceptions가 null인 경우.</exception>
    /// <exception cref="ArgumentException">exceptions 컬렉션에 null 요소가 있거나 비어 있는 경우.</exception>
    /// <exception cref="InvalidOperationException">기본 Task가 이미 완료 상태 중 하나에 있는 경우.</exception>
    public void SetException(IEnumerable<Exception> exceptions)
    {
        if (!TrySetException(exceptions))
        {
            throw new InvalidOperationException("Task가 이미 완료되었습니다.");
        }
    }

    /// <summary>
    ///     기본 Task를 지정된 결과와 함께 RanToCompletion 상태로 전환합니다.
    /// </summary>
    /// <param name="result">Task에 바인딩할 결과 값입니다.</param>
    /// <exception cref="InvalidOperationException">기본 Task가 이미 완료 상태 중 하나에 있는 경우.</exception>
    public void SetResult(TResult result)
    {
        if (!TrySetResult(result))
        {
            throw new InvalidOperationException("Task가 이미 완료되었습니다.");
        }
    }

    /// <summary>
    ///     기본 Task를 Canceled 상태로 전환하려고 시도합니다.
    /// </summary>
    /// <returns>작업이 성공적으로 완료되면 true이고, 그렇지 않으면 false입니다.</returns>
    public bool TrySetCanceled()
    {
        return TrySetCanceled(CancellationToken.None);
    }

    /// <summary>
    ///     기본 Task를 지정된 CancellationToken을 사용하여 Canceled 상태로 전환하려고 시도합니다.
    /// </summary>
    /// <param name="cancellationToken">취소에 사용된 CancellationToken입니다.</param>
    /// <returns>작업이 성공적으로 완료되면 true이고, 그렇지 않으면 false입니다.</returns>
    public bool TrySetCanceled(CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            if (_task.IsCompleted)
            {
                return false;
            }

            _task.SetCanceled();
            return true;
        }
    }

    /// <summary>
    ///     기본 Task를 지정된 예외와 함께 Faulted 상태로 전환하려고 시도합니다.
    /// </summary>
    /// <param name="exception">Task에 바인딩할 예외입니다.</param>
    /// <returns>작업이 성공적으로 완료되면 true이고, 그렇지 않으면 false입니다.</returns>
    /// <exception cref="ArgumentNullException">exception이 null인 경우.</exception>
    public bool TrySetException(Exception exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        lock (_lock)
        {
            if (_task.IsCompleted)
            {
                return false;
            }

            _task.SetException(exception);
            return true;
        }
    }

    /// <summary>
    ///     기본 Task를 지정된 예외 컬렉션과 함께 Faulted 상태로 전환하려고 시도합니다.
    /// </summary>
    /// <param name="exceptions">Task에 바인딩할 예외 컬렉션입니다.</param>
    /// <returns>작업이 성공적으로 완료되면 true이고, 그렇지 않으면 false입니다.</returns>
    /// <exception cref="ArgumentNullException">exceptions가 null인 경우.</exception>
    /// <exception cref="ArgumentException">exceptions 컬렉션에 null 요소가 있거나 비어 있는 경우.</exception>
    public bool TrySetException(IEnumerable<Exception> exceptions)
    {
        if (exceptions == null)
        {
            throw new ArgumentNullException(nameof(exceptions));
        }

        var exceptionList = new List<Exception>();
        foreach (var ex in exceptions)
        {
            if (ex == null)
            {
                throw new ArgumentException("예외 컬렉션에 null 요소가 포함되어 있습니다.", nameof(exceptions));
            }

            exceptionList.Add(ex);
        }

        if (exceptionList.Count == 0)
        {
            throw new ArgumentException("예외 컬렉션이 비어 있습니다.", nameof(exceptions));
        }

        lock (_lock)
        {
            if (_task.IsCompleted)
            {
                return false;
            }

            _task.SetException(new AggregateException(exceptionList));
            return true;
        }
    }

    /// <summary>
    ///     기본 Task를 지정된 결과와 함께 RanToCompletion 상태로 전환하려고 시도합니다.
    /// </summary>
    /// <param name="result">Task에 바인딩할 결과 값입니다.</param>
    /// <returns>작업이 성공적으로 완료되면 true이고, 그렇지 않으면 false입니다.</returns>
    public bool TrySetResult(TResult result)
    {
        lock (_lock)
        {
            if (_task.IsCompleted)
            {
                return false;
            }

            _task.SetResult(result);
            return true;
        }
    }
}
#endif
