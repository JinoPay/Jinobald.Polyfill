#if NETFRAMEWORK

namespace System;

internal static partial class StringEx
{
    // 인스턴스 메서드
    extension(string str)
    {
        /// <summary>
        /// 지정된 비교 규칙을 사용하여 이 문자열의 해시 코드를 반환합니다.
        /// </summary>
        /// <param name="comparisonType">비교에 사용할 규칙을 지정하는 열거형 값 중 하나입니다.</param>
        /// <returns>부호 있는 32비트 정수 해시 코드입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.gethashcode#system-string-gethashcode(system-stringcomparison)
        public int GetHashCode(StringComparison comparisonType)
        {
            string s = comparisonType switch
            {
                StringComparison.CurrentCultureIgnoreCase => str.ToLower(),
                StringComparison.OrdinalIgnoreCase => str.ToUpperInvariant(),
                StringComparison.InvariantCultureIgnoreCase => str.ToUpperInvariant(),
                _ => str
            };
            return s.GetHashCode();
        }
    }

#if FeatureMemory
    // 정적 메서드 (ReadOnlySpan 버전)
    extension(string)
    {
        /// <summary>
        /// 제공된 읽기 전용 문자 범위에 대한 해시 코드를 반환합니다.
        /// </summary>
        /// <param name="value">읽기 전용 문자 범위입니다.</param>
        /// <returns>부호 있는 32비트 정수 해시 코드입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.gethashcode#system-string-gethashcode(system-readonlyspan((system-char)))
        public static int GetHashCode(ReadOnlySpan<char> value)
        {
            return value.ToString().GetHashCode();
        }

        /// <summary>
        /// 지정된 규칙을 사용하여 제공된 읽기 전용 문자 범위에 대한 해시 코드를 반환합니다.
        /// </summary>
        /// <param name="value">읽기 전용 문자 범위입니다.</param>
        /// <param name="comparisonType">비교에 사용할 규칙을 지정하는 열거형 값 중 하나입니다.</param>
        /// <returns>부호 있는 32비트 정수 해시 코드입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.gethashcode#system-string-gethashcode(system-readonlyspan((system-char))-system-stringcomparison)
        public static int GetHashCode(ReadOnlySpan<char> value, StringComparison comparisonType)
        {
            var str = value.ToString();
            var s = comparisonType switch
            {
                StringComparison.CurrentCultureIgnoreCase => str.ToLower(),
                StringComparison.OrdinalIgnoreCase => str.ToUpperInvariant(),
                StringComparison.InvariantCultureIgnoreCase => str.ToUpperInvariant(),
                _ => str
            };
            return s.GetHashCode();
        }
    }
#endif
}

#endif
