#if NETFRAMEWORK

namespace System;

internal static partial class StringEx
{
    extension(string str)
    {
        /// <summary>
        /// 지정된 구분 문자를 기준으로 문자열을 부분 문자열로 분할합니다.
        /// </summary>
        /// <param name="separator">이 문자열의 부분 문자열을 구분하는 문자입니다.</param>
        /// <param name="options">부분 문자열을 자르고 빈 부분 문자열을 포함할지 여부를 지정하는 열거형 값의 비트 조합입니다.</param>
        /// <returns>이 문자열을 separator 문자로 구분한 부분 문자열을 포함하는 배열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.split#system-string-split(system-char-system-stringsplitoptions)
        public string[] Split(char separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return str.Split(new[] { separator }, options);
        }

        /// <summary>
        /// 지정된 구분 문자를 기준으로 문자열을 최대 개수의 부분 문자열로 분할합니다.
        /// </summary>
        /// <param name="separator">이 문자열의 부분 문자열을 구분하는 문자입니다.</param>
        /// <param name="count">반환할 부분 문자열의 최대 개수입니다.</param>
        /// <param name="options">부분 문자열을 자르고 빈 부분 문자열을 포함할지 여부를 지정하는 열거형 값의 비트 조합입니다.</param>
        /// <returns>이 문자열을 separator 문자로 구분한 부분 문자열을 최대 count개 포함하는 배열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.split#system-string-split(system-char-system-int32-system-stringsplitoptions)
        public string[] Split(char separator, int count, StringSplitOptions options = StringSplitOptions.None)
        {
            return str.Split(new[] { separator }, count, options);
        }

        /// <summary>
        /// 지정된 구분 문자열을 기준으로 문자열을 부분 문자열로 분할합니다.
        /// </summary>
        /// <param name="separator">이 문자열의 부분 문자열을 구분하는 문자열입니다.</param>
        /// <param name="options">부분 문자열을 자르고 빈 부분 문자열을 포함할지 여부를 지정하는 열거형 값의 비트 조합입니다.</param>
        /// <returns>이 문자열을 separator 문자열로 구분한 부분 문자열을 포함하는 배열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.split#system-string-split(system-string-system-stringsplitoptions)
        public string[] Split(string? separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return string.IsNullOrEmpty(separator)
                ? str.Split((char[]?)null, options)
                : str.Split(new[] { separator }, options);
        }

        /// <summary>
        /// 지정된 구분 문자열을 기준으로 문자열을 최대 개수의 부분 문자열로 분할합니다.
        /// </summary>
        /// <param name="separator">이 문자열의 부분 문자열을 구분하는 문자열입니다.</param>
        /// <param name="count">반환할 부분 문자열의 최대 개수입니다.</param>
        /// <param name="options">부분 문자열을 자르고 빈 부분 문자열을 포함할지 여부를 지정하는 열거형 값의 비트 조합입니다.</param>
        /// <returns>이 문자열을 separator 문자열로 구분한 부분 문자열을 최대 count개 포함하는 배열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.split#system-string-split(system-string-system-int32-system-stringsplitoptions)
        public string[] Split(string? separator, int count, StringSplitOptions options = StringSplitOptions.None)
        {
            return string.IsNullOrEmpty(separator)
                ? str.Split((char[]?)null, count, options)
                : str.Split(new[] { separator }, count, options);
        }
    }
}

#endif
