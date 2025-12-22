namespace System;

/// <summary>
/// String 클래스에 대한 폴리필 확장 메서드를 제공합니다.
/// </summary>
internal static partial class StringEx
{
#if NETFRAMEWORK && !NET40_OR_GREATER
    // 정적 메서드용 extension 블록
    extension(string)
    {
        /// <summary>
        /// 지정된 문자열이 null이거나, 비어 있거나, 공백 문자로만 구성되어 있는지 여부를 나타냅니다.
        /// </summary>
        /// <param name="value">테스트할 문자열입니다.</param>
        /// <returns>value 매개 변수가 null이거나 빈 문자열("")이거나 공백 문자로만 구성되어 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.isnullorwhitespace
        public static bool IsNullOrWhiteSpace(string? value)
        {
            if (value == null)
            {
                return true;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
#endif
}

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481

/// <summary>
/// String 클래스에 대한 Index 및 Range 기반 확장 메서드를 제공합니다.
/// </summary>
public static class StringRangeExtensions
{
    /// <summary>
    /// 지정된 Range에 해당하는 부분 문자열을 반환합니다.
    /// </summary>
    /// <param name="text">소스 문자열입니다.</param>
    /// <param name="range">범위입니다.</param>
    /// <returns>지정된 범위의 부분 문자열입니다.</returns>
    public static string Substring(this string text, Range range)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        var (offset, length) = range.GetOffsetAndLength(text.Length);
        return text.Substring(offset, length);
    }

    /// <summary>
    /// 지정된 Index부터 문자열 끝까지의 부분 문자열을 반환합니다.
    /// </summary>
    /// <param name="text">소스 문자열입니다.</param>
    /// <param name="startIndex">시작 인덱스입니다.</param>
    /// <returns>지정된 인덱스부터 끝까지의 부분 문자열입니다.</returns>
    public static string Substring(this string text, Index startIndex)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        int actualIndex = startIndex.GetOffset(text.Length);
        return text.Substring(actualIndex);
    }
}

#endif
