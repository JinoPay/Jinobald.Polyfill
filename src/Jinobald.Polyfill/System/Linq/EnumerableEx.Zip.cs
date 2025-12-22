// Jinobald.Polyfill - Modern LINQ 확장 메서드 (.NET 6.0+)
// .NET 6.0 이상에서 추가된 LINQ 메서드들을 하위 버전에서 사용 가능하도록 폴리필

#if NET35

namespace System.Linq;

public static partial class EnumerableEx
{
    #region Zip - .NET 4.0+

    /// <summary>
    ///     지정된 함수를 두 시퀀스의 해당 요소에 적용하여 결과 시퀀스를 생성합니다.
    /// </summary>
    /// <typeparam name="TFirst">첫 번째 입력 시퀀스의 요소 형식입니다.</typeparam>
    /// <typeparam name="TSecond">두 번째 입력 시퀀스의 요소 형식입니다.</typeparam>
    /// <typeparam name="TResult">결과 시퀀스의 요소 형식입니다.</typeparam>
    /// <param name="first">병합할 첫 번째 시퀀스입니다.</param>
    /// <param name="second">병합할 두 번째 시퀀스입니다.</param>
    /// <param name="resultSelector">두 요소를 병합하는 방법을 지정하는 함수입니다.</param>
    /// <returns>두 입력 시퀀스의 병합된 요소를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="first" />, <paramref name="second" /> 또는 <paramref name="resultSelector" />가 null인 경우.
    /// </exception>
    /// <remarks>
    ///     이 메서드는 두 시퀀스 중 더 짧은 시퀀스가 끝날 때까지만 요소를 생성합니다.
    /// </remarks>
    public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
        this IEnumerable<TFirst> first,
        IEnumerable<TSecond> second,
        Func<TFirst, TSecond, TResult> resultSelector)
    {
        if (first == null)
        {
            throw new ArgumentNullException("first");
        }

        if (second == null)
        {
            throw new ArgumentNullException("second");
        }

        if (resultSelector == null)
        {
            throw new ArgumentNullException("resultSelector");
        }

        return ZipIterator(first, second, resultSelector);
    }

    private static IEnumerable<TResult> ZipIterator<TFirst, TSecond, TResult>(
        IEnumerable<TFirst> first,
        IEnumerable<TSecond> second,
        Func<TFirst, TSecond, TResult> resultSelector)
    {
        using (IEnumerator<TFirst> e1 = first.GetEnumerator())
        using (IEnumerator<TSecond> e2 = second.GetEnumerator())
        {
            while (e1.MoveNext() && e2.MoveNext())
            {
                yield return resultSelector(e1.Current, e2.Current);
            }
        }
    }

    #endregion
}
#endif
