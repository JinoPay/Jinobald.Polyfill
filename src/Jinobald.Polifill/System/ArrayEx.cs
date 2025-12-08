#if NET35
using System.Collections.ThreadSafe;
using System.Runtime.CompilerServices;

namespace System;

public static class ArrayEx
{
    private static readonly CacheDict<Type, Array> _emptyArrays = new(256);

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static void Fill<T>(T[] array, T value)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));

        for (var index = 0; index < array.Length; index++) array[index] = value;
    }

    extension(Array)
    {
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static T[] Empty<T>()
        {
            var type = typeof(T);
            if (type == typeof(Type)) return (T[])(object)Type.EmptyTypes;

            if (_emptyArrays.TryGetValue(type, out var array)) return (T[])array;

            var result = new T[0];
            _emptyArrays[type] = result;
            return result;
        }
    }
}
#endif