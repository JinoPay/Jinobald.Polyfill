#if !NET5_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    ///     메서드가 반환될 때 지정된 멤버가 null이 아님을 지정합니다.
    ///     멤버 초기화를 보장하는 메서드에 사용됩니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class MemberNotNullAttribute : Attribute
    {
        /// <summary>
        ///     지정된 멤버 이름으로 <see cref="MemberNotNullAttribute" /> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="member">
        ///     null이 아님을 보장할 멤버의 이름입니다.
        /// </param>
        public MemberNotNullAttribute(string member)
        {
            Members = new[] { member };
        }

        /// <summary>
        ///     지정된 멤버 이름들로 <see cref="MemberNotNullAttribute" /> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="members">
        ///     null이 아님을 보장할 멤버들의 이름입니다.
        /// </param>
        public MemberNotNullAttribute(params string[] members)
        {
            Members = members;
        }

        /// <summary>
        ///     null이 아님을 보장할 멤버들의 이름을 가져옵니다.
        /// </summary>
        public string[] Members { get; }
    }
}

#endif
