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
