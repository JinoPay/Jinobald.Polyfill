// Jinobald.Polyfill - AggregateBy 확장 메서드
// .NET 9.0+에서 추가된 AggregateBy 메서드를 하위 버전에서 사용 가능하도록 폴리필

#if NET35

using System.Collections.Generic;

namespace System.Linq;

public static partial class EnumerableEx
{
    #region AggregateBy - .NET 9.0+

    /// <summary>
    ///     지정된 키 선택기 함수에 따라 시퀀스의 요소를 그룹화하고 각 그룹에 대해 결과를 집계합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">그룹화에 사용되는 키의 형식입니다.</typeparam>
    /// <typeparam name="TAccumulate">집계 결과의 형식입니다.</typeparam>
    /// <param name="source">그룹화하고 집계할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="seed">집계의 초기값입니다.</param>
    /// <param name="func">각 요소에 대해 호출할 누적기 함수입니다.</param>
    /// <returns>각 고유 키와 해당 그룹의 집계 결과를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" />, <paramref name="keySelector" /> 또는 <paramref name="func" />가 null인 경우.
    /// </exception>
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETSTANDARD2_0
    public static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<TSource, TKey, TAccumulate>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        TAccumulate seed,
        Func<TAccumulate, TSource, TAccumulate> func)
        where TKey : notnull
    {
        return AggregateBy(source, keySelector, seed, func, null);
    }

    /// <summary>
    ///     지정된 키 선택기 함수와 비교자에 따라 시퀀스의 요소를 그룹화하고 각 그룹에 대해 결과를 집계합니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">그룹화에 사용되는 키의 형식입니다.</typeparam>
    /// <typeparam name="TAccumulate">집계 결과의 형식입니다.</typeparam>
    /// <param name="source">그룹화하고 집계할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="seed">집계의 초기값입니다.</param>
    /// <param name="func">각 요소에 대해 호출할 누적기 함수입니다.</param>
    /// <param name="keyComparer">키를 비교하는 데 사용할 <see cref="IEqualityComparer{T}" />입니다.</param>
    /// <returns>각 고유 키와 해당 그룹의 집계 결과를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" />, <paramref name="keySelector" /> 또는 <paramref name="func" />가 null인 경우.
    /// </exception>
    public static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<TSource, TKey, TAccumulate>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        TAccumulate seed,
        Func<TAccumulate, TSource, TAccumulate> func,
        IEqualityComparer<TKey>? keyComparer)
        where TKey : notnull
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException("keySelector");
        }

        if (func == null)
        {
            throw new ArgumentNullException("func");
        }

        return AggregateByIterator(source, keySelector, seed, func, keyComparer);
    }

    private static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateByIterator<TSource, TKey, TAccumulate>(
        IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        TAccumulate seed,
        Func<TAccumulate, TSource, TAccumulate> func,
        IEqualityComparer<TKey>? keyComparer)
        where TKey : notnull
    {
        // 순서를 유지하면서 집계를 추적
        Dictionary<TKey, TAccumulate> aggregateByKey = new Dictionary<TKey, TAccumulate>(keyComparer);
        List<TKey> keysInOrder = new List<TKey>();

        foreach (TSource element in source)
        {
            TKey key = keySelector(element);

            if (aggregateByKey.ContainsKey(key))
            {
                aggregateByKey[key] = func(aggregateByKey[key], element);
            }
            else
            {
                aggregateByKey[key] = func(seed, element);
                keysInOrder.Add(key);
            }
        }

        foreach (TKey key in keysInOrder)
        {
            yield return new KeyValuePair<TKey, TAccumulate>(key, aggregateByKey[key]);
        }
    }

    /// <summary>
    ///     지정된 키 선택기 함수에 따라 시퀀스의 요소를 그룹화하고 각 그룹에 대해 결과를 집계합니다.
    ///     각 키에 대한 초기값은 시드 선택기 함수로 결정됩니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">그룹화에 사용되는 키의 형식입니다.</typeparam>
    /// <typeparam name="TAccumulate">집계 결과의 형식입니다.</typeparam>
    /// <param name="source">그룹화하고 집계할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="seedSelector">각 키에 대한 초기값을 생성하는 함수입니다.</param>
    /// <param name="func">각 요소에 대해 호출할 누적기 함수입니다.</param>
    /// <returns>각 고유 키와 해당 그룹의 집계 결과를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" />, <paramref name="keySelector" />, <paramref name="seedSelector" /> 또는 <paramref name="func" />가 null인 경우.
    /// </exception>
    public static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<TSource, TKey, TAccumulate>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TKey, TAccumulate> seedSelector,
        Func<TAccumulate, TSource, TAccumulate> func)
        where TKey : notnull
    {
        return AggregateBy(source, keySelector, seedSelector, func, null);
    }

    /// <summary>
    ///     지정된 키 선택기 함수와 비교자에 따라 시퀀스의 요소를 그룹화하고 각 그룹에 대해 결과를 집계합니다.
    ///     각 키에 대한 초기값은 시드 선택기 함수로 결정됩니다.
    /// </summary>
    /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
    /// <typeparam name="TKey">그룹화에 사용되는 키의 형식입니다.</typeparam>
    /// <typeparam name="TAccumulate">집계 결과의 형식입니다.</typeparam>
    /// <param name="source">그룹화하고 집계할 시퀀스입니다.</param>
    /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
    /// <param name="seedSelector">각 키에 대한 초기값을 생성하는 함수입니다.</param>
    /// <param name="func">각 요소에 대해 호출할 누적기 함수입니다.</param>
    /// <param name="keyComparer">키를 비교하는 데 사용할 <see cref="IEqualityComparer{T}" />입니다.</param>
    /// <returns>각 고유 키와 해당 그룹의 집계 결과를 포함하는 시퀀스입니다.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="source" />, <paramref name="keySelector" />, <paramref name="seedSelector" /> 또는 <paramref name="func" />가 null인 경우.
    /// </exception>
    public static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<TSource, TKey, TAccumulate>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TKey, TAccumulate> seedSelector,
        Func<TAccumulate, TSource, TAccumulate> func,
        IEqualityComparer<TKey>? keyComparer)
        where TKey : notnull
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException("keySelector");
        }

        if (seedSelector == null)
        {
            throw new ArgumentNullException("seedSelector");
        }

        if (func == null)
        {
            throw new ArgumentNullException("func");
        }

        return AggregateByWithSeedSelectorIterator(source, keySelector, seedSelector, func, keyComparer);
    }

    private static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateByWithSeedSelectorIterator<TSource, TKey, TAccumulate>(
        IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TKey, TAccumulate> seedSelector,
        Func<TAccumulate, TSource, TAccumulate> func,
        IEqualityComparer<TKey>? keyComparer)
        where TKey : notnull
    {
        // 순서를 유지하면서 집계를 추적
        Dictionary<TKey, TAccumulate> aggregateByKey = new Dictionary<TKey, TAccumulate>(keyComparer);
        List<TKey> keysInOrder = new List<TKey>();

        foreach (TSource element in source)
        {
            TKey key = keySelector(element);

            if (aggregateByKey.ContainsKey(key))
            {
                aggregateByKey[key] = func(aggregateByKey[key], element);
            }
            else
            {
                aggregateByKey[key] = func(seedSelector(key), element);
                keysInOrder.Add(key);
            }
        }

        foreach (TKey key in keysInOrder)
        {
            yield return new KeyValuePair<TKey, TAccumulate>(key, aggregateByKey[key]);
        }
    }
