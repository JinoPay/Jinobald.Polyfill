#if NET35 || NET40
namespace System.Runtime.CompilerServices;

/// <summary>
///     비동기 메서드의 반환 형식으로 사용될 때 특성이 지정된 형식을 빌드하기 위해 언어 컴파일러에서 사용해야 하는 비동기 메서드 빌더의 형식을 나타냅니다.
/// </summary>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Delegate |
    AttributeTargets.Enum, Inherited
        = false, AllowMultiple = false)]
public sealed class AsyncMethodBuilderAttribute : Attribute
{
    /// <summary>
    ///     AsyncMethodBuilderAttribute 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public AsyncMethodBuilderAttribute(Type builderType)
    {
        BuilderType = builderType;
    }

    /// <summary>
    ///     연결된 빌더의 형식을 가져옵니다.
    /// </summary>
    public Type BuilderType { get; }
}

#endif
