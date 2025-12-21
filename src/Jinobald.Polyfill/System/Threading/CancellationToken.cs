#if NET35
namespace System.Threading;

/// <summary>
/// 작업을 취소해야 한다는 알림을 전파합니다.
/// </summary>
public struct CancellationToken
{
    private readonly CancellationTokenSource? _source;

    /// <summary>
    /// 빈 CancellationToken 값을 반환합니다.
    /// </summary>
    public static CancellationToken None
    {
        get { return new CancellationToken(); }
    }

    /// <summary>
    /// 이 토큰에 대해 취소가 요청되었는지 여부를 가져옵니다.
    /// </summary>
    public bool IsCancellationRequested
    {
        get { return _source != null && _source.IsCancellationRequested; }
    }

    /// <summary>
    /// 이 토큰이 취소된 상태가 될 수 있는지 여부를 가져옵니다.
    /// </summary>
    public bool CanBeCanceled
    {
        get { return _source != null; }
    }

    /// <summary>
    /// 토큰이 취소될 때 신호를 받는 WaitHandle을 가져옵니다.
    /// </summary>
    public WaitHandle WaitHandle
    {
        get
        {
            if (_source == null)
                return new ManualResetEvent(false);
            return _source.WaitHandle;
        }
    }

    internal CancellationToken(CancellationTokenSource source)
    {
        _source = source;
    }

    /// <summary>
    /// 이 토큰에 대해 취소가 요청된 경우 OperationCanceledException을 throw합니다.
    /// </summary>
    public void ThrowIfCancellationRequested()
    {
        if (IsCancellationRequested)
            throw new OperationCanceledException("The operation was canceled.");
    }

    /// <summary>
    /// 이 CancellationToken이 취소될 때 호출될 대리자를 등록합니다.
    /// </summary>
    public CancellationTokenRegistration Register(Action callback)
    {
        if (callback == null)
            throw new ArgumentNullException(nameof(callback));

        if (_source == null)
            return new CancellationTokenRegistration();

        return _source.Register(callback);
    }

    /// <summary>
    /// 이 CancellationToken이 취소될 때 호출될 대리자를 등록합니다.
    /// </summary>
    public CancellationTokenRegistration Register(Action<object?> callback, object? state)
    {
        if (callback == null)
            throw new ArgumentNullException(nameof(callback));

        if (_source == null)
            return new CancellationTokenRegistration();

        return _source.Register(callback, state);
    }

    public override bool Equals(object? obj)
    {
        if (obj is CancellationToken)
        {
            var other = (CancellationToken)obj;
            return _source == other._source;
        }
        return false;
    }

    public bool Equals(CancellationToken other)
    {
        return _source == other._source;
    }

    public override int GetHashCode()
    {
        return _source != null ? _source.GetHashCode() : 0;
    }

    public static bool operator ==(CancellationToken left, CancellationToken right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CancellationToken left, CancellationToken right)
    {
        return !left.Equals(right);
    }
}

#endif
