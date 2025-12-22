// Jinobald.Polyfill - 기본값 오버로드 확장 메서드
// .NET 6.0+에서 추가된 FirstOrDefault, LastOrDefault, SingleOrDefault 기본값 오버로드를 하위 버전에서 사용 가능하도록 폴리필

#if NET35

namespace System.Linq;

public static partial class EnumerableEx
{
    #region FirstOrDefault 기본값 오버로드 - .NET 6.0+

    /// <summary>
    ///     시퀀스의 첫 번째 요소를 반환하거나, 시퀀스가 비어 있으면 지정된 기본값을 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">첫 번째 요소를 반환할 시퀀스입니다.</param>
    /// <param name="defaultValue">시퀀스가 비어 있으면 반환할 기본값입니다.</param>
    /// <returns>
    ///     시퀀스가 비어 있지 않으면 시퀀스의 첫 번째 요소이고,
    ///     그렇지 않으면 <paramref name="defaultValue" />입니다.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    public static TSource FirstOrDefault<TSource>(
        this IEnumerable<TSource> source,
        TSource defaultValue)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (source is IList<TSource> list)
        {
            if (list.Count > 0)
            {
                return list[0];
            }
        }
        else
        {
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    return enumerator.Current;
                }
            }
        }

        return defaultValue;
    }

    /// <summary>
    ///     조건을 만족하는 시퀀스의 첫 번째 요소를 반환하거나, 해당 요소가 없으면 지정된 기본값을 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">요소를 반환할 시퀀스입니다.</param>
    /// <param name="predicate">각 요소를 테스트하는 조건 함수입니다.</param>
    /// <param name="defaultValue">조건을 만족하는 요소가 없으면 반환할 기본값입니다.</param>
    /// <returns>
    ///     조건을 만족하는 첫 번째 요소이거나,
    ///     해당 요소가 없으면 <paramref name="defaultValue" />입니다.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="predicate" />가 null인 경우.
    /// </exception>
    public static TSource FirstOrDefault<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, bool> predicate,
        TSource defaultValue)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (predicate == null)
        {
            throw new ArgumentNullException("predicate");
        }

        foreach (TSource element in source)
        {
            if (predicate(element))
            {
                return element;
            }
        }

        return defaultValue;
    }

    #endregion

    #region LastOrDefault 기본값 오버로드 - .NET 6.0+

    /// <summary>
    ///     시퀀스의 마지막 요소를 반환하거나, 시퀀스가 비어 있으면 지정된 기본값을 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">마지막 요소를 반환할 시퀀스입니다.</param>
    /// <param name="defaultValue">시퀀스가 비어 있으면 반환할 기본값입니다.</param>
    /// <returns>
    ///     시퀀스가 비어 있지 않으면 시퀀스의 마지막 요소이고,
    ///     그렇지 않으면 <paramref name="defaultValue" />입니다.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    public static TSource LastOrDefault<TSource>(
        this IEnumerable<TSource> source,
        TSource defaultValue)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (source is IList<TSource> list)
        {
            int count = list.Count;
            if (count > 0)
            {
                return list[count - 1];
            }
        }
        else
        {
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    TSource result;
                    do
                    {
                        result = enumerator.Current;
                    }
                    while (enumerator.MoveNext());

                    return result;
                }
            }
        }

        return defaultValue;
    }

    /// <summary>
    ///     조건을 만족하는 시퀀스의 마지막 요소를 반환하거나, 해당 요소가 없으면 지정된 기본값을 반환합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">요소를 반환할 시퀀스입니다.</param>
    /// <param name="predicate">각 요소를 테스트하는 조건 함수입니다.</param>
    /// <param name="defaultValue">조건을 만족하는 요소가 없으면 반환할 기본값입니다.</param>
    /// <returns>
    ///     조건을 만족하는 마지막 요소이거나,
    ///     해당 요소가 없으면 <paramref name="defaultValue" />입니다.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="predicate" />가 null인 경우.
    /// </exception>
    public static TSource LastOrDefault<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, bool> predicate,
        TSource defaultValue)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (predicate == null)
        {
            throw new ArgumentNullException("predicate");
        }

        TSource result = defaultValue;
        foreach (TSource element in source)
        {
            if (predicate(element))
            {
                result = element;
            }
        }

        return result;
    }

    #endregion

    #region SingleOrDefault 기본값 오버로드 - .NET 6.0+

    /// <summary>
    ///     시퀀스의 유일한 요소를 반환하거나, 시퀀스가 비어 있으면 지정된 기본값을 반환합니다.
    ///     시퀀스에 요소가 둘 이상이면 예외를 throw합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">유일한 요소를 반환할 시퀀스입니다.</param>
    /// <param name="defaultValue">시퀀스가 비어 있으면 반환할 기본값입니다.</param>
    /// <returns>
    ///     시퀀스의 유일한 요소이거나,
    ///     시퀀스가 비어 있으면 <paramref name="defaultValue" />입니다.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
    /// <exception cref="InvalidOperationException">시퀀스에 요소가 둘 이상인 경우.</exception>
    public static TSource SingleOrDefault<TSource>(
        this IEnumerable<TSource> source,
        TSource defaultValue)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (source is IList<TSource> list)
        {
            switch (list.Count)
            {
                case 0:
                    return defaultValue;
                case 1:
                    return list[0];
            }
        }
        else
        {
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return defaultValue;
                }

                TSource result = enumerator.Current;
                if (!enumerator.MoveNext())
                {
                    return result;
                }
            }
        }

        throw new InvalidOperationException("Sequence contains more than one element");
    }

    /// <summary>
    ///     조건을 만족하는 시퀀스의 유일한 요소를 반환하거나, 해당 요소가 없으면 지정된 기본값을 반환합니다.
    ///     조건을 만족하는 요소가 둘 이상이면 예외를 throw합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <param name="source">요소를 반환할 시퀀스입니다.</param>
    /// <param name="predicate">각 요소를 테스트하는 조건 함수입니다.</param>
    /// <param name="defaultValue">조건을 만족하는 요소가 없으면 반환할 기본값입니다.</param>
    /// <returns>
    ///     조건을 만족하는 유일한 요소이거나,
    ///     해당 요소가 없으면 <paramref name="defaultValue" />입니다.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" /> 또는 <paramref name="predicate" />가 null인 경우.
    /// </exception>
    /// <exception cref="InvalidOperationException">조건을 만족하는 요소가 둘 이상인 경우.</exception>
    public static TSource SingleOrDefault<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, bool> predicate,
        TSource defaultValue)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (predicate == null)
        {
            throw new ArgumentNullException("predicate");
        }

        TSource result = defaultValue;
        long count = 0;

        foreach (TSource element in source)
        {
            if (predicate(element))
            {
                result = element;
                checked { count++; }

                if (count > 1)
                {
                    throw new InvalidOperationException("Sequence contains more than one matching element");
                }
            }
        }

        return result;
    }

    #endregion
}
#endif
