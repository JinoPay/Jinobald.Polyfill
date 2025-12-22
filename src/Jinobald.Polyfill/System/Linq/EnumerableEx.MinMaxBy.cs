// Jinobald.Polyfill - MinBy/MaxBy 확장 메서드
// .NET 6.0+에서 추가된 MinBy/MaxBy 메서드를 하위 버전에서 사용 가능하도록 폴리필

#if NET35

using System.Collections.Generic;

namespace System.Linq;

public static partial class EnumerableEx
{
    #region MinBy - .NET 6.0+

    /// <summary>
    ///     지정된 키 선택기 함수에 따라 시퀀스에서 최소값을 갖는 요소를 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">비교할 키의 형식입니다.</typeparam>
    /// <param name="source">값을 결정할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>키 함수에 따라 최소값을 갖는 요소입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <exception cref="InvalidOperationException">시퀀스가 비어 있는 경우.</exception>
    public static TSource? MinBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        return MinBy(source, keySelector, null);
    }

    /// <summary>
    ///     지정된 키 선택기 함수와 비교자에 따라 시퀀스에서 최소값을 갖는 요소를 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">비교할 키의 형식입니다.</typeparam>
    /// <param name="source">값을 결정할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="comparer">키를 비교하는 데 사용할 <see cref="IComparer{T}" />입니다.</param>
    /// <returns>키 함수에 따라 최소값을 갖는 요소입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <exception cref="InvalidOperationException">시퀀스가 비어 있는 경우.</exception>
    public static TSource? MinBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IComparer<TKey>? comparer)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException("keySelector");
        }

        comparer ??= Comparer<TKey>.Default;

        using (IEnumerator<TSource> enumerator = source.GetEnumerator())
        {
            if (!enumerator.MoveNext())
            {
                if (default(TSource) == null)
                {
                    return default;
                }

                throw new InvalidOperationException("Sequence contains no elements");
            }

            TSource minElement = enumerator.Current;
            TKey minKey = keySelector(minElement);

            while (enumerator.MoveNext())
            {
                TSource currentElement = enumerator.Current;
                TKey currentKey = keySelector(currentElement);

                if (comparer.Compare(currentKey, minKey) < 0)
                {
                    minElement = currentElement;
                    minKey = currentKey;
                }
            }

            return minElement;
        }
    }

    #endregion

    #region MaxBy - .NET 6.0+

    /// <summary>
    ///     지정된 키 선택기 함수에 따라 시퀀스에서 최대값을 갖는 요소를 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">비교할 키의 형식입니다.</typeparam>
    /// <param name="source">값을 결정할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>키 함수에 따라 최대값을 갖는 요소입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <exception cref="InvalidOperationException">시퀀스가 비어 있는 경우.</exception>
    public static TSource? MaxBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        return MaxBy(source, keySelector, null);
    }

    /// <summary>
    ///     지정된 키 선택기 함수와 비교자에 따라 시퀀스에서 최대값을 갖는 요소를 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">비교할 키의 형식입니다.</typeparam>
    /// <param name="source">값을 결정할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="comparer">키를 비교하는 데 사용할 <see cref="IComparer{T}" />입니다.</param>
    /// <returns>키 함수에 따라 최대값을 갖는 요소입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <exception cref="InvalidOperationException">시퀀스가 비어 있는 경우.</exception>
    public static TSource? MaxBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IComparer<TKey>? comparer)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException("keySelector");
        }

        comparer ??= Comparer<TKey>.Default;

        using (IEnumerator<TSource> enumerator = source.GetEnumerator())
        {
            if (!enumerator.MoveNext())
            {
                if (default(TSource) == null)
                {
                    return default;
                }

                throw new InvalidOperationException("Sequence contains no elements");
            }

            TSource maxElement = enumerator.Current;
            TKey maxKey = keySelector(maxElement);

            while (enumerator.MoveNext())
            {
                TSource currentElement = enumerator.Current;
                TKey currentKey = keySelector(currentElement);

                if (comparer.Compare(currentKey, maxKey) > 0)
                {
                    maxElement = currentElement;
                    maxKey = currentKey;
                }
            }

            return maxElement;
        }
    }

    #endregion
}
#endif
