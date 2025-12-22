#if !NET5_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    ///     매개변수가 지정된 <see cref="bool" /> 값일 때 메서드가 반환되지 않음을 지정합니다.
    ///     조건부로 예외를 throw하는 메서드에 사용됩니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class DoesNotReturnIfAttribute : Attribute
    {
        /// <summary>
        ///     지정된 매개변수 값으로 <see cref="DoesNotReturnIfAttribute" /> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="parameterValue">
        ///     메서드가 반환되지 않음을 나타내는 매개변수 값입니다.
        ///     true이면 매개변수가 true일 때 메서드가 반환되지 않음을 의미합니다.
        /// </param>
        public DoesNotReturnIfAttribute(bool parameterValue)
        {
            ParameterValue = parameterValue;
        }

        /// <summary>
        ///     메서드가 반환되지 않음을 나타내는 매개변수 값을 가져옵니다.
        /// </summary>
        public bool ParameterValue { get; }
    }
}

#endif
