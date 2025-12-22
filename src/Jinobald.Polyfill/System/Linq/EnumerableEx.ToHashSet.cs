// Jinobald.Polyfill - ToHashSet 확장 메서드
// .NET Framework 4.7.2+ / .NET Core 2.0+에서 추가된 ToHashSet 메서드를 하위 버전에서 사용 가능하도록 폴리필

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471

namespace System.Linq;

public static partial class EnumerableEx
{
    #region ToHashSet - .NET 4.7.2+ / .NET Core 2.0+

    /// <summary>
    ///     <see cref="IEnumerable{T}" />로부터 <see cref="HashSet{T}" />을 생성합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">HashSet을 생성할 원본 시퀀스입니다.</param>
    /// <returns>소스 시퀀스의 고유 요소를 포함하는 <see cref="HashSet{T}" />입니다.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    /// <remarks>
    ///     이 메서드는 시퀀스의 중복 요소를 제거하고 고유한 요소만 포함하는 HashSet을 반환합니다.
    ///     요소의 고유성을 비교하기 위해 기본 동등 비교자를 사용합니다.
    /// </remarks>
    public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        return new HashSet<TSource>(source);
    }

    /// <summary>
    ///     지정된 동등 비교자를 사용하여 <see cref="IEnumerable{T}" />로부터 <see cref="HashSet{T}" />을 생성합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">HashSet을 생성할 원본 시퀀스입니다.</param>
    /// <param name="comparer">요소를 비교하는 데 사용할 <see cref="IEqualityComparer{T}" />입니다. 또는 null이면 기본 비교자를 사용합니다.</param>
    /// <returns>소스 시퀀스의 고유 요소를 포함하는 <see cref="HashSet{T}" />입니다.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    /// <remarks>
    ///     이 메서드는 시퀀스의 중복 요소를 제거하고 고유한 요소만 포함하는 HashSet을 반환합니다.
    ///     요소의 고유성을 비교하기 위해 지정된 동등 비교자를 사용합니다.
    /// </remarks>
    public static HashSet<TSource> ToHashSet<TSource>(
        this IEnumerable<TSource> source,
        IEqualityComparer<TSource>? comparer)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        return new HashSet<TSource>(source, comparer);
    }

    #endregion
}
#endif
