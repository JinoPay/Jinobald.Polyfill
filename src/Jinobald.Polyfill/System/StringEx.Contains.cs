#if NETFRAMEWORK

namespace System;

internal static partial class StringEx
{
    extension(string str)
    {
        /// <summary>
        /// 지정된 문자가 이 문자열 내에 있는지 여부를 나타내는 값을 반환합니다.
        /// </summary>
        /// <param name="value">찾을 문자입니다.</param>
        /// <returns>value 매개 변수가 이 문자열 내에서 발생하면 true이고, 그렇지 않으면 false입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.contains#system-string-contains(system-char)
        public bool Contains(char value)
        {
            return str.IndexOf(value) >= 0;
        }

        /// <summary>
        /// 지정된 비교 규칙을 사용하여 지정된 부분 문자열이 이 문자열 내에 있는지 여부를 나타내는 값을 반환합니다.
        /// </summary>
        /// <param name="value">찾을 문자열입니다.</param>
        /// <param name="comparisonType">검색 규칙을 지정하는 열거형 값 중 하나입니다.</param>
        /// <returns>value 매개 변수가 이 문자열 내에서 발생하면 true이고, 그렇지 않으면 false입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.contains#system-string-contains(system-string-system-stringcomparison)
        public bool Contains(string value, StringComparison comparisonType)
        {
            return str.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        /// 지정된 비교 규칙을 사용하여 지정된 문자가 이 문자열 내에 있는지 여부를 나타내는 값을 반환합니다.
        /// </summary>
        /// <param name="value">찾을 문자입니다.</param>
        /// <param name="comparisonType">검색 규칙을 지정하는 열거형 값 중 하나입니다.</param>
        /// <returns>value 매개 변수가 이 문자열 내에서 발생하면 true이고, 그렇지 않으면 false입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.contains#system-string-contains(system-char-system-stringcomparison)
        public bool Contains(char value, StringComparison comparisonType)
        {
            return str.IndexOf(value.ToString(), comparisonType) >= 0;
        }
    }
}

#endif
