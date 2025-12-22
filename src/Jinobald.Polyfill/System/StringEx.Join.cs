namespace System;

internal static partial class StringEx
{
    extension(string)
    {
#if NETFRAMEWORK
        /// <summary>
        /// 지정된 구분 기호를 각 멤버 사이에 사용하여 문자열 배열을 연결합니다.
        /// </summary>
        /// <param name="separator">구분 기호로 사용할 문자입니다.</param>
        /// <param name="values">연결할 문자열 배열입니다.</param>
        /// <returns>separator 문자로 구분된 values의 멤버로 구성된 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join#system-string-join(system-char-system-string())
        public static string Join(char separator, params string?[] values)
        {
#if AllowUnsafeBlocks
            return Join(separator, new ReadOnlySpan<string?>(values));
#else
            return string.Join(new string(separator, 1), values);
#endif
        }

        /// <summary>
        /// 지정된 구분 기호를 각 멤버 사이에 사용하여 개체 배열의 문자열 표현을 연결합니다.
        /// </summary>
        /// <param name="separator">구분 기호로 사용할 문자입니다.</param>
        /// <param name="values">연결할 개체 배열입니다.</param>
        /// <returns>separator 문자로 구분된 values 요소의 문자열 표현으로 구성된 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join#system-string-join(system-char-system-object())
        public static string Join(char separator, params object?[] values)
        {
#if NET35
            string?[] stringValues = new string?[values.Length];
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
        /// 지정된 구분 기호를 각 요소 사이에 사용하여 문자열 배열의 지정된 요소를 연결합니다.
        /// </summary>
        /// <param name="separator">구분 기호로 사용할 문자입니다.</param>
        /// <param name="value">연결할 문자열 배열입니다.</param>
        /// <param name="startIndex">사용할 value의 첫 번째 요소입니다.</param>
        /// <param name="count">사용할 value의 요소 수입니다.</param>
        /// <returns>separator 문자로 구분된 value의 문자열로 구성된 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join#system-string-join(system-char-system-string()-system-int32-system-int32)
        public static string Join(char separator, string?[] value, int startIndex, int count)
        {
#if AllowUnsafeBlocks
            return Join(separator, new ReadOnlySpan<string?>(value, startIndex, count));
#else
            return string.Join(new string(separator, 1), value, startIndex, count);
#endif
        }

        /// <summary>
        /// 지정된 구분 기호를 각 요소 사이에 사용하여 컬렉션의 멤버를 연결합니다.
        /// </summary>
        /// <typeparam name="T">values 멤버의 형식입니다.</typeparam>
        /// <param name="separator">구분 기호로 사용할 문자입니다.</param>
        /// <param name="values">연결할 개체 컬렉션입니다.</param>
        /// <returns>separator 문자로 구분된 values 멤버로 구성된 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join#system-string-join-1(system-char-system-collections-generic-ienumerable((-0)))
        public static string Join<T>(char separator, IEnumerable<T> values)
        {
#if NET35
            var list = new List<string?>();
            foreach (T value in values)
            {
                list.Add(value?.ToString());
            }
            return string.Join(new string(separator, 1), list.ToArray());
#else
            return string.Join(new string(separator, 1), values);
#endif
        }
#endif

#if !NET9_0_OR_GREATER
        /// <summary>
        /// 지정된 구분 기호를 각 멤버 사이에 사용하여 개체 범위의 문자열 표현을 연결합니다.
        /// </summary>
        /// <param name="separator">구분 기호로 사용할 문자입니다.</param>
        /// <param name="values">연결할 개체 범위입니다.</param>
        /// <returns>separator 문자로 구분된 values 요소의 문자열 표현으로 구성된 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join#system-string-join(system-char-system-readonlyspan((system-object)))
        public static string Join(char separator, scoped ReadOnlySpan<object?> values)
        {
            return Join(separator, values.ToArray());
        }

        /// <summary>
        /// 지정된 구분 기호를 각 멤버 사이에 사용하여 문자열 범위를 연결합니다.
        /// </summary>
        /// <param name="separator">구분 기호로 사용할 문자입니다.</param>
        /// <param name="values">연결할 문자열 범위입니다.</param>
        /// <returns>separator 문자로 구분된 values의 멤버로 구성된 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join#system-string-join(system-char-system-readonlyspan((system-string)))
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
        /// <param name="separator">구분 기호로 사용할 문자열입니다.</param>
        /// <param name="values">연결할 문자열 범위입니다.</param>
        /// <returns>separator로 구분된 values의 멤버로 구성된 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join#system-string-join(system-string-system-readonlyspan((system-string)))
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
                        if (index > 0 && separator.Length > 0)
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

        /// <summary>
        /// 지정된 구분 기호를 각 멤버 사이에 사용하여 개체 범위의 문자열 표현을 연결합니다.
        /// </summary>
        /// <param name="separator">구분 기호로 사용할 문자열입니다.</param>
        /// <param name="values">연결할 개체 범위입니다.</param>
        /// <returns>separator로 구분된 values 요소의 문자열 표현으로 구성된 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join#system-string-join(system-string-system-readonlyspan((system-object)))
        public static string Join(string? separator, scoped ReadOnlySpan<object?> values)
        {
            return string.Join(separator, values.ToArray());
        }
#endif
    }
}
