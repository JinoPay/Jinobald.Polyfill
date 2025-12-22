#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

namespace System;

/// <summary>
/// Array 클래스에 대한 확장 메서드를 제공합니다.
/// </summary>
public static class ArrayEx
{
    extension(Array)
    {
        /// <summary>
        /// 지정된 형식의 빈 배열을 반환합니다.
        /// </summary>
        /// <typeparam name="T">배열 요소의 형식입니다.</typeparam>
        /// <returns>빈 배열입니다.</returns>
        /// <remarks>
        /// 이 메서드는 동일한 형식에 대해 항상 같은 배열 인스턴스를 반환합니다.
        /// </remarks>
        public static T[] Empty<T>()
        {
            return EmptyArray<T>.Value;
        }
    }

    /// <summary>
    /// 빈 배열의 캐시된 인스턴스를 제공합니다.
    /// </summary>
    /// <typeparam name="T">배열 요소의 형식입니다.</typeparam>
    private static class EmptyArray<T>
    {
        /// <summary>
        /// 형식 T의 빈 배열입니다.
        /// </summary>
        internal static readonly T[] Value = [];
    }
}

#endif
