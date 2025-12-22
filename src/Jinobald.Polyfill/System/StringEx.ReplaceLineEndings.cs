#if NETFRAMEWORK

namespace System;

internal static partial class StringEx
{
    extension(string str)
    {
        /// <summary>
        /// 현재 문자열의 모든 줄 바꿈 시퀀스를 Environment.NewLine으로 바꿉니다.
        /// </summary>
        /// <returns>모든 줄 바꿈 시퀀스가 Environment.NewLine으로 바뀐 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.replacelineendings#system-string-replacelineendings
        public string ReplaceLineEndings()
        {
            return ReplaceLineEndings(Environment.NewLine);
        }

        /// <summary>
        /// 현재 문자열의 모든 줄 바꿈 시퀀스를 지정된 대체 텍스트로 바꿉니다.
        /// </summary>
        /// <param name="replacementText">줄 바꿈 시퀀스를 대체할 텍스트입니다.</param>
        /// <returns>모든 줄 바꿈 시퀀스가 replacementText로 바뀐 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.replacelineendings#system-string-replacelineendings(system-string)
        public string ReplaceLineEndings(string replacementText)
        {
            if (replacementText == null)
            {
                throw new ArgumentNullException(nameof(replacementText));
            }

            // 줄 바꿈 순서 중요: \r\n을 먼저 처리해야 \r과 \n이 개별적으로 처리되지 않음
            return str
                .Replace("\r\n", replacementText)
                .Replace("\r", replacementText)
                .Replace("\n", replacementText);
        }
    }
}

#endif
