#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46
namespace System.Runtime.CompilerServices;

/// <summary>
///     완료될 때 연속 작업을 예약하는 작업을 나타냅니다.
/// </summary>
public interface INotifyCompletion
{
    /// <summary>
    ///     인스턴스가 완료될 때 호출되는 연속 작업을 예약합니다.
    /// </summary>
    void OnCompleted(Action continuation);
}

/// <summary>
///     실행 컨텍스트를 캡처하지 않고 연속 작업을 예약할 수 있는 awaiter를 나타냅니다.
/// </summary>
public interface ICriticalNotifyCompletion : INotifyCompletion
{
    /// <summary>
    ///     현재 실행 컨텍스트를 캡처하지 않고 인스턴스가 완료될 때 호출되는 연속 작업을 예약합니다.
    /// </summary>
    /// <param name="continuation">완료 시 실행할 동작입니다.</param>
    void UnsafeOnCompleted(Action continuation);
}

#endif
