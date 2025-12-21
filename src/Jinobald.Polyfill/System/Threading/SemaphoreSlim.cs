#if NET35
namespace System.Threading;

/// <summary>
/// Semaphore의 경량 버전입니다.
/// </summary>
public class SemaphoreSlim : IDisposable
{
    private readonly object _lock = new object();
    private Semaphore? _semaphore;
    private int _currentCount;
    private readonly int _maxCount;
    private bool _disposed;

    /// <summary>
    /// SemaphoreSlim 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public SemaphoreSlim(int initialCount, int maxCount)
    {
        if (initialCount < 0)
            throw new ArgumentOutOfRangeException(nameof(initialCount));

        if (maxCount < 1)
            throw new ArgumentOutOfRangeException(nameof(maxCount));

        if (initialCount > maxCount)
            throw new ArgumentOutOfRangeException(nameof(initialCount), "initialCount는 maxCount보다 작거나 같아야 합니다");

        _currentCount = initialCount;
        _maxCount = maxCount;
    }

    /// <summary>
    /// SemaphoreSlim 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public SemaphoreSlim(int initialCount) : this(initialCount, initialCount)
    {
    }

    /// <summary>
    /// 세마포어의 현재 개수를 가져옵니다.
    /// </summary>
    public int CurrentCount
    {
        get
        {
            ThrowIfDisposed();
            lock (_lock)
            {
                return _currentCount;
            }
        }
    }

    /// <summary>
    /// 세마포어를 획득합니다.
    /// </summary>
    public void Wait()
    {
        ThrowIfDisposed();

        lock (_lock)
        {
            while (_currentCount == 0)
            {
                Monitor.Wait(_lock);
            }

            _currentCount--;
        }
    }

    /// <summary>
    /// 세마포어를 획득하거나 지정된 시간 초과까지 대기합니다.
    /// </summary>
    public bool Wait(int millisecondsTimeout)
    {
        ThrowIfDisposed();

        if (millisecondsTimeout < -1)
            throw new ArgumentOutOfRangeException(nameof(millisecondsTimeout));

        lock (_lock)
        {
            if (_currentCount > 0)
            {
                _currentCount--;
                return true;
            }

            if (millisecondsTimeout == 0)
                return false;

            DateTime endTime = DateTime.UtcNow.AddMilliseconds(millisecondsTimeout == Timeout.Infinite ? 0 : millisecondsTimeout);

            while (_currentCount == 0)
            {
                int remainingTime = millisecondsTimeout;

                if (millisecondsTimeout != Timeout.Infinite)
                {
                    remainingTime = (int)(endTime - DateTime.UtcNow).TotalMilliseconds;
                    if (remainingTime <= 0)
                        return false;
                }

                if (!Monitor.Wait(_lock, remainingTime))
                    return false;
            }

            _currentCount--;
            return true;
        }
    }

    /// <summary>
    /// 세마포어를 획득하거나 지정된 시간 초과까지 대기합니다.
    /// </summary>
    public bool Wait(TimeSpan timeout)
    {
        ThrowIfDisposed();

        if (timeout < TimeSpan.Zero && timeout != TimeSpan.FromMilliseconds(Timeout.Infinite))
            throw new ArgumentOutOfRangeException(nameof(timeout));

        int millisecondsTimeout = timeout == TimeSpan.FromMilliseconds(Timeout.Infinite) ?
            Timeout.Infinite : (int)timeout.TotalMilliseconds;

        return Wait(millisecondsTimeout);
    }

    /// <summary>
    /// 세마포어를 해제합니다.
    /// </summary>
    public int Release()
    {
        return Release(1);
    }

    /// <summary>
    /// 세마포어를 지정된 횟수만큼 해제합니다.
    /// </summary>
    public int Release(int releaseCount)
    {
        ThrowIfDisposed();

        if (releaseCount < 1)
            throw new ArgumentOutOfRangeException(nameof(releaseCount));

        lock (_lock)
        {
            int previousCount = _currentCount;

            if (_currentCount + releaseCount > _maxCount)
                throw new SemaphoreFullException("세마포어가 가득 찼습니다");

            _currentCount += releaseCount;

            for (int i = 0; i < releaseCount; i++)
            {
                Monitor.Pulse(_lock);
            }

            return previousCount;
        }
    }

    /// <summary>
    /// SemaphoreSlim 클래스의 현재 인스턴스에서 사용하는 모든 리소스를 해제합니다.
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
            lock (_lock)
            {
                if (_semaphore != null)
                {
                    _semaphore.Close();
                    _semaphore = null;
                }
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
