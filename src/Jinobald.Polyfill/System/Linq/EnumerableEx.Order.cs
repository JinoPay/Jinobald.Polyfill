// Jinobald.Polyfill - Order/OrderDescending 확장 메서드
// .NET 7.0+에서 추가된 Order/OrderDescending 메서드를 하위 버전에서 사용 가능하도록 폴리필

#if NET35

using System.Collections.Generic;

namespace System.Linq;

public static partial class EnumerableEx
{
    #region Order - .NET 7.0+

    /// <summary>
    ///     시퀀스의 요소를 오름차순으로 정렬합니다.
    /// </summary>
    /// <typeparam name="T">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">정렬할 시퀀스입니다.</param>
    /// <returns>요소가 오름차순으로 정렬된 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    /// <remarks>
    ///     이 메서드는 OrderBy(x =&gt; x)의 간편한 대안입니다.
    ///     요소 자체를 정렬 키로 사용하여 오름차순으로 정렬합니다.
    /// </remarks>
    public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> source)
    {
        return Order(source, null);
    }

    /// <summary>
    ///     지정된 비교자를 사용하여 시퀀스의 요소를 오름차순으로 정렬합니다.
    /// </summary>
    /// <typeparam name="T">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">정렬할 시퀀스입니다.</param>
    /// <param name="comparer">요소를 비교하는 데 사용할 <see cref="IComparer{T}" />입니다.</param>
    /// <returns>요소가 오름차순으로 정렬된 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    public static IOrderedEnumerable<T> Order<T>(
        this IEnumerable<T> source,
        IComparer<T>? comparer)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        return source.OrderBy(static x => x, comparer);
    }

    #endregion

    #region OrderDescending - .NET 7.0+

    /// <summary>
    ///     시퀀스의 요소를 내림차순으로 정렬합니다.
    /// </summary>
    /// <typeparam name="T">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">정렬할 시퀀스입니다.</param>
    /// <returns>요소가 내림차순으로 정렬된 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    /// <remarks>
    ///     이 메서드는 OrderByDescending(x =&gt; x)의 간편한 대안입니다.
    ///     요소 자체를 정렬 키로 사용하여 내림차순으로 정렬합니다.
    /// </remarks>
    public static IOrderedEnumerable<T> OrderDescending<T>(this IEnumerable<T> source)
    {
        return OrderDescending(source, null);
    }

    /// <summary>
    ///     지정된 비교자를 사용하여 시퀀스의 요소를 내림차순으로 정렬합니다.
    /// </summary>
    /// <typeparam name="T">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">정렬할 시퀀스입니다.</param>
    /// <param name="comparer">요소를 비교하는 데 사용할 <see cref="IComparer{T}" />입니다.</param>
    /// <returns>요소가 내림차순으로 정렬된 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    public static IOrderedEnumerable<T> OrderDescending<T>(
        this IEnumerable<T> source,
        IComparer<T>? comparer)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        return source.OrderByDescending(static x => x, comparer);
    }

    #endregion
}
#endif
