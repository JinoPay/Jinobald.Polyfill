#if NETFRAMEWORK

namespace System;

internal static partial class StringEx
{
    extension(string str)
    {
        /// <summary>
        /// 현재 문자열에서 지정된 문자의 모든 선행 및 후행 인스턴스를 제거합니다.
        /// </summary>
        /// <param name="trimChar">제거할 문자입니다.</param>
        /// <returns>trimChar 문자의 모든 인스턴스가 현재 문자열의 시작 및 끝에서 제거된 후 남아 있는 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.trim#system-string-trim(system-char)
        public string Trim(char trimChar)
        {
            return str.Trim(new[] { trimChar });
        }

        /// <summary>
        /// 현재 문자열에서 지정된 문자의 모든 선행 인스턴스를 제거합니다.
        /// </summary>
        /// <param name="trimChar">제거할 문자입니다.</param>
        /// <returns>trimChar 문자의 모든 인스턴스가 현재 문자열의 시작에서 제거된 후 남아 있는 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.trimstart#system-string-trimstart(system-char)
        public string TrimStart(char trimChar)
        {
            return str.TrimStart(new[] { trimChar });
        }

        /// <summary>
        /// 현재 문자열에서 지정된 문자의 모든 후행 인스턴스를 제거합니다.
        /// </summary>
        /// <param name="trimChar">제거할 문자입니다.</param>
        /// <returns>trimChar 문자의 모든 인스턴스가 현재 문자열의 끝에서 제거된 후 남아 있는 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.trimend#system-string-trimend(system-char)
        public string TrimEnd(char trimChar)
        {
            return str.TrimEnd(new[] { trimChar });
        }
    }
}

#endif
