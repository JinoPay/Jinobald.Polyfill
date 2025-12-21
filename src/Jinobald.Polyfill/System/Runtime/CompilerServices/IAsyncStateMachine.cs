#if NET35 || NET40
namespace System.Runtime.CompilerServices;

/// <summary>
/// 비동기 메서드에 대해 생성된 상태 시스템을 나타냅니다.
/// </summary>
public interface IAsyncStateMachine
{
    /// <summary>
    /// 상태 시스템을 다음 상태로 이동합니다.
    /// </summary>
    void MoveNext();

    /// <summary>
    /// 힙 할당 복제본으로 상태 시스템을 구성합니다.
    /// </summary>
    void SetStateMachine(IAsyncStateMachine stateMachine);
}

#endif
