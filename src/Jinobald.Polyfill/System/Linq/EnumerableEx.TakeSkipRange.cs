// Jinobald.Polyfill - Take/Skip Range 오버로드 확장 메서드
// .NET 6.0+에서 추가된 Take(Range), TakeLast, SkipLast 메서드를 하위 버전에서 사용 가능하도록 폴리필

#if NET35

namespace System.Linq;

public static partial class EnumerableEx
{
    #region Take(Range) - .NET 6.0+

    /// <summary>
    ///     지정된 범위의 요소를 시퀀스에서 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">요소를 반환할 시퀀스입니다.</param>
    /// <param name="range">가져올 요소의 범위입니다.</param>
    /// <returns>지정된 범위에 해당하는 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    /// <remarks>
    ///     이 메서드는 Range 구조체를 지원하여 시작 인덱스와 끝 인덱스 모두 지정할 수 있습니다.
    ///     예: source.Take(2..5)는 인덱스 2부터 4까지의 요소를 반환합니다.
    ///     예: source.Take(..^2)는 마지막 2개를 제외한 모든 요소를 반환합니다.
    /// </remarks>
    public static IEnumerable<TSource> Take<TSource>(
        this IEnumerable<TSource> source,
        Range range)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        return TakeRangeIterator(source, range);
    }

    private static IEnumerable<TSource> TakeRangeIterator<TSource>(
        IEnumerable<TSource> source,
        Range range)
    {
        Index startIndex = range.Start;
        Index endIndex = range.End;

        // 배열이나 리스트인 경우 직접 인덱싱 가능
        if (source is IList<TSource> list)
        {
            int count = list.Count;
            int start = startIndex.GetOffset(count);
            int end = endIndex.GetOffset(count);

            start = Math.Max(0, start);
            end = Math.Min(count, end);

            for (int i = start; i < end; i++)
            {
                yield return list[i];
            }

            yield break;
        }

        // 시작과 끝이 둘 다 시작에서부터인 경우
        if (!startIndex.IsFromEnd && !endIndex.IsFromEnd)
        {
            int start = startIndex.Value;
            int end = endIndex.Value;
            int current = 0;

            foreach (TSource item in source)
            {
                if (current >= end)
                {
                    yield break;
                }

                if (current >= start)
                {
                    yield return item;
                }

                current++;
            }

            yield break;
        }

        // 복잡한 경우: 배열로 변환 후 처리
        TSource[] array = source.ToArray();
        int arrayCount = array.Length;
        int startOffset = startIndex.GetOffset(arrayCount);
        int endOffset = endIndex.GetOffset(arrayCount);

        startOffset = Math.Max(0, startOffset);
        endOffset = Math.Min(arrayCount, endOffset);

        for (int i = startOffset; i < endOffset; i++)
        {
            yield return array[i];
        }
    }

    #endregion

    #region TakeLast - .NET Standard 2.1+ / .NET Core 2.0+

    /// <summary>
    ///     시퀀스의 마지막 <paramref name="count" />개 요소를 포함하는 새 시퀀스를 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">요소를 반환할 시퀀스입니다.</param>
    /// <param name="count">반환할 요소의 수입니다.</param>
    /// <returns>소스 시퀀스의 마지막 <paramref name="count" />개 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    /// <remarks>
    ///     <paramref name="count" />가 0 이하이면 빈 시퀀스를 반환합니다.
    ///     <paramref name="count" />가 시퀀스 길이보다 크면 전체 시퀀스를 반환합니다.
    /// </remarks>
    public static IEnumerable<TSource> TakeLast<TSource>(
        this IEnumerable<TSource> source,
        int count)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (count <= 0)
        {
            return Enumerable.Empty<TSource>();
        }

        return TakeLastIterator(source, count);
    }

    private static IEnumerable<TSource> TakeLastIterator<TSource>(
        IEnumerable<TSource> source,
        int count)
    {
        // IList인 경우 직접 인덱싱
        if (source is IList<TSource> list)
        {
            int listCount = list.Count;
            int start = Math.Max(0, listCount - count);

            for (int i = start; i < listCount; i++)
            {
                yield return list[i];
            }

            yield break;
        }

        // 그렇지 않으면 순환 버퍼 사용
        Queue<TSource> queue = new Queue<TSource>(count);

        foreach (TSource item in source)
        {
            if (queue.Count == count)
            {
                queue.Dequeue();
            }

            queue.Enqueue(item);
        }

        foreach (TSource item in queue)
        {
            yield return item;
        }
    }

    #endregion

    #region SkipLast - .NET Standard 2.1+ / .NET Core 2.0+

    /// <summary>
    ///     시퀀스의 마지막 <paramref name="count" />개 요소를 제외한 새 시퀀스를 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">요소를 반환할 시퀀스입니다.</param>
    /// <param name="count">건너뛸 요소의 수입니다.</param>
    /// <returns>소스 시퀀스에서 마지막 <paramref name="count" />개 요소를 제외한 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    /// <remarks>
    ///     <paramref name="count" />가 0 이하이면 전체 시퀀스를 반환합니다.
    ///     <paramref name="count" />가 시퀀스 길이 이상이면 빈 시퀀스를 반환합니다.
    /// </remarks>
    public static IEnumerable<TSource> SkipLast<TSource>(
        this IEnumerable<TSource> source,
        int count)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (count <= 0)
        {
            return source;
        }

        return SkipLastIterator(source, count);
    }

    private static IEnumerable<TSource> SkipLastIterator<TSource>(
        IEnumerable<TSource> source,
        int count)
    {
        // IList인 경우 직접 인덱싱
        if (source is IList<TSource> list)
        {
            int end = Math.Max(0, list.Count - count);

            for (int i = 0; i < end; i++)
            {
                yield return list[i];
            }

            yield break;
        }

        // 그렇지 않으면 순환 버퍼 사용
        Queue<TSource> queue = new Queue<TSource>(count);

        foreach (TSource item in source)
        {
            if (queue.Count == count)
            {
                yield return queue.Dequeue();
            }

            queue.Enqueue(item);
        }
    }

    #endregion
}
#endif
