#if NET35
using System.Runtime.CompilerServices;

namespace System;

public static class UIntPtrEx
{
    extension(IntPtr)
    {
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static UIntPtr Add(UIntPtr pointer, int offset)
        {
            return UIntPtr.Size switch
            {
                4 => new UIntPtr(unchecked((uint)((int)pointer + offset))),
                8 => new UIntPtr(unchecked((ulong)((long)pointer + offset))),
                _ => throw new NotSupportedException("Not supported platform")
            };
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static UIntPtr Subtract(UIntPtr pointer, int offset)
        {
            return UIntPtr.Size switch
            {
                4 => new UIntPtr(unchecked((uint)((int)pointer - offset))),
                8 => new UIntPtr(unchecked((ulong)((long)pointer - offset))),
                _ => throw new NotSupportedException("Not supported platform")
            };
        }
    }
}
#endif