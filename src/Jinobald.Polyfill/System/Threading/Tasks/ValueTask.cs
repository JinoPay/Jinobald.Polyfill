#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks;

/// <summary>
///     ValueTask 내부에서 사용하기 위한 캐시된 완료 Task 및 헬퍼 메서드입니다.
/// </summary>
internal static class TaskHelpers
{
    /// <summary>
    ///     완료된 Task 인스턴스입니다.
    /// </summary>
    internal static readonly Task CompletedTask = CreateCompletedTask();

    private static Task CreateCompletedTask()
    {
#if NET35
        return Task.CompletedTask;
#else
        return FromResult<object?>(null);
#endif
    }

    /// <summary>
    ///     지정된 결과로 완료된 Task를 만듭니다.
    /// </summary>
    internal static Task<TResult> FromResult<TResult>(TResult result)
    {
#if NET46
        return Task.FromResult(result);
#else
        var tcs = new TaskCompletionSource<TResult>();
        tcs.SetResult(result);
        return tcs.Task;
#endif
    }

    /// <summary>
    ///     지정된 예외로 완료된 Task를 만듭니다.
    /// </summary>
    internal static Task FromException(Exception exception)
    {
#if NET46
        return Task.FromException(exception);
#else
        var tcs = new TaskCompletionSource<object?>();
        tcs.SetException(exception);
        return tcs.Task;
#endif
    }

    /// <summary>
    ///     지정된 예외로 완료된 Task를 만듭니다.
    /// </summary>
    internal static Task<TResult> FromException<TResult>(Exception exception)
    {
#if NET46
        return Task.FromException<TResult>(exception);
#else
        var tcs = new TaskCompletionSource<TResult>();
        tcs.SetException(exception);
        return tcs.Task;
#endif
    }

    /// <summary>
    ///     취소된 Task를 만듭니다.
    /// </summary>
    internal static Task FromCanceled(CancellationToken cancellationToken)
    {
#if NET46
        return Task.FromCanceled(cancellationToken);
#else
        var tcs = new TaskCompletionSource<object?>();
        tcs.SetCanceled();
        return tcs.Task;
#endif
    }

    /// <summary>
    ///     취소된 Task를 만듭니다.
    /// </summary>
    internal static Task<TResult> FromCanceled<TResult>(CancellationToken cancellationToken)
    {
#if NET46
        return Task.FromCanceled<TResult>(cancellationToken);
#else
        var tcs = new TaskCompletionSource<TResult>();
        tcs.SetCanceled();
        return tcs.Task;
#endif
    }
}

