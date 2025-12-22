#if NET35
namespace System.Threading.Tasks;

/// <summary>
///     Task의 수명 주기에서 현재 단계를 나타냅니다.
/// </summary>
public enum TaskStatus
{
    /// <summary>
    ///     작업이 초기화되었지만 아직 예약되지 않았습니다.
    /// </summary>
    Created = 0,

    /// <summary>
    ///     작업이 내부적으로 활성화 및 예약되기를 기다리고 있습니다.
    /// </summary>
    WaitingForActivation = 1,

    /// <summary>
    ///     작업이 실행을 위해 예약되었지만 아직 실행이 시작되지 않았습니다.
    /// </summary>
    WaitingToRun = 2,

    /// <summary>
    ///     작업이 실행 중이지만 아직 완료되지 않았습니다.
    /// </summary>
    Running = 3,

    /// <summary>
    ///     작업이 실행을 완료했으며 연결된 자식 작업이 완료되기를 암시적으로 기다리고 있습니다.
    /// </summary>
    WaitingForChildrenToComplete = 4,

    /// <summary>
    ///     작업이 성공적으로 실행을 완료했습니다.
    /// </summary>
    RanToCompletion = 5,

    /// <summary>
    ///     작업이 토큰이 신호 상태에 있는 동안 자체 CancellationToken으로 OperationCanceledException을 throw하여 취소를 인정했거나 작업이 실행을 시작하기 전에 작업의
    ///     CancellationToken이 이미 신호되었습니다.
    /// </summary>
    Canceled = 6,

    /// <summary>
    ///     작업이 처리되지 않은 예외로 인해 완료되었습니다.
    /// </summary>
    Faulted = 7
}

#endif
