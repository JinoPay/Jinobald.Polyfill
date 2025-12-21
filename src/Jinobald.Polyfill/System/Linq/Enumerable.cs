// Jinobald.Polyfill - LINQ to Objects 폴리필
// .NET 2.0에서 LINQ 기능을 사용할 수 있도록 하는 확장 메서드 모음

#if NET20

using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/>를 구현하는 객체를 쿼리하기 위한 정적 메서드 집합을 제공합니다.
    /// </summary>
    public static class Enumerable
    {
        #region Where - 필터링

        /// <summary>
        /// 조건자에 따라 값 시퀀스를 필터링합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">필터링할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">각 요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>조건을 만족하는 입력 시퀀스의 요소를 포함하는 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Where<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return WhereIterator(source, predicate);
        }

        /// <summary>
        /// 조건자에 따라 값 시퀀스를 필터링합니다. 조건자 함수의 논리에 각 요소의 인덱스가 사용됩니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">필터링할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">각 소스 요소를 조건에 대해 테스트하는 함수입니다. 이 함수의 두 번째 매개 변수는 소스 요소의 인덱스를 나타냅니다.</param>
        /// <returns>조건을 만족하는 입력 시퀀스의 요소를 포함하는 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Where<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return WhereIterator(source, predicate);
        }

        private static IEnumerable<TSource> WhereIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            foreach (TSource element in source)
            {
                if (predicate(element))
                {
                    yield return element;
                }
            }
        }

        private static IEnumerable<TSource> WhereIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            int index = -1;
            foreach (TSource element in source)
            {
                checked { index++; }
                if (predicate(element, index))
                {
                    yield return element;
                }
            }
        }

        #endregion

        #region Select - 투영

        /// <summary>
        /// 시퀀스의 각 요소를 새 폼에 투영합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TResult">selector에서 반환하는 값의 형식입니다.</typeparam>
        /// <param name="source">변환 함수를 호출할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>각 요소에 대해 변환 함수를 호출하여 생성된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static IEnumerable<TResult> Select<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return SelectIterator(source, selector);
        }

        /// <summary>
        /// 시퀀스의 각 요소를 새 폼에 투영합니다. 요소의 인덱스가 selector 함수의 논리에 사용됩니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TResult">selector에서 반환하는 값의 형식입니다.</typeparam>
        /// <param name="source">변환 함수를 호출할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 소스 요소에 적용할 변환 함수입니다. 이 함수의 두 번째 매개 변수는 소스 요소의 인덱스를 나타냅니다.</param>
        /// <returns>각 요소에 대해 변환 함수를 호출하여 생성된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static IEnumerable<TResult> Select<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, int, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return SelectIterator(source, selector);
        }

        private static IEnumerable<TResult> SelectIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            foreach (TSource element in source)
            {
                yield return selector(element);
            }
        }

        private static IEnumerable<TResult> SelectIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, int, TResult> selector)
        {
            int index = -1;
            foreach (TSource element in source)
            {
                checked { index++; }
                yield return selector(element, index);
            }
        }

        #endregion

        #region SelectMany - 평탄화 투영

        /// <summary>
        /// 시퀀스의 각 요소를 <see cref="IEnumerable{T}"/>에 투영하고 결과 시퀀스를 하나의 시퀀스로 평탄화합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TResult">selector에서 반환하는 시퀀스 요소의 형식입니다.</typeparam>
        /// <param name="source">변환 함수를 호출할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>변환 함수를 입력 시퀀스의 각 요소에 호출하여 생성된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static IEnumerable<TResult> SelectMany<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TResult>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return SelectManyIterator(source, selector);
        }

        /// <summary>
        /// 시퀀스의 각 요소를 <see cref="IEnumerable{T}"/>에 투영하고 결과 시퀀스를 하나의 시퀀스로 평탄화합니다.
        /// 각 소스 요소의 인덱스가 해당 요소의 투영된 폼에 사용됩니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TResult">selector에서 반환하는 시퀀스 요소의 형식입니다.</typeparam>
        /// <param name="source">변환 함수를 호출할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 소스 요소에 적용할 변환 함수입니다. 이 함수의 두 번째 매개 변수는 소스 요소의 인덱스를 나타냅니다.</param>
        /// <returns>변환 함수를 입력 시퀀스의 각 요소에 호출하여 생성된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static IEnumerable<TResult> SelectMany<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, int, IEnumerable<TResult>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return SelectManyIterator(source, selector);
        }

        /// <summary>
        /// 시퀀스의 각 요소를 <see cref="IEnumerable{T}"/>에 투영하고, 결과 시퀀스를 하나의 시퀀스로 평탄화한 다음,
        /// 그 안의 각 요소에 대해 결과 선택기 함수를 호출합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TCollection">collectionSelector에서 수집한 중간 요소의 형식입니다.</typeparam>
        /// <typeparam name="TResult">결과 시퀀스의 요소 형식입니다.</typeparam>
        /// <param name="source">변환 함수를 호출할 값의 시퀀스입니다.</param>
        /// <param name="collectionSelector">입력 시퀀스의 각 요소에 적용할 변환 함수입니다.</param>
        /// <param name="resultSelector">중간 시퀀스의 각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>collectionSelector의 각 요소에 대해 일대다 변환 함수를 호출한 다음
        /// 해당 중간 시퀀스 요소와 해당 소스 요소 각각을 결과 요소에 매핑하여 생성된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="collectionSelector"/> 또는 <paramref name="resultSelector"/>가 null인 경우.</exception>
        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null) throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return SelectManyIterator(source, collectionSelector, resultSelector);
        }

        private static IEnumerable<TResult> SelectManyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TResult>> selector)
        {
            foreach (TSource element in source)
            {
                foreach (TResult subElement in selector(element))
                {
                    yield return subElement;
                }
            }
        }

        private static IEnumerable<TResult> SelectManyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, int, IEnumerable<TResult>> selector)
        {
            int index = -1;
            foreach (TSource element in source)
            {
                checked { index++; }
                foreach (TResult subElement in selector(element, index))
                {
                    yield return subElement;
                }
            }
        }

        private static IEnumerable<TResult> SelectManyIterator<TSource, TCollection, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            foreach (TSource element in source)
            {
                foreach (TCollection subElement in collectionSelector(element))
                {
                    yield return resultSelector(element, subElement);
                }
            }
        }

        #endregion

        #region First / FirstOrDefault - 첫 번째 요소

        /// <summary>
        /// 시퀀스의 첫 번째 요소를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">첫 번째 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>지정된 시퀀스의 첫 번째 요소입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException">소스 시퀀스가 비어 있는 경우.</exception>
        public static TSource First<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // IList<T>에 대한 최적화
            if (source is IList<TSource> list)
            {
                if (list.Count > 0)
                {
                    return list[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (e.MoveNext())
                    {
                        return e.Current;
                    }
                }
            }

            throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
        }

        /// <summary>
        /// 지정된 조건을 만족하는 시퀀스의 첫 번째 요소를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">각 요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>조건을 통과하는 시퀀스의 첫 번째 요소입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException">조건을 만족하는 요소가 없거나 소스 시퀀스가 비어 있는 경우.</exception>
        public static TSource First<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (TSource element in source)
            {
                if (predicate(element))
                {
                    return element;
                }
            }

            throw new InvalidOperationException("시퀀스에 일치하는 요소가 없습니다.");
        }

        /// <summary>
        /// 시퀀스의 첫 번째 요소를 반환하거나, 시퀀스에 요소가 없으면 기본값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">첫 번째 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>소스 시퀀스가 비어 있으면 default(TSource)이고, 그렇지 않으면 source의 첫 번째 요소입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static TSource? FirstOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // IList<T>에 대한 최적화
            if (source is IList<TSource> list)
            {
                if (list.Count > 0)
                {
                    return list[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (e.MoveNext())
                    {
                        return e.Current;
                    }
                }
            }

            return default;
        }

        /// <summary>
        /// 조건을 만족하는 시퀀스의 첫 번째 요소를 반환하거나, 해당 요소가 없으면 기본값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">각 요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>조건을 만족하는 요소가 없으면 default(TSource)이고, 그렇지 않으면 조건을 통과하는 첫 번째 요소입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        public static TSource? FirstOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (TSource element in source)
            {
                if (predicate(element))
                {
                    return element;
                }
            }

            return default;
        }

        #endregion

        #region Last / LastOrDefault - 마지막 요소

        /// <summary>
        /// 시퀀스의 마지막 요소를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">마지막 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>소스 시퀀스의 마지막 위치에 있는 값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException">소스 시퀀스가 비어 있는 경우.</exception>
        public static TSource Last<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // IList<T>에 대한 최적화
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
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (e.MoveNext())
                    {
                        TSource result;
                        do
                        {
                            result = e.Current;
                        }
                        while (e.MoveNext());
                        return result;
                    }
                }
            }

            throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
        }

        /// <summary>
        /// 지정된 조건을 만족하는 시퀀스의 마지막 요소를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">각 요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>조건을 통과하는 시퀀스의 마지막 요소입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException">조건을 만족하는 요소가 없거나 소스 시퀀스가 비어 있는 경우.</exception>
        public static TSource Last<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            TSource result = default!;
            bool found = false;

            foreach (TSource element in source)
            {
                if (predicate(element))
                {
                    result = element;
                    found = true;
                }
            }

            if (found)
            {
                return result;
            }

            throw new InvalidOperationException("시퀀스에 일치하는 요소가 없습니다.");
        }

        /// <summary>
        /// 시퀀스의 마지막 요소를 반환하거나, 시퀀스에 요소가 없으면 기본값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">마지막 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>소스 시퀀스가 비어 있으면 default(TSource)이고, 그렇지 않으면 source의 마지막 요소입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static TSource? LastOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // IList<T>에 대한 최적화
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
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (e.MoveNext())
                    {
                        TSource result;
                        do
                        {
                            result = e.Current;
                        }
                        while (e.MoveNext());
                        return result;
                    }
                }
            }

            return default;
        }

        /// <summary>
        /// 조건을 만족하는 시퀀스의 마지막 요소를 반환하거나, 해당 요소가 없으면 기본값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">각 요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>조건을 만족하는 요소가 없으면 default(TSource)이고, 그렇지 않으면 조건을 통과하는 마지막 요소입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        public static TSource? LastOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            TSource? result = default;

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

        #region Single / SingleOrDefault - 단일 요소

        /// <summary>
        /// 시퀀스의 유일한 요소를 반환하고, 시퀀스에 요소가 정확히 하나가 아니면 예외를 throw합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">단일 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>입력 시퀀스의 단일 요소입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException">입력 시퀀스에 요소가 둘 이상 포함된 경우 또는 입력 시퀀스가 비어 있는 경우.</exception>
        public static TSource Single<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // IList<T>에 대한 최적화
            if (source is IList<TSource> list)
            {
                switch (list.Count)
                {
                    case 0:
                        throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                    case 1:
                        return list[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (!e.MoveNext())
                    {
                        throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                    }

                    TSource result = e.Current;
                    if (!e.MoveNext())
                    {
                        return result;
                    }
                }
            }

            throw new InvalidOperationException("시퀀스에 요소가 둘 이상 있습니다.");
        }

        /// <summary>
        /// 지정된 조건을 만족하는 시퀀스의 유일한 요소를 반환하고, 해당 요소가 둘 이상이면 예외를 throw합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">단일 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>조건을 만족하는 입력 시퀀스의 단일 요소입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException">조건을 만족하는 요소가 둘 이상이거나 없는 경우.</exception>
        public static TSource Single<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            TSource result = default!;
            long count = 0;

            foreach (TSource element in source)
            {
                if (predicate(element))
                {
                    result = element;
                    checked { count++; }

                    if (count > 1)
                    {
                        throw new InvalidOperationException("시퀀스에 일치하는 요소가 둘 이상 있습니다.");
                    }
                }
            }

            if (count == 0)
            {
                throw new InvalidOperationException("시퀀스에 일치하는 요소가 없습니다.");
            }

            return result;
        }

        /// <summary>
        /// 시퀀스의 유일한 요소를 반환하거나, 시퀀스가 비어 있으면 기본값을 반환합니다.
        /// 시퀀스에 요소가 둘 이상 있으면 예외를 throw합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">단일 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>입력 시퀀스의 단일 요소이거나, 시퀀스에 요소가 없으면 default(TSource)입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException">입력 시퀀스에 요소가 둘 이상 포함된 경우.</exception>
        public static TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // IList<T>에 대한 최적화
            if (source is IList<TSource> list)
            {
                switch (list.Count)
                {
                    case 0:
                        return default;
                    case 1:
                        return list[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (!e.MoveNext())
                    {
                        return default;
                    }

                    TSource result = e.Current;
                    if (!e.MoveNext())
                    {
                        return result;
                    }
                }
            }

            throw new InvalidOperationException("시퀀스에 요소가 둘 이상 있습니다.");
        }

        /// <summary>
        /// 지정된 조건을 만족하는 시퀀스의 유일한 요소를 반환하거나, 해당 요소가 없으면 기본값을 반환합니다.
        /// 조건을 만족하는 요소가 둘 이상이면 예외를 throw합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">단일 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>조건을 만족하는 입력 시퀀스의 단일 요소이거나, 해당 요소가 없으면 default(TSource)입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException">조건을 만족하는 요소가 둘 이상인 경우.</exception>
        public static TSource? SingleOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            TSource? result = default;
            long count = 0;

            foreach (TSource element in source)
            {
                if (predicate(element))
                {
                    result = element;
                    checked { count++; }

                    if (count > 1)
                    {
                        throw new InvalidOperationException("시퀀스에 일치하는 요소가 둘 이상 있습니다.");
                    }
                }
            }

            return result;
        }

        #endregion

        #region Any / All - 존재 확인

        /// <summary>
        /// 시퀀스에 요소가 있는지 확인합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">비어 있는지 확인할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>소스 시퀀스에 요소가 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static bool Any<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // ICollection<T>에 대한 최적화
            if (source is ICollection<TSource> collection)
            {
                return collection.Count > 0;
            }

            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                return e.MoveNext();
            }
        }

        /// <summary>
        /// 시퀀스의 요소 중 조건을 만족하는 요소가 있는지 확인합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">조건을 적용할 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">각 요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>소스 시퀀스의 요소 중 지정된 조건자에서 테스트를 통과한 요소가 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        public static bool Any<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (TSource element in source)
            {
                if (predicate(element))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 시퀀스의 모든 요소가 조건을 만족하는지 확인합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">조건을 적용할 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">각 요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>소스 시퀀스의 모든 요소가 지정된 조건자에서 테스트를 통과하거나 시퀀스가 비어 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        public static bool All<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (TSource element in source)
            {
                if (!predicate(element))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Contains - 요소 포함 확인

        /// <summary>
        /// 기본 같음 비교자를 사용하여 시퀀스에 지정된 요소가 포함되어 있는지 확인합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">값을 찾을 시퀀스입니다.</param>
        /// <param name="value">시퀀스에서 찾을 값입니다.</param>
        /// <returns>소스 시퀀스에 지정된 값을 가진 요소가 포함되어 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static bool Contains<TSource>(
            this IEnumerable<TSource> source,
            TSource value)
        {
            return Contains(source, value, null);
        }

        /// <summary>
        /// 지정된 <see cref="IEqualityComparer{T}"/>를 사용하여 시퀀스에 지정된 요소가 포함되어 있는지 확인합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">값을 찾을 시퀀스입니다.</param>
        /// <param name="value">시퀀스에서 찾을 값입니다.</param>
        /// <param name="comparer">값을 비교할 같음 비교자입니다.</param>
        /// <returns>소스 시퀀스에 지정된 값을 가진 요소가 포함되어 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static bool Contains<TSource>(
            this IEnumerable<TSource> source,
            TSource value,
            IEqualityComparer<TSource>? comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // ICollection<T>에 대한 최적화 (기본 비교자 사용 시)
            if (comparer == null && source is ICollection<TSource> collection)
            {
                return collection.Contains(value);
            }

            comparer ??= EqualityComparer<TSource>.Default;

            foreach (TSource element in source)
            {
                if (comparer.Equals(element, value))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Count / LongCount - 요소 수

        /// <summary>
        /// 시퀀스의 요소 수를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">개수를 셀 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>입력 시퀀스의 요소 수입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">source의 요소 수가 <see cref="int.MaxValue"/>보다 큰 경우.</exception>
        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // ICollection<T>에 대한 최적화
            if (source is ICollection<TSource> collection)
            {
                return collection.Count;
            }

            // ICollection에 대한 최적화 (비제네릭)
            if (source is ICollection nonGenericCollection)
            {
                return nonGenericCollection.Count;
            }

            int count = 0;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                checked
                {
                    while (e.MoveNext())
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 조건을 만족하는 시퀀스의 요소 수를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">조건에 대해 테스트하고 셀 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">각 요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>조건을 만족하는 시퀀스의 요소 수입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">개수가 <see cref="int.MaxValue"/>를 초과하는 경우.</exception>
        public static int Count<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            int count = 0;
            foreach (TSource element in source)
            {
                checked
                {
                    if (predicate(element))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 시퀀스의 요소 수를 나타내는 <see cref="long"/>을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">개수를 셀 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>소스 시퀀스의 요소 수입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">source의 요소 수가 <see cref="long.MaxValue"/>보다 큰 경우.</exception>
        public static long LongCount<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long count = 0;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                checked
                {
                    while (e.MoveNext())
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 조건을 만족하는 시퀀스의 요소 수를 나타내는 <see cref="long"/>을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">조건에 대해 테스트하고 셀 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">각 요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>조건을 만족하는 시퀀스의 요소 수입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">개수가 <see cref="long.MaxValue"/>를 초과하는 경우.</exception>
        public static long LongCount<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            long count = 0;
            foreach (TSource element in source)
            {
                checked
                {
                    if (predicate(element))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        #endregion

        #region ElementAt / ElementAtOrDefault - 인덱스로 접근

        /// <summary>
        /// 시퀀스에서 지정된 인덱스에 있는 요소를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="index">검색할 요소의 인덱스(0부터 시작)입니다.</param>
        /// <returns>소스 시퀀스에서 지정된 위치에 있는 요소입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>가 0보다 작거나 source의 요소 수보다 크거나 같은 경우.</exception>
        public static TSource ElementAt<TSource>(
            this IEnumerable<TSource> source,
            int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // IList<T>에 대한 최적화
            if (source is IList<TSource> list)
            {
                return list[index];
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "인덱스는 0보다 크거나 같아야 합니다.");
            }

            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (index == 0)
                    {
                        return e.Current;
                    }
                    index--;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(index), "인덱스가 시퀀스의 범위를 벗어났습니다.");
        }

        /// <summary>
        /// 시퀀스에서 지정된 인덱스에 있는 요소를 반환하거나, 인덱스가 범위를 벗어나면 기본값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="index">검색할 요소의 인덱스(0부터 시작)입니다.</param>
        /// <returns>인덱스가 소스 시퀀스의 범위를 벗어나면 default(TSource)이고, 그렇지 않으면 소스 시퀀스에서 지정된 위치에 있는 요소입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static TSource? ElementAtOrDefault<TSource>(
            this IEnumerable<TSource> source,
            int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (index < 0)
            {
                return default;
            }

            // IList<T>에 대한 최적화
            if (source is IList<TSource> list)
            {
                if (index < list.Count)
                {
                    return list[index];
                }
                return default;
            }

            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (index == 0)
                    {
                        return e.Current;
                    }
                    index--;
                }
            }

            return default;
        }

        #endregion

        #region ToArray / ToList / ToDictionary - 변환

        /// <summary>
        /// <see cref="IEnumerable{T}"/>에서 배열을 만듭니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">배열을 만들 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>입력 시퀀스의 요소가 포함된 배열입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // ICollection<T>에 대한 최적화
            if (source is ICollection<TSource> collection)
            {
                int count = collection.Count;
                if (count == 0)
                {
                    return Array.Empty<TSource>();
                }

                TSource[] result = new TSource[count];
                collection.CopyTo(result, 0);
                return result;
            }

            // 일반적인 경우: List를 통해 변환
            return new List<TSource>(source).ToArray();
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>에서 <see cref="List{T}"/>을 만듭니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">List를 만들 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>입력 시퀀스의 요소가 포함된 <see cref="List{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new List<TSource>(source);
        }

        /// <summary>
        /// 지정된 키 선택기 함수에 따라 <see cref="IEnumerable{T}"/>에서 <see cref="Dictionary{TKey, TValue}"/>를 만듭니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">Dictionary를 만들 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
        /// <returns>키와 값이 포함된 <see cref="Dictionary{TKey, TValue}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        /// <exception cref="ArgumentException">keySelector가 중복 키를 생성하는 경우.</exception>
        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector) where TKey : notnull
        {
            return ToDictionary(source, keySelector, x => x, null);
        }

        /// <summary>
        /// 지정된 키 선택기 함수 및 키 비교자에 따라 <see cref="IEnumerable{T}"/>에서 <see cref="Dictionary{TKey, TValue}"/>를 만듭니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">Dictionary를 만들 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
        /// <param name="comparer">키를 비교할 <see cref="IEqualityComparer{TKey}"/>입니다.</param>
        /// <returns>키와 값이 포함된 <see cref="Dictionary{TKey, TValue}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        /// <exception cref="ArgumentException">keySelector가 중복 키를 생성하는 경우.</exception>
        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer) where TKey : notnull
        {
            return ToDictionary(source, keySelector, x => x, comparer);
        }

        /// <summary>
        /// 지정된 키 선택기 및 요소 선택기 함수에 따라 <see cref="IEnumerable{T}"/>에서 <see cref="Dictionary{TKey, TValue}"/>를 만듭니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TElement">elementSelector에서 반환하는 값의 형식입니다.</typeparam>
        /// <param name="source">Dictionary를 만들 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
        /// <param name="elementSelector">각 요소에서 결과 요소 값을 생성하는 변환 함수입니다.</param>
        /// <returns>키와 값이 포함된 <see cref="Dictionary{TKey, TValue}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="keySelector"/> 또는 <paramref name="elementSelector"/>가 null인 경우.</exception>
        /// <exception cref="ArgumentException">keySelector가 중복 키를 생성하는 경우.</exception>
        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector) where TKey : notnull
        {
            return ToDictionary(source, keySelector, elementSelector, null);
        }

        /// <summary>
        /// 지정된 키 선택기 함수, 비교자 및 요소 선택기 함수에 따라 <see cref="IEnumerable{T}"/>에서 <see cref="Dictionary{TKey, TValue}"/>를 만듭니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <typeparam name="TElement">elementSelector에서 반환하는 값의 형식입니다.</typeparam>
        /// <param name="source">Dictionary를 만들 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
        /// <param name="elementSelector">각 요소에서 결과 요소 값을 생성하는 변환 함수입니다.</param>
        /// <param name="comparer">키를 비교할 <see cref="IEqualityComparer{TKey}"/>입니다.</param>
        /// <returns>키와 값이 포함된 <see cref="Dictionary{TKey, TValue}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="keySelector"/> 또는 <paramref name="elementSelector"/>가 null인 경우.</exception>
        /// <exception cref="ArgumentException">keySelector가 중복 키를 생성하는 경우.</exception>
        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer) where TKey : notnull
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));

            Dictionary<TKey, TElement> d = new Dictionary<TKey, TElement>(comparer);
            foreach (TSource element in source)
            {
                d.Add(keySelector(element), elementSelector(element));
            }

            return d;
        }

        #endregion

        #region Empty - 빈 시퀀스

        /// <summary>
        /// 지정된 형식 인수가 있는 비어 있는 <see cref="IEnumerable{T}"/>을 반환합니다.
        /// </summary>
        /// <typeparam name="TResult">반환되는 제네릭 <see cref="IEnumerable{T}"/>의 형식 매개 변수에 할당할 형식입니다.</typeparam>
        /// <returns>형식 인수가 TResult인 빈 <see cref="IEnumerable{T}"/>입니다.</returns>
        public static IEnumerable<TResult> Empty<TResult>()
        {
            return EmptyEnumerable<TResult>.Instance;
        }

        private static class EmptyEnumerable<TElement>
        {
            public static readonly TElement[] Instance = Array.Empty<TElement>();
        }

        #endregion

        #region Range / Repeat - 생성

        /// <summary>
        /// 지정된 범위 내에서 정수 시퀀스를 생성합니다.
        /// </summary>
        /// <param name="start">시퀀스에서 첫 번째 정수의 값입니다.</param>
        /// <param name="count">생성할 순차적 정수의 수입니다.</param>
        /// <returns>순차적 정수가 포함된 <see cref="IEnumerable{Int32}"/>입니다.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/>가 0보다 작거나 start + count -1이 <see cref="int.MaxValue"/>보다 큰 경우.</exception>
        public static IEnumerable<int> Range(int start, int count)
        {
            long max = (long)start + count - 1;
            if (count < 0 || max > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return RangeIterator(start, count);
        }

        private static IEnumerable<int> RangeIterator(int start, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return start + i;
            }
        }

        /// <summary>
        /// 반복되는 값이 포함된 시퀀스를 생성합니다.
        /// </summary>
        /// <typeparam name="TResult">결과 시퀀스에서 반복할 값의 형식입니다.</typeparam>
        /// <param name="element">반복할 값입니다.</param>
        /// <param name="count">생성된 시퀀스에서 값을 반복할 횟수입니다.</param>
        /// <returns>반복되는 값이 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/>가 0보다 작은 경우.</exception>
        public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return RepeatIterator(element, count);
        }

        private static IEnumerable<TResult> RepeatIterator<TResult>(TResult element, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return element;
            }
        }

        #endregion

        #region Concat - 연결

        /// <summary>
        /// 두 시퀀스를 연결합니다.
        /// </summary>
        /// <typeparam name="TSource">입력 시퀀스 요소의 형식입니다.</typeparam>
        /// <param name="first">연결할 첫 번째 시퀀스입니다.</param>
        /// <param name="second">첫 번째 시퀀스에 연결할 시퀀스입니다.</param>
        /// <returns>두 입력 시퀀스의 연결된 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> 또는 <paramref name="second"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Concat<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return ConcatIterator(first, second);
        }

        private static IEnumerable<TSource> ConcatIterator<TSource>(
            IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            foreach (TSource element in first)
            {
                yield return element;
            }

            foreach (TSource element in second)
            {
                yield return element;
            }
        }

        #endregion

        #region Distinct - 중복 제거

        /// <summary>
        /// 기본 같음 비교자를 사용하여 값을 비교하고 시퀀스에서 고유 요소를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">중복 요소를 제거할 시퀀스입니다.</param>
        /// <returns>소스 시퀀스의 고유 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
        {
            return Distinct(source, null);
        }

        /// <summary>
        /// 지정된 <see cref="IEqualityComparer{T}"/>를 사용하여 값을 비교하고 시퀀스에서 고유 요소를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">중복 요소를 제거할 시퀀스입니다.</param>
        /// <param name="comparer">값을 비교할 <see cref="IEqualityComparer{T}"/>입니다.</param>
        /// <returns>소스 시퀀스의 고유 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Distinct<TSource>(
            this IEnumerable<TSource> source,
            IEqualityComparer<TSource>? comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return DistinctIterator(source, comparer);
        }

        private static IEnumerable<TSource> DistinctIterator<TSource>(
            IEnumerable<TSource> source,
            IEqualityComparer<TSource>? comparer)
        {
            HashSet<TSource> seen = new HashSet<TSource>(comparer);
            foreach (TSource element in source)
            {
                if (seen.Add(element))
                {
                    yield return element;
                }
            }
        }

        #endregion

        #region Skip / Take - 분할

        /// <summary>
        /// 시퀀스에서 지정된 수의 요소를 건너뛴 다음 나머지 요소를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="count">나머지 요소를 반환하기 전에 건너뛸 요소의 수입니다.</param>
        /// <returns>입력 시퀀스에서 지정된 인덱스 후에 발생하는 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Skip<TSource>(
            this IEnumerable<TSource> source,
            int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return SkipIterator(source, count);
        }

        private static IEnumerable<TSource> SkipIterator<TSource>(
            IEnumerable<TSource> source,
            int count)
        {
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                while (count > 0 && e.MoveNext())
                {
                    count--;
                }

                if (count <= 0)
                {
                    while (e.MoveNext())
                    {
                        yield return e.Current;
                    }
                }
            }
        }

        /// <summary>
        /// 시퀀스의 시작 부분에서 지정된 수의 연속 요소를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">요소를 반환할 시퀀스입니다.</param>
        /// <param name="count">반환할 요소의 수입니다.</param>
        /// <returns>입력 시퀀스의 시작 부분에서 지정된 수의 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Take<TSource>(
            this IEnumerable<TSource> source,
            int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return TakeIterator(source, count);
        }

        private static IEnumerable<TSource> TakeIterator<TSource>(
            IEnumerable<TSource> source,
            int count)
        {
            if (count > 0)
            {
                foreach (TSource element in source)
                {
                    yield return element;
                    if (--count == 0)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 지정된 조건이 true인 동안 시퀀스에서 요소를 건너뛴 다음 나머지 요소를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="predicate">각 요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>조건을 통과하지 못하는 첫 번째 요소로 시작하는 입력 시퀀스의 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> SkipWhile<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return SkipWhileIterator(source, predicate);
        }

        private static IEnumerable<TSource> SkipWhileIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            bool yielding = false;
            foreach (TSource element in source)
            {
                if (!yielding && !predicate(element))
                {
                    yielding = true;
                }

                if (yielding)
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// 지정된 조건이 true인 동안 시퀀스에서 요소를 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">요소를 반환할 시퀀스입니다.</param>
        /// <param name="predicate">각 요소를 조건에 대해 테스트하는 함수입니다.</param>
        /// <returns>조건을 통과하지 못하는 요소 이전에 발생하는 입력 시퀀스의 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="predicate"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> TakeWhile<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return TakeWhileIterator(source, predicate);
        }

        private static IEnumerable<TSource> TakeWhileIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            foreach (TSource element in source)
            {
                if (!predicate(element))
                {
                    break;
                }
                yield return element;
            }
        }

        #endregion

        #region Reverse - 역순

        /// <summary>
        /// 시퀀스의 요소 순서를 거꾸로 바꿉니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">역순으로 바꿀 값의 시퀀스입니다.</param>
        /// <returns>요소가 역순으로 정렬된 시퀀스입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return ReverseIterator(source);
        }

        private static IEnumerable<TSource> ReverseIterator<TSource>(IEnumerable<TSource> source)
        {
            TSource[] buffer = source.ToArray();
            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                yield return buffer[i];
            }
        }

        #endregion

        #region Cast / OfType - 형식 변환

        /// <summary>
        /// <see cref="IEnumerable"/>의 요소를 지정된 형식으로 캐스팅합니다.
        /// </summary>
        /// <typeparam name="TResult">source의 요소를 캐스팅할 형식입니다.</typeparam>
        /// <param name="source">TResult로 캐스팅할 요소가 포함된 <see cref="IEnumerable"/>입니다.</param>
        /// <returns>지정된 형식으로 캐스팅된 소스 시퀀스의 각 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidCastException">시퀀스의 요소를 TResult로 캐스팅할 수 없는 경우.</exception>
        public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // 이미 올바른 형식인 경우 최적화
            if (source is IEnumerable<TResult> typedSource)
            {
                return typedSource;
            }

            return CastIterator<TResult>(source);
        }

        private static IEnumerable<TResult> CastIterator<TResult>(IEnumerable source)
        {
            foreach (object? obj in source)
            {
                yield return (TResult)obj!;
            }
        }

        /// <summary>
        /// 지정된 형식에 따라 <see cref="IEnumerable"/>의 요소를 필터링합니다.
        /// </summary>
        /// <typeparam name="TResult">시퀀스의 요소를 필터링할 형식입니다.</typeparam>
        /// <param name="source">필터링할 요소가 포함된 <see cref="IEnumerable"/>입니다.</param>
        /// <returns>TResult로 캐스팅할 수 있는 입력 시퀀스의 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return OfTypeIterator<TResult>(source);
        }

        private static IEnumerable<TResult> OfTypeIterator<TResult>(IEnumerable source)
        {
            foreach (object? obj in source)
            {
                if (obj is TResult result)
                {
                    yield return result;
                }
            }
        }

        #endregion

        #region SequenceEqual - 시퀀스 비교

        /// <summary>
        /// 요소의 형식에 대한 기본 같음 비교자를 사용하여 요소를 비교하고 두 시퀀스가 같은지 확인합니다.
        /// </summary>
        /// <typeparam name="TSource">입력 시퀀스 요소의 형식입니다.</typeparam>
        /// <param name="first">second와 비교할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="second">첫 번째 시퀀스와 비교할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>두 소스 시퀀스가 같은 길이이고 해당 요소가 같으면 true이고, 그렇지 않으면 false입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> 또는 <paramref name="second"/>가 null인 경우.</exception>
        public static bool SequenceEqual<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return SequenceEqual(first, second, null);
        }

        /// <summary>
        /// 지정된 <see cref="IEqualityComparer{T}"/>를 사용하여 요소를 비교하고 두 시퀀스가 같은지 확인합니다.
        /// </summary>
        /// <typeparam name="TSource">입력 시퀀스 요소의 형식입니다.</typeparam>
        /// <param name="first">second와 비교할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="second">첫 번째 시퀀스와 비교할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="comparer">요소를 비교하는 데 사용할 <see cref="IEqualityComparer{T}"/>입니다.</param>
        /// <returns>두 소스 시퀀스가 같은 길이이고 해당 요소가 지정된 같음 비교자에 따라 같으면 true이고, 그렇지 않으면 false입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> 또는 <paramref name="second"/>가 null인 경우.</exception>
        public static bool SequenceEqual<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource>? comparer)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            comparer ??= EqualityComparer<TSource>.Default;

            using (IEnumerator<TSource> e1 = first.GetEnumerator())
            using (IEnumerator<TSource> e2 = second.GetEnumerator())
            {
                while (e1.MoveNext())
                {
                    if (!e2.MoveNext() || !comparer.Equals(e1.Current, e2.Current))
                    {
                        return false;
                    }
                }

                return !e2.MoveNext();
            }
        }

        #endregion

        #region DefaultIfEmpty - 기본값 처리

        /// <summary>
        /// 시퀀스가 비어 있으면 지정된 시퀀스의 요소 또는 singleton 컬렉션에 있는 형식 매개 변수의 기본값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">시퀀스가 비어 있으면 기본값을 반환할 시퀀스입니다.</param>
        /// <returns>소스 시퀀스가 비어 있으면 TSource 형식의 기본값이 포함된 <see cref="IEnumerable{T}"/> 개체이고, 그렇지 않으면 source입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static IEnumerable<TSource?> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return DefaultIfEmpty(source, default);
        }

        /// <summary>
        /// 시퀀스가 비어 있으면 지정된 시퀀스의 요소 또는 singleton 컬렉션에 있는 지정된 값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">시퀀스가 비어 있으면 지정된 값을 반환할 시퀀스입니다.</param>
        /// <param name="defaultValue">시퀀스가 비어 있으면 반환할 값입니다.</param>
        /// <returns>소스 시퀀스가 비어 있으면 defaultValue가 포함된 <see cref="IEnumerable{T}"/>이고, 그렇지 않으면 source입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static IEnumerable<TSource?> DefaultIfEmpty<TSource>(
            this IEnumerable<TSource> source,
            TSource? defaultValue)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return DefaultIfEmptyIterator(source, defaultValue);
        }

        private static IEnumerable<TSource?> DefaultIfEmptyIterator<TSource>(
            IEnumerable<TSource> source,
            TSource? defaultValue)
        {
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    do
                    {
                        yield return e.Current;
                    }
                    while (e.MoveNext());
                }
                else
                {
                    yield return defaultValue;
                }
            }
        }

        #endregion

        #region Append / Prepend - 요소 추가

        /// <summary>
        /// 시퀀스의 끝에 값을 추가합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">값을 추가할 시퀀스입니다.</param>
        /// <param name="element">source의 끝에 추가할 값입니다.</param>
        /// <returns>element로 끝나는 새 시퀀스입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Append<TSource>(
            this IEnumerable<TSource> source,
            TSource element)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return AppendIterator(source, element);
        }

        private static IEnumerable<TSource> AppendIterator<TSource>(
            IEnumerable<TSource> source,
            TSource element)
        {
            foreach (TSource e in source)
            {
                yield return e;
            }
            yield return element;
        }

        /// <summary>
        /// 시퀀스의 시작 부분에 값을 추가합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">값을 추가할 시퀀스입니다.</param>
        /// <param name="element">source의 시작 부분에 추가할 값입니다.</param>
        /// <returns>element로 시작하는 새 시퀀스입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Prepend<TSource>(
            this IEnumerable<TSource> source,
            TSource element)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return PrependIterator(source, element);
        }

        private static IEnumerable<TSource> PrependIterator<TSource>(
            IEnumerable<TSource> source,
            TSource element)
        {
            yield return element;
            foreach (TSource e in source)
            {
                yield return e;
            }
        }

        #endregion
    }
}

#endif
