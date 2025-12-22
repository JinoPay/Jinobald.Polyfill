// Jinobald.Polyfill - DistinctBy 확장 메서드
// .NET 6.0+에서 추가된 DistinctBy 메서드를 하위 버전에서 사용 가능하도록 폴리필

#if NET35

namespace System.Linq;

public static partial class EnumerableEx
{
    #region DistinctBy - .NET 6.0+

    /// <summary>
    ///     지정된 키 선택기 함수에 따라 시퀀스에서 고유한 요소를 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">고유성을 판단하는 데 사용되는 키의 형식입니다.</typeparam>
    /// <param name="source">중복 요소를 제거할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>키에 따라 고유한 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <remarks>
    ///     이 메서드는 지연 실행을 사용합니다.
    ///     각 키에 대해 처음 발견된 요소만 반환됩니다.
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
    /// <typeparam name="TKey">고유성을 판단하는 데 사용되는 키의 형식입니다.</typeparam>
    /// <param name="source">중복 요소를 제거할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="comparer">키를 비교하는 데 사용할 <see cref="IEqualityComparer{T}" />입니다. 또는 null이면 기본 비교자를 사용합니다.</param>
    /// <returns>키에 따라 고유한 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <remarks>
    ///     이 메서드는 지연 실행을 사용합니다.
    ///     각 키에 대해 처음 발견된 요소만 반환됩니다.
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

        foreach (TSource item in source)
        {
            TKey key = keySelector(item);
            if (seenKeys.Add(key))
            {
                yield return item;
            }
        }
    }

    #endregion
}
#endif
