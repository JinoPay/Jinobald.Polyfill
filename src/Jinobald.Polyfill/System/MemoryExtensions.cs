#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

namespace System;

/// <summary>
/// Memory, ReadOnlyMemory, Span, ReadOnlySpan에 대한 확장 메서드를 제공합니다.
/// </summary>
//Link: https://learn.microsoft.com/en-us/dotnet/api/system.memoryextensions
public static class MemoryExtensions
{
    /// <summary>
    /// 문자열에 대한 읽기 전용 문자 범위를 만듭니다.
    /// </summary>
    /// <param name="text">문자열입니다.</param>
    /// <returns>문자열의 읽기 전용 범위입니다.</returns>
    public static ReadOnlySpan<char> AsSpan(this string? text)
    {
        if (text == null)
        {
            return default;
        }

        return new ReadOnlySpan<char>(text.ToCharArray());
    }

    /// <summary>
    /// 지정된 위치에서 시작하는 문자열의 일부에 대한 읽기 전용 문자 범위를 만듭니다.
    /// </summary>
    /// <param name="text">문자열입니다.</param>
    /// <param name="start">시작 인덱스입니다.</param>
    /// <returns>문자열의 읽기 전용 범위입니다.</returns>
    public static ReadOnlySpan<char> AsSpan(this string? text, int start)
    {
        if (text == null)
        {
            if (start != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }
            return default;
        }

        if ((uint)start > (uint)text.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(start));
        }

        return new ReadOnlySpan<char>(text.ToCharArray(), start, text.Length - start);
    }

    /// <summary>
    /// 지정된 위치에서 시작하여 지정된 길이를 가지는 문자열의 일부에 대한 읽기 전용 문자 범위를 만듭니다.
    /// </summary>
    /// <param name="text">문자열입니다.</param>
    /// <param name="start">시작 인덱스입니다.</param>
    /// <param name="length">범위의 길이입니다.</param>
    /// <returns>문자열의 읽기 전용 범위입니다.</returns>
    public static ReadOnlySpan<char> AsSpan(this string? text, int start, int length)
    {
        if (text == null)
        {
            if (start != 0 || length != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }
            return default;
        }

        if ((uint)start > (uint)text.Length || (uint)length > (uint)(text.Length - start))
        {
            throw new ArgumentOutOfRangeException();
        }

        return new ReadOnlySpan<char>(text.ToCharArray(), start, length);
    }

    /// <summary>
    /// 배열에 대한 범위를 만듭니다.
    /// </summary>
    /// <typeparam name="T">배열 요소의 형식입니다.</typeparam>
    /// <param name="array">배열입니다.</param>
    /// <returns>배열의 범위입니다.</returns>
    public static Span<T> AsSpan<T>(this T[]? array)
    {
        return new Span<T>(array);
    }

    /// <summary>
    /// 지정된 위치에서 시작하는 배열의 일부에 대한 범위를 만듭니다.
    /// </summary>
    /// <typeparam name="T">배열 요소의 형식입니다.</typeparam>
    /// <param name="array">배열입니다.</param>
    /// <param name="start">시작 인덱스입니다.</param>
    /// <returns>배열의 범위입니다.</returns>
    public static Span<T> AsSpan<T>(this T[]? array, int start)
    {
        if (array == null)
        {
            if (start != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }
            return default;
        }

        if ((uint)start > (uint)array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(start));
        }

        return new Span<T>(array, start, array.Length - start);
    }

    /// <summary>
    /// 지정된 위치에서 시작하여 지정된 길이를 가지는 배열의 일부에 대한 범위를 만듭니다.
    /// </summary>
    /// <typeparam name="T">배열 요소의 형식입니다.</typeparam>
    /// <param name="array">배열입니다.</param>
    /// <param name="start">시작 인덱스입니다.</param>
    /// <param name="length">범위의 길이입니다.</param>
    /// <returns>배열의 범위입니다.</returns>
    public static Span<T> AsSpan<T>(this T[]? array, int start, int length)
    {
        return new Span<T>(array!, start, length);
    }
}

#endif
