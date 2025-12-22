#if !NET5_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    ///     출력이 null이 아님을 지정합니다.
    ///     메서드가 반환되면 출력 값이 null이 아님을 컴파일러에 알립니다.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue,
        Inherited = false)]
    public sealed class NotNullAttribute : Attribute
    {
        /// <summary>
        ///     <see cref="NotNullAttribute" /> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public NotNullAttribute()
        {
        }
    }
}

#endif
