#if NET40 || NET35

namespace System.Runtime.CompilerServices
{
    /// <summary>
    ///     Allows you to obtain the method or property name of the caller to the method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class CallerMemberNameAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CallerMemberNameAttribute" /> class.
        /// </summary>
        public CallerMemberNameAttribute()
        {
        }
    }
}

#endif
