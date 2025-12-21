#if NET35
namespace System.Threading;

/// <summary>
/// 신호를 보낼 때까지 유효한 상태를 나타냅니다.
/// </summary>
public class CountdownEvent : IDisposable
{
    private readonly object _lock = new object();
    private int _count;
    private ManualResetEventSlim _event;
    private bool _disposed;

    /// <summary>
    /// CountdownEvent 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public CountdownEvent(int initialCount)
    {
        if (initialCount < 0)
            throw new ArgumentOutOfRangeException(nameof(initialCount));

        _count = initialCount;
        _event = new ManualResetEventSlim(initialCount == 0);
    }

    /// <summary>
    /// 남은 신호 개수를 가져옵니다.
    /// </summary>
    public int CurrentCount
    {
        get
        {
            ThrowIfDisposed();
            lock (_lock)
            {
                return _count;
            }
        }
    }

    /// <summary>
    /// 신호가 모두 수신되었는지 여부를 가져옵니다.
    /// </summary>
    public bool IsSet
    {
        get
        {
            ThrowIfDisposed();
            return _event.IsSet;
        }
    }

    /// <summary>
    /// 이벤트의 WaitHandle을 가져옵니다.
    /// </summary>
    public WaitHandle WaitHandle
    {
        get
        {
            ThrowIfDisposed();
            return _event.WaitHandle;
        }
    }

    /// <summary>
    /// 신호를 수신합니다.
    /// </summary>
    public void Signal()
    {
        Signal(1);
    }

    /// <summary>
    /// 지정된 횟수만큼 신호를 수신합니다.
    /// </summary>
    public void Signal(int signalCount)
    {
        ThrowIfDisposed();

        if (signalCount < 1)
            throw new ArgumentOutOfRangeException(nameof(signalCount));

        lock (_lock)
        {
            if (_count < signalCount)
                throw new InvalidOperationException("CountdownEvent is already at zero");

            _count -= signalCount;

            if (_count == 0)
            {
                _event.Set();
            }
        }
    }

    /// <summary>
    /// 초기 개수를 추가합니다.
    /// </summary>
    public void AddCount()
    {
        AddCount(1);
    }

    /// <summary>
    /// 지정된 개수를 초기 개수에 추가합니다.
    /// </summary>
    public void AddCount(int signalCount)
    {
        ThrowIfDisposed();

        if (signalCount < 1)
            throw new ArgumentOutOfRangeException(nameof(signalCount));

        lock (_lock)
        {
            if (IsSet)
                throw new InvalidOperationException("CountdownEvent is already at zero");

            _count += signalCount;
        }
    }

    /// <summary>
    /// 신호가 모두 수신될 때까지 대기합니다.
    /// </summary>
    public void Wait()
    {
        ThrowIfDisposed();
        _event.Wait();
    }

    /// <summary>
    /// 신호가 모두 수신될 때까지 대기하거나 지정된 시간 초과까지 대기합니다.
    /// </summary>
    public bool Wait(int millisecondsTimeout)
    {
        ThrowIfDisposed();
        return _event.Wait(millisecondsTimeout);
    }

    /// <summary>
    /// 신호가 모두 수신될 때까지 대기하거나 지정된 시간 초과까지 대기합니다.
    /// </summary>
    public bool Wait(TimeSpan timeout)
    {
        ThrowIfDisposed();
        return _event.Wait(timeout);
    }

    /// <summary>
    /// CountdownEvent 클래스의 현재 인스턴스에서 사용하는 모든 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            if (_event != null)
            {
                _event.Dispose();
            }
        }

        _disposed = true;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().FullName);
    }
}

#endif
