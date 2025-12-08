using System.Runtime.CompilerServices;

namespace System.Threading;

public static class InterlockedEx
{
    extension(Thread)
    {
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static void MemoryBarrier()
        {
            Thread.MemoryBarrier();
        }
    }
}