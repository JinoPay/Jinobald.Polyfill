#if !NET5_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    ///     메서드가 지정된 <see cref="bool" /> 값을 반환할 때 지정된 멤버가 null이 아님을 지정합니다.
    ///     조건부로 멤버 초기화를 보장하는 메서드에 사용됩니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class MemberNotNullWhenAttribute : Attribute
    {
        /// <summary>
        ///     지정된 반환 값과 멤버 이름으로 <see cref="MemberNotNullWhenAttribute" /> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="returnValue">
        ///     멤버가 null이 아님을 나타내는 반환 값입니다.
        /// </param>
        /// <param name="member">
        ///     null이 아님을 보장할 멤버의 이름입니다.
        /// </param>
        public MemberNotNullWhenAttribute(bool returnValue, string member)
        {
            ReturnValue = returnValue;
            Members = new[] { member };
        }

        /// <summary>
        ///     지정된 반환 값과 멤버 이름들로 <see cref="MemberNotNullWhenAttribute" /> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="returnValue">
        ///     멤버가 null이 아님을 나타내는 반환 값입니다.
        /// </param>
        /// <param name="members">
        ///     null이 아님을 보장할 멤버들의 이름입니다.
        /// </param>
        public MemberNotNullWhenAttribute(bool returnValue, params string[] members)
        {
            ReturnValue = returnValue;
            Members = members;
        }

        /// <summary>
        ///     멤버가 null이 아님을 나타내는 반환 값을 가져옵니다.
        /// </summary>
        public bool ReturnValue { get; }

        /// <summary>
        ///     null이 아님을 보장할 멤버들의 이름을 가져옵니다.
        /// </summary>
        public string[] Members { get; }
    }
}

#endif
