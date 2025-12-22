#if NET35
namespace System.Threading.Tasks;

/// <summary>
///     작업의 생성 및 실행에 대한 선택적 동작을 제어하는 플래그를 지정합니다.
/// </summary>
[Flags]
public enum TaskCreationOptions
{
    /// <summary>
    ///     기본 동작을 사용하도록 지정합니다.
    /// </summary>
    None = 0,

    /// <summary>
    ///     오래 실행되는 거친 작업임을 힌트로 지정합니다.
    ///     이는 스케줄러가 과다 구독을 허용할 수 있음을 나타냅니다.
    /// </summary>
    LongRunning = 1,

    /// <summary>
    ///     작업이 부모에 연결되도록 지정합니다.
    ///     부모 작업이 모든 자식 작업이 완료될 때까지 최종 상태로 전환되지 않음을 의미합니다.
    /// </summary>
    AttachedToParent = 2,

    /// <summary>
    ///     연결된 자식 작업(AttachedToParent로 생성된 작업)이
    ///     이 작업에 연결하는 것을 허용하지 않도록 지정합니다.
    /// </summary>
    DenyChildAttach = 4,

    /// <summary>
    ///     기본적으로 예약된 작업이 현재 작업의 컨텍스트 대신
    ///     기본 작업 스케줄러에서 실행되도록 지정합니다.
    /// </summary>
    HideScheduler = 8,

    /// <summary>
    ///     취소 토큰이 신호될 때 예외를 throw하는 대신
    ///     작업을 취소된 상태로 전환하도록 지정합니다.
    /// </summary>
    PreferFairness = 16,

    /// <summary>
    ///     기본 스레드 풀에서 이 작업을 실행하도록 지정합니다.
    /// </summary>
    RunContinuationsAsynchronously = 64
}

#endif
