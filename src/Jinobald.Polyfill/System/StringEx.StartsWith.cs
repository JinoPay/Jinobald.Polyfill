#if NETFRAMEWORK

namespace System;

internal static partial class StringEx
{
    extension(string str)
    {
        /// <summary>
        /// 이 문자열 인스턴스가 지정된 문자로 시작하는지 여부를 확인합니다.
        /// </summary>
        /// <param name="value">비교할 문자입니다.</param>
        /// <returns>value가 이 문자열의 시작 부분과 일치하면 true이고, 그렇지 않으면 false입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.startswith#system-string-startswith(system-char)
        public bool StartsWith(char value)
        {
            return str.Length > 0 && str[0] == value;
        }

        /// <summary>
        /// 이 문자열 인스턴스의 끝이 지정된 문자와 일치하는지 여부를 확인합니다.
        /// </summary>
        /// <param name="value">비교할 문자입니다.</param>
        /// <returns>value가 이 문자열의 끝 부분과 일치하면 true이고, 그렇지 않으면 false입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.endswith#system-string-endswith(system-char)
        public bool EndsWith(char value)
        {
            return str.Length > 0 && str[str.Length - 1] == value;
        }
    }
}

#endif