#else
    public static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<TSource, TKey, TAccumulate>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        TAccumulate seed,
        Func<TAccumulate, TSource, TAccumulate> func)
    {
        return AggregateBy(source, keySelector, seed, func, null);
    }

    public static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<TSource, TKey, TAccumulate>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        TAccumulate seed,
        Func<TAccumulate, TSource, TAccumulate> func,
        IEqualityComparer<TKey>? keyComparer)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException("keySelector");
        }

        if (func == null)
        {
            throw new ArgumentNullException("func");
        }

        return AggregateByIterator(source, keySelector, seed, func, keyComparer);
    }

    private static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateByIterator<TSource, TKey, TAccumulate>(
        IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        TAccumulate seed,
        Func<TAccumulate, TSource, TAccumulate> func,
        IEqualityComparer<TKey>? keyComparer)
    {
        // 순서를 유지하면서 집계를 추적
        Dictionary<TKey, TAccumulate> aggregateByKey = new Dictionary<TKey, TAccumulate>(keyComparer);
        List<TKey> keysInOrder = new List<TKey>();

        foreach (TSource element in source)
        {
            TKey key = keySelector(element);

            if (aggregateByKey.ContainsKey(key))
            {
                aggregateByKey[key] = func(aggregateByKey[key], element);
            }
            else
            {
                aggregateByKey[key] = func(seed, element);
                keysInOrder.Add(key);
            }
        }

        foreach (TKey key in keysInOrder)
        {
            yield return new KeyValuePair<TKey, TAccumulate>(key, aggregateByKey[key]);
        }
    }

    public static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<TSource, TKey, TAccumulate>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TKey, TAccumulate> seedSelector,
        Func<TAccumulate, TSource, TAccumulate> func)
    {
        return AggregateBy(source, keySelector, seedSelector, func, null);
    }

    public static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<TSource, TKey, TAccumulate>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TKey, TAccumulate> seedSelector,
        Func<TAccumulate, TSource, TAccumulate> func,
        IEqualityComparer<TKey>? keyComparer)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException("keySelector");
        }

        if (seedSelector == null)
        {
            throw new ArgumentNullException("seedSelector");
        }

        if (func == null)
        {
            throw new ArgumentNullException("func");
        }

        return AggregateByWithSeedSelectorIterator(source, keySelector, seedSelector, func, keyComparer);
    }

    private static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateByWithSeedSelectorIterator<TSource, TKey, TAccumulate>(
        IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TKey, TAccumulate> seedSelector,
        Func<TAccumulate, TSource, TAccumulate> func,
        IEqualityComparer<TKey>? keyComparer)
    {
        // 순서를 유지하면서 집계를 추적
        Dictionary<TKey, TAccumulate> aggregateByKey = new Dictionary<TKey, TAccumulate>(keyComparer);
        List<TKey> keysInOrder = new List<TKey>();

        foreach (TSource element in source)
        {
            TKey key = keySelector(element);

            if (aggregateByKey.ContainsKey(key))
            {
                aggregateByKey[key] = func(aggregateByKey[key], element);
            }
            else
            {
                aggregateByKey[key] = func(seedSelector(key), element);
                keysInOrder.Add(key);
            }
        }

        foreach (TKey key in keysInOrder)
        {
            yield return new KeyValuePair<TKey, TAccumulate>(key, aggregateByKey[key]);
        }
    }
#endif

    #endregion
}
#endif
