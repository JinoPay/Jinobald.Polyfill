// Jinobald.Polyfill - Modern LINQ 확장 메서드 (.NET 6.0+)
// MinBy, MaxBy 메서드들

#if NET35

using System.Collections.Generic;

namespace System.Linq;

public static partial class EnumerableEx
{
    #region MinBy - .NET 6.0+

    /// <summary>
    ///     지정된 키 선택기 함수에 따라 제네릭 시퀀스의 최솟값을 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">요소를 비교할 키의 형식입니다.</typeparam>
    /// <param name="source">최솟값을 확인할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>시퀀스에서 최소 키를 가진 값입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <exception cref="InvalidOperationException">소스 시퀀스가 비어 있는 경우.</exception>
    public static TSource? MinBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        return MinBy(source, keySelector, null);
    }

    /// <summary>
    ///     지정된 키 선택기 함수와 비교자에 따라 제네릭 시퀀스의 최솟값을 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">요소를 비교할 키의 형식입니다.</typeparam>
    /// <param name="source">최솟값을 확인할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="comparer">키를 비교할 비교자입니다.</param>
    /// <returns>시퀀스에서 최소 키를 가진 값입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <exception cref="InvalidOperationException">소스 시퀀스가 비어 있는 경우.</exception>
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
                // 참조 타입인 경우 null 반환, 값 타입인 경우 InvalidOperationException
                if (default(TSource) == null)
                {
                    return default;
                }

                throw new InvalidOperationException("Sequence contains no elements");
            }

            TSource minValue = enumerator.Current;
            TKey minKey = keySelector(minValue);

            while (enumerator.MoveNext())
            {
                TSource currentValue = enumerator.Current;
                TKey currentKey = keySelector(currentValue);

                if (comparer.Compare(currentKey, minKey) < 0)
                {
                    minValue = currentValue;
                    minKey = currentKey;
                }
            }

            return minValue;
        }
    }

    #endregion

    #region MaxBy - .NET 6.0+

    /// <summary>
    ///     지정된 키 선택기 함수에 따라 제네릭 시퀀스의 최댓값을 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">요소를 비교할 키의 형식입니다.</typeparam>
    /// <param name="source">최댓값을 확인할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <returns>시퀀스에서 최대 키를 가진 값입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <exception cref="InvalidOperationException">소스 시퀀스가 비어 있는 경우.</exception>
    public static TSource? MaxBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        return MaxBy(source, keySelector, null);
    }

    /// <summary>
    ///     지정된 키 선택기 함수와 비교자에 따라 제네릭 시퀀스의 최댓값을 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">요소를 비교할 키의 형식입니다.</typeparam>
    /// <param name="source">최댓값을 확인할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="comparer">키를 비교할 비교자입니다.</param>
    /// <returns>시퀀스에서 최대 키를 가진 값입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="keySelector" />가 null인 경우.
    /// </exception>
    /// <exception cref="InvalidOperationException">소스 시퀀스가 비어 있는 경우.</exception>
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
                // 참조 타입인 경우 null 반환, 값 타입인 경우 InvalidOperationException
                if (default(TSource) == null)
                {
                    return default;
                }

                throw new InvalidOperationException("Sequence contains no elements");
            }

            TSource maxValue = enumerator.Current;
            TKey maxKey = keySelector(maxValue);

            while (enumerator.MoveNext())
            {
                TSource currentValue = enumerator.Current;
                TKey currentKey = keySelector(currentValue);

                if (comparer.Compare(currentKey, maxKey) > 0)
                {
                    maxValue = currentValue;
                    maxKey = currentKey;
                }
            }

            return maxValue;
        }
    }

    #endregion
}
#endif
