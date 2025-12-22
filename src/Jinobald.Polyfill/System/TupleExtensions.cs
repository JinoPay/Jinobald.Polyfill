#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462

namespace System;

/// <summary>
/// <see cref="Tuple"/> 및 <see cref="ValueTuple"/>에 대한 확장 메서드를 제공합니다.
/// </summary>
public static class TupleExtensions
{
    #region Tuple to ValueTuple (ToValueTuple)

    /// <summary>
    /// <see cref="Tuple{T1}"/>을 <see cref="ValueTuple{T1}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 튜플입니다.</param>
    /// <returns>변환된 값 튜플입니다.</returns>
    public static ValueTuple<T1> ToValueTuple<T1>(this Tuple<T1> value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return ValueTuple.Create(value.Item1);
    }

    /// <summary>
    /// <see cref="Tuple{T1, T2}"/>을 <see cref="ValueTuple{T1, T2}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 튜플입니다.</param>
    /// <returns>변환된 값 튜플입니다.</returns>
    public static (T1, T2) ToValueTuple<T1, T2>(this Tuple<T1, T2> value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return (value.Item1, value.Item2);
    }

    /// <summary>
    /// <see cref="Tuple{T1, T2, T3}"/>을 <see cref="ValueTuple{T1, T2, T3}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 튜플입니다.</param>
    /// <returns>변환된 값 튜플입니다.</returns>
    public static (T1, T2, T3) ToValueTuple<T1, T2, T3>(this Tuple<T1, T2, T3> value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return (value.Item1, value.Item2, value.Item3);
    }

