#if !NET9_0_OR_GREATER

namespace System;

using System;
using System.Collections.Generic;

static partial class StringEx
{
    extension(string)
    {
#if FeatureMemory

#if !NET9_0_OR_GREATER

        /// <summary>
        /// Concatenates the string representations of a span of objects, using the specified separator between each member.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join(system-char-system-readonlyspan((system-object)))
        public static string Join(char separator, scoped ReadOnlySpan<object?> values) =>
            Join(separator, values.ToArray());

        /// <summary>
        /// Concatenates a span of strings, using the specified separator between each member.
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

                        value.CopyTo(span);

                        span = span.Slice(value.Length);
                    }
                }
            }

            return result;
#else
            return Join(separator, values.ToArray());
#endif
        }

        /// <summary>
        /// Concatenates a span of strings, using the specified separator between each member.
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
                            separator.CopyTo(span);

                            span = span.Slice(separator.Length);
                        }

                        var value = values[index];

                        if (value is null)
                        {
                            continue;
                        }

                        value.CopyTo(span);

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
        /// Returns the hash code for the provided read-only character span.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.gethashcode?view=net-10.0#system-string-gethashcode(system-readonlyspan((system-char)))
        public static int GetHashCode(ReadOnlySpan<char> value) =>
            value.ToString().GetHashCode();

        /// <summary>
        /// Returns the hash code for the provided read-only character span using the specified rules.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.gethashcode?view=net-10.0#system-string-gethashcode(system-readonlyspan((system-char))-system-stringcomparison)
        public static int GetHashCode(ReadOnlySpan<char> value,StringComparison comparisonType) =>
            value.ToString().GetHashCode(comparisonType);
#endif
#endif

#if !NET9_0_OR_GREATER && FeatureMemory
        /// <summary>
        /// Concatenates the string representations of a span of objects, using the specified separator between each member.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join(system-string-system-readonlyspan((system-object)))
        public static string Join(string? separator, scoped ReadOnlySpan<object?> values) =>
            string.Join(separator, values.ToArray());
#endif

#if NETSTANDARD2_0 || NETFRAMEWORK

        /// <summary>
        /// Concatenates an array of strings, using the specified separator between each member.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join(system-char-system-string())
        public static string Join(char separator, params string?[] values) =>
#if AllowUnsafeBlocks && FeatureMemory
            Join(separator, new ReadOnlySpan<string?>(values));
#else
            string.Join(new string(separator, 1), values);
#endif

        /// <summary>
        /// Concatenates the string representations of an array of objects, using the specified separator between each member.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join(system-char-system-object())
        public static string Join(char separator, params object?[] values) =>
            string.Join(new string(separator, 1), values);

        /// <summary>
        /// Concatenates the specified elements of a string array, using the specified separator between each element.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join(system-char-system-string()-system-int32-system-int32)
        public static string Join(char separator, string?[] value, int startIndex, int count) =>
#if AllowUnsafeBlocks && FeatureMemory
            Join(separator, new ReadOnlySpan<string?>(value, startIndex, count));
#else
            string.Join(new string(separator, 1), value, startIndex, count);
#endif

        /// <summary>
        /// Concatenates the specified elements of a string array, using the specified separator between each element.
        /// </summary>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.join?view=net-10.0#system-string-join-1(system-char-system-collections-generic-ienumerable((-0)))
        public static string Join<T>(char separator, IEnumerable<T> values) =>
            string.Join(new string(separator, 1), values);
#endif

#if NETFRAMEWORK || NETSTANDARD2_0 || NETCOREAPP2X
#if FeatureMemory
        /// <summary>
        /// Creates a new string with a specific length and initializes it after creation by using the specified callback.
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
        /// Returns a value indicating whether a specified character occurs within this string.
        /// </summary>
        public static bool Contains(string str, char value) =>
            str.IndexOf(value) >= 0;

#if !NETCOREAPP3_0_OR_GREATER
        /// <summary>
        /// Returns a value indicating whether a specified substring occurs within this string, using the specified comparison rules.
        /// </summary>
        public static bool Contains(string str, string value, StringComparison comparisonType) =>
            str.IndexOf(value, comparisonType) >= 0;
#endif

        /// <summary>
        /// Returns a value indicating whether a specified character occurs within this string, using the specified comparison rules.
        /// </summary>
        public static bool Contains(string str, char value, StringComparison comparisonType) =>
            str.IndexOf(value.ToString(), comparisonType) >= 0;
#endif

