#if !NET5_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    ///     메서드가 지정된 <see cref="bool" /> 값을 반환할 때 인수가 null이 아님을 지정합니다.
    ///     반환 값에 따라 매개변수의 null 상태를 조건부로 지정합니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class NotNullWhenAttribute : Attribute
    {
        /// <summary>
        ///     지정된 반환 값으로 <see cref="NotNullWhenAttribute" /> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="returnValue">
        ///     인수가 null이 아님을 나타내는 반환 값입니다.
        ///     true이면 메서드가 true를 반환할 때 인수가 null이 아님을 의미합니다.
        /// </param>
        public NotNullWhenAttribute(bool returnValue)
        {
            ReturnValue = returnValue;
        }

        /// <summary>
        ///     인수가 null이 아님을 나타내는 반환 값을 가져옵니다.
        /// </summary>
        public bool ReturnValue { get; }
    }
}

#endif