    /// <summary>
    /// <see cref="Tuple{T1, T2, T3, T4}"/>을 <see cref="ValueTuple{T1, T2, T3, T4}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T4">네 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 튜플입니다.</param>
    /// <returns>변환된 값 튜플입니다.</returns>
    public static (T1, T2, T3, T4) ToValueTuple<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return (value.Item1, value.Item2, value.Item3, value.Item4);
    }

    /// <summary>
    /// <see cref="Tuple{T1, T2, T3, T4, T5}"/>을 <see cref="ValueTuple{T1, T2, T3, T4, T5}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T4">네 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T5">다섯 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 튜플입니다.</param>
    /// <returns>변환된 값 튜플입니다.</returns>
    public static (T1, T2, T3, T4, T5) ToValueTuple<T1, T2, T3, T4, T5>(this Tuple<T1, T2, T3, T4, T5> value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return (value.Item1, value.Item2, value.Item3, value.Item4, value.Item5);
    }

    /// <summary>
    /// <see cref="Tuple{T1, T2, T3, T4, T5, T6}"/>을 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T4">네 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T5">다섯 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T6">여섯 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 튜플입니다.</param>
    /// <returns>변환된 값 튜플입니다.</returns>
    public static (T1, T2, T3, T4, T5, T6) ToValueTuple<T1, T2, T3, T4, T5, T6>(
        this Tuple<T1, T2, T3, T4, T5, T6> value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return (value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6);
    }

    /// <summary>
    /// <see cref="Tuple{T1, T2, T3, T4, T5, T6, T7}"/>을 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T4">네 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T5">다섯 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T6">여섯 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T7">일곱 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 튜플입니다.</param>
    /// <returns>변환된 값 튜플입니다.</returns>
    public static (T1, T2, T3, T4, T5, T6, T7) ToValueTuple<T1, T2, T3, T4, T5, T6, T7>(
        this Tuple<T1, T2, T3, T4, T5, T6, T7> value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return (value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6, value.Item7);
    }

    #endregion

    #region ValueTuple to Tuple (ToTuple)

    /// <summary>
    /// <see cref="ValueTuple{T1}"/>을 <see cref="Tuple{T1}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 값 튜플입니다.</param>
    /// <returns>변환된 튜플입니다.</returns>
    public static Tuple<T1> ToTuple<T1>(this ValueTuple<T1> value)
    {
        return Tuple.Create(value.Item1);
    }

    /// <summary>
    /// <see cref="ValueTuple{T1, T2}"/>을 <see cref="Tuple{T1, T2}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 값 튜플입니다.</param>
    /// <returns>변환된 튜플입니다.</returns>
    public static Tuple<T1, T2> ToTuple<T1, T2>(this (T1, T2) value)
    {
        return Tuple.Create(value.Item1, value.Item2);
    }

    /// <summary>
    /// <see cref="ValueTuple{T1, T2, T3}"/>을 <see cref="Tuple{T1, T2, T3}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 값 튜플입니다.</param>
    /// <returns>변환된 튜플입니다.</returns>
    public static Tuple<T1, T2, T3> ToTuple<T1, T2, T3>(this (T1, T2, T3) value)
    {
        return Tuple.Create(value.Item1, value.Item2, value.Item3);
    }

    /// <summary>
    /// <see cref="ValueTuple{T1, T2, T3, T4}"/>을 <see cref="Tuple{T1, T2, T3, T4}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T4">네 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 값 튜플입니다.</param>
    /// <returns>변환된 튜플입니다.</returns>
    public static Tuple<T1, T2, T3, T4> ToTuple<T1, T2, T3, T4>(this (T1, T2, T3, T4) value)
    {
        return Tuple.Create(value.Item1, value.Item2, value.Item3, value.Item4);
    }

    /// <summary>
    /// <see cref="ValueTuple{T1, T2, T3, T4, T5}"/>을 <see cref="Tuple{T1, T2, T3, T4, T5}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T4">네 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T5">다섯 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 값 튜플입니다.</param>
    /// <returns>변환된 튜플입니다.</returns>
    public static Tuple<T1, T2, T3, T4, T5> ToTuple<T1, T2, T3, T4, T5>(this (T1, T2, T3, T4, T5) value)
    {
        return Tuple.Create(value.Item1, value.Item2, value.Item3, value.Item4, value.Item5);
    }

    /// <summary>
    /// <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/>을 <see cref="Tuple{T1, T2, T3, T4, T5, T6}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T4">네 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T5">다섯 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T6">여섯 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 값 튜플입니다.</param>
    /// <returns>변환된 튜플입니다.</returns>
    public static Tuple<T1, T2, T3, T4, T5, T6> ToTuple<T1, T2, T3, T4, T5, T6>(this (T1, T2, T3, T4, T5, T6) value)
    {
        return Tuple.Create(value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6);
    }

    /// <summary>
    /// <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/>을 <see cref="Tuple{T1, T2, T3, T4, T5, T6, T7}"/>으로 변환합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T4">네 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T5">다섯 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T6">여섯 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T7">일곱 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">변환할 값 튜플입니다.</param>
    /// <returns>변환된 튜플입니다.</returns>
    public static Tuple<T1, T2, T3, T4, T5, T6, T7> ToTuple<T1, T2, T3, T4, T5, T6, T7>(
        this (T1, T2, T3, T4, T5, T6, T7) value)
    {
        return Tuple.Create(value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6,
            value.Item7);
    }

    #endregion

    #region Deconstruct

    /// <summary>
    /// <see cref="Tuple{T1}"/>을 개별 요소로 분해합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">분해할 튜플입니다.</param>
    /// <param name="item1">첫 번째 요소입니다.</param>
    public static void Deconstruct<T1>(this Tuple<T1> value, out T1 item1)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        item1 = value.Item1;
    }

    /// <summary>
    /// <see cref="Tuple{T1, T2}"/>을 개별 요소로 분해합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">분해할 튜플입니다.</param>
    /// <param name="item1">첫 번째 요소입니다.</param>
    /// <param name="item2">두 번째 요소입니다.</param>
    public static void Deconstruct<T1, T2>(this Tuple<T1, T2> value, out T1 item1, out T2 item2)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        item1 = value.Item1;
        item2 = value.Item2;
    }

    /// <summary>
    /// <see cref="Tuple{T1, T2, T3}"/>을 개별 요소로 분해합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">분해할 튜플입니다.</param>
    /// <param name="item1">첫 번째 요소입니다.</param>
    /// <param name="item2">두 번째 요소입니다.</param>
    /// <param name="item3">세 번째 요소입니다.</param>
    public static void Deconstruct<T1, T2, T3>(this Tuple<T1, T2, T3> value, out T1 item1, out T2 item2, out T3 item3)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        item1 = value.Item1;
        item2 = value.Item2;
        item3 = value.Item3;
    }

    /// <summary>
    /// <see cref="Tuple{T1, T2, T3, T4}"/>을 개별 요소로 분해합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T4">네 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">분해할 튜플입니다.</param>
    /// <param name="item1">첫 번째 요소입니다.</param>
    /// <param name="item2">두 번째 요소입니다.</param>
    /// <param name="item3">세 번째 요소입니다.</param>
    /// <param name="item4">네 번째 요소입니다.</param>
    public static void Deconstruct<T1, T2, T3, T4>(
        this Tuple<T1, T2, T3, T4> value,
        out T1 item1,
        out T2 item2,
        out T3 item3,
        out T4 item4)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        item1 = value.Item1;
        item2 = value.Item2;
        item3 = value.Item3;
        item4 = value.Item4;
    }

    /// <summary>
    /// <see cref="Tuple{T1, T2, T3, T4, T5}"/>을 개별 요소로 분해합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T4">네 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T5">다섯 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">분해할 튜플입니다.</param>
    /// <param name="item1">첫 번째 요소입니다.</param>
    /// <param name="item2">두 번째 요소입니다.</param>
    /// <param name="item3">세 번째 요소입니다.</param>
    /// <param name="item4">네 번째 요소입니다.</param>
    /// <param name="item5">다섯 번째 요소입니다.</param>
    public static void Deconstruct<T1, T2, T3, T4, T5>(
        this Tuple<T1, T2, T3, T4, T5> value,
        out T1 item1,
        out T2 item2,
        out T3 item3,
        out T4 item4,
        out T5 item5)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        item1 = value.Item1;
        item2 = value.Item2;
        item3 = value.Item3;
        item4 = value.Item4;
        item5 = value.Item5;
    }

    /// <summary>
    /// <see cref="Tuple{T1, T2, T3, T4, T5, T6}"/>을 개별 요소로 분해합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T4">네 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T5">다섯 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T6">여섯 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">분해할 튜플입니다.</param>
    /// <param name="item1">첫 번째 요소입니다.</param>
    /// <param name="item2">두 번째 요소입니다.</param>
    /// <param name="item3">세 번째 요소입니다.</param>
    /// <param name="item4">네 번째 요소입니다.</param>
    /// <param name="item5">다섯 번째 요소입니다.</param>
    /// <param name="item6">여섯 번째 요소입니다.</param>
    public static void Deconstruct<T1, T2, T3, T4, T5, T6>(
        this Tuple<T1, T2, T3, T4, T5, T6> value,
        out T1 item1,
        out T2 item2,
        out T3 item3,
        out T4 item4,
        out T5 item5,
        out T6 item6)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        item1 = value.Item1;
        item2 = value.Item2;
        item3 = value.Item3;
        item4 = value.Item4;
        item5 = value.Item5;
        item6 = value.Item6;
    }

    /// <summary>
    /// <see cref="Tuple{T1, T2, T3, T4, T5, T6, T7}"/>을 개별 요소로 분해합니다.
    /// </summary>
    /// <typeparam name="T1">첫 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T2">두 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T3">세 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T4">네 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T5">다섯 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T6">여섯 번째 요소의 형식입니다.</typeparam>
    /// <typeparam name="T7">일곱 번째 요소의 형식입니다.</typeparam>
    /// <param name="value">분해할 튜플입니다.</param>
    /// <param name="item1">첫 번째 요소입니다.</param>
    /// <param name="item2">두 번째 요소입니다.</param>
    /// <param name="item3">세 번째 요소입니다.</param>
    /// <param name="item4">네 번째 요소입니다.</param>
    /// <param name="item5">다섯 번째 요소입니다.</param>
    /// <param name="item6">여섯 번째 요소입니다.</param>
    /// <param name="item7">일곱 번째 요소입니다.</param>
    public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7>(
        this Tuple<T1, T2, T3, T4, T5, T6, T7> value,
        out T1 item1,
        out T2 item2,
        out T3 item3,
        out T4 item4,
        out T5 item5,
        out T6 item6,
        out T7 item7)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        item1 = value.Item1;
        item2 = value.Item2;
        item3 = value.Item3;
        item4 = value.Item4;
        item5 = value.Item5;
        item6 = value.Item6;
        item7 = value.Item7;
    }

    #endregion
}

#endif