#if !NETCOREAPP3_0_OR_GREATER
        /// <summary>
        /// Determines whether this string instance starts with the specified character.
        /// </summary>
        public static bool StartsWith(string str, char value) =>
            str.Length > 0 && str[0] == value;

        /// <summary>
        /// Determines whether the end of this string instance matches the specified character.
        /// </summary>
        public static bool EndsWith(string str, char value) =>
            str.Length > 0 && str[str.Length - 1] == value;

        /// <summary>
        /// Returns the hash code for this string using the specified comparison rules.
        /// </summary>
        public static int GetHashCode(string str, StringComparison comparisonType)
        {
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

#if !NETCOREAPP2_1_OR_GREATER
        /// <summary>
        /// Returns a new string in which all occurrences of a specified string are replaced with another specified string, using the provided comparison type.
        /// </summary>
        public static string Replace(string str, string oldValue, string? newValue, StringComparison comparisonType)
        {
            if (oldValue == null) throw new ArgumentNullException(nameof(oldValue));
            if (oldValue.Length == 0) throw new ArgumentException("String cannot be of zero length.", nameof(oldValue));

            newValue ??= string.Empty;
            if (comparisonType == StringComparison.Ordinal) return str.Replace(oldValue, newValue);

            var result = str;
            var index = 0;
            while ((index = result.IndexOf(oldValue, index, comparisonType)) != -1)
            {
                result = result.Remove(index, oldValue.Length).Insert(index, newValue);
                index += newValue.Length;
            }
            return result;
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string are replaced with another specified string, using the provided culture and case sensitivity.
        /// </summary>
        public static string Replace(string str, string oldValue, string? newValue, bool ignoreCase, System.Globalization.CultureInfo? culture)
        {
            if (oldValue == null) throw new ArgumentNullException(nameof(oldValue));
            if (oldValue.Length == 0) throw new ArgumentException("String cannot be of zero length.", nameof(oldValue));

            newValue ??= string.Empty;
            culture ??= System.Globalization.CultureInfo.CurrentCulture;

            var result = str;
            var index = 0;
            while ((index = culture.CompareInfo.IndexOf(result, oldValue, index, ignoreCase ? System.Globalization.CompareOptions.IgnoreCase : System.Globalization.CompareOptions.None)) != -1)
            {
                result = result.Remove(index, oldValue.Length).Insert(index, newValue);
                index += newValue.Length;
            }
            return result;
        }
#endif

#if !NET5_0_OR_GREATER
        /// <summary>
        /// Splits a string into substrings based on a specified delimiting character.
        /// </summary>
        public static string[] Split(string str, char separator, StringSplitOptions options = StringSplitOptions.None) =>
            str.Split(new[] { separator }, options);

        /// <summary>
        /// Splits a string into a maximum number of substrings based on a specified delimiting character.
        /// </summary>
        public static string[] Split(string str, char separator, int count, StringSplitOptions options = StringSplitOptions.None) =>
            str.Split(new[] { separator }, count, options);

        /// <summary>
        /// Splits a string into substrings based on a specified delimiting string.
        /// </summary>
        public static string[] Split(string str, string? separator, StringSplitOptions options = StringSplitOptions.None) =>
            string.IsNullOrEmpty(separator) ? str.Split((char[]?)null, options) : str.Split(new[] { separator }, options);

        /// <summary>
        /// Splits a string into a maximum number of substrings based on a specified delimiting string.
        /// </summary>
        public static string[] Split(string str, string? separator, int count, StringSplitOptions options = StringSplitOptions.None) =>
            string.IsNullOrEmpty(separator) ? str.Split((char[]?)null, count, options) : str.Split(new[] { separator }, count, options);

        /// <summary>
        /// Removes all leading and trailing instances of a character from the current string.
        /// </summary>
        public static string Trim(string str, char trimChar) =>
            str.Trim(new[] { trimChar });

        /// <summary>
        /// Removes all leading instances of a character from the current string.
        /// </summary>
        public static string TrimStart(string str, char trimChar) =>
            str.TrimStart(new[] { trimChar });

        /// <summary>
        /// Removes all trailing instances of a character from the current string.
        /// </summary>
        public static string TrimEnd(string str, char trimChar) =>
            str.TrimEnd(new[] { trimChar });

        /// <summary>
        /// Replaces all newline sequences in the current string with Environment.NewLine.
        /// </summary>
        public static string ReplaceLineEndings(string str) =>
            ReplaceLineEndings(str, Environment.NewLine);

        /// <summary>
        /// Replaces all newline sequences in the current string with the specified replacement text.
        /// </summary>
        public static string ReplaceLineEndings(string str, string replacementText)
        {
            if (replacementText == null) throw new ArgumentNullException(nameof(replacementText));
            return str.Replace("\r\n", replacementText).Replace("\n", replacementText).Replace("\r", replacementText);
        }
#endif

#if NETFRAMEWORK && !NET40_OR_GREATER
        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
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
