// Jinobald.Polyfill - Set 연산 By 확장 메서드
// .NET 6.0+에서 추가된 UnionBy, IntersectBy, ExceptBy 메서드를 하위 버전에서 사용 가능하도록 폴리필

#if NET35

namespace System.Linq;

public static partial class EnumerableEx
{
    #region UnionBy - .NET 6.0+

    /// <summary>
    ///     지정된 키 선택기 함수를 사용하여 두 시퀀스의 합집합을 생성합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">고유성을 판단하는 데 사용되는 키의 형식입니다.</typeparam>
    /// <param name="first">합집합의 첫 번째 시퀀스입니다.</param>
    /// <param name="second">합집합의 두 번째 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>두 입력 시퀀스의 고유한 요소를 포함하는 시퀀스입니다.</returns>
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
    /// <typeparam name="TKey">고유성을 판단하는 데 사용되는 키의 형식입니다.</typeparam>
    /// <param name="first">합집합의 첫 번째 시퀀스입니다.</param>
    /// <param name="second">합집합의 두 번째 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="comparer">키를 비교하는 데 사용할 <see cref="IEqualityComparer{T}" />입니다.</param>
    /// <returns>두 입력 시퀀스의 고유한 요소를 포함하는 시퀀스입니다.</returns>
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

        foreach (TSource item in first)
        {
            TKey key = keySelector(item);
            if (seenKeys.Add(key))
            {
                yield return item;
            }
        }

        foreach (TSource item in second)
        {
            TKey key = keySelector(item);
            if (seenKeys.Add(key))
            {
                yield return item;
            }
        }
    }

    #endregion

    #region IntersectBy - .NET 6.0+

    /// <summary>
    ///     지정된 키 선택기 함수를 사용하여 두 시퀀스의 교집합을 생성합니다.
    /// </summary>
    /// <typeparam name="TSource">첫 번째 시퀀스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">고유성을 판단하는 데 사용되는 키의 형식입니다.</typeparam>
    /// <param name="first">교집합 연산의 첫 번째 시퀀스입니다.</param>
    /// <param name="second">키를 포함하는 두 번째 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>두 시퀀스에 모두 존재하는 요소를 포함하는 시퀀스입니다.</returns>
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
    /// <typeparam name="TSource">첫 번째 시퀀스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">고유성을 판단하는 데 사용되는 키의 형식입니다.</typeparam>
    /// <param name="first">교집합 연산의 첫 번째 시퀀스입니다.</param>
    /// <param name="second">키를 포함하는 두 번째 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="comparer">키를 비교하는 데 사용할 <see cref="IEqualityComparer{T}" />입니다.</param>
    /// <returns>두 시퀀스에 모두 존재하는 요소를 포함하는 시퀀스입니다.</returns>
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
        HashSet<TKey> secondKeys = new HashSet<TKey>(second, comparer);
        HashSet<TKey> seenKeys = new HashSet<TKey>(comparer);

        foreach (TSource item in first)
        {
            TKey key = keySelector(item);
            if (secondKeys.Contains(key) && seenKeys.Add(key))
            {
                yield return item;
            }
        }
    }

    #endregion

    #region ExceptBy - .NET 6.0+

    /// <summary>
    ///     지정된 키 선택기 함수를 사용하여 두 시퀀스의 차집합을 생성합니다.
    /// </summary>
    /// <typeparam name="TSource">첫 번째 시퀀스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">고유성을 판단하는 데 사용되는 키의 형식입니다.</typeparam>
    /// <param name="first">차집합 연산의 첫 번째 시퀀스입니다.</param>
    /// <param name="second">제외할 키를 포함하는 두 번째 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>첫 번째 시퀀스에만 존재하는 요소를 포함하는 시퀀스입니다.</returns>
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
    /// <typeparam name="TSource">첫 번째 시퀀스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">고유성을 판단하는 데 사용되는 키의 형식입니다.</typeparam>
    /// <param name="first">차집합 연산의 첫 번째 시퀀스입니다.</param>
    /// <param name="second">제외할 키를 포함하는 두 번째 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="comparer">키를 비교하는 데 사용할 <see cref="IEqualityComparer{T}" />입니다.</param>
    /// <returns>첫 번째 시퀀스에만 존재하는 요소를 포함하는 시퀀스입니다.</returns>
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
        HashSet<TKey> seenKeys = new HashSet<TKey>(comparer);

        foreach (TSource item in first)
        {
            TKey key = keySelector(item);
            if (!excludedKeys.Contains(key) && seenKeys.Add(key))
            {
                yield return item;
            }
        }
    }

    #endregion
}
#endif
