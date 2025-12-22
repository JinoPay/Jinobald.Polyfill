#if NET35
namespace System.Threading;

/// <summary>
///     CancellationToken에 취소해야 한다는 신호를 보냅니다.
/// </summary>
public class CancellationTokenSource : IDisposable
{
    private readonly object _lock = new();
    private volatile bool _cancellationRequested;
    private bool _disposed;
    private List<Action>? _callbacks;
    private ManualResetEvent? _waitHandle;
    private Timer? _timer;

    /// <summary>
    ///     CancellationTokenSource 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public CancellationTokenSource()
    {
    }

    /// <summary>
    ///     지정된 지연 후 취소되는 CancellationTokenSource 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public CancellationTokenSource(int millisecondsDelay)
    {
        if (millisecondsDelay < -1)
        {
            throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));
        }

        if (millisecondsDelay != Timeout.Infinite)
        {
            _timer = new Timer(_ => Cancel(), null, millisecondsDelay, Timeout.Infinite);
        }
    }

    /// <summary>
    ///     이 CancellationTokenSource에 대해 취소가 요청되었는지 여부를 가져옵니다.
    /// </summary>
    public bool IsCancellationRequested => _cancellationRequested;

    /// <summary>
    ///     이 CancellationTokenSource와 연결된 CancellationToken을 가져옵니다.
    /// </summary>
    public CancellationToken Token
    {
        get
        {
            ThrowIfDisposed();
            return new CancellationToken(this);
        }
    }

    internal WaitHandle WaitHandle
    {
        get
        {
            ThrowIfDisposed();
            lock (_lock)
            {
                if (_waitHandle == null)
                {
                    _waitHandle = new ManualResetEvent(_cancellationRequested);
                }

                return _waitHandle;
            }
        }
    }

    /// <summary>
    ///     CancellationTokenSource 클래스의 현재 인스턴스에서 사용하는 모든 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     원본 토큰 중 하나라도 취소된 상태일 때 취소된 상태가 되는 CancellationTokenSource를 만듭니다.
    /// </summary>
    public static CancellationTokenSource CreateLinkedTokenSource(CancellationToken token1, CancellationToken token2)
    {
        var cts = new CancellationTokenSource();

        if (token1.CanBeCanceled)
        {
            token1.Register(() => cts.Cancel());
        }

        if (token2.CanBeCanceled)
        {
            token2.Register(() => cts.Cancel());
        }

        return cts;
    }

    /// <summary>
    ///     원본 토큰 중 하나라도 취소된 상태일 때 취소된 상태가 되는 CancellationTokenSource를 만듭니다.
    /// </summary>
    public static CancellationTokenSource CreateLinkedTokenSource(params CancellationToken[] tokens)
    {
        if (tokens == null)
        {
            throw new ArgumentNullException(nameof(tokens));
        }

        var cts = new CancellationTokenSource();

        foreach (CancellationToken token in tokens)
        {
            if (token.CanBeCanceled)
            {
                token.Register(() => cts.Cancel());
            }
        }

        return cts;
    }

    /// <summary>
    ///     취소 요청을 전달합니다.
    /// </summary>
    public void Cancel()
    {
        Cancel(false);
    }

    /// <summary>
    ///     취소 요청을 전달합니다.
    /// </summary>
    public void Cancel(bool throwOnFirstException)
    {
        ThrowIfDisposed();

        if (_cancellationRequested)
        {
            return;
        }

        lock (_lock)
        {
            if (_cancellationRequested)
            {
                return;
            }

            _cancellationRequested = true;

            if (_waitHandle != null)
            {
                _waitHandle.Set();
            }

            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        ExecuteCallbacks(throwOnFirstException);
    }

    /// <summary>
    ///     지정된 시간 범위 후에 이 CancellationTokenSource에 대한 취소 작업을 예약합니다.
    /// </summary>
    public void CancelAfter(int millisecondsDelay)
    {
        ThrowIfDisposed();

        if (millisecondsDelay < -1)
        {
            throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));
        }

        if (_cancellationRequested)
        {
            return;
        }

        lock (_lock)
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }

            if (millisecondsDelay != Timeout.Infinite)
            {
                _timer = new Timer(_ => Cancel(), null, millisecondsDelay, Timeout.Infinite);
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }

            if (_waitHandle != null)
            {
                _waitHandle.Close();
                _waitHandle = null;
            }

            _callbacks = null;
        }

        _disposed = true;
    }

    private void ExecuteCallbacks(bool throwOnFirstException)
    {
        List<Action>? callbacks;
        lock (_lock)
        {
            if (_callbacks == null || _callbacks.Count == 0)
            {
                return;
            }

            callbacks = new List<Action>(_callbacks);
            _callbacks.Clear();
        }

        List<Exception>? exceptions = null;

        foreach (Action? callback in callbacks)
        {
            try
            {
                callback();
            }
            catch (Exception ex)
            {
                if (throwOnFirstException)
                {
                    throw;
                }

                if (exceptions == null)
                {
                    exceptions = new List<Exception>();
                }

                exceptions.Add(ex);
            }
        }

        if (exceptions != null && exceptions.Count > 0)
        {
            throw new AggregateException(exceptions);
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    internal CancellationTokenRegistration Register(Action callback)
    {
        ThrowIfDisposed();

        if (_cancellationRequested)
        {
            callback();
            return default;
        }

        lock (_lock)
        {
            if (_callbacks == null)
            {
                _callbacks = new List<Action>();
            }

            _callbacks.Add(callback);
        }

        return new CancellationTokenRegistration(this, callback);
    }

    internal CancellationTokenRegistration Register(Action<object?> callback, object? state)
    {
        return Register(() => callback(state));
    }

    internal void Unregister(Action callback)
    {
        if (_disposed)
        {
            return;
        }

        lock (_lock)
        {
            if (_callbacks != null)
            {
                _callbacks.Remove(callback);
            }
        }
    }
}

#endif
