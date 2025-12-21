#if NET47 || NET472 || NET48 || NETSTANDARD2_0 || NETSTANDARD2_1

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Specifies that a type has required members or that a member is required.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class RequiredMemberAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredMemberAttribute"/> class.
        /// </summary>
        public RequiredMemberAttribute()
        {
        }
    }
}

#endif
