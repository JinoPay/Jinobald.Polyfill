#if NET35
namespace System.Threading.Tasks;

/// <summary>
/// 완료된 Parallel 루프 실행의 상태를 제공합니다.
/// </summary>
public struct ParallelLoopResult
{
    private readonly bool _isCompleted;
    private readonly long? _lowestBreakIteration;

    internal ParallelLoopResult(bool isCompleted, long? lowestBreakIteration)
    {
        _isCompleted = isCompleted;
        _lowestBreakIteration = lowestBreakIteration;
    }

    /// <summary>
    /// 루프가 완료될 때까지 실행되었는지 여부를 가져옵니다.
    /// 즉, 루프의 모든 반복이 실행되었고 루프가 조기 종료를 요청받지 않았음을 나타냅니다.
    /// </summary>
    public bool IsCompleted => _isCompleted;

    /// <summary>
    /// Break() 문이 호출된 가장 낮은 반복의 인덱스를 가져옵니다.
    /// </summary>
    /// <remarks>
    /// Break()가 호출되지 않은 경우 이 속성은 null을 반환합니다.
    /// </remarks>
    public long? LowestBreakIteration => _lowestBreakIteration;
}

#endif
