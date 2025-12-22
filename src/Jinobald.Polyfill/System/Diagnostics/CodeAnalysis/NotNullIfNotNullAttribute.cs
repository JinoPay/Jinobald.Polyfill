#if !NET5_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    ///     지정된 매개변수가 null이 아니면 반환 값이 null이 아님을 지정합니다.
    ///     매개변수 간의 null 상태 관계를 표현합니다.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue,
        AllowMultiple = true,
        Inherited = false)]
    public sealed class NotNullIfNotNullAttribute : Attribute
    {
        /// <summary>
        ///     지정된 매개변수 이름으로 <see cref="NotNullIfNotNullAttribute" /> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="parameterName">
        ///     null 여부를 확인할 매개변수의 이름입니다.
        ///     이 매개변수가 null이 아니면 반환 값도 null이 아님을 의미합니다.
        /// </param>
        public NotNullIfNotNullAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        /// <summary>
        ///     null 여부를 확인할 매개변수의 이름을 가져옵니다.
        /// </summary>
        public string ParameterName { get; }
    }
}

#endif
