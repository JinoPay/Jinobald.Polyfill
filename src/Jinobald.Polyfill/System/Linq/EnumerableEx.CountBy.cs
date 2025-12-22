// Jinobald.Polyfill - CountBy 확장 메서드
// .NET 9.0+에서 추가된 CountBy 메서드를 하위 버전에서 사용 가능하도록 폴리필

#if NET35

using System.Collections.Generic;

namespace System.Linq;

public static partial class EnumerableEx
{
    #region CountBy - .NET 9.0+

    /// <summary>
    ///     지정된 키 선택기 함수에 따라 시퀀스의 요소를 그룹화하고 각 그룹의 요소 수를 계산합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">그룹화에 사용되는 키의 형식입니다.</typeparam>
    /// <param name="source">그룹화할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>각 고유 키와 해당 키를 갖는 요소의 수를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <remarks>
    ///     이 메서드는 GroupBy와 Count를 결합한 편의 메서드입니다.
    ///     각 키에 대해 해당 키를 갖는 요소의 수를 반환합니다.
    /// </remarks>
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETSTANDARD2_0
    public static IEnumerable<KeyValuePair<TKey, int>> CountBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
        where TKey : notnull
    {
        return CountBy(source, keySelector, null);
    }

    /// <summary>
    ///     지정된 키 선택기 함수와 비교자에 따라 시퀀스의 요소를 그룹화하고 각 그룹의 요소 수를 계산합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">그룹화에 사용되는 키의 형식입니다.</typeparam>
    /// <param name="source">그룹화할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="keyComparer">키를 비교하는 데 사용할 <see cref="IEqualityComparer{T}" />입니다.</param>
    /// <returns>각 고유 키와 해당 키를 갖는 요소의 수를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    public static IEnumerable<KeyValuePair<TKey, int>> CountBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? keyComparer)
        where TKey : notnull
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException("keySelector");
        }

        return CountByIterator(source, keySelector, keyComparer);
    }

    private static IEnumerable<KeyValuePair<TKey, int>> CountByIterator<TSource, TKey>(
        IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? keyComparer)
        where TKey : notnull
    {
        // 순서를 유지하면서 카운트를 추적
        Dictionary<TKey, int> countByKey = new Dictionary<TKey, int>(keyComparer);
        List<TKey> keysInOrder = new List<TKey>();

        foreach (TSource element in source)
        {
            TKey key = keySelector(element);

            if (countByKey.ContainsKey(key))
            {
                countByKey[key]++;
            }
            else
            {
                countByKey[key] = 1;
                keysInOrder.Add(key);
            }
        }

        foreach (TKey key in keysInOrder)
        {
            yield return new KeyValuePair<TKey, int>(key, countByKey[key]);
        }
    }
#else
    public static IEnumerable<KeyValuePair<TKey, int>> CountBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        return CountBy(source, keySelector, null);
    }

    public static IEnumerable<KeyValuePair<TKey, int>> CountBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? keyComparer)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException("keySelector");
        }

        return CountByIterator(source, keySelector, keyComparer);
    }

    private static IEnumerable<KeyValuePair<TKey, int>> CountByIterator<TSource, TKey>(
        IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? keyComparer)
    {
        // 순서를 유지하면서 카운트를 추적
        Dictionary<TKey, int> countByKey = new Dictionary<TKey, int>(keyComparer);
        List<TKey> keysInOrder = new List<TKey>();

        foreach (TSource element in source)
        {
            TKey key = keySelector(element);

            if (countByKey.ContainsKey(key))
            {
                countByKey[key]++;
            }
            else
            {
                countByKey[key] = 1;
                keysInOrder.Add(key);
            }
        }

        foreach (TKey key in keysInOrder)
        {
            yield return new KeyValuePair<TKey, int>(key, countByKey[key]);
        }
    }
#endif

    #endregion
}
#endif
