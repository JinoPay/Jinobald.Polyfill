// Jinobald.Polyfill - Append/Prepend 확장 메서드
// .NET Framework 4.7.1+에서 추가된 Append/Prepend 메서드를 하위 버전에서 사용 가능하도록 폴리필

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47

namespace System.Linq;

public static partial class EnumerableEx
{
    #region Append - .NET 4.7.1+

    /// <summary>
    ///     시퀀스의 끝에 값을 추가합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">값을 추가할 시퀀스입니다.</param>
    /// <param name="element">추가할 값입니다.</param>
    /// <returns>지정된 요소가 끝에 추가된 새 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    /// <remarks>
    ///     이 메서드는 지연 실행을 사용합니다.
    ///     원본 시퀀스는 수정되지 않습니다.
    /// </remarks>
    public static IEnumerable<TSource> Append<TSource>(
        this IEnumerable<TSource> source,
        TSource element)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        return AppendIterator(source, element);
    }

    private static IEnumerable<TSource> AppendIterator<TSource>(
        IEnumerable<TSource> source,
        TSource element)
    {
        foreach (TSource item in source)
        {
            yield return item;
        }

        yield return element;
    }

    #endregion

    #region Prepend - .NET 4.7.1+

    /// <summary>
    ///     시퀀스의 앞에 값을 추가합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">값을 추가할 시퀀스입니다.</param>
    /// <param name="element">앞에 추가할 값입니다.</param>
    /// <returns>지정된 요소가 앞에 추가된 새 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    /// <remarks>
    ///     이 메서드는 지연 실행을 사용합니다.
    ///     원본 시퀀스는 수정되지 않습니다.
    /// </remarks>
    public static IEnumerable<TSource> Prepend<TSource>(
        this IEnumerable<TSource> source,
        TSource element)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        return PrependIterator(source, element);
    }

    private static IEnumerable<TSource> PrependIterator<TSource>(
        IEnumerable<TSource> source,
        TSource element)
    {
        yield return element;

        foreach (TSource item in source)
        {
            yield return item;
        }
    }

    #endregion
}
#endif
