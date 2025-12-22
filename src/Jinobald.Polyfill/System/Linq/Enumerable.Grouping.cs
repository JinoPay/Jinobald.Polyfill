// Jinobald.Polyfill - LINQ 그룹화 연산자
// GroupBy, ToLookup 및 지원 클래스

#if NET20
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class Enumerable
    {
        #region GroupBy - 그룹화

        /// <summary>
        /// 키 선택기 함수에 따라 시퀀스의 요소를 그룹화합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">그룹화할 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소의 키를 추출하는 함수입니다.</param>
        /// <returns>각각 개체 시퀀스와 키가 포함된 IGrouping 개체의 컬렉션입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return new GroupedEnumerable<TSource, TKey, TSource>(source, keySelector, x => x, null);
        }

        /// <summary>
        /// 지정된 비교자를 사용하여 키 선택기 함수에 따라 시퀀스의 요소를 그룹화합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">그룹화할 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소의 키를 추출하는 함수입니다.</param>
        /// <param name="comparer">키를 비교할 <see cref="IEqualityComparer{TKey}"/>입니다.</param>
        /// <returns>각각 개체 시퀀스와 키가 포함된 IGrouping 개체의 컬렉션입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            return new GroupedEnumerable<TSource, TKey, TSource>(source, keySelector, x => x, comparer);
        }

        /// <summary>
        /// 지정된 키 선택기 및 요소 선택기 함수에 따라 시퀀스의 요소를 그룹화합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TElement">각 그룹의 요소 형식입니다.</typeparam>
        /// <param name="source">그룹화할 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소의 키를 추출하는 함수입니다.</param>
        /// <param name="elementSelector">소스 요소를 그룹 요소에 매핑하는 함수입니다.</param>
        /// <returns>각각 TElement 형식의 개체 시퀀스와 키가 포함된 IGrouping 개체의 컬렉션입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="keySelector"/> 또는 <paramref name="elementSelector"/>가 null인 경우.</exception>
        public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            return new GroupedEnumerable<TSource, TKey, TElement>(source, keySelector, elementSelector, null);
        }

        /// <summary>
        /// 지정된 비교자, 키 선택기 및 요소 선택기 함수에 따라 시퀀스의 요소를 그룹화합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TElement">각 그룹의 요소 형식입니다.</typeparam>
        /// <param name="source">그룹화할 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소의 키를 추출하는 함수입니다.</param>
        /// <param name="elementSelector">소스 요소를 그룹 요소에 매핑하는 함수입니다.</param>
        /// <param name="comparer">키를 비교할 <see cref="IEqualityComparer{TKey}"/>입니다.</param>
        /// <returns>각각 TElement 형식의 개체 시퀀스와 키가 포함된 IGrouping 개체의 컬렉션입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="keySelector"/> 또는 <paramref name="elementSelector"/>가 null인 경우.</exception>
        public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
        {
            return new GroupedEnumerable<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }

        /// <summary>
        /// 지정된 키 선택기 및 결과 선택기 함수에 따라 시퀀스의 요소를 그룹화합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TResult">resultSelector에서 반환하는 결과 값의 형식입니다.</typeparam>
        /// <param name="source">그룹화할 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소의 키를 추출하는 함수입니다.</param>
        /// <param name="resultSelector">각 그룹에서 결과 값을 만드는 함수입니다.</param>
        /// <returns>TResult 형식 요소의 컬렉션입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="keySelector"/> 또는 <paramref name="resultSelector"/>가 null인 경우.</exception>
        public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
        {
            return new GroupedResultEnumerable<TSource, TKey, TSource, TResult>(
                source, keySelector, x => x, resultSelector, null);
        }

        /// <summary>
        /// 지정된 비교자, 키 선택기 및 결과 선택기 함수에 따라 시퀀스의 요소를 그룹화합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TResult">resultSelector에서 반환하는 결과 값의 형식입니다.</typeparam>
        /// <param name="source">그룹화할 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소의 키를 추출하는 함수입니다.</param>
        /// <param name="resultSelector">각 그룹에서 결과 값을 만드는 함수입니다.</param>
        /// <param name="comparer">키를 비교할 <see cref="IEqualityComparer{TKey}"/>입니다.</param>
        /// <returns>TResult 형식 요소의 컬렉션입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="keySelector"/> 또는 <paramref name="resultSelector"/>가 null인 경우.</exception>
        public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, IEnumerable<TSource>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            return new GroupedResultEnumerable<TSource, TKey, TSource, TResult>(
                source, keySelector, x => x, resultSelector, comparer);
        }

        /// <summary>
        /// 지정된 키 선택기, 요소 선택기 및 결과 선택기 함수에 따라 시퀀스의 요소를 그룹화합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TElement">각 그룹의 요소 형식입니다.</typeparam>
        /// <typeparam name="TResult">resultSelector에서 반환하는 결과 값의 형식입니다.</typeparam>
        /// <param name="source">그룹화할 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소의 키를 추출하는 함수입니다.</param>
        /// <param name="elementSelector">소스 요소를 그룹 요소에 매핑하는 함수입니다.</param>
        /// <param name="resultSelector">각 그룹에서 결과 값을 만드는 함수입니다.</param>
        /// <returns>TResult 형식 요소의 컬렉션입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="keySelector"/>, <paramref name="elementSelector"/> 또는 <paramref name="resultSelector"/>가 null인 경우.</exception>
        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            return new GroupedResultEnumerable<TSource, TKey, TElement, TResult>(
                source, keySelector, elementSelector, resultSelector, null);
        }

        /// <summary>
        /// 지정된 비교자, 키 선택기, 요소 선택기 및 결과 선택기 함수에 따라 시퀀스의 요소를 그룹화합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TElement">각 그룹의 요소 형식입니다.</typeparam>
        /// <typeparam name="TResult">resultSelector에서 반환하는 결과 값의 형식입니다.</typeparam>
        /// <param name="source">그룹화할 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소의 키를 추출하는 함수입니다.</param>
        /// <param name="elementSelector">소스 요소를 그룹 요소에 매핑하는 함수입니다.</param>
        /// <param name="resultSelector">각 그룹에서 결과 값을 만드는 함수입니다.</param>
        /// <param name="comparer">키를 비교할 <see cref="IEqualityComparer{TKey}"/>입니다.</param>
        /// <returns>TResult 형식 요소의 컬렉션입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="keySelector"/>, <paramref name="elementSelector"/> 또는 <paramref name="resultSelector"/>가 null인 경우.</exception>
        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            return new GroupedResultEnumerable<TSource, TKey, TElement, TResult>(
                source, keySelector, elementSelector, resultSelector, comparer);
        }

        #endregion

        #region ToLookup - Lookup 생성

        /// <summary>
        /// 지정된 키 선택기 함수에 따라 <see cref="IEnumerable{T}"/>에서 <see cref="Lookup{TKey, TElement}"/>을 만듭니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">Lookup을 만들 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
        /// <returns>키와 값이 포함된 <see cref="Lookup{TKey, TElement}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return Lookup<TKey, TSource>.Create(source, keySelector, x => x, null);
        }

        /// <summary>
        /// 지정된 키 선택기 함수 및 비교자에 따라 <see cref="IEnumerable{T}"/>에서 <see cref="Lookup{TKey, TElement}"/>을 만듭니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">Lookup을 만들 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
        /// <param name="comparer">키를 비교할 <see cref="IEqualityComparer{TKey}"/>입니다.</param>
        /// <returns>키와 값이 포함된 <see cref="Lookup{TKey, TElement}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            return Lookup<TKey, TSource>.Create(source, keySelector, x => x, comparer);
        }

        /// <summary>
        /// 지정된 키 선택기 및 요소 선택기 함수에 따라 <see cref="IEnumerable{T}"/>에서 <see cref="Lookup{TKey, TElement}"/>을 만듭니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TElement">elementSelector에서 반환하는 값의 형식입니다.</typeparam>
        /// <param name="source">Lookup을 만들 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
        /// <param name="elementSelector">각 요소에서 결과 요소 값을 생성하는 변환 함수입니다.</param>
        /// <returns>키와 값이 포함된 <see cref="Lookup{TKey, TElement}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="keySelector"/> 또는 <paramref name="elementSelector"/>가 null인 경우.</exception>
        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            return Lookup<TKey, TElement>.Create(source, keySelector, elementSelector, null);
        }

        /// <summary>
        /// 지정된 키 선택기, 요소 선택기 함수 및 비교자에 따라 <see cref="IEnumerable{T}"/>에서 <see cref="Lookup{TKey, TElement}"/>을 만듭니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TElement">elementSelector에서 반환하는 값의 형식입니다.</typeparam>
        /// <param name="source">Lookup을 만들 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
        /// <param name="elementSelector">각 요소에서 결과 요소 값을 생성하는 변환 함수입니다.</param>
        /// <param name="comparer">키를 비교할 <see cref="IEqualityComparer{TKey}"/>입니다.</param>
        /// <returns>키와 값이 포함된 <see cref="Lookup{TKey, TElement}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="keySelector"/> 또는 <paramref name="elementSelector"/>가 null인 경우.</exception>
        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
        {
            return Lookup<TKey, TElement>.Create(source, keySelector, elementSelector, comparer);
        }

        #endregion
    }

    #region GroupBy 지원 클래스

    /// <summary>
    /// 그룹화된 열거형의 내부 구현입니다.
    /// </summary>
    internal sealed class GroupedEnumerable<TSource, TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly Func<TSource, TElement> _elementSelector;
        private readonly IEqualityComparer<TKey>? _comparer;

        public GroupedEnumerable(
            IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
            _elementSelector = elementSelector ?? throw new ArgumentNullException(nameof(elementSelector));
            _comparer = comparer;
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            return Lookup<TKey, TElement>.Create(_source, _keySelector, _elementSelector, _comparer).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// 결과 선택기가 있는 그룹화된 열거형의 내부 구현입니다.
    /// </summary>
    internal sealed class GroupedResultEnumerable<TSource, TKey, TElement, TResult> : IEnumerable<TResult>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly Func<TSource, TElement> _elementSelector;
        private readonly Func<TKey, IEnumerable<TElement>, TResult> _resultSelector;
        private readonly IEqualityComparer<TKey>? _comparer;

        public GroupedResultEnumerable(
            IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
            _elementSelector = elementSelector ?? throw new ArgumentNullException(nameof(elementSelector));
            _resultSelector = resultSelector ?? throw new ArgumentNullException(nameof(resultSelector));
            _comparer = comparer;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            Lookup<TKey, TElement> lookup =
 Lookup<TKey, TElement>.Create(_source, _keySelector, _elementSelector, _comparer);
            return lookup.ApplyResultSelector(_resultSelector).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion
}

#endif
