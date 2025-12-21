#if NET35 || NET40

namespace System.Runtime.CompilerServices;

using System.Threading.Tasks;

/// <summary>
/// Task에 대한 awaiter를 제공합니다.
/// </summary>
public struct TaskAwaiter : INotifyCompletion
{
    private readonly Task _task;

    internal TaskAwaiter(Task task)
    {
        _task = task;
    }

    /// <summary>
    /// 대기 중인 작업이 완료되었는지 여부를 가져옵니다.
    /// </summary>
    public bool IsCompleted
    {
        get { return _task.IsCompleted; }
    }

    /// <summary>
    /// 완료된 작업에 대한 await를 종료합니다.
    /// </summary>
    public void GetResult()
    {
        _task.Wait();
    }

    /// <summary>
    /// 이 awaiter와 연결된 작업에 연속 작업을 예약합니다.
    /// </summary>
    public void OnCompleted(Action continuation)
    {
        if (continuation == null)
            throw new ArgumentNullException(nameof(continuation));

        _task.ContinueWith(_ => continuation());
    }
}

/// <summary>
/// Task&lt;TResult&gt;에 대한 awaiter를 제공합니다.
/// </summary>
public struct TaskAwaiter<TResult> : INotifyCompletion
{
    private readonly Task<TResult> _task;

    internal TaskAwaiter(Task<TResult> task)
    {
        _task = task;
    }

    /// <summary>
    /// 대기 중인 작업이 완료되었는지 여부를 가져옵니다.
    /// </summary>
    public bool IsCompleted
    {
        get { return _task.IsCompleted; }
    }

    /// <summary>
    /// 완료된 작업에 대한 await를 종료합니다.
    /// </summary>
    public TResult GetResult()
    {
        return _task.Result;
    }

    /// <summary>
    /// 이 awaiter와 연결된 작업에 연속 작업을 예약합니다.
    /// </summary>
    public void OnCompleted(Action continuation)
    {
        if (continuation == null)
            throw new ArgumentNullException(nameof(continuation));

        _task.ContinueWith(_ => continuation());
    }
}

#endif
