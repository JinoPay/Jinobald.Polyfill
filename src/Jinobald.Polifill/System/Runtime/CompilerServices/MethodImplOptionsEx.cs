namespace System.Runtime.CompilerServices;

public static class MethodImplOptionsEx
{
    public const MethodImplOptions Unmanaged = (MethodImplOptions)0x0004;

    public const MethodImplOptions NoInlining = (MethodImplOptions)0x0008;

    public const MethodImplOptions ForwardRef = (MethodImplOptions)0x0010;

    public const MethodImplOptions Synchronized = (MethodImplOptions)0x0020;
    
    public const MethodImplOptions NoOptimization = (MethodImplOptions)0x0040;

    public const MethodImplOptions PreserveSig = (MethodImplOptions)0x0080;
    
    public const MethodImplOptions AggressiveInlining = (MethodImplOptions)0x0100;
    
    public const MethodImplOptions AggressiveOptimization = (MethodImplOptions)0x0200;
    
    public const MethodImplOptions InternalCall = (MethodImplOptions)0x1000;
    
    public const MethodImplOptions Async = (MethodImplOptions)0x2000;
}