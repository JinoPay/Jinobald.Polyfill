// Jinobald.Polyfill - LINQ 집합 연산자
// Union, Intersect, Except, Zip

#if NET20

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class Enumerable
    {
        #region Union - 합집합

        /// <summary>
        /// 기본 같음 비교자를 사용하여 두 시퀀스의 합집합을 구합니다.
        /// </summary>
        /// <typeparam name="TSource">입력 시퀀스 요소의 형식입니다.</typeparam>
        /// <param name="first">중복 요소도 포함하며 합집합의 고유한 첫 번째 요소 집합을 형성하는 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="second">중복 요소도 포함하며 합집합의 고유한 두 번째 요소 집합을 형성하는 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>두 입력 시퀀스에서 가져온 고유한 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> 또는 <paramref name="second"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Union<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return Union(first, second, null);
        }

        /// <summary>
        /// 지정된 <see cref="IEqualityComparer{T}"/>를 사용하여 두 시퀀스의 합집합을 구합니다.
        /// </summary>
        /// <typeparam name="TSource">입력 시퀀스 요소의 형식입니다.</typeparam>
        /// <param name="first">중복 요소도 포함하며 합집합의 고유한 첫 번째 요소 집합을 형성하는 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="second">중복 요소도 포함하며 합집합의 고유한 두 번째 요소 집합을 형성하는 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="comparer">값을 비교할 <see cref="IEqualityComparer{T}"/>입니다.</param>
        /// <returns>두 입력 시퀀스에서 가져온 고유한 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> 또는 <paramref name="second"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Union<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource>? comparer)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return UnionIterator(first, second, comparer);
        }

        private static IEnumerable<TSource> UnionIterator<TSource>(
            IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource>? comparer)
        {
            HashSet<TSource> seen = new HashSet<TSource>(comparer);
            foreach (TSource element in first)
            {
                if (seen.Add(element))
                {
                    yield return element;
                }
            }

            foreach (TSource element in second)
            {
                if (seen.Add(element))
                {
                    yield return element;
                }
            }
        }

        #endregion

        #region Intersect - 교집합

        /// <summary>
        /// 기본 같음 비교자를 사용하여 값을 비교하고 두 시퀀스의 교집합을 구합니다.
        /// </summary>
        /// <typeparam name="TSource">입력 시퀀스 요소의 형식입니다.</typeparam>
        /// <param name="first">second에도 나타나는 고유한 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="second">first에도 나타나는 고유한 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>두 시퀀스에 모두 나타나는 요소가 포함된 시퀀스입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> 또는 <paramref name="second"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Intersect<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return Intersect(first, second, null);
        }

        /// <summary>
        /// 지정된 <see cref="IEqualityComparer{T}"/>를 사용하여 값을 비교하고 두 시퀀스의 교집합을 구합니다.
        /// </summary>
        /// <typeparam name="TSource">입력 시퀀스 요소의 형식입니다.</typeparam>
        /// <param name="first">second에도 나타나는 고유한 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="second">first에도 나타나는 고유한 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="comparer">값을 비교할 <see cref="IEqualityComparer{T}"/>입니다.</param>
        /// <returns>두 시퀀스에 모두 나타나는 요소가 포함된 시퀀스입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> 또는 <paramref name="second"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Intersect<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource>? comparer)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return IntersectIterator(first, second, comparer);
        }

        private static IEnumerable<TSource> IntersectIterator<TSource>(
            IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource>? comparer)
        {
            HashSet<TSource> secondSet = new HashSet<TSource>(second, comparer);
            HashSet<TSource> yielded = new HashSet<TSource>(comparer);

            foreach (TSource element in first)
            {
                if (secondSet.Contains(element) && yielded.Add(element))
                {
                    yield return element;
                }
            }
        }

        #endregion

        #region Except - 차집합

        /// <summary>
        /// 기본 같음 비교자를 사용하여 값을 비교하고 두 시퀀스의 차집합을 구합니다.
        /// </summary>
        /// <typeparam name="TSource">입력 시퀀스 요소의 형식입니다.</typeparam>
        /// <param name="first">second에 나타나지 않는 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="second">첫 번째 시퀀스에도 나타나는 요소가 반환된 시퀀스에서 제거되는 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <returns>두 시퀀스 요소의 차집합이 포함된 시퀀스입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> 또는 <paramref name="second"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Except<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return Except(first, second, null);
        }

        /// <summary>
        /// 지정된 <see cref="IEqualityComparer{T}"/>를 사용하여 값을 비교하고 두 시퀀스의 차집합을 구합니다.
        /// </summary>
        /// <typeparam name="TSource">입력 시퀀스 요소의 형식입니다.</typeparam>
        /// <param name="first">second에 나타나지 않는 요소를 반환할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="second">첫 번째 시퀀스에도 나타나는 요소가 반환된 시퀀스에서 제거되는 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="comparer">값을 비교할 <see cref="IEqualityComparer{T}"/>입니다.</param>
        /// <returns>두 시퀀스 요소의 차집합이 포함된 시퀀스입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> 또는 <paramref name="second"/>가 null인 경우.</exception>
        public static IEnumerable<TSource> Except<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource>? comparer)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return ExceptIterator(first, second, comparer);
        }

        private static IEnumerable<TSource> ExceptIterator<TSource>(
            IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource>? comparer)
        {
            HashSet<TSource> excludeSet = new HashSet<TSource>(second, comparer);

            foreach (TSource element in first)
            {
                if (excludeSet.Add(element))
                {
                    yield return element;
                }
            }
        }

        #endregion

        #region Zip - 병합

        /// <summary>
        /// 지정된 함수를 사용하여 두 시퀀스의 해당 요소를 하나의 시퀀스로 병합합니다.
        /// </summary>
        /// <typeparam name="TFirst">첫 번째 입력 시퀀스의 요소 형식입니다.</typeparam>
        /// <typeparam name="TSecond">두 번째 입력 시퀀스의 요소 형식입니다.</typeparam>
        /// <typeparam name="TResult">결과 시퀀스의 요소 형식입니다.</typeparam>
        /// <param name="first">병합할 첫 번째 시퀀스입니다.</param>
        /// <param name="second">병합할 두 번째 시퀀스입니다.</param>
        /// <param name="resultSelector">두 시퀀스의 요소를 병합하는 방법을 지정하는 함수입니다.</param>
        /// <returns>두 입력 시퀀스의 병합된 요소가 포함된 <see cref="IEnumerable{T}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/>, <paramref name="second"/> 또는 <paramref name="resultSelector"/>가 null인 경우.</exception>
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

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
}

#endif
