#if NET35
namespace System.Threading.Tasks;

/// <summary>
/// ContinueWith 또는 Task를 사용하여 만든 작업의 동작을 제어하는 옵션을 지정합니다.
/// </summary>
[Flags]
public enum TaskContinuationOptions
{
    /// <summary>
    /// 기본 동작을 사용하도록 지정합니다.
    /// </summary>
    None = 0,

    /// <summary>
    /// 오래 실행되는 거친 작업임을 힌트로 지정합니다.
    /// 이는 스케줄러가 과다 구독을 허용할 수 있음을 나타냅니다.
    /// </summary>
    LongRunning = 1,

    /// <summary>
    /// 작업이 부모에 연결되도록 지정합니다.
    /// </summary>
    AttachedToParent = 2,

    /// <summary>
    /// 연결된 자식 작업이 이 작업에 연결하는 것을 허용하지 않도록 지정합니다.
    /// </summary>
    DenyChildAttach = 4,

    /// <summary>
    /// 예약된 작업이 기본 작업 스케줄러에서 실행되도록 지정합니다.
    /// </summary>
    HideScheduler = 8,

    /// <summary>
    /// 이미 완료된 작업에 대해 연속을 동기적으로 실행하지 않도록 지정합니다.
    /// </summary>
    PreferFairness = 16,

    /// <summary>
    /// 연속 작업이 비동기적으로 예약되도록 지정합니다.
    /// </summary>
    RunContinuationsAsynchronously = 64,

    /// <summary>
    /// 선행 작업이 완료되었을 때만 연속이 예약되도록 지정합니다.
    /// </summary>
    NotOnRanToCompletion = 65536,

    /// <summary>
    /// 선행 작업이 실패했을 때만 연속이 예약되도록 지정합니다.
    /// </summary>
    NotOnFaulted = 131072,

    /// <summary>
    /// 선행 작업이 취소되었을 때만 연속이 예약되도록 지정합니다.
    /// </summary>
    NotOnCanceled = 262144,

    /// <summary>
    /// 선행 작업이 완료되었을 때만 연속이 예약되도록 지정합니다.
    /// </summary>
    OnlyOnRanToCompletion = NotOnFaulted | NotOnCanceled,

    /// <summary>
    /// 선행 작업이 실패했을 때만 연속이 예약되도록 지정합니다.
    /// </summary>
    OnlyOnFaulted = NotOnRanToCompletion | NotOnCanceled,

    /// <summary>
    /// 선행 작업이 취소되었을 때만 연속이 예약되도록 지정합니다.
    /// </summary>
    OnlyOnCanceled = NotOnRanToCompletion | NotOnFaulted,

    /// <summary>
    /// 연속 작업이 선행 작업의 스레드에서 동기적으로 실행되도록 지정합니다.
    /// </summary>
    ExecuteSynchronously = 524288,

    /// <summary>
    /// 취소된 경우 연속을 취소하도록 지정합니다.
    /// </summary>
    LazyCancellation = 1048576
}

#endif
