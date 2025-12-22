// Jinobald.Polyfill - ElementAt Index 오버로드 확장 메서드
// .NET 6.0+에서 추가된 ElementAt(Index), ElementAtOrDefault(Index) 메서드를 하위 버전에서 사용 가능하도록 폴리필

#if NET35

namespace System.Linq;

public static partial class EnumerableEx
{
    #region ElementAt(Index) - .NET 6.0+

    /// <summary>
    ///     시퀀스의 지정된 인덱스에 있는 요소를 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">요소를 반환할 시퀀스입니다.</param>
    /// <param name="index">검색할 요소의 인덱스입니다. 끝에서부터 셀 수 있습니다.</param>
    /// <returns>시퀀스의 지정된 위치에 있는 요소입니다.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="index" />가 시퀀스의 범위를 벗어난 경우.
    /// </exception>
    /// <remarks>
    ///     이 메서드는 ^1과 같이 끝에서부터 인덱싱할 수 있는 Index 구조체를 지원합니다.
    /// </remarks>
    public static TSource ElementAt<TSource>(
        this IEnumerable<TSource> source,
        Index index)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (!index.IsFromEnd)
        {
            return source.ElementAt(index.Value);
        }

        // 끝에서부터 인덱싱하는 경우
        if (source is IList<TSource> list)
        {
            return list[list.Count - index.Value];
        }

        // 컬렉션인 경우 먼저 개수를 알 수 있음
        if (TryGetNonEnumeratedCount(source, out int count))
        {
            return source.ElementAt(count - index.Value);
        }

        // 그렇지 않으면 배열로 변환 후 인덱싱
        TSource[] array = source.ToArray();
        return array[array.Length - index.Value];
    }

    #endregion

    #region ElementAtOrDefault(Index) - .NET 6.0+

    /// <summary>
    ///     시퀀스의 지정된 인덱스에 있는 요소를 반환하거나, 인덱스가 범위를 벗어나면 기본값을 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">요소를 반환할 시퀀스입니다.</param>
    /// <param name="index">검색할 요소의 인덱스입니다. 끝에서부터 셀 수 있습니다.</param>
    /// <returns>
    ///     시퀀스의 지정된 위치에 있는 요소이거나,
    ///     인덱스가 범위를 벗어나면 default(TSource)입니다.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    /// <remarks>
    ///     이 메서드는 ^1과 같이 끝에서부터 인덱싱할 수 있는 Index 구조체를 지원합니다.
    /// </remarks>
    public static TSource? ElementAtOrDefault<TSource>(
        this IEnumerable<TSource> source,
        Index index)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (!index.IsFromEnd)
        {
            return source.ElementAtOrDefault(index.Value);
        }

        // 끝에서부터 인덱싱하는 경우
        if (source is IList<TSource> list)
        {
            int actualIndex = list.Count - index.Value;
            if (actualIndex < 0 || actualIndex >= list.Count)
            {
                return default;
            }

            return list[actualIndex];
        }

        // 컬렉션인 경우 먼저 개수를 알 수 있음
        if (TryGetNonEnumeratedCount(source, out int count))
        {
            int actualIndex = count - index.Value;
            if (actualIndex < 0 || actualIndex >= count)
            {
                return default;
            }

            return source.ElementAt(actualIndex);
        }

        // 그렇지 않으면 배열로 변환 후 인덱싱
        TSource[] array = source.ToArray();
        int arrayIndex = array.Length - index.Value;
        if (arrayIndex < 0 || arrayIndex >= array.Length)
        {
            return default;
        }

        return array[arrayIndex];
    }

    #endregion
}
#endif
