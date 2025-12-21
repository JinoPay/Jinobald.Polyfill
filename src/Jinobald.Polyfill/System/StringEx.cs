using System.Globalization;

#if !NET9_0_OR_GREATER

namespace System;

internal static partial class StringEx
{
    extension(string)
    {
#if FeatureMemory
#if !NET9_0_OR_GREATER

        /// <summary>
        /// 지정된 구분 기호를 각 멤버 사이에 사용하여 개체 범위의 문자열 표현을 연결합니다.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join(system-char-system-readonlyspan((system-object)))
        public static string Join(char separator, scoped ReadOnlySpan<object?> values) =>
            Join(separator, values.ToArray());

        /// <summary>
        /// 지정된 구분 기호를 각 멤버 사이에 사용하여 문자열 범위를 연결합니다.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join(system-char-system-readonlyspan((system-string)))
        public static string Join(char separator, scoped ReadOnlySpan<string?> values)
        {
            if (values.Length == 0)
            {
                return string.Empty;
            }

            if (values.Length == 1)
            {
                return values[0] ?? string.Empty;
            }

#if AllowUnsafeBlocks
            var length = 0;

            foreach (var value in values)
            {
                length += 1;
                if (value != null)
                {
                    length += value.Length;
                }
            }

            length -= 1;

            var result = new string(separator, length);

            unsafe
            {
                fixed (char* strPtr = result)
                {
                    var span = new Span<char>(strPtr, length);

                    for (var index = 0; index < values.Length; index++)
                    {
                        if (index > 0)
                        {
                            span = span.Slice(1);
                        }

                        var value = values[index];

                        if (value != null)
                        {
                            value.AsSpan().CopyTo(span);
                            span = span.Slice(value.Length);
                        }
                    }
                }
            }

            return result;
#else
            return Join(separator, values.ToArray());
#endif
        }

        /// <summary>
        /// 지정된 구분 기호를 각 멤버 사이에 사용하여 문자열 범위를 연결합니다.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join(system-string-system-readonlyspan((system-string)))
        public static string Join(string? separator, scoped ReadOnlySpan<string?> values)
        {
            if (values.Length == 0)
            {
                return string.Empty;
            }

            if (values.Length == 1)
            {
                return values[0] ?? string.Empty;
            }

#if AllowUnsafeBlocks
            separator ??= string.Empty;

            var length = 0;

            foreach (var value in values)
            {
                length += separator.Length;
                if (value != null)
                {
                    length += value.Length;
                }
            }

            length -= separator.Length;

            var result = new string('\0', length);

            unsafe
            {
                fixed (char* strPtr = result)
                {
                    var span = new Span<char>(strPtr, length);

                    for (var index = 0; index < values.Length; index++)
                    {
                        if (index > 0 &&
                            separator.Length > 0)
                        {
                            separator.AsSpan().CopyTo(span);

                            span = span.Slice(separator.Length);
                        }

                        var value = values[index];

                        if (value is null)
                        {
                            continue;
                        }

                        value.AsSpan().CopyTo(span);

                        span = span.Slice(value.Length);
                    }
                }
            }

            return result;
#else
            return string.Join(separator, values.ToArray());
#endif
        }
#endif

#if !NETCOREAPP3_0_OR_GREATER
        /// <summary>
        /// 제공된 읽기 전용 문자 범위에 대한 해시 코드를 반환합니다.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.gethashcode?view=net-10.0#system-string-gethashcode(system-readonlyspan((system-char)))
        public static int GetHashCode(ReadOnlySpan<char> value) =>
            value.ToString().GetHashCode();

        /// <summary>
        /// 지정된 규칙을 사용하여 제공된 읽기 전용 문자 범위에 대한 해시 코드를 반환합니다.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.gethashcode?view=net-10.0#system-string-gethashcode(system-readonlyspan((system-char))-system-stringcomparison)
        public static int GetHashCode(ReadOnlySpan<char> value,StringComparison comparisonType)
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
#endif
#endif

#if !NET9_0_OR_GREATER && FeatureMemory
        /// <summary>
        /// 지정된 구분 기호를 각 멤버 사이에 사용하여 개체 범위의 문자열 표현을 연결합니다.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join(system-string-system-readonlyspan((system-object)))
        public static string Join(string? separator, scoped ReadOnlySpan<object?> values) =>
            string.Join(separator, values.ToArray());
#endif

#if NETSTANDARD2_0 || NETFRAMEWORK

        /// <summary>
        ///     지정된 구분 기호를 각 멤버 사이에 사용하여 문자열 배열을 연결합니다.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join(system-char-system-string())
        public static string Join(char separator, params string?[] values) =>
#if AllowUnsafeBlocks && FeatureMemory
            Join(separator, new ReadOnlySpan<string?>(values));
#else
            string.Join(new string(separator, 1), values);
#endif

