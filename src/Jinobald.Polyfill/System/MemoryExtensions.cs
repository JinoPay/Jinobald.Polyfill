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
        if (value == null)
        {
            for (int i = 0; i < span.Length; i++)
            {
                if (span[i] == null)
                {
                    return i;
                }
            }
        }
        else
        {
            for (int i = 0; i < span.Length; i++)
            {
                if (value.Equals(span[i]))
                {
                    return i;
                }
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
        if (value == null)
        {
            for (int i = span.Length - 1; i >= 0; i--)
            {
                if (span[i] == null)
                {
                    return i;
                }
            }
        }
        else
        {
            for (int i = span.Length - 1; i >= 0; i--)
            {
                if (value.Equals(span[i]))
                {
                    return i;
                }
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
            T left = span[i];
            T right = other[i];

            if (left == null)
            {
                if (right != null)
                {
                    return false;
                }
            }
            else if (!left.Equals(right))
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

    /// <summary>
    /// 문자 범위에서 선행 및 후행 공백 문자를 제거합니다.
    /// </summary>
    /// <param name="span">트리밍할 문자 범위입니다.</param>
    /// <returns>공백이 제거된 범위입니다.</returns>
    public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span)
    {
        return span.TrimStart().TrimEnd();
    }

    /// <summary>
    /// 문자 범위에서 선행 공백 문자를 제거합니다.
    /// </summary>
    /// <param name="span">트리밍할 문자 범위입니다.</param>
    /// <returns>선행 공백이 제거된 범위입니다.</returns>
    public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span)
    {
        int start = 0;
        while (start < span.Length && char.IsWhiteSpace(span[start]))
        {
            start++;
        }

        return span.Slice(start);
    }

    /// <summary>
    /// 문자 범위에서 후행 공백 문자를 제거합니다.
    /// </summary>
    /// <param name="span">트리밍할 문자 범위입니다.</param>
    /// <returns>후행 공백이 제거된 범위입니다.</returns>
    public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span)
    {
        int end = span.Length - 1;
        while (end >= 0 && char.IsWhiteSpace(span[end]))
        {
            end--;
        }

        return span.Slice(0, end + 1);
    }

    /// <summary>
    /// 문자 범위에서 지정된 문자들의 선행 및 후행을 제거합니다.
    /// </summary>
    /// <param name="span">트리밍할 문자 범위입니다.</param>
    /// <param name="trimChars">제거할 문자들입니다.</param>
    /// <returns>지정된 문자가 제거된 범위입니다.</returns>
    public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, char trimChar)
    {
        return span.TrimStart(trimChar).TrimEnd(trimChar);
    }

    /// <summary>
    /// 문자 범위에서 지정된 문자의 선행을 제거합니다.
    /// </summary>
    /// <param name="span">트리밍할 문자 범위입니다.</param>
    /// <param name="trimChar">제거할 문자입니다.</param>
    /// <returns>선행 문자가 제거된 범위입니다.</returns>
    public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span, char trimChar)
    {
        int start = 0;
        while (start < span.Length && span[start] == trimChar)
        {
            start++;
        }

        return span.Slice(start);
    }

    /// <summary>
    /// 문자 범위에서 지정된 문자의 후행을 제거합니다.
    /// </summary>
    /// <param name="span">트리밍할 문자 범위입니다.</param>
    /// <param name="trimChar">제거할 문자입니다.</param>
    /// <returns>후행 문자가 제거된 범위입니다.</returns>
    public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span, char trimChar)
    {
        int end = span.Length - 1;
        while (end >= 0 && span[end] == trimChar)
        {
            end--;
        }

        return span.Slice(0, end + 1);
    }

    /// <summary>
    /// 문자 범위에서 지정된 문자들의 선행 및 후행을 제거합니다.
    /// </summary>
    /// <param name="span">트리밍할 문자 범위입니다.</param>
    /// <param name="trimChars">제거할 문자들입니다.</param>
    /// <returns>지정된 문자가 제거된 범위입니다.</returns>
    public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars)
    {
        return span.TrimStart(trimChars).TrimEnd(trimChars);
    }

    /// <summary>
    /// 문자 범위에서 지정된 문자들의 선행을 제거합니다.
    /// </summary>
    /// <param name="span">트리밍할 문자 범위입니다.</param>
    /// <param name="trimChars">제거할 문자들입니다.</param>
    /// <returns>선행 문자가 제거된 범위입니다.</returns>
    public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars)
    {
        if (trimChars.Length == 0)
        {
            return span.TrimStart();
        }

        int start = 0;
        while (start < span.Length && trimChars.Contains(span[start]))
        {
            start++;
        }

        return span.Slice(start);
    }

    /// <summary>
    /// 문자 범위에서 지정된 문자들의 후행을 제거합니다.
    /// </summary>
    /// <param name="span">트리밍할 문자 범위입니다.</param>
    /// <param name="trimChars">제거할 문자들입니다.</param>
    /// <returns>후행 문자가 제거된 범위입니다.</returns>
    public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars)
    {
        if (trimChars.Length == 0)
        {
            return span.TrimEnd();
        }

        int end = span.Length - 1;
        while (end >= 0 && trimChars.Contains(span[end]))
        {
            end--;
        }

        return span.Slice(0, end + 1);
    }

    /// <summary>
    /// Span에서 지정된 값들 중 하나의 첫 번째 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 Span입니다.</param>
    /// <param name="values">찾을 값들입니다.</param>
    /// <returns>값의 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int IndexOfAny<T>(this Span<T> span, ReadOnlySpan<T> values)
        where T : IEquatable<T>
    {
        return ((ReadOnlySpan<T>)span).IndexOfAny(values);
    }

    /// <summary>
    /// ReadOnlySpan에서 지정된 값들 중 하나의 첫 번째 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 ReadOnlySpan입니다.</param>
    /// <param name="values">찾을 값들입니다.</param>
    /// <returns>값의 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int IndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values)
        where T : IEquatable<T>
    {
        for (int i = 0; i < span.Length; i++)
        {
            for (int j = 0; j < values.Length; j++)
            {
                if (span[i].Equals(values[j]))
                {
                    return i;
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Span에서 지정된 값들 중 하나의 첫 번째 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 Span입니다.</param>
    /// <param name="value0">찾을 첫 번째 값입니다.</param>
    /// <param name="value1">찾을 두 번째 값입니다.</param>
    /// <returns>값의 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int IndexOfAny<T>(this Span<T> span, T value0, T value1)
        where T : IEquatable<T>
    {
        return ((ReadOnlySpan<T>)span).IndexOfAny(value0, value1);
    }

    /// <summary>
    /// ReadOnlySpan에서 지정된 값들 중 하나의 첫 번째 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 ReadOnlySpan입니다.</param>
    /// <param name="value0">찾을 첫 번째 값입니다.</param>
    /// <param name="value1">찾을 두 번째 값입니다.</param>
    /// <returns>값의 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int IndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1)
        where T : IEquatable<T>
    {
        for (int i = 0; i < span.Length; i++)
        {
            if (span[i].Equals(value0) || span[i].Equals(value1))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Span에서 지정된 값들 중 하나의 첫 번째 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 Span입니다.</param>
    /// <param name="value0">찾을 첫 번째 값입니다.</param>
    /// <param name="value1">찾을 두 번째 값입니다.</param>
    /// <param name="value2">찾을 세 번째 값입니다.</param>
    /// <returns>값의 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int IndexOfAny<T>(this Span<T> span, T value0, T value1, T value2)
        where T : IEquatable<T>
    {
        return ((ReadOnlySpan<T>)span).IndexOfAny(value0, value1, value2);
    }

    /// <summary>
    /// ReadOnlySpan에서 지정된 값들 중 하나의 첫 번째 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 ReadOnlySpan입니다.</param>
    /// <param name="value0">찾을 첫 번째 값입니다.</param>
    /// <param name="value1">찾을 두 번째 값입니다.</param>
    /// <param name="value2">찾을 세 번째 값입니다.</param>
    /// <returns>값의 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int IndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2)
        where T : IEquatable<T>
    {
        for (int i = 0; i < span.Length; i++)
        {
            if (span[i].Equals(value0) || span[i].Equals(value1) || span[i].Equals(value2))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Span에서 지정된 값들 중 하나의 마지막 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 Span입니다.</param>
    /// <param name="values">찾을 값들입니다.</param>
    /// <returns>값의 마지막 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int LastIndexOfAny<T>(this Span<T> span, ReadOnlySpan<T> values)
        where T : IEquatable<T>
    {
        return ((ReadOnlySpan<T>)span).LastIndexOfAny(values);
    }

    /// <summary>
    /// ReadOnlySpan에서 지정된 값들 중 하나의 마지막 인덱스를 찾습니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">검색할 ReadOnlySpan입니다.</param>
    /// <param name="values">찾을 값들입니다.</param>
    /// <returns>값의 마지막 인덱스, 또는 찾지 못한 경우 -1입니다.</returns>
    public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values)
        where T : IEquatable<T>
    {
        for (int i = span.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < values.Length; j++)
            {
                if (span[i].Equals(values[j]))
                {
                    return i;
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// 두 문자 범위가 지정된 비교 방식으로 같은지 비교합니다.
    /// </summary>
    /// <param name="span">비교할 첫 번째 범위입니다.</param>
    /// <param name="other">비교할 두 번째 범위입니다.</param>
    /// <param name="comparisonType">비교 유형입니다.</param>
    /// <returns>같으면 true, 그렇지 않으면 false입니다.</returns>
    public static bool Equals(this ReadOnlySpan<char> span, ReadOnlySpan<char> other, StringComparison comparisonType)
    {
        if (span.Length != other.Length)
        {
            return false;
        }

        return span.ToString().Equals(other.ToString(), comparisonType);
    }

    /// <summary>
    /// 두 문자 범위를 지정된 비교 방식으로 비교합니다.
    /// </summary>
    /// <param name="span">비교할 첫 번째 범위입니다.</param>
    /// <param name="other">비교할 두 번째 범위입니다.</param>
    /// <param name="comparisonType">비교 유형입니다.</param>
    /// <returns>span이 other보다 작으면 음수, 같으면 0, 크면 양수입니다.</returns>
    public static int CompareTo(this ReadOnlySpan<char> span, ReadOnlySpan<char> other, StringComparison comparisonType)
    {
        return string.Compare(span.ToString(), other.ToString(), comparisonType);
    }

    /// <summary>
    /// 문자 범위가 지정된 값으로 시작하는지 확인합니다.
    /// </summary>
    /// <param name="span">확인할 범위입니다.</param>
    /// <param name="value">확인할 값입니다.</param>
    /// <param name="comparisonType">비교 유형입니다.</param>
    /// <returns>지정된 값으로 시작하면 true, 그렇지 않으면 false입니다.</returns>
    public static bool StartsWith(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
    {
        if (value.Length > span.Length)
        {
            return false;
        }

        return span.Slice(0, value.Length).ToString().Equals(value.ToString(), comparisonType);
    }

    /// <summary>
    /// 문자 범위가 지정된 값으로 끝나는지 확인합니다.
    /// </summary>
    /// <param name="span">확인할 범위입니다.</param>
    /// <param name="value">확인할 값입니다.</param>
    /// <param name="comparisonType">비교 유형입니다.</param>
    /// <returns>지정된 값으로 끝나면 true, 그렇지 않으면 false입니다.</returns>
    public static bool EndsWith(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
    {
        if (value.Length > span.Length)
        {
            return false;
        }

        return span.Slice(span.Length - value.Length).ToString().Equals(value.ToString(), comparisonType);
    }

    /// <summary>
    /// 문자 범위가 지정된 값을 포함하는지 확인합니다.
    /// </summary>
    /// <param name="span">확인할 범위입니다.</param>
    /// <param name="value">찾을 값입니다.</param>
    /// <param name="comparisonType">비교 유형입니다.</param>
    /// <returns>값을 포함하면 true, 그렇지 않으면 false입니다.</returns>
    public static bool Contains(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
    {
        return span.ToString().IndexOf(value.ToString(), comparisonType) >= 0;
    }

    /// <summary>
    /// 문자열의 Index 위치에서 시작하는 부분 문자열을 가져옵니다.
    /// </summary>
    /// <param name="text">소스 문자열입니다.</param>
    /// <param name="startIndex">시작 인덱스입니다.</param>
    /// <returns>문자열의 읽기 전용 범위입니다.</returns>
    public static ReadOnlySpan<char> AsSpan(this string? text, Index startIndex)
    {
        if (text == null)
        {
            return default;
        }

        int actualIndex = startIndex.GetOffset(text.Length);
        return text.AsSpan(actualIndex);
    }

    /// <summary>
    /// 문자열의 Range 범위에 해당하는 부분을 가져옵니다.
    /// </summary>
    /// <param name="text">소스 문자열입니다.</param>
    /// <param name="range">범위입니다.</param>
    /// <returns>문자열의 읽기 전용 범위입니다.</returns>
    public static ReadOnlySpan<char> AsSpan(this string? text, Range range)
    {
        if (text == null)
        {
            return default;
        }

        var (offset, length) = range.GetOffsetAndLength(text.Length);
        return text.AsSpan(offset, length);
    }

    /// <summary>
    /// 배열의 Index 위치에서 시작하는 Span을 가져옵니다.
    /// </summary>
    /// <typeparam name="T">배열 요소의 형식입니다.</typeparam>
    /// <param name="array">소스 배열입니다.</param>
    /// <param name="startIndex">시작 인덱스입니다.</param>
    /// <returns>배열의 범위입니다.</returns>
    public static Span<T> AsSpan<T>(this T[]? array, Index startIndex)
    {
        if (array == null)
        {
            return default;
        }

        int actualIndex = startIndex.GetOffset(array.Length);
        return array.AsSpan(actualIndex);
    }

    /// <summary>
    /// 배열의 지정된 범위에서 Memory를 만듭니다.
    /// </summary>
    /// <typeparam name="T">배열 요소 형식입니다.</typeparam>
    /// <param name="array">소스 배열입니다.</param>
    /// <param name="range">범위입니다.</param>
    /// <returns>배열 범위에서 만들어진 Memory입니다.</returns>
    public static Memory<T> AsMemory<T>(this T[]? array, Range range)
    {
        if (array == null)
        {
            return default;
        }

        var (offset, length) = range.GetOffsetAndLength(array.Length);
        return new Memory<T>(array, offset, length);
    }

    /// <summary>
    /// 배열의 Index 위치에서 시작하는 Memory를 가져옵니다.
    /// </summary>
    /// <typeparam name="T">배열 요소 형식입니다.</typeparam>
    /// <param name="array">소스 배열입니다.</param>
    /// <param name="startIndex">시작 인덱스입니다.</param>
    /// <returns>배열의 Memory입니다.</returns>
    public static Memory<T> AsMemory<T>(this T[]? array, Index startIndex)
    {
        if (array == null)
        {
            return default;
        }

        int actualIndex = startIndex.GetOffset(array.Length);
        return array.AsMemory(actualIndex);
    }

    /// <summary>
    /// 문자열의 Range 범위에 해당하는 ReadOnlyMemory를 가져옵니다.
    /// </summary>
    /// <param name="text">소스 문자열입니다.</param>
    /// <param name="range">범위입니다.</param>
    /// <returns>문자열의 ReadOnlyMemory입니다.</returns>
    public static ReadOnlyMemory<char> AsMemory(this string? text, Range range)
    {
        if (text == null)
        {
            return default;
        }

        var (offset, length) = range.GetOffsetAndLength(text.Length);
        return text.AsMemory(offset, length);
    }

    /// <summary>
    /// 문자열의 Index 위치에서 시작하는 ReadOnlyMemory를 가져옵니다.
    /// </summary>
    /// <param name="text">소스 문자열입니다.</param>
    /// <param name="startIndex">시작 인덱스입니다.</param>
    /// <returns>문자열의 ReadOnlyMemory입니다.</returns>
    public static ReadOnlyMemory<char> AsMemory(this string? text, Index startIndex)
    {
        if (text == null)
        {
            return default;
        }

        int actualIndex = startIndex.GetOffset(text.Length);
        return text.AsMemory(actualIndex);
    }

    /// <summary>
    /// 문자 범위를 대문자로 변환하여 대상에 복사합니다.
    /// </summary>
    /// <param name="source">소스 문자 범위입니다.</param>
    /// <param name="destination">대상 문자 범위입니다.</param>
    /// <returns>변환된 문자 수입니다.</returns>
    public static int ToUpperInvariant(this ReadOnlySpan<char> source, Span<char> destination)
    {
        if (source.Length > destination.Length)
        {
            throw new ArgumentException("대상이 너무 짧습니다.");
        }

        for (int i = 0; i < source.Length; i++)
        {
            destination[i] = char.ToUpperInvariant(source[i]);
        }

        return source.Length;
    }

    /// <summary>
    /// 문자 범위를 소문자로 변환하여 대상에 복사합니다.
    /// </summary>
    /// <param name="source">소스 문자 범위입니다.</param>
    /// <param name="destination">대상 문자 범위입니다.</param>
    /// <returns>변환된 문자 수입니다.</returns>
    public static int ToLowerInvariant(this ReadOnlySpan<char> source, Span<char> destination)
    {
        if (source.Length > destination.Length)
        {
            throw new ArgumentException("대상이 너무 짧습니다.");
        }

        for (int i = 0; i < source.Length; i++)
        {
            destination[i] = char.ToLowerInvariant(source[i]);
        }

        return source.Length;
    }

    /// <summary>
    /// 문자 범위가 비어 있거나 공백 문자만 포함하는지 확인합니다.
    /// </summary>
    /// <param name="span">확인할 범위입니다.</param>
    /// <returns>비어 있거나 공백만 포함하면 true, 그렇지 않으면 false입니다.</returns>
    public static bool IsWhiteSpace(this ReadOnlySpan<char> span)
    {
        for (int i = 0; i < span.Length; i++)
        {
            if (!char.IsWhiteSpace(span[i]))
            {
                return false;
            }
        }

        return true;
    }
}

#endif
