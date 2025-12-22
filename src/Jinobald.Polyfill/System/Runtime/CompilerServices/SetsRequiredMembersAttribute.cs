#if NET47 || NET472 || NET48 || NETSTANDARD2_0 || NETSTANDARD2_1
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Indicates that the compiler should enforce that a constructor initializes all required members.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed class SetsRequiredMembersAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetsRequiredMembersAttribute"/> class.
        /// </summary>
        public SetsRequiredMembersAttribute()
        {
        }
    }
}

#endif
