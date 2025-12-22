#if NETFRAMEWORK

using System.Globalization;

namespace System;

internal static partial class StringEx
{
    extension(string str)
    {
        /// <summary>
        /// 제공된 비교 형식을 사용하여 지정된 문자열의 모든 항목을 다른 지정된 문자열로 바꾼 새 문자열을 반환합니다.
        /// </summary>
        /// <param name="oldValue">바꿀 문자열입니다.</param>
        /// <param name="newValue">oldValue의 모든 항목을 바꿀 문자열입니다.</param>
        /// <param name="comparisonType">비교 규칙을 지정하는 열거형 값 중 하나입니다.</param>
        /// <returns>oldValue의 모든 인스턴스가 newValue로 바뀐 현재 문자열과 동일한 새 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.replace#system-string-replace(system-string-system-string-system-stringcomparison)
        public string Replace(string oldValue, string? newValue, StringComparison comparisonType)
        {
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }

            if (oldValue.Length == 0)
            {
                throw new ArgumentException("String cannot be of zero length.", nameof(oldValue));
            }

            newValue ??= string.Empty;

            if (comparisonType == StringComparison.Ordinal)
            {
                return str.Replace(oldValue, newValue);
            }

            string result = str;
            int index = 0;

            while ((index = result.IndexOf(oldValue, index, comparisonType)) != -1)
            {
                result = result.Remove(index, oldValue.Length).Insert(index, newValue);
                index += newValue.Length;
            }

            return result;
        }

        /// <summary>
        /// 제공된 문화권 및 대/소문자 구분을 사용하여 지정된 문자열의 모든 항목을 다른 지정된 문자열로 바꾼 새 문자열을 반환합니다.
        /// </summary>
        /// <param name="oldValue">바꿀 문자열입니다.</param>
        /// <param name="newValue">oldValue의 모든 항목을 바꿀 문자열입니다.</param>
        /// <param name="ignoreCase">비교할 때 대/소문자를 무시할지 여부입니다.</param>
        /// <param name="culture">비교에 사용할 문화권입니다. culture가 null이면 현재 문화권이 사용됩니다.</param>
        /// <returns>oldValue의 모든 인스턴스가 newValue로 바뀐 현재 문자열과 동일한 새 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.replace#system-string-replace(system-string-system-string-system-boolean-system-globalization-cultureinfo)
        public string Replace(string oldValue, string? newValue, bool ignoreCase, CultureInfo? culture)
        {
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }

            if (oldValue.Length == 0)
            {
                throw new ArgumentException("String cannot be of zero length.", nameof(oldValue));
            }

            newValue ??= string.Empty;
            culture ??= CultureInfo.CurrentCulture;

            string result = str;
            int index = 0;

            while ((index = culture.CompareInfo.IndexOf(result, oldValue, index,
                       ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None)) != -1)
            {
                result = result.Remove(index, oldValue.Length).Insert(index, newValue);
                index += newValue.Length;
            }

            return result;
        }
    }
}

#endif
