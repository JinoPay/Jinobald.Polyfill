
using System.Runtime.CompilerServices;
using System.Text;

namespace System;

public static partial class StringEx
{
    extension(string)
    {
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static string Concat(IEnumerable<string>? values)
        {
            if (values is null) throw new ArgumentNullException(nameof(values));

            var stringList = new List<string>();
            var length = 0;
            foreach (var item in values)
            {
                stringList.Add(item);
                length += item.Length;
            }

            return ConcatExtractedExtracted(stringList.ToArray(), 0, stringList.Count, length);
        }

        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static string Concat<T>(IEnumerable<T>? values)
        {
            if (values is null) throw new ArgumentNullException(nameof(values));

            var stringList = new List<string>();
            var length = 0;
            foreach (var item in values)
            {
                if (item is null) continue;

                var itemToString = item.ToString();
                stringList.Add(itemToString);
                length += itemToString.Length;
            }

            return ConcatExtractedExtracted(stringList.ToArray(), 0, stringList.Count, length);
        }

        public static string Concat<T>(IEnumerable<T>? values, Func<T, string>? converter)
        {
            if (values is null) throw new ArgumentNullException(nameof(values));

            if (converter is null) throw new ArgumentNullException(nameof(converter));

            var stringList = new List<string>();
            var length = 0;
            foreach (var item in values)
            {
                var itemToString = converter.Invoke(item);
                stringList.Add(itemToString);
                length += itemToString.Length;
            }

            return ConcatExtractedExtracted(stringList.ToArray(), 0, stringList.Count, length);
        }

        public static string Concat(object[]? array, int arrayIndex)
        {
            if (array is null) throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Non-negative number is required.");

            return arrayIndex == array.Length
                ? string.Empty
                : ConcatExtracted(array, arrayIndex, array.Length - arrayIndex);
        }

        public static string Concat(object[]? array, int arrayIndex, int countLimit)
        {
            if (array is null) throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Non-negative number is required.");

            if (countLimit < 0)
                throw new ArgumentOutOfRangeException(nameof(countLimit), "Non-negative number is required.");

            if (countLimit > array.Length - arrayIndex)
                throw new ArgumentException(
                    "startIndex plus countLimit is greater than the number of elements in array.", nameof(array));

            return arrayIndex == array.Length ? string.Empty : ConcatExtracted(array, arrayIndex, countLimit);
        }

        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static string Concat(params object[] values)
        {
            return string.Concat(values);
        }

        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static string Concat(params string[] value)
        {
            return string.Concat(value);
        }

        public static string Concat(string[]? array, int arrayIndex)
        {
            if (array is null) throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Non-negative number is required.");

            return arrayIndex == array.Length
                ? string.Empty
                : ConcatExtracted(array, arrayIndex, array.Length - arrayIndex);
        }

        public static string Concat(string[]? array, int arrayIndex, int countLimit)
        {
            if (array is null) throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Non-negative number is required.");

            if (countLimit < 0)
                throw new ArgumentOutOfRangeException(nameof(countLimit), "Non-negative number is required.");

            if (countLimit > array.Length - arrayIndex)
                throw new ArgumentException(
                    "startIndex plus countLimit is greater than the number of elements in array.", nameof(array));

            return arrayIndex == array.Length ? string.Empty : ConcatExtracted(array, arrayIndex, countLimit);
        }

        private static string ConcatExtracted(object[] array, int startIndex, int count)
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

            return ConcatExtractedExtracted(newArray, 0, count, length);
        }

        private static string ConcatExtracted(string[] array, int startIndex, int count)
        {
            var length = 0;
            var maxIndex = startIndex + count;
            for (var index = startIndex; index < maxIndex; index++)
            {
                var item = array[index];
                if (item is null) continue;

                length += item.Length;
            }

            return ConcatExtractedExtracted(array, startIndex, maxIndex, length);
        }

        private static string ConcatExtractedExtracted(string[] array, int startIndex, int maxIndex, int length)
        {
            if (length <= 0) return string.Empty;

            var result = new StringBuilder(length);
            for (var index = startIndex; index < maxIndex; index++)
            {
                var item = array[index];
                result.Append(item);
            }

            return result.ToString();
        }
    }
}