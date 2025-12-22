// .NET 2.0: Action, Action<T>, Action<T1,T2>, Action<T1,T2,T3>, Action<T1,T2,T3,T4> 없음
// .NET 3.5: Action<T1-T4>까지 BCL에 포함, Action<T5-T8> 없음
// .NET 4.0+: Action<T1-T16> 모두 BCL에 포함

#if NET35
namespace System
{
    /// <summary>
    ///     5개의 매개 변수가 있고 값을 반환하지 않는 메서드를 캡슐화합니다.
    /// </summary>
    public delegate void Action<in T1, in T2, in T3, in T4, in T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    /// <summary>
    ///     6개의 매개 변수가 있고 값을 반환하지 않는 메서드를 캡슐화합니다.
    /// </summary>
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5,
        T6 arg6);

    /// <summary>
    ///     7개의 매개 변수가 있고 값을 반환하지 않는 메서드를 캡슐화합니다.
    /// </summary>
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4,
        T5 arg5, T6 arg6, T7 arg7);

    /// <summary>
    ///     8개의 매개 변수가 있고 값을 반환하지 않는 메서드를 캡슐화합니다.
    /// </summary>
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(T1 arg1, T2 arg2, T3 arg3,
        T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

    /// <summary>
    ///     9개의 매개 변수가 있고 값을 반환하지 않는 메서드를 캡슐화합니다.
    /// </summary>
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9>(T1 arg1, T2 arg2,
        T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

    /// <summary>
    ///     10개의 매개 변수가 있고 값을 반환하지 않는 메서드를 캡슐화합니다.
    /// </summary>
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10>(T1 arg1, T2 arg2,
        T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

    /// <summary>
    ///     11개의 매개 변수가 있고 값을 반환하지 않는 메서드를 캡슐화합니다.
    /// </summary>
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11>(T1 arg1,
        T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

    /// <summary>
    ///     12개의 매개 변수가 있고 값을 반환하지 않는 메서드를 캡슐화합니다.
    /// </summary>
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12>(
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11,
        T12 arg12);

    /// <summary>
    ///     13개의 매개 변수가 있고 값을 반환하지 않는 메서드를 캡슐화합니다.
    /// </summary>
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12,
        in T13>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11,
        T12 arg12, T13 arg13);

    /// <summary>
    ///     14개의 매개 변수가 있고 값을 반환하지 않는 메서드를 캡슐화합니다.
    /// </summary>
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12,
        in T13, in T14>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10,
        T11 arg11, T12 arg12, T13 arg13, T14 arg14);

    /// <summary>
    ///     15개의 매개 변수가 있고 값을 반환하지 않는 메서드를 캡슐화합니다.
    /// </summary>
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12,
        in T13, in T14, in T15>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
        T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

    /// <summary>
    ///     16개의 매개 변수가 있고 값을 반환하지 않는 메서드를 캡슐화합니다.
    /// </summary>
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12,
        in T13, in T14, in T15, in T16>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
        T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);
}
#endif
