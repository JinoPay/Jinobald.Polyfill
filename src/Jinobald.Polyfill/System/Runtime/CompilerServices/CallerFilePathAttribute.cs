#if NET40 || NET35 || NET20

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Allows you to obtain the full path of the source file that contains the caller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class CallerFilePathAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallerFilePathAttribute"/> class.
        /// </summary>
        public CallerFilePathAttribute()
        {
        }
    }
}

#endif
