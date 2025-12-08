#if NET35
using System.Runtime.CompilerServices;

namespace System;

public static partial class StringEx
{
    extension(string)
    {
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static bool IsNullOrWhiteSpace(string? value)
        {
            //Added in .NET 4.0
            return string.IsNullOrEmpty(value) || value.All(char.IsWhiteSpace);
        }
    }
}
#endif