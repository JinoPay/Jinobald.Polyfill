#if !NET5_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    ///     출력이 null일 수 있음을 지정합니다.
    ///     null을 허용하지 않는 참조 타입이라도 반환 값이 null일 수 있음을 컴파일러에 알립니다.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue,
        Inherited = false)]
    public sealed class MaybeNullAttribute : Attribute
    {
        /// <summary>
        ///     <see cref="MaybeNullAttribute" /> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public MaybeNullAttribute()
        {
        }
    }
}

#endif