/// <summary>
///     비동기 작업의 결과를 나타내거나, 결과를 사용할 수 없는 경우 Task의 래퍼를 제공합니다.
///     이 구조체는 동기적으로 완료되는 작업에서 할당을 줄이기 위해 사용됩니다.
/// </summary>
/// <typeparam name="TResult">결과 형식입니다.</typeparam>
public readonly struct ValueTask<TResult> : IEquatable<ValueTask<TResult>>
{
    private readonly Task<TResult>? _task;
    private readonly TResult _result;
    private readonly bool _hasResult;

    /// <summary>
    ///     지정된 결과를 사용하여 성공적으로 완료된 작업을 만듭니다.
    /// </summary>
    /// <param name="result">결과 값입니다.</param>
    public ValueTask(TResult result)
    {
        _result = result;
        _task = null;
        _hasResult = true;
    }

    /// <summary>
    ///     지정된 Task를 래핑하는 작업을 만듭니다.
    /// </summary>
    /// <param name="task">래핑할 Task입니다.</param>
    public ValueTask(Task<TResult> task)
    {
        _task = task ?? throw new ArgumentNullException(nameof(task));
        _result = default!;
        _hasResult = false;
    }

    /// <summary>
    ///     이 작업이 완료되었는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsCompleted => _hasResult || (_task?.IsCompleted ?? true);

    /// <summary>
    ///     이 작업이 성공적으로 완료되었는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsCompletedSuccessfully => _hasResult || (_task?.Status == TaskStatus.RanToCompletion);

    /// <summary>
    ///     이 작업이 실패했는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsFaulted => _task?.IsFaulted ?? false;

    /// <summary>
    ///     이 작업이 취소되었는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsCanceled => _task?.IsCanceled ?? false;

    /// <summary>
    ///     결과 값을 가져옵니다.
    /// </summary>
    public TResult Result => _hasResult ? _result : _task!.Result;

    /// <summary>
    ///     이 ValueTask를 Task로 변환합니다.
    /// </summary>
    /// <returns>이 ValueTask를 나타내는 Task입니다.</returns>
    public Task<TResult> AsTask()
    {
        if (_hasResult)
        {
            return TaskHelpers.FromResult(_result);
        }

        return _task ?? TaskHelpers.FromResult(default(TResult)!);
    }

    /// <summary>
    ///     이 작업을 구성하는 데 사용되는 awaiter를 만듭니다.
    /// </summary>
    /// <returns>awaiter 인스턴스입니다.</returns>
    public ValueTaskAwaiter<TResult> GetAwaiter()
    {
        return new ValueTaskAwaiter<TResult>(this);
    }

    /// <summary>
    ///     이 작업을 기다리도록 연속 작업을 구성합니다.
    /// </summary>
    /// <param name="continueOnCapturedContext">true이면 캡처된 컨텍스트에서 연속 작업을 실행하고, 그렇지 않으면 false입니다.</param>
    /// <returns>대기할 수 있는 개체입니다.</returns>
    public ConfiguredValueTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext)
    {
        return new ConfiguredValueTaskAwaitable<TResult>(this, continueOnCapturedContext);
    }

    /// <summary>
    ///     이 인스턴스가 지정된 ValueTask와 같은지 여부를 확인합니다.
    /// </summary>
    /// <param name="other">비교할 ValueTask입니다.</param>
    /// <returns>두 인스턴스가 같으면 true이고, 그렇지 않으면 false입니다.</returns>
    public bool Equals(ValueTask<TResult> other)
    {
        if (_hasResult && other._hasResult)
        {
            return EqualityComparer<TResult>.Default.Equals(_result, other._result);
        }

        return ReferenceEquals(_task, other._task);
    }

    /// <summary>
    ///     이 인스턴스가 지정된 개체와 같은지 여부를 확인합니다.
    /// </summary>
    /// <param name="obj">비교할 개체입니다.</param>
    /// <returns>두 인스턴스가 같으면 true이고, 그렇지 않으면 false입니다.</returns>
    public override bool Equals(object? obj)
    {
        return obj is ValueTask<TResult> other && Equals(other);
    }

    /// <summary>
    ///     이 인스턴스의 해시 코드를 반환합니다.
    /// </summary>
    /// <returns>해시 코드입니다.</returns>
    public override int GetHashCode()
    {
        if (_hasResult)
        {
            return _result?.GetHashCode() ?? 0;
        }

        return _task?.GetHashCode() ?? 0;
    }

    /// <summary>
    ///     두 ValueTask가 같은지 비교합니다.
    /// </summary>
    public static bool operator ==(ValueTask<TResult> left, ValueTask<TResult> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     두 ValueTask가 다른지 비교합니다.
    /// </summary>
    public static bool operator !=(ValueTask<TResult> left, ValueTask<TResult> right)
    {
        return !left.Equals(right);
    }
}

