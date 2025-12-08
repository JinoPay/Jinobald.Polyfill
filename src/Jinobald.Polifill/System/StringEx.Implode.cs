#if NET35
using System.Text;

namespace System;

public static partial class StringEx
{
    extension(string)
    {
        public static string Implode(string? separator, IEnumerable<string> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            if (separator is null) return Concat(values);

            var stringList = values.ToList();
            return ImplodeExtracted(separator, stringList.ToArray(), 0, stringList.Count);
        }

        public static string Implode(string? separator, IEnumerable<string> values, string? start, string? end)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            if (separator is null) return Concat(values);

            var stringList = values.ToList();

            if (stringList.Count == 0) return string.Empty;

            start ??= string.Empty;
            end ??= string.Empty;
            return start + ImplodeExtracted(separator, stringList.ToArray(), 0, stringList.Count) + end;
        }

        public static string Implode<T>(string? separator, IEnumerable<T> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            if (separator is null) return Concat(values);

            var stringList = values.Select(item => item?.ToString()).ToList();
            return ImplodeExtracted(separator, stringList.ToArray(), 0, stringList.Count);
        }

        public static string Implode<T>(string? separator, IEnumerable<T> values, Func<T, string> converter)
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));

            if (values == null) throw new ArgumentNullException(nameof(values));

            if (separator is null) return Concat(values, converter);

            var stringList = values.Select(item => item?.ToString()).ToList();
            return ImplodeExtracted(separator, stringList.ToArray(), 0, stringList.Count);
        }

        public static string Implode<T>(string? separator, IEnumerable<T> values, Func<T, string> converter,
            string? start, string? end)
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));

            if (values == null) throw new ArgumentNullException(nameof(values));

            if (separator is null) return Concat(values, converter);

            var stringList = values.Select(item => item?.ToString()).ToList();

            if (stringList.Count == 0) return string.Empty;

            start ??= string.Empty;
            end ??= string.Empty;
            return start + ImplodeExtracted(separator, stringList.ToArray(), 0, stringList.Count) + end;
        }

        public static string Implode<T>(string? separator, IEnumerable<T> values, string? start, string? end)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            if (separator is null) return Concat(values);

            var stringList = values.Select(item => item?.ToString()).ToList();

            if (stringList.Count == 0) return string.Empty;

            start ??= string.Empty;
            end ??= string.Empty;
            return start + ImplodeExtracted(separator, stringList.ToArray(), 0, stringList.Count) + end;
        }

        public static string Implode(string? separator, object[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Non-negative number is required.");

            if (arrayIndex == array.Length) return string.Empty;

            return ImplodeExtracted(separator ?? string.Empty, array, arrayIndex, array.Length - arrayIndex);
        }

        public static string Implode(string? separator, object[] array, int arrayIndex, int countLimit)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Non-negative number is required.");

            if (countLimit < 0)
                throw new ArgumentOutOfRangeException(nameof(countLimit), "Non-negative number is required.");

            if (countLimit > array.Length - arrayIndex)
                throw new ArgumentException("The array can not contain the number of elements.", nameof(array));

            if (arrayIndex == array.Length) return string.Empty;

            return ImplodeExtracted(separator ?? string.Empty, array, arrayIndex, countLimit);
        }

        public static string Implode(string? separator, params object[] values)
        {
            if (separator is null) return string.Concat(values);

            if (values == null) throw new ArgumentNullException(nameof(values));

            var array = new string?[values.Length];
            var index = 0;
            foreach (var item in values) array[index++] = item?.ToString();

            return ImplodeExtracted(separator, array, 0, array.Length);
        }

        public static string Implode(string separator, params string[]? value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return ImplodeExtracted(separator ?? string.Empty, value, 0, value.Length);
        }

        public static string Implode(string? separator, string[]? array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Non-negative number is required.");

            if (arrayIndex == array.Length) return string.Empty;

            return ImplodeExtracted(separator ?? string.Empty, array, arrayIndex, array.Length - arrayIndex);
        }

        public static string Implode(string? separator, string[]? array, int arrayIndex, int countLimit)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Non-negative number is required.");

            if (countLimit < 0)
                throw new ArgumentOutOfRangeException(nameof(countLimit), "Non-negative number is required.");

            if (countLimit > array.Length - arrayIndex)
                throw new ArgumentException("The array can not contain the number of elements.", nameof(array));

            if (arrayIndex == array.Length) return string.Empty;

            return ImplodeExtracted(separator ?? string.Empty, array, arrayIndex, countLimit);
        }

        private static string ImplodeExtracted(string separator, object[] array, int startIndex, int count)
        {
            var length = 0;
            var maxIndex = startIndex + count;
            var newArray = new string[count];
            for (var index = startIndex; index < maxIndex; index++)
            {
                var item = array[index];

                var itemToString = item?.ToString();
                if (itemToString is null) continue;

                newArray[index - startIndex] = itemToString;
                length += itemToString.Length;
            }

            length += separator.Length * (count - 1);
            return ImplodeExtractedExtracted(separator, newArray, 0, count, length);
        }

        private static string ImplodeExtracted(string separator, string?[] array, int startIndex, int count)
        {
            var length = 0;
            var maxIndex = startIndex + count;
            for (var index = startIndex; index < maxIndex; index++)
            {
                var item = array[index];
                if (item is null) continue;

                length += item.Length;
            }

            length += separator.Length * (count - 1);
            return ImplodeExtractedExtracted(separator, array, startIndex, maxIndex, length);
        }

        private static string ImplodeExtractedExtracted(string separator, string?[] array, int startIndex, int maxIndex,
            int length)
        {
            if (length <= 0) return string.Empty;

            var result = new StringBuilder(length);
            var first = true;
            for (var index = startIndex; index < maxIndex; index++)
            {
                var item = array[index];
                if (first)
                    first = false;
                else
                    result.Append(separator);

                result.Append(item);
            }

            return result.ToString();
        }
    }
}
#endif