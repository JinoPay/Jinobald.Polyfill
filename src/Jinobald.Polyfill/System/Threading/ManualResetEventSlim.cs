#if NET35
namespace System.Threading;

/// <summary>
///     ManualResetEvent의 경량 버전입니다.
/// </summary>
public class ManualResetEventSlim : IDisposable
{
    private readonly object _lock = new();
    private bool _disposed;
    private volatile bool _isSet;
    private int _refCount;
    private ManualResetEvent? _event;

    /// <summary>
    ///     ManualResetEventSlim 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public ManualResetEventSlim(bool initialState = false)
    {
        _isSet = initialState;
        _refCount = 1;
    }

    /// <summary>
    ///     이벤트가 신호를 받은 상태인지 여부를 가져옵니다.
    /// </summary>
    public bool IsSet
    {
        get
        {
            ThrowIfDisposed();
            return _isSet;
        }
    }

    /// <summary>
    ///     이벤트의 WaitHandle을 가져옵니다.
    /// </summary>
    public WaitHandle WaitHandle
    {
        get
        {
            ThrowIfDisposed();
            lock (_lock)
            {
                if (_event == null)
                {
                    _event = new ManualResetEvent(_isSet);
                }

                return _event;
            }
        }
    }

    /// <summary>
    ///     ManualResetEventSlim 클래스의 현재 인스턴스에서 사용하는 모든 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     이벤트가 신호를 받을 때까지 대기합니다.
    /// </summary>
    public bool Wait(int millisecondsTimeout)
    {
        ThrowIfDisposed();

        if (_isSet)
        {
            return true;
        }

        return WaitHandle.WaitOne(millisecondsTimeout, false);
    }

    /// <summary>
    ///     이벤트가 신호를 받을 때까지 대기합니다.
    /// </summary>
    public bool Wait(TimeSpan timeout)
    {
        ThrowIfDisposed();

        if (timeout < TimeSpan.Zero && timeout != TimeSpan.FromMilliseconds(Timeout.Infinite))
        {
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        int millisecondsTimeout = timeout == TimeSpan.FromMilliseconds(Timeout.Infinite)
            ? Timeout.Infinite
            : (int)timeout.TotalMilliseconds;

        return Wait(millisecondsTimeout);
    }

    /// <summary>
    ///     이벤트를 신호하지 않은 상태로 재설정합니다.
    /// </summary>
    public void Reset()
    {
        ThrowIfDisposed();

        lock (_lock)
        {
            if (_isSet)
            {
                _isSet = false;
                if (_event != null)
                {
                    _event.Reset();
                }
            }
        }
    }

    /// <summary>
    ///     이벤트를 신호 상태로 설정합니다.
    /// </summary>
    public void Set()
    {
        ThrowIfDisposed();

        lock (_lock)
        {
            if (!_isSet)
            {
                _isSet = true;
                if (_event != null)
                {
                    _event.Set();
                }
            }
        }
    }

    /// <summary>
    ///     이벤트가 신호를 받을 때까지 무한정 대기합니다.
    /// </summary>
    public void Wait()
    {
        ThrowIfDisposed();

        if (_isSet)
        {
            return;
        }

        WaitHandle.WaitOne();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            lock (_lock)
            {
                if (_event != null)
                {
                    _event.Close();
                    _event = null;
                }
            }
        }

        _disposed = true;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }
}

#endif
