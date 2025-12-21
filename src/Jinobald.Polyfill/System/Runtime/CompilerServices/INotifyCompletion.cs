#if NET35 || NET40
namespace System.Runtime.CompilerServices;

/// <summary>
/// 완료될 때 연속 작업을 예약하는 작업을 나타냅니다.
/// </summary>
public interface INotifyCompletion
{
    /// <summary>
    /// 인스턴스가 완료될 때 호출되는 연속 작업을 예약합니다.
    /// </summary>
    void OnCompleted(Action continuation);
}

#endif
