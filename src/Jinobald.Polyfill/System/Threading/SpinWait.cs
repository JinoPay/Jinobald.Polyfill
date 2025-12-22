#if NET35
namespace System.Threading;

/// <summary>
///     회전 대기를 지원하는 구조체입니다.
/// </summary>
public struct SpinWait
{
    private const int SpinCountThreshold = 10;

    /// <summary>
    ///     SpinWait의 회전 횟수를 가져옵니다.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    ///     다음 회전에서 yield를 수행해야 하는지 여부를 가져옵니다.
    /// </summary>
    public bool NextSpinWillYield => Count >= SpinCountThreshold;

    /// <summary>
    ///     한 번 회전합니다.
    /// </summary>
    public void SpinOnce()
    {
        if (NextSpinWillYield)
        {
#if NET46_OR_GREATER
            Thread.Yield();
#else
            Thread.Sleep(0);
#endif
        }
        else
        {
            int spinCount = 1 << Count;
            for (int i = 0; i < spinCount; i++)
            {
                Thread.SpinWait(1);
            }
        }

        Count = (Count + 1) % 33;
    }

    /// <summary>
    ///     회전을 리셋합니다.
    /// </summary>
    public void Reset()
    {
        Count = 0;
    }

    /// <summary>
    ///     조건이 true가 될 때까지 회전합니다.
    /// </summary>
    public static void SpinUntil(Func<bool> condition)
    {
        SpinUntil(condition, Timeout.Infinite);
    }

    /// <summary>
    ///     조건이 true가 될 때까지 회전합니다.
    /// </summary>
    public static bool SpinUntil(Func<bool> condition, int millisecondsTimeout)
    {
        if (condition == null)
        {
            throw new ArgumentNullException(nameof(condition));
        }

        if (millisecondsTimeout < -1)
        {
            throw new ArgumentOutOfRangeException(nameof(millisecondsTimeout));
        }

        SpinWait spinner = default;
        DateTime endTime =
            DateTime.UtcNow.AddMilliseconds(millisecondsTimeout == Timeout.Infinite ? 0 : millisecondsTimeout);

        while (!condition())
        {
            if (millisecondsTimeout != Timeout.Infinite)
            {
                TimeSpan remaining = endTime - DateTime.UtcNow;
                if (remaining.TotalMilliseconds <= 0)
                {
                    return false;
                }
            }

            spinner.SpinOnce();
        }

        return true;
    }

    /// <summary>
    ///     조건이 true가 될 때까지 회전합니다.
    /// </summary>
    public static bool SpinUntil(Func<bool> condition, TimeSpan timeout)
    {
        if (timeout < TimeSpan.Zero && timeout != TimeSpan.FromMilliseconds(Timeout.Infinite))
        {
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        int millisecondsTimeout = timeout == TimeSpan.FromMilliseconds(Timeout.Infinite)
            ? Timeout.Infinite
            : (int)timeout.TotalMilliseconds;

        return SpinUntil(condition, millisecondsTimeout);
    }
}

#endif
