#if NET35
namespace System.Threading.Tasks;

/// <summary>
/// 현재 반복과 상호 작용하는 Parallel 루프의 반복을 활성화합니다.
/// </summary>
public class ParallelLoopState
{
    private volatile bool _stopRequested;
    private volatile bool _breakRequested;
    private long? _lowestBreakIteration;
    private volatile bool _exceptionWasThrown;
    private readonly object _lock = new object();

    internal ParallelLoopState()
    {
    }

    /// <summary>
    /// 루프의 현재 반복이 Stop() 호출로 인해 중지를 요청받았는지 여부를 가져옵니다.
    /// </summary>
    public bool IsStopped => _stopRequested;

    /// <summary>
    /// 루프의 현재 반복이 Break() 호출로 인해 중단을 요청받았는지 여부를 가져옵니다.
    /// </summary>
    public bool ShouldExitCurrentIteration
    {
        get
        {
            return _stopRequested || _breakRequested || _exceptionWasThrown;
        }
    }

    /// <summary>
    /// 루프의 다른 반복에서 예외가 throw되었는지 여부를 가져옵니다.
    /// </summary>
    public bool IsExceptional => _exceptionWasThrown;

    /// <summary>
    /// Break()를 호출한 가장 낮은 반복의 인덱스를 가져옵니다.
    /// </summary>
    public long? LowestBreakIteration
    {
        get
        {
            lock (_lock)
            {
                return _lowestBreakIteration;
            }
        }
    }

    /// <summary>
    /// 현재 반복보다 큰 인덱스의 반복 실행을 가능한 빨리 중지하도록 Parallel 루프에 알립니다.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Stop() 메서드가 이미 호출된 경우
    /// </exception>
    public void Break()
    {
        if (_stopRequested)
            throw new InvalidOperationException("Stop이 이미 호출되어 Break를 호출할 수 없습니다.");

        _breakRequested = true;
    }

    /// <summary>
    /// 가능한 빨리 루프 실행을 중지하도록 Parallel 루프에 알립니다.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Break() 메서드가 이미 호출된 경우
    /// </exception>
    public void Stop()
    {
        if (_breakRequested)
            throw new InvalidOperationException("Break가 이미 호출되어 Stop을 호출할 수 없습니다.");

        _stopRequested = true;
    }

    internal void SetLowestBreakIteration(long iteration)
    {
        lock (_lock)
        {
            if (_lowestBreakIteration == null || iteration < _lowestBreakIteration)
            {
                _lowestBreakIteration = iteration;
            }
        }
    }

    internal void SetExceptionThrown()
    {
        _exceptionWasThrown = true;
    }

    internal bool IsBreakRequested => _breakRequested;
    internal bool IsStopRequested => _stopRequested;
}

#endif
