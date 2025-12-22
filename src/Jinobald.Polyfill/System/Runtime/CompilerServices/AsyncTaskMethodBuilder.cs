#if NET35
namespace System.Runtime.CompilerServices;

/// <summary>
///     Task를 반환하는 비동기 메서드에 대한 빌더를 나타냅니다.
/// </summary>
public struct AsyncTaskMethodBuilder
{
    private Task _task;

    /// <summary>
    ///     이 빌더에 대한 작업을 가져옵니다.
    /// </summary>
    public Task Task
    {
        get
        {
            if (_task == null)
            {
                _task = new Task();
            }

            return _task;
        }
    }

    /// <summary>
    ///     AsyncTaskMethodBuilder 클래스의 인스턴스를 만듭니다.
    /// </summary>
    public static AsyncTaskMethodBuilder Create()
    {
        return default;
    }

    /// <summary>
    ///     연결된 상태 시스템으로 빌더 실행을 시작합니다.
    /// </summary>
    public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
    {
        if (stateMachine == null)
        {
            throw new ArgumentNullException(nameof(stateMachine));
        }

        stateMachine.MoveNext();
    }

    /// <summary>
    ///     빌더를 지정된 상태 시스템과 연결합니다.
    /// </summary>
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {
        // Not used in this implementation
    }

    /// <summary>
    ///     작업을 성공적으로 완료된 것으로 표시합니다.
    /// </summary>
    public void SetResult()
    {
        Task.SetResult();
    }

    /// <summary>
    ///     작업을 실패한 것으로 표시하고 지정된 예외를 작업에 바인딩합니다.
    /// </summary>
    public void SetException(Exception exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        Task.SetException(exception);
    }

    /// <summary>
    ///     지정된 awaiter가 완료될 때 상태 시스템이 다음 작업으로 진행하도록 예약합니다.
    /// </summary>
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        awaiter.OnCompleted(stateMachine.MoveNext);
    }

    /// <summary>
    ///     지정된 awaiter가 완료될 때 상태 시스템이 다음 작업으로 진행하도록 예약합니다.
    /// </summary>
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        awaiter.OnCompleted(stateMachine.MoveNext);
    }
}

/// <summary>
///     Task&lt;TResult&gt;를 반환하는 비동기 메서드에 대한 빌더를 나타냅니다.
/// </summary>
public struct AsyncTaskMethodBuilder<TResult>
{
    private Task<TResult> _task;

    /// <summary>
    ///     이 빌더에 대한 작업을 가져옵니다.
    /// </summary>
    public Task<TResult> Task
    {
        get
        {
            if (_task == null)
            {
                _task = new Task<TResult>();
            }

            return _task;
        }
    }

    /// <summary>
    ///     AsyncTaskMethodBuilder&lt;TResult&gt; 클래스의 인스턴스를 만듭니다.
    /// </summary>
    public static AsyncTaskMethodBuilder<TResult> Create()
    {
        return default;
    }

    /// <summary>
    ///     연결된 상태 시스템으로 빌더 실행을 시작합니다.
    /// </summary>
    public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
    {
        if (stateMachine == null)
        {
            throw new ArgumentNullException(nameof(stateMachine));
        }

        stateMachine.MoveNext();
    }

    /// <summary>
    ///     빌더를 지정된 상태 시스템과 연결합니다.
    /// </summary>
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {
        // Not used in this implementation
    }

    /// <summary>
    ///     작업을 지정된 결과로 성공적으로 완료된 것으로 표시합니다.
    /// </summary>
    public void SetResult(TResult result)
    {
        Task.SetResult(result);
    }

    /// <summary>
    ///     작업을 실패한 것으로 표시하고 지정된 예외를 작업에 바인딩합니다.
    /// </summary>
    public void SetException(Exception exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        Task.SetException(exception);
    }

    /// <summary>
    ///     지정된 awaiter가 완료될 때 상태 시스템이 다음 작업으로 진행하도록 예약합니다.
    /// </summary>
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        awaiter.OnCompleted(stateMachine.MoveNext);
    }

    /// <summary>
    ///     지정된 awaiter가 완료될 때 상태 시스템이 다음 작업으로 진행하도록 예약합니다.
    /// </summary>
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        awaiter.OnCompleted(stateMachine.MoveNext);
    }
}

#endif