/// <summary>
///     비동기 작업을 나타내며 할당을 줄이기 위해 동기 작업을 지원합니다.
/// </summary>
public readonly struct ValueTask : IEquatable<ValueTask>
{
    private readonly Task? _task;
    private readonly bool _isCompleted;

    /// <summary>
    ///     성공적으로 완료된 ValueTask를 만듭니다.
    /// </summary>
    public ValueTask()
    {
        _task = null;
        _isCompleted = true;
    }

    /// <summary>
    ///     지정된 Task를 래핑하는 ValueTask를 만듭니다.
    /// </summary>
    /// <param name="task">래핑할 Task입니다.</param>
    public ValueTask(Task task)
    {
        _task = task ?? throw new ArgumentNullException(nameof(task));
        _isCompleted = false;
    }

    /// <summary>
    ///     이 작업이 완료되었는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsCompleted => _isCompleted || (_task?.IsCompleted ?? true);

    /// <summary>
    ///     이 작업이 성공적으로 완료되었는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsCompletedSuccessfully => _isCompleted || (_task?.Status == TaskStatus.RanToCompletion);

    /// <summary>
    ///     이 작업이 실패했는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsFaulted => _task?.IsFaulted ?? false;

    /// <summary>
    ///     이 작업이 취소되었는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsCanceled => _task?.IsCanceled ?? false;

    /// <summary>
    ///     이 ValueTask를 Task로 변환합니다.
    /// </summary>
    /// <returns>이 ValueTask를 나타내는 Task입니다.</returns>
    public Task AsTask()
    {
        if (_isCompleted)
        {
            return TaskHelpers.CompletedTask;
        }

        return _task ?? TaskHelpers.CompletedTask;
    }

    /// <summary>
    ///     이 작업을 구성하는 데 사용되는 awaiter를 만듭니다.
    /// </summary>
    /// <returns>awaiter 인스턴스입니다.</returns>
    public ValueTaskAwaiter GetAwaiter()
    {
        return new ValueTaskAwaiter(this);
    }

    /// <summary>
    ///     이 작업을 기다리도록 연속 작업을 구성합니다.
    /// </summary>
    /// <param name="continueOnCapturedContext">true이면 캡처된 컨텍스트에서 연속 작업을 실행하고, 그렇지 않으면 false입니다.</param>
    /// <returns>대기할 수 있는 개체입니다.</returns>
    public ConfiguredValueTaskAwaitable ConfigureAwait(bool continueOnCapturedContext)
    {
        return new ConfiguredValueTaskAwaitable(this, continueOnCapturedContext);
    }

    /// <summary>
    ///     이 인스턴스가 지정된 ValueTask와 같은지 여부를 확인합니다.
    /// </summary>
    /// <param name="other">비교할 ValueTask입니다.</param>
    /// <returns>두 인스턴스가 같으면 true이고, 그렇지 않으면 false입니다.</returns>
    public bool Equals(ValueTask other)
    {
        if (_isCompleted && other._isCompleted)
        {
            return true;
        }

        return ReferenceEquals(_task, other._task);
    }

    /// <summary>
    ///     이 인스턴스가 지정된 개체와 같은지 여부를 확인합니다.
    /// </summary>
    /// <param name="obj">비교할 개체입니다.</param>
    /// <returns>두 인스턴스가 같으면 true이고, 그렇지 않으면 false입니다.</returns>
    public override bool Equals(object? obj)
    {
        return obj is ValueTask other && Equals(other);
    }

    /// <summary>
    ///     이 인스턴스의 해시 코드를 반환합니다.
    /// </summary>
    /// <returns>해시 코드입니다.</returns>
    public override int GetHashCode()
    {
        return _task?.GetHashCode() ?? 0;
    }

    /// <summary>
    ///     두 ValueTask가 같은지 비교합니다.
    /// </summary>
    public static bool operator ==(ValueTask left, ValueTask right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     두 ValueTask가 다른지 비교합니다.
    /// </summary>
    public static bool operator !=(ValueTask left, ValueTask right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    ///     성공적으로 완료된 ValueTask를 가져옵니다.
    /// </summary>
    public static ValueTask CompletedTask => new();

    /// <summary>
    ///     지정된 예외와 함께 실패한 ValueTask를 만듭니다.
    /// </summary>
    /// <param name="exception">실패 예외입니다.</param>
    /// <returns>예외와 함께 실패한 ValueTask입니다.</returns>
    public static ValueTask FromException(Exception exception)
    {
        return new ValueTask(TaskHelpers.FromException(exception));
    }

    /// <summary>
    ///     지정된 예외와 함께 실패한 ValueTask를 만듭니다.
    /// </summary>
    /// <typeparam name="TResult">결과 형식입니다.</typeparam>
    /// <param name="exception">실패 예외입니다.</param>
    /// <returns>예외와 함께 실패한 ValueTask입니다.</returns>
    public static ValueTask<TResult> FromException<TResult>(Exception exception)
    {
        return new ValueTask<TResult>(TaskHelpers.FromException<TResult>(exception));
    }

    /// <summary>
    ///     지정된 결과와 함께 성공적으로 완료된 ValueTask를 만듭니다.
    /// </summary>
    /// <typeparam name="TResult">결과 형식입니다.</typeparam>
    /// <param name="result">결과 값입니다.</param>
    /// <returns>결과와 함께 성공적으로 완료된 ValueTask입니다.</returns>
    public static ValueTask<TResult> FromResult<TResult>(TResult result)
    {
        return new ValueTask<TResult>(result);
    }

    /// <summary>
    ///     취소된 ValueTask를 만듭니다.
    /// </summary>
    /// <param name="cancellationToken">취소 토큰입니다.</param>
    /// <returns>취소된 ValueTask입니다.</returns>
    public static ValueTask FromCanceled(CancellationToken cancellationToken)
    {
        return new ValueTask(TaskHelpers.FromCanceled(cancellationToken));
    }

    /// <summary>
    ///     취소된 ValueTask를 만듭니다.
    /// </summary>
    /// <typeparam name="TResult">결과 형식입니다.</typeparam>
    /// <param name="cancellationToken">취소 토큰입니다.</param>
    /// <returns>취소된 ValueTask입니다.</returns>
    public static ValueTask<TResult> FromCanceled<TResult>(CancellationToken cancellationToken)
    {
        return new ValueTask<TResult>(TaskHelpers.FromCanceled<TResult>(cancellationToken));
    }
}
#endif
