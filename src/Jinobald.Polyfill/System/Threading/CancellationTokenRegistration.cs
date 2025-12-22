#if NET35
namespace System.Threading;

/// <summary>
///     CancellationToken에 등록된 콜백 대리자를 나타냅니다.
/// </summary>
public struct CancellationTokenRegistration : IDisposable
{
    private readonly CancellationTokenSource? _source;
    private readonly Action? _callback;

    internal CancellationTokenRegistration(CancellationTokenSource source, Action callback)
    {
        _source = source;
        _callback = callback;
    }

    /// <summary>
    ///     등록을 삭제하고 연결된 CancellationToken에서 대상 콜백의 등록을 취소합니다.
    /// </summary>
    public void Dispose()
    {
        if (_source != null && _callback != null)
        {
            _source.Unregister(_callback);
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is CancellationTokenRegistration)
        {
            var other = (CancellationTokenRegistration)obj;
            return _source == other._source && _callback == other._callback;
        }

        return false;
    }

    public bool Equals(CancellationTokenRegistration other)
    {
        return _source == other._source && _callback == other._callback;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        if (_source != null)
        {
            hash = (hash * 31) + _source.GetHashCode();
        }

        if (_callback != null)
        {
            hash = (hash * 31) + _callback.GetHashCode();
        }

        return hash;
    }

    public static bool operator ==(CancellationTokenRegistration left, CancellationTokenRegistration right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CancellationTokenRegistration left, CancellationTokenRegistration right)
    {
        return !left.Equals(right);
    }
}

#endif
