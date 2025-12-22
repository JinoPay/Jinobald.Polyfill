#if !NET5_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    ///     null을 허용하는 입력에 null 값을 허용하지 않음을 지정합니다.
    ///     null을 허용하는 참조 타입의 매개변수나 속성에 null을 전달하면 안 됨을 컴파일러에 알립니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    public sealed class DisallowNullAttribute : Attribute
    {
        /// <summary>
        ///     <see cref="DisallowNullAttribute" /> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public DisallowNullAttribute()
        {
        }
    }
}

#endif
