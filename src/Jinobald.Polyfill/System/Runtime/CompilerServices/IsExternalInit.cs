#if NET46 || NET462 || NET47 || NET472 || NET48 || NETSTANDARD2_0 || NETSTANDARD2_1
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Reserved to be used by the compiler for tracking metadata.
    /// This class should not be used by developers in source code.
    /// This class is required for record types and init-only properties.
    /// </summary>
    internal static class IsExternalInit
    {
    }
}

#endif