        /// <summary>
        ///     지정된 구분 기호를 각 멤버 사이에 사용하여 개체 배열의 문자열 표현을 연결합니다.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join(system-char-system-object())
        public static string Join(char separator, params object?[] values)
        {
#if NET35
            var stringValues = new string?[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                stringValues[i] = values[i]?.ToString();
            }
            return string.Join(new string(separator, 1), stringValues);
#else
            return string.Join(new string(separator, 1), values);
#endif
        }

        /// <summary>
        ///     지정된 구분 기호를 각 요소 사이에 사용하여 문자열 배열의 지정된 요소를 연결합니다.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join(system-char-system-string()-system-int32-system-int32)
        public static string Join(char separator, string?[] value, int startIndex, int count) =>
#if AllowUnsafeBlocks && FeatureMemory
            Join(separator, new ReadOnlySpan<string?>(value, startIndex, count));
#else
            string.Join(new string(separator, 1), value, startIndex, count);
#endif

        /// <summary>
        ///     지정된 구분 기호를 각 요소 사이에 사용하여 문자열 배열의 지정된 요소를 연결합니다.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join-1(system-char-system-collections-generic-ienumerable((-0)))
        public static string Join<T>(char separator, IEnumerable<T> values)
        {
#if NET35
            var list = new List<string?>();
            foreach (var value in values)
            {
                list.Add(value?.ToString());
            }
            return string.Join(new string(separator, 1), list.ToArray());
#else
            return string.Join(new string(separator, 1), values);
#endif
        }
#endif

#if NETFRAMEWORK || NETSTANDARD2_0 || NETCOREAPP2X
#if FeatureMemory
        /// <summary>
        /// 지정된 길이로 새 문자열을 만들고 지정된 콜백을 사용하여 만든 후 초기화합니다.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.create?view=net-10.0#system-string-create-1(system-int32-0-system-buffers-spanaction((system-char-0)))
        public static string Create<TState>(int length, TState state, System.Buffers.SpanAction<char, TState> action)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (length == 0)
            {
                return string.Empty;
            }

#if AllowUnsafeBlocks
            var str = new string('\0', length);

            unsafe
            {
                fixed (char* strPtr = str)
                {
                    action(new Span<char>(strPtr, length), state);
                }
            }

            return str;
#else
            var pool = System.Buffers.ArrayPool<char>.Shared;
            var chars = pool.Rent(length);

            try
            {
                var span = chars.AsSpan(0, length);
                // IMPORTANT: Clear the span to avoid garbage data from pooled buffer
                // ArrayPool doesn't clear buffers for performance
                span.Clear();
                action(span, state);

                return new string(chars, 0, length);
            }
            finally
            {
                pool.Return(chars);
            }
#endif
        }
#endif
#endif

#if !NETCOREAPP2_1_OR_GREATER
        /// <summary>
        ///     지정된 문자가 이 문자열 내에 있는지 여부를 나타내는 값을 반환합니다.
        /// </summary>
        public static bool Contains(string str, char value)
        {
            return str.IndexOf(value) >= 0;
        }

#if !NETCOREAPP3_0_OR_GREATER
        /// <summary>
        ///     지정된 비교 규칙을 사용하여 지정된 부분 문자열이 이 문자열 내에 있는지 여부를 나타내는 값을 반환합니다.
        /// </summary>
        public static bool Contains(string str, string value, StringComparison comparisonType)
        {
            return str.IndexOf(value, comparisonType) >= 0;
        }
#endif

        /// <summary>
        ///     지정된 비교 규칙을 사용하여 지정된 문자가 이 문자열 내에 있는지 여부를 나타내는 값을 반환합니다.
        /// </summary>
        public static bool Contains(string str, char value, StringComparison comparisonType)
        {
            return str.IndexOf(value.ToString(), comparisonType) >= 0;
        }
#endif

#if !NETCOREAPP3_0_OR_GREATER
        /// <summary>
        ///     이 문자열 인스턴스가 지정된 문자로 시작하는지 여부를 확인합니다.
        /// </summary>
        public static bool StartsWith(string str, char value)
        {
            return str.Length > 0 && str[0] == value;
        }

        /// <summary>
        ///     이 문자열 인스턴스의 끝이 지정된 문자와 일치하는지 여부를 확인합니다.
        /// </summary>
        public static bool EndsWith(string str, char value)
        {
            return str.Length > 0 && str[str.Length - 1] == value;
        }

