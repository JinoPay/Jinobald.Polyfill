#if NET35
namespace System.Threading.Tasks;

/// <summary>
/// Task 개체를 만들고 예약하기 위한 지원을 제공합니다.
/// </summary>
public class TaskFactory
{
    private readonly CancellationToken _cancellationToken;

    /// <summary>
    /// TaskFactory 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public TaskFactory()
    {
        _cancellationToken = CancellationToken.None;
    }

    /// <summary>
    /// 지정된 CancellationToken으로 TaskFactory 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public TaskFactory(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
    }

    /// <summary>
    /// Task를 만들고 시작합니다.
    /// </summary>
    public Task StartNew(Action action)
    {
        return StartNew(action, _cancellationToken);
    }

    /// <summary>
    /// Task를 만들고 시작합니다.
    /// </summary>
    public Task StartNew(Action action, CancellationToken cancellationToken)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        return Task.Run(action, cancellationToken);
    }

    /// <summary>
    /// Task를 만들고 시작합니다.
    /// </summary>
    public Task<TResult> StartNew<TResult>(Func<TResult> function)
    {
        return StartNew(function, _cancellationToken);
    }

    /// <summary>
    /// Task를 만들고 시작합니다.
    /// </summary>
    public Task<TResult> StartNew<TResult>(Func<TResult> function, CancellationToken cancellationToken)
    {
        if (function == null)
            throw new ArgumentNullException(nameof(function));

        return Task.Run(function, cancellationToken);
    }

    /// <summary>
    /// 제공된 작업 집합이 완료되면 시작되는 연속 Task를 만듭니다.
    /// </summary>
    public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction)
    {
        if (tasks == null)
            throw new ArgumentNullException(nameof(tasks));
        if (continuationAction == null)
            throw new ArgumentNullException(nameof(continuationAction));

        return Task.WhenAll(tasks).ContinueWith(_ => continuationAction(tasks));
    }

    /// <summary>
    /// 제공된 작업 집합이 완료되면 시작되는 연속 Task를 만듭니다.
    /// </summary>
    public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction)
    {
        if (tasks == null)
            throw new ArgumentNullException(nameof(tasks));
        if (continuationFunction == null)
            throw new ArgumentNullException(nameof(continuationFunction));

        return Task.WhenAll(tasks).ContinueWith(_ => continuationFunction(tasks));
    }

    /// <summary>
    /// 제공된 집합의 Task 중 하나가 완료되면 시작되는 연속 Task를 만듭니다.
    /// </summary>
    public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction)
    {
        if (tasks == null)
            throw new ArgumentNullException(nameof(tasks));
        if (continuationAction == null)
            throw new ArgumentNullException(nameof(continuationAction));

        return Task.Run(() =>
        {
            var index = Task.WaitAny(tasks);
            continuationAction(tasks[index]);
        });
    }

    /// <summary>
    /// 제공된 집합의 Task 중 하나가 완료되면 시작되는 연속 Task를 만듭니다.
    /// </summary>
    public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction)
    {
        if (tasks == null)
            throw new ArgumentNullException(nameof(tasks));
        if (continuationFunction == null)
            throw new ArgumentNullException(nameof(continuationFunction));

        return Task.Run(() =>
        {
            var index = Task.WaitAny(tasks);
            return continuationFunction(tasks[index]);
        });
    }
}

/// <summary>
/// Task&lt;TResult&gt; 개체를 만들고 예약하기 위한 지원을 제공합니다.
/// </summary>
public class TaskFactory<TResult>
{
    private readonly CancellationToken _cancellationToken;

    /// <summary>
    /// TaskFactory&lt;TResult&gt; 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public TaskFactory()
    {
        _cancellationToken = CancellationToken.None;
    }

    /// <summary>
    /// 지정된 CancellationToken으로 TaskFactory&lt;TResult&gt; 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public TaskFactory(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
    }

    /// <summary>
    /// Task&lt;TResult&gt;를 만들고 시작합니다.
    /// </summary>
    public Task<TResult> StartNew(Func<TResult> function)
    {
        return StartNew(function, _cancellationToken);
    }

    /// <summary>
    /// Task&lt;TResult&gt;를 만들고 시작합니다.
    /// </summary>
    public Task<TResult> StartNew(Func<TResult> function, CancellationToken cancellationToken)
    {
        if (function == null)
            throw new ArgumentNullException(nameof(function));

        return Task.Run(function, cancellationToken);
    }
}

#endif
