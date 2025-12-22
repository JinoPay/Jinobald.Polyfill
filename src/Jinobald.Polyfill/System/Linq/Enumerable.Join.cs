// Jinobald.Polyfill - LINQ 조인 연산자
// Join, GroupJoin

#if NET20
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class Enumerable
    {
        #region Join - 조인

        /// <summary>
        /// 일치하는 키를 기준으로 두 시퀀스의 요소를 상호 연결합니다.
        /// </summary>
        /// <typeparam name="TOuter">첫 번째 시퀀스의 요소 형식입니다.</typeparam>
        /// <typeparam name="TInner">두 번째 시퀀스의 요소 형식입니다.</typeparam>
        /// <typeparam name="TKey">키 선택기 함수에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TResult">결과 요소의 형식입니다.</typeparam>
        /// <param name="outer">조인할 첫 번째 시퀀스입니다.</param>
        /// <param name="inner">첫 번째 시퀀스에 조인할 시퀀스입니다.</param>
        /// <param name="outerKeySelector">첫 번째 시퀀스의 각 요소에서 조인 키를 추출하는 함수입니다.</param>
        /// <param name="innerKeySelector">두 번째 시퀀스의 각 요소에서 조인 키를 추출하는 함수입니다.</param>
        /// <param name="resultSelector">일치하는 두 요소에서 결과 요소를 만드는 함수입니다.</param>
        /// <returns>두 시퀀스에 대해 내부 조인을 수행하여 가져온 TResult 형식 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException">인수가 null인 경우.</exception>
        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            return Join(outer, inner, outerKeySelector, innerKeySelector, resultSelector, null);
        }

        /// <summary>
        /// 일치하는 키를 기준으로 두 시퀀스의 요소를 상호 연결합니다. 지정된 비교자를 사용하여 키를 비교합니다.
        /// </summary>
        /// <typeparam name="TOuter">첫 번째 시퀀스의 요소 형식입니다.</typeparam>
        /// <typeparam name="TInner">두 번째 시퀀스의 요소 형식입니다.</typeparam>
        /// <typeparam name="TKey">키 선택기 함수에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TResult">결과 요소의 형식입니다.</typeparam>
        /// <param name="outer">조인할 첫 번째 시퀀스입니다.</param>
        /// <param name="inner">첫 번째 시퀀스에 조인할 시퀀스입니다.</param>
        /// <param name="outerKeySelector">첫 번째 시퀀스의 각 요소에서 조인 키를 추출하는 함수입니다.</param>
        /// <param name="innerKeySelector">두 번째 시퀀스의 각 요소에서 조인 키를 추출하는 함수입니다.</param>
        /// <param name="resultSelector">일치하는 두 요소에서 결과 요소를 만드는 함수입니다.</param>
        /// <param name="comparer">키를 해시하고 비교하는 데 사용할 <see cref="IEqualityComparer{TKey}"/>입니다.</param>
        /// <returns>두 시퀀스에 대해 내부 조인을 수행하여 가져온 TResult 형식 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException">인수가 null인 경우.</exception>
        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (outer == null) throw new ArgumentNullException(nameof(outer));
            if (inner == null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return JoinIterator(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        private static IEnumerable<TResult> JoinIterator<TOuter, TInner, TKey, TResult>(
            IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            Lookup<TKey, TInner> lookup = Lookup<TKey, TInner>.CreateForJoin(inner, innerKeySelector, comparer);

            foreach (TOuter outerElement in outer)
            {
                TKey key = outerKeySelector(outerElement);
                if (key != null)
                {
                    foreach (TInner innerElement in lookup[key])
                    {
                        yield return resultSelector(outerElement, innerElement);
                    }
                }
            }
        }

        #endregion

        #region GroupJoin - 그룹 조인

        /// <summary>
        /// 키가 같은지 여부를 기준으로 두 시퀀스의 요소를 상호 연결하고 결과를 그룹화합니다.
        /// </summary>
        /// <typeparam name="TOuter">첫 번째 시퀀스의 요소 형식입니다.</typeparam>
        /// <typeparam name="TInner">두 번째 시퀀스의 요소 형식입니다.</typeparam>
        /// <typeparam name="TKey">키 선택기 함수에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TResult">결과 요소의 형식입니다.</typeparam>
        /// <param name="outer">조인할 첫 번째 시퀀스입니다.</param>
        /// <param name="inner">첫 번째 시퀀스에 조인할 시퀀스입니다.</param>
        /// <param name="outerKeySelector">첫 번째 시퀀스의 각 요소에서 조인 키를 추출하는 함수입니다.</param>
        /// <param name="innerKeySelector">두 번째 시퀀스의 각 요소에서 조인 키를 추출하는 함수입니다.</param>
        /// <param name="resultSelector">첫 번째 시퀀스의 요소와 일치하는 두 번째 시퀀스 요소의 컬렉션에서 결과 요소를 만드는 함수입니다.</param>
        /// <returns>두 시퀀스에 대해 그룹화된 조인을 수행하여 가져온 TResult 형식 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException">인수가 null인 경우.</exception>
        public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
        {
            return GroupJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, null);
        }

        /// <summary>
        /// 키가 같은지 여부를 기준으로 두 시퀀스의 요소를 상호 연결하고 결과를 그룹화합니다. 지정된 비교자를 사용하여 키를 비교합니다.
        /// </summary>
        /// <typeparam name="TOuter">첫 번째 시퀀스의 요소 형식입니다.</typeparam>
        /// <typeparam name="TInner">두 번째 시퀀스의 요소 형식입니다.</typeparam>
        /// <typeparam name="TKey">키 선택기 함수에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TResult">결과 요소의 형식입니다.</typeparam>
        /// <param name="outer">조인할 첫 번째 시퀀스입니다.</param>
        /// <param name="inner">첫 번째 시퀀스에 조인할 시퀀스입니다.</param>
        /// <param name="outerKeySelector">첫 번째 시퀀스의 각 요소에서 조인 키를 추출하는 함수입니다.</param>
        /// <param name="innerKeySelector">두 번째 시퀀스의 각 요소에서 조인 키를 추출하는 함수입니다.</param>
        /// <param name="resultSelector">첫 번째 시퀀스의 요소와 일치하는 두 번째 시퀀스 요소의 컬렉션에서 결과 요소를 만드는 함수입니다.</param>
        /// <param name="comparer">키를 해시하고 비교하는 데 사용할 <see cref="IEqualityComparer{TKey}"/>입니다.</param>
        /// <returns>두 시퀀스에 대해 그룹화된 조인을 수행하여 가져온 TResult 형식 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException">인수가 null인 경우.</exception>
        public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (outer == null) throw new ArgumentNullException(nameof(outer));
            if (inner == null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return GroupJoinIterator(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        private static IEnumerable<TResult> GroupJoinIterator<TOuter, TInner, TKey, TResult>(
            IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        {
            Lookup<TKey, TInner> lookup = Lookup<TKey, TInner>.CreateForJoin(inner, innerKeySelector, comparer);

            foreach (TOuter outerElement in outer)
            {
                TKey key = outerKeySelector(outerElement);
                yield return resultSelector(outerElement, lookup[key]);
            }
        }

        #endregion
    }
}

#endif