        /// <summary>
        ///     지정된 비교 규칙을 사용하여 이 문자열의 해시 코드를 반환합니다.
        /// </summary>
        public static int GetHashCode(string str, StringComparison comparisonType)
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
#endif

#if !NETCOREAPP2_1_OR_GREATER
        /// <summary>
        ///     제공된 비교 형식을 사용하여 지정된 문자열의 모든 항목을 다른 지정된 문자열로 바꾼 새 문자열을 반환합니다.
        /// </summary>
        public static string Replace(string str, string oldValue, string? newValue, StringComparison comparisonType)
        {
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }

            if (oldValue.Length == 0)
            {
                throw new ArgumentException("문자열 길이가 0일 수 없습니다.", nameof(oldValue));
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
        ///     제공된 문화권 및 대/소문자 구분을 사용하여 지정된 문자열의 모든 항목을 다른 지정된 문자열로 바꾼 새 문자열을 반환합니다.
        /// </summary>
        public static string Replace(string str, string oldValue, string? newValue, bool ignoreCase,
            CultureInfo? culture)
        {
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }

            if (oldValue.Length == 0)
            {
                throw new ArgumentException("문자열 길이가 0일 수 없습니다.", nameof(oldValue));
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
#endif

#if !NET5_0_OR_GREATER
        /// <summary>
        ///     지정된 구분 문자를 기준으로 문자열을 부분 문자열로 분할합니다.
        /// </summary>
        public static string[] Split(string str, char separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return str.Split(new[] { separator }, options);
        }

        /// <summary>
        ///     지정된 구분 문자를 기준으로 문자열을 최대 개수의 부분 문자열로 분할합니다.
        /// </summary>
        public static string[] Split(string str, char separator, int count,
            StringSplitOptions options = StringSplitOptions.None)
        {
            return str.Split(new[] { separator }, count, options);
        }

        /// <summary>
        ///     지정된 구분 문자열을 기준으로 문자열을 부분 문자열로 분할합니다.
        /// </summary>
        public static string[] Split(string str, string? separator,
            StringSplitOptions options = StringSplitOptions.None)
        {
            return string.IsNullOrEmpty(separator)
                ? str.Split((char[]?)null, options)
                : str.Split(new[] { separator }, options);
        }

        /// <summary>
        ///     지정된 구분 문자열을 기준으로 문자열을 최대 개수의 부분 문자열로 분할합니다.
        /// </summary>
        public static string[] Split(string str, string? separator, int count,
            StringSplitOptions options = StringSplitOptions.None)
        {
            return string.IsNullOrEmpty(separator)
                ? str.Split((char[]?)null, count, options)
                : str.Split(new[] { separator }, count, options);
        }

        /// <summary>
        ///     현재 문자열에서 문자의 모든 선행 및 후행 인스턴스를 제거합니다.
        /// </summary>
        public static string Trim(string str, char trimChar)
        {
            return str.Trim(new[] { trimChar });
        }

        /// <summary>
        ///     현재 문자열에서 문자의 모든 선행 인스턴스를 제거합니다.
        /// </summary>
        public static string TrimStart(string str, char trimChar)
        {
            return str.TrimStart(new[] { trimChar });
        }

        /// <summary>
        ///     현재 문자열에서 문자의 모든 후행 인스턴스를 제거합니다.
        /// </summary>
        public static string TrimEnd(string str, char trimChar)
        {
            return str.TrimEnd(new[] { trimChar });
        }

        /// <summary>
        ///     현재 문자열의 모든 줄 바꿈 시퀀스를 Environment.NewLine으로 바꿉니다.
        /// </summary>
        public static string ReplaceLineEndings(string str)
        {
            return ReplaceLineEndings(str, Environment.NewLine);
        }

        /// <summary>
        ///     현재 문자열의 모든 줄 바꿈 시퀀스를 지정된 대체 텍스트로 바꿉니다.
        /// </summary>
        public static string ReplaceLineEndings(string str, string replacementText)
        {
            if (replacementText == null)
            {
                throw new ArgumentNullException(nameof(replacementText));
            }

            return str.Replace("\r\n", replacementText).Replace("\n", replacementText).Replace("\r", replacementText);
        }
#endif

#if NETFRAMEWORK && !NET40_OR_GREATER
        /// <summary>
        /// 지정된 문자열이 null이거나, 비어 있거나, 공백 문자로만 구성되어 있는지 여부를 나타냅니다.
        /// </summary>
        public static bool IsNullOrWhiteSpace(string? value)
        {
            if (value == null) return true;
            for (var i = 0; i < value.Length; i++)
                if (!char.IsWhiteSpace(value[i])) return false;
            return true;
        }
#endif
    }
}
#endif
