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

    /// <summary>
    /// 배열의 지정된 범위에서 Span을 만듭니다.
    /// </summary>
    /// <typeparam name="T">배열 요소의 형식입니다.</typeparam>
    /// <param name="array">소스 배열입니다.</param>
    /// <param name="range">범위입니다.</param>
    /// <returns>배열 범위에서 만들어진 Span입니다.</returns>
    public static Span<T> AsSpan<T>(this T[]? array, Range range)
    {
        if (array == null)
        {
            return default;
        }

        var (offset, length) = range.GetOffsetAndLength(array.Length);
        return new Span<T>(array, offset, length);
    }

    /// <summary>
    /// 배열에서 Memory를 만듭니다.
    /// </summary>
    /// <typeparam name="T">배열 요소 형식입니다.</typeparam>
    /// <param name="array">소스 배열입니다.</param>
    /// <returns>배열에서 만들어진 Memory입니다.</returns>
    public static Memory<T> AsMemory<T>(this T[]? array)
    {
        return new Memory<T>(array);
    }

    /// <summary>
    /// 배열의 지정된 세그먼트에서 Memory를 만듭니다.
    /// </summary>
    /// <typeparam name="T">배열 요소 형식입니다.</typeparam>
    /// <param name="array">소스 배열입니다.</param>
    /// <param name="start">시작 인덱스입니다.</param>
    /// <returns>배열 세그먼트에서 만들어진 Memory입니다.</returns>
    public static Memory<T> AsMemory<T>(this T[]? array, int start)
    {
        if (array == null)
        {
            if (start != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            return default;
        }

        return new Memory<T>(array, start, array.Length - start);
    }

    /// <summary>
    /// 배열의 지정된 세그먼트에서 Memory를 만듭니다.
    /// </summary>
    /// <typeparam name="T">배열 요소 형식입니다.</typeparam>
    /// <param name="array">소스 배열입니다.</param>
    /// <param name="start">시작 인덱스입니다.</param>
    /// <param name="length">길이입니다.</param>
    /// <returns>배열 세그먼트에서 만들어진 Memory입니다.</returns>
    public static Memory<T> AsMemory<T>(this T[]? array, int start, int length)
    {
        return new Memory<T>(array!, start, length);
    }

    /// <summary>
    /// 문자열에서 ReadOnlyMemory를 만듭니다.
    /// </summary>
    /// <param name="text">소스 문자열입니다.</param>
    /// <returns>문자열에서 만들어진 ReadOnlyMemory입니다.</returns>
    public static ReadOnlyMemory<char> AsMemory(this string? text)
    {
        if (text == null)
        {
            return default;
        }

        return new ReadOnlyMemory<char>(text.ToCharArray());
    }

    /// <summary>
    /// 문자열의 지정된 세그먼트에서 ReadOnlyMemory를 만듭니다.
    /// </summary>
    /// <param name="text">소스 문자열입니다.</param>
    /// <param name="start">시작 인덱스입니다.</param>
    /// <returns>문자열 세그먼트에서 만들어진 ReadOnlyMemory입니다.</returns>
    public static ReadOnlyMemory<char> AsMemory(this string? text, int start)
    {
        if (text == null)
        {
            if (start != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            return default;
        }

        return new ReadOnlyMemory<char>(text.ToCharArray(), start, text.Length - start);
    }

    /// <summary>
    /// 문자열의 지정된 세그먼트에서 ReadOnlyMemory를 만듭니다.
    /// </summary>
    /// <param name="text">소스 문자열입니다.</param>
    /// <param name="start">시작 인덱스입니다.</param>
    /// <param name="length">길이입니다.</param>
    /// <returns>문자열 세그먼트에서 만들어진 ReadOnlyMemory입니다.</returns>
    public static ReadOnlyMemory<char> AsMemory(this string? text, int start, int length)
    {
        if (text == null)
        {
            if (start != 0 || length != 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            return default;
        }

        return new ReadOnlyMemory<char>(text.ToCharArray(), start, length);
    }

    /// <summary>
    /// Span에서 지정된 값의 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 Span입니다.</param>
    /// <param name="value">찾을 값입니다.</param>
    /// <returns>값의 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int IndexOf<T>(this Span<T> span, T value)
        where T : IEquatable<T>
    {
        return ((ReadOnlySpan<T>)span).IndexOf(value);
    }

    /// <summary>
    /// ReadOnlySpan에서 지정된 값의 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 ReadOnlySpan입니다.</param>
    /// <param name="value">찾을 값입니다.</param>
    /// <returns>값의 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int IndexOf<T>(this ReadOnlySpan<T> span, T value)
        where T : IEquatable<T>
    {
        for (int i = 0; i < span.Length; i++)
        {
            if (span[i].Equals(value))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Span에서 지정된 시퀀스의 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 Span입니다.</param>
    /// <param name="value">찾을 시퀀스입니다.</param>
    /// <returns>시퀀스의 시작 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int IndexOf<T>(this Span<T> span, ReadOnlySpan<T> value)
        where T : IEquatable<T>
    {
        return ((ReadOnlySpan<T>)span).IndexOf(value);
    }

    /// <summary>
    /// ReadOnlySpan에서 지정된 시퀀스의 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 ReadOnlySpan입니다.</param>
    /// <param name="value">찾을 시퀀스입니다.</param>
    /// <returns>시퀀스의 시작 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int IndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
        where T : IEquatable<T>
    {
        if (value.Length == 0)
        {
            return 0;
        }

        if (value.Length > span.Length)
        {
            return -1;
        }

        for (int i = 0; i <= span.Length - value.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < value.Length; j++)
            {
                if (!span[i + j].Equals(value[j]))
                {
                    found = false;
                    break;
                }
            }

            if (found)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Span에서 지정된 값의 마지막 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 Span입니다.</param>
    /// <param name="value">찾을 값입니다.</param>
    /// <returns>값의 마지막 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int LastIndexOf<T>(this Span<T> span, T value)
        where T : IEquatable<T>
    {
        return ((ReadOnlySpan<T>)span).LastIndexOf(value);
    }

    /// <summary>
    /// ReadOnlySpan에서 지정된 값의 마지막 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 ReadOnlySpan입니다.</param>
    /// <param name="value">찾을 값입니다.</param>
    /// <returns>값의 마지막 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int LastIndexOf<T>(this ReadOnlySpan<T> span, T value)
        where T : IEquatable<T>
    {
        for (int i = span.Length - 1; i >= 0; i--)
        {
            if (span[i].Equals(value))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// 두 Span이 동일한지 비교합니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">비교할 첫 번째 Span입니다.</param>
    /// <param name="other">비교할 두 번째 Span입니다.</param>
    /// <returns>동일하면 true, 그렇지 않으면 false입니다.</returns>
    public static bool SequenceEqual<T>(this Span<T> span, ReadOnlySpan<T> other)
        where T : IEquatable<T>
    {
        return ((ReadOnlySpan<T>)span).SequenceEqual(other);
    }

    /// <summary>
    /// 두 ReadOnlySpan이 동일한지 비교합니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">비교할 첫 번째 ReadOnlySpan입니다.</param>
    /// <param name="other">비교할 두 번째 ReadOnlySpan입니다.</param>
    /// <returns>동일하면 true, 그렇지 않으면 false입니다.</returns>
    public static bool SequenceEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other)
        where T : IEquatable<T>
    {
        if (span.Length != other.Length)
        {
            return false;
        }

        for (int i = 0; i < span.Length; i++)
        {
            if (!span[i].Equals(other[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Span이 지정된 값으로 시작하는지 확인합니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">확인할 Span입니다.</param>
    /// <param name="value">확인할 시퀀스입니다.</param>
    /// <returns>지정된 값으로 시작하면 true, 그렇지 않으면 false입니다.</returns>
    public static bool StartsWith<T>(this Span<T> span, ReadOnlySpan<T> value)
        where T : IEquatable<T>
    {
        return ((ReadOnlySpan<T>)span).StartsWith(value);
    }

    /// <summary>
    /// ReadOnlySpan이 지정된 값으로 시작하는지 확인합니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">확인할 ReadOnlySpan입니다.</param>
    /// <param name="value">확인할 시퀀스입니다.</param>
    /// <returns>지정된 값으로 시작하면 true, 그렇지 않으면 false입니다.</returns>
    public static bool StartsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
        where T : IEquatable<T>
    {
        if (value.Length > span.Length)
        {
            return false;
        }

        for (int i = 0; i < value.Length; i++)
        {
            if (!span[i].Equals(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Span이 지정된 값으로 끝나는지 확인합니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">확인할 Span입니다.</param>
    /// <param name="value">확인할 시퀀스입니다.</param>
    /// <returns>지정된 값으로 끝나면 true, 그렇지 않으면 false입니다.</returns>
    public static bool EndsWith<T>(this Span<T> span, ReadOnlySpan<T> value)
        where T : IEquatable<T>
    {
        return ((ReadOnlySpan<T>)span).EndsWith(value);
    }

    /// <summary>
    /// ReadOnlySpan이 지정된 값으로 끝나는지 확인합니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">확인할 ReadOnlySpan입니다.</param>
    /// <param name="value">확인할 시퀀스입니다.</param>
    /// <returns>지정된 값으로 끝나면 true, 그렇지 않으면 false입니다.</returns>
    public static bool EndsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
        where T : IEquatable<T>
    {
        if (value.Length > span.Length)
        {
            return false;
        }

        int offset = span.Length - value.Length;
        for (int i = 0; i < value.Length; i++)
        {
            if (!span[offset + i].Equals(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Span에 지정된 값이 포함되어 있는지 확인합니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">확인할 Span입니다.</param>
    /// <param name="value">찾을 값입니다.</param>
    /// <returns>포함되어 있으면 true, 그렇지 않으면 false입니다.</returns>
    public static bool Contains<T>(this Span<T> span, T value)
        where T : IEquatable<T>
    {
        return IndexOf(span, value) >= 0;
    }

    /// <summary>
    /// ReadOnlySpan에 지정된 값이 포함되어 있는지 확인합니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">확인할 ReadOnlySpan입니다.</param>
    /// <param name="value">찾을 값입니다.</param>
    /// <returns>포함되어 있으면 true, 그렇지 않으면 false입니다.</returns>
    public static bool Contains<T>(this ReadOnlySpan<T> span, T value)
        where T : IEquatable<T>
    {
        return IndexOf(span, value) >= 0;
    }

    /// <summary>
    /// Span의 요소를 반전합니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">반전할 Span입니다.</param>
    public static void Reverse<T>(this Span<T> span)
    {
        int left = 0;
        int right = span.Length - 1;

        while (left < right)
        {
            T temp = span[left];
            span[left] = span[right];
            span[right] = temp;
            left++;
            right--;
        }
    }
}

#endif
