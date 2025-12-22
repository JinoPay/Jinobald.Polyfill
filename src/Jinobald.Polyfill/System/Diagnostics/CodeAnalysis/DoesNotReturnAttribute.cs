#if !NET5_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    ///     메서드가 반환되지 않음을 지정합니다.
    ///     예외를 throw하거나 무한 루프를 실행하는 메서드에 사용됩니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class DoesNotReturnAttribute : Attribute
    {
        /// <summary>
        ///     <see cref="DoesNotReturnAttribute" /> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public DoesNotReturnAttribute()
        {
        }
    }
}

#endif
