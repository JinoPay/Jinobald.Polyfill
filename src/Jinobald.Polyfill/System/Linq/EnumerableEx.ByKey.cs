// Jinobald.Polyfill - Modern LINQ 확장 메서드 (.NET 6.0+)
// 키 기반 집합 연산 메서드들 (DistinctBy, ExceptBy, IntersectBy, UnionBy)

#if NETFRAMEWORK

using System.Collections.Generic;

namespace System.Linq;

public static partial class EnumerableEx
{
    #region DistinctBy - .NET 6.0+

    /// <summary>
    ///     지정된 키 선택기 함수에 따라 시퀀스에서 고유한 요소를 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">키의 형식입니다.</typeparam>
    /// <param name="source">중복 요소를 제거할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>고유한 키를 가진 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <remarks>
    ///     같은 키를 가진 요소들 중 첫 번째 요소만 결과에 포함됩니다.
    /// </remarks>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        return DistinctBy(source, keySelector, null);
    }

    /// <summary>
    ///     지정된 키 선택기 함수와 비교자에 따라 시퀀스에서 고유한 요소를 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">키의 형식입니다.</typeparam>
    /// <param name="source">중복 요소를 제거할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="comparer">키를 비교하는 데 사용할 비교자입니다.</param>
    /// <returns>고유한 키를 가진 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <remarks>
    ///     같은 키를 가진 요소들 중 첫 번째 요소만 결과에 포함됩니다.
    /// </remarks>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException("keySelector");
        }

        return DistinctByIterator(source, keySelector, comparer);
    }

    private static IEnumerable<TSource> DistinctByIterator<TSource, TKey>(
        IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>(comparer);
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    #endregion

    #region ExceptBy - .NET 6.0+

    /// <summary>
    ///     지정된 키 선택기 함수를 사용하여 두 시퀀스의 차집합을 생성합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">키의 형식입니다.</typeparam>
    /// <param name="first">second에 없는 요소를 반환할 시퀀스입니다.</param>
    /// <param name="second">first에서 제외할 키의 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>두 시퀀스의 차집합인 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="first" />, <paramref name="second" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    public static IEnumerable<TSource> ExceptBy<TSource, TKey>(
        this IEnumerable<TSource> first,
        IEnumerable<TKey> second,
        Func<TSource, TKey> keySelector)
    {
        return ExceptBy(first, second, keySelector, null);
    }

    /// <summary>
    ///     지정된 키 선택기 함수와 비교자를 사용하여 두 시퀀스의 차집합을 생성합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">키의 형식입니다.</typeparam>
    /// <param name="first">second에 없는 요소를 반환할 시퀀스입니다.</param>
    /// <param name="second">first에서 제외할 키의 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="comparer">키를 비교하는 데 사용할 비교자입니다.</param>
    /// <returns>두 시퀀스의 차집합인 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="first" />, <paramref name="second" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    public static IEnumerable<TSource> ExceptBy<TSource, TKey>(
        this IEnumerable<TSource> first,
        IEnumerable<TKey> second,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (first == null)
        {
            throw new ArgumentNullException("first");
        }

        if (second == null)
        {
            throw new ArgumentNullException("second");
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException("keySelector");
        }

        return ExceptByIterator(first, second, keySelector, comparer);
    }

    private static IEnumerable<TSource> ExceptByIterator<TSource, TKey>(
        IEnumerable<TSource> first,
        IEnumerable<TKey> second,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        HashSet<TKey> excludedKeys = new HashSet<TKey>(second, comparer);
        foreach (TSource element in first)
        {
            if (excludedKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    #endregion

    #region IntersectBy - .NET 6.0+

    /// <summary>
    ///     지정된 키 선택기 함수를 사용하여 두 시퀀스의 교집합을 생성합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">키의 형식입니다.</typeparam>
    /// <param name="first">second에도 나타나는 요소를 반환할 시퀀스입니다.</param>
    /// <param name="second">first에도 있는 키의 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>두 시퀀스의 교집합인 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="first" />, <paramref name="second" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    public static IEnumerable<TSource> IntersectBy<TSource, TKey>(
        this IEnumerable<TSource> first,
        IEnumerable<TKey> second,
        Func<TSource, TKey> keySelector)
    {
        return IntersectBy(first, second, keySelector, null);
    }

    /// <summary>
    ///     지정된 키 선택기 함수와 비교자를 사용하여 두 시퀀스의 교집합을 생성합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">키의 형식입니다.</typeparam>
    /// <param name="first">second에도 나타나는 요소를 반환할 시퀀스입니다.</param>
    /// <param name="second">first에도 있는 키의 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="comparer">키를 비교하는 데 사용할 비교자입니다.</param>
    /// <returns>두 시퀀스의 교집합인 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="first" />, <paramref name="second" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    public static IEnumerable<TSource> IntersectBy<TSource, TKey>(
        this IEnumerable<TSource> first,
        IEnumerable<TKey> second,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (first == null)
        {
            throw new ArgumentNullException("first");
        }

        if (second == null)
        {
            throw new ArgumentNullException("second");
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException("keySelector");
        }

        return IntersectByIterator(first, second, keySelector, comparer);
    }

    private static IEnumerable<TSource> IntersectByIterator<TSource, TKey>(
        IEnumerable<TSource> first,
        IEnumerable<TKey> second,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        HashSet<TKey> keys = new HashSet<TKey>(second, comparer);
        foreach (TSource element in first)
        {
            if (keys.Remove(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    #endregion

    #region UnionBy - .NET 6.0+

    /// <summary>
    ///     지정된 키 선택기 함수를 사용하여 두 시퀀스의 합집합을 생성합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">키의 형식입니다.</typeparam>
    /// <param name="first">합집합의 첫 번째 시퀀스입니다.</param>
    /// <param name="second">합집합의 두 번째 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>두 입력 시퀀스의 고유한 키를 가진 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="first" />, <paramref name="second" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    public static IEnumerable<TSource> UnionBy<TSource, TKey>(
        this IEnumerable<TSource> first,
        IEnumerable<TSource> second,
        Func<TSource, TKey> keySelector)
    {
        return UnionBy(first, second, keySelector, null);
    }

    /// <summary>
    ///     지정된 키 선택기 함수와 비교자를 사용하여 두 시퀀스의 합집합을 생성합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">키의 형식입니다.</typeparam>
    /// <param name="first">합집합의 첫 번째 시퀀스입니다.</param>
    /// <param name="second">합집합의 두 번째 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="comparer">키를 비교하는 데 사용할 비교자입니다.</param>
    /// <returns>두 입력 시퀀스의 고유한 키를 가진 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="first" />, <paramref name="second" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    public static IEnumerable<TSource> UnionBy<TSource, TKey>(
        this IEnumerable<TSource> first,
        IEnumerable<TSource> second,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        if (first == null)
        {
            throw new ArgumentNullException("first");
        }

        if (second == null)
        {
            throw new ArgumentNullException("second");
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException("keySelector");
        }

        return UnionByIterator(first, second, keySelector, comparer);
    }

    private static IEnumerable<TSource> UnionByIterator<TSource, TKey>(
        IEnumerable<TSource> first,
        IEnumerable<TSource> second,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>(comparer);

        foreach (TSource element in first)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }

        foreach (TSource element in second)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    #endregion
}
#endif
