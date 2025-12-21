// Jinobald.Polyfill - LINQ 집계 연산자
// Aggregate, Sum, Average, Min, Max

#if NET20

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class Enumerable
    {
        #region Aggregate - 사용자 정의 집계

        /// <summary>
        /// 시퀀스에 누적기 함수를 적용합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">집계할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="func">각 요소에 대해 호출할 누적기 함수입니다.</param>
        /// <returns>최종 누적기 값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="func"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static TSource Aggregate<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, TSource> func)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                }

                TSource result = e.Current;
                while (e.MoveNext())
                {
                    result = func(result, e.Current);
                }
                return result;
            }
        }

        /// <summary>
        /// 시퀀스에 누적기 함수를 적용합니다. 지정된 초기값이 초기 누적기 값으로 사용됩니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TAccumulate">누적기 값의 형식입니다.</typeparam>
        /// <param name="source">집계할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="seed">초기 누적기 값입니다.</param>
        /// <param name="func">각 요소에 대해 호출할 누적기 함수입니다.</param>
        /// <returns>최종 누적기 값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="func"/>가 null인 경우.</exception>
        public static TAccumulate Aggregate<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            TAccumulate result = seed;
            foreach (TSource element in source)
            {
                result = func(result, element);
            }
            return result;
        }

        /// <summary>
        /// 시퀀스에 누적기 함수를 적용합니다. 지정된 초기값이 초기 누적기 값으로 사용되며, 지정된 함수를 사용하여 결과 값을 선택합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TAccumulate">누적기 값의 형식입니다.</typeparam>
        /// <typeparam name="TResult">결과 값의 형식입니다.</typeparam>
        /// <param name="source">집계할 <see cref="IEnumerable{T}"/>입니다.</param>
        /// <param name="seed">초기 누적기 값입니다.</param>
        /// <param name="func">각 요소에 대해 호출할 누적기 함수입니다.</param>
        /// <param name="resultSelector">최종 누적기 값을 결과 값으로 변환하는 함수입니다.</param>
        /// <returns>변환된 최종 누적기 값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="func"/> 또는 <paramref name="resultSelector"/>가 null인 경우.</exception>
        public static TResult Aggregate<TSource, TAccumulate, TResult>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            TAccumulate result = seed;
            foreach (TSource element in source)
            {
                result = func(result, element);
            }
            return resultSelector(result);
        }

        #endregion

        #region Sum - 합계

        /// <summary>
        /// <see cref="int"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">합이 <see cref="int.MaxValue"/>보다 큰 경우.</exception>
        public static int Sum(this IEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int sum = 0;
            checked
            {
                foreach (int v in source)
                {
                    sum += v;
                }
            }
            return sum;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="int"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">합이 <see cref="int.MaxValue"/>보다 큰 경우.</exception>
        public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// nullable <see cref="int"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <param name="source">합을 계산할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">합이 <see cref="int.MaxValue"/>보다 큰 경우.</exception>
        public static int? Sum(this IEnumerable<int?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int sum = 0;
            checked
            {
                foreach (int? v in source)
                {
                    if (v.HasValue)
                    {
                        sum += v.GetValueOrDefault();
                    }
                }
            }
            return sum;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="int"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">합이 <see cref="int.MaxValue"/>보다 큰 경우.</exception>
        public static int? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// <see cref="long"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">합이 <see cref="long.MaxValue"/>보다 큰 경우.</exception>
        public static long Sum(this IEnumerable<long> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long sum = 0;
            checked
            {
                foreach (long v in source)
                {
                    sum += v;
                }
            }
            return sum;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="long"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">합이 <see cref="long.MaxValue"/>보다 큰 경우.</exception>
        public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// nullable <see cref="long"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <param name="source">합을 계산할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">합이 <see cref="long.MaxValue"/>보다 큰 경우.</exception>
        public static long? Sum(this IEnumerable<long?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long sum = 0;
            checked
            {
                foreach (long? v in source)
                {
                    if (v.HasValue)
                    {
                        sum += v.GetValueOrDefault();
                    }
                }
            }
            return sum;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="long"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">합이 <see cref="long.MaxValue"/>보다 큰 경우.</exception>
        public static long? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// <see cref="float"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static float Sum(this IEnumerable<float> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            foreach (float v in source)
            {
                sum += v;
            }
            return (float)sum;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="float"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// nullable <see cref="float"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <param name="source">합을 계산할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static float? Sum(this IEnumerable<float?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            foreach (float? v in source)
            {
                if (v.HasValue)
                {
                    sum += v.GetValueOrDefault();
                }
            }
            return (float)sum;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="float"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static float? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// <see cref="double"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static double Sum(this IEnumerable<double> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            foreach (double v in source)
            {
                sum += v;
            }
            return sum;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="double"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// nullable <see cref="double"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <param name="source">합을 계산할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static double? Sum(this IEnumerable<double?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            foreach (double? v in source)
            {
                if (v.HasValue)
                {
                    sum += v.GetValueOrDefault();
                }
            }
            return sum;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="double"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static double? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// <see cref="decimal"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">합이 <see cref="decimal.MaxValue"/>보다 큰 경우.</exception>
        public static decimal Sum(this IEnumerable<decimal> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal sum = 0;
            foreach (decimal v in source)
            {
                sum += v;
            }
            return sum;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="decimal"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">합이 <see cref="decimal.MaxValue"/>보다 큰 경우.</exception>
        public static decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// nullable <see cref="decimal"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <param name="source">합을 계산할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">합이 <see cref="decimal.MaxValue"/>보다 큰 경우.</exception>
        public static decimal? Sum(this IEnumerable<decimal?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal sum = 0;
            foreach (decimal? v in source)
            {
                if (v.HasValue)
                {
                    sum += v.GetValueOrDefault();
                }
            }
            return sum;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="decimal"/> 값 시퀀스의 합을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">합을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 합입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="OverflowException">합이 <see cref="decimal.MaxValue"/>보다 큰 경우.</exception>
        public static decimal? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return source.Select(selector).Sum();
        }

        #endregion

        #region Average - 평균

        /// <summary>
        /// <see cref="int"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <returns>값 시퀀스의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static double Average(this IEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long sum = 0;
            long count = 0;
            checked
            {
                foreach (int v in source)
                {
                    sum += v;
                    count++;
                }
            }

            if (count == 0)
            {
                throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
            }

            return (double)sum / count;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="int"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// nullable <see cref="int"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <param name="source">평균을 계산할 nullable 값의 시퀀스입니다.</param>
        /// <returns>값 시퀀스의 평균이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static double? Average(this IEnumerable<int?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long sum = 0;
            long count = 0;
            checked
            {
                foreach (int? v in source)
                {
                    if (v.HasValue)
                    {
                        sum += v.GetValueOrDefault();
                        count++;
                    }
                }
            }

            if (count == 0)
            {
                return null;
            }

            return (double)sum / count;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="int"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// <see cref="long"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <returns>값 시퀀스의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static double Average(this IEnumerable<long> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long sum = 0;
            long count = 0;
            checked
            {
                foreach (long v in source)
                {
                    sum += v;
                    count++;
                }
            }

            if (count == 0)
            {
                throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
            }

            return (double)sum / count;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="long"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// nullable <see cref="long"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <param name="source">평균을 계산할 nullable 값의 시퀀스입니다.</param>
        /// <returns>값 시퀀스의 평균이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static double? Average(this IEnumerable<long?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long sum = 0;
            long count = 0;
            checked
            {
                foreach (long? v in source)
                {
                    if (v.HasValue)
                    {
                        sum += v.GetValueOrDefault();
                        count++;
                    }
                }
            }

            if (count == 0)
            {
                return null;
            }

            return (double)sum / count;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="long"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// <see cref="float"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <returns>값 시퀀스의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static float Average(this IEnumerable<float> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            long count = 0;
            foreach (float v in source)
            {
                sum += v;
                count++;
            }

            if (count == 0)
            {
                throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
            }

            return (float)(sum / count);
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="float"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static float Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// nullable <see cref="float"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <param name="source">평균을 계산할 nullable 값의 시퀀스입니다.</param>
        /// <returns>값 시퀀스의 평균이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static float? Average(this IEnumerable<float?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            long count = 0;
            foreach (float? v in source)
            {
                if (v.HasValue)
                {
                    sum += v.GetValueOrDefault();
                    count++;
                }
            }

            if (count == 0)
            {
                return null;
            }

            return (float)(sum / count);
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="float"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static float? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// <see cref="double"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <returns>값 시퀀스의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static double Average(this IEnumerable<double> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            long count = 0;
            foreach (double v in source)
            {
                sum += v;
                count++;
            }

            if (count == 0)
            {
                throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
            }

            return sum / count;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="double"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// nullable <see cref="double"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <param name="source">평균을 계산할 nullable 값의 시퀀스입니다.</param>
        /// <returns>값 시퀀스의 평균이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static double? Average(this IEnumerable<double?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            long count = 0;
            foreach (double? v in source)
            {
                if (v.HasValue)
                {
                    sum += v.GetValueOrDefault();
                    count++;
                }
            }

            if (count == 0)
            {
                return null;
            }

            return sum / count;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="double"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// <see cref="decimal"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <returns>값 시퀀스의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static decimal Average(this IEnumerable<decimal> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal sum = 0;
            long count = 0;
            foreach (decimal v in source)
            {
                sum += v;
                count++;
            }

            if (count == 0)
            {
                throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
            }

            return sum / count;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="decimal"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static decimal Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select(selector).Average();
        }

        /// <summary>
        /// nullable <see cref="decimal"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <param name="source">평균을 계산할 nullable 값의 시퀀스입니다.</param>
        /// <returns>값 시퀀스의 평균이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static decimal? Average(this IEnumerable<decimal?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal sum = 0;
            long count = 0;
            foreach (decimal? v in source)
            {
                if (v.HasValue)
                {
                    sum += v.GetValueOrDefault();
                    count++;
                }
            }

            if (count == 0)
            {
                return null;
            }

            return sum / count;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="decimal"/> 값 시퀀스의 평균을 계산합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">평균을 계산할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 평균입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static decimal? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return source.Select(selector).Average();
        }

        #endregion

        #region Min - 최소값

        /// <summary>
        /// <see cref="int"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static int Min(this IEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int value;
            using (IEnumerator<int> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    int x = e.Current;
                    if (x < value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="int"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static int Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// nullable <see cref="int"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <param name="source">최소값을 확인할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최소값이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static int? Min(this IEnumerable<int?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int? value = null;
            foreach (int? x in source)
            {
                if (x.HasValue)
                {
                    if (!value.HasValue || x.GetValueOrDefault() < value.GetValueOrDefault())
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="int"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// <see cref="long"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static long Min(this IEnumerable<long> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long value;
            using (IEnumerator<long> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    long x = e.Current;
                    if (x < value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="long"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static long Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// nullable <see cref="long"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <param name="source">최소값을 확인할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최소값이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static long? Min(this IEnumerable<long?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long? value = null;
            foreach (long? x in source)
            {
                if (x.HasValue)
                {
                    if (!value.HasValue || x.GetValueOrDefault() < value.GetValueOrDefault())
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="long"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// <see cref="float"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static float Min(this IEnumerable<float> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            float value;
            using (IEnumerator<float> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    float x = e.Current;
                    if (x < value || float.IsNaN(x))
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="float"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static float Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// nullable <see cref="float"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <param name="source">최소값을 확인할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최소값이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static float? Min(this IEnumerable<float?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            float? value = null;
            foreach (float? x in source)
            {
                if (x.HasValue)
                {
                    if (!value.HasValue || x.GetValueOrDefault() < value.GetValueOrDefault() || float.IsNaN(x.GetValueOrDefault()))
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="float"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// <see cref="double"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static double Min(this IEnumerable<double> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double value;
            using (IEnumerator<double> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    double x = e.Current;
                    if (x < value || double.IsNaN(x))
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="double"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static double Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// nullable <see cref="double"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <param name="source">최소값을 확인할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최소값이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static double? Min(this IEnumerable<double?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double? value = null;
            foreach (double? x in source)
            {
                if (x.HasValue)
                {
                    if (!value.HasValue || x.GetValueOrDefault() < value.GetValueOrDefault() || double.IsNaN(x.GetValueOrDefault()))
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="double"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// <see cref="decimal"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static decimal Min(this IEnumerable<decimal> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal value;
            using (IEnumerator<decimal> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    decimal x = e.Current;
                    if (x < value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="decimal"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static decimal Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// nullable <see cref="decimal"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <param name="source">최소값을 확인할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최소값이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static decimal? Min(this IEnumerable<decimal?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal? value = null;
            foreach (decimal? x in source)
            {
                if (x.HasValue)
                {
                    if (!value.HasValue || x.GetValueOrDefault() < value.GetValueOrDefault())
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="decimal"/> 값 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return source.Select(selector).Min();
        }

        /// <summary>
        /// 제네릭 시퀀스의 최소값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static TSource? Min<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparer<TSource> comparer = Comparer<TSource>.Default;
            TSource? value = default;
            if (value == null)
            {
                foreach (TSource x in source)
                {
                    if (x != null && (value == null || comparer.Compare(x, value) < 0))
                    {
                        value = x;
                    }
                }
                return value;
            }
            else
            {
                bool hasValue = false;
                foreach (TSource x in source)
                {
                    if (hasValue)
                    {
                        if (comparer.Compare(x, value) < 0)
                        {
                            value = x;
                        }
                    }
                    else
                    {
                        value = x;
                        hasValue = true;
                    }
                }

                if (hasValue)
                {
                    return value;
                }

                throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
            }
        }

        /// <summary>
        /// 시퀀스의 각 요소에 대해 변환 함수를 호출하고 최소 결과 값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TResult">selector에서 반환하는 값의 형식입니다.</typeparam>
        /// <param name="source">최소값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최소값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static TResult? Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select(selector).Min();
        }

        #endregion

        #region Max - 최대값

        /// <summary>
        /// <see cref="int"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static int Max(this IEnumerable<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int value;
            using (IEnumerator<int> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    int x = e.Current;
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="int"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// nullable <see cref="int"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <param name="source">최대값을 확인할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최대값이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static int? Max(this IEnumerable<int?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int? value = null;
            foreach (int? x in source)
            {
                if (x.HasValue)
                {
                    if (!value.HasValue || x.GetValueOrDefault() > value.GetValueOrDefault())
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="int"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// <see cref="long"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static long Max(this IEnumerable<long> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long value;
            using (IEnumerator<long> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    long x = e.Current;
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="long"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static long Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// nullable <see cref="long"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <param name="source">최대값을 확인할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최대값이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static long? Max(this IEnumerable<long?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long? value = null;
            foreach (long? x in source)
            {
                if (x.HasValue)
                {
                    if (!value.HasValue || x.GetValueOrDefault() > value.GetValueOrDefault())
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="long"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// <see cref="float"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static float Max(this IEnumerable<float> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            float value;
            using (IEnumerator<float> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    float x = e.Current;
                    if (x > value)
                    {
                        value = x;
                    }
                    else if (float.IsNaN(value))
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="float"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// nullable <see cref="float"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <param name="source">최대값을 확인할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최대값이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static float? Max(this IEnumerable<float?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            float? value = null;
            foreach (float? x in source)
            {
                if (x.HasValue)
                {
                    if (!value.HasValue || x.GetValueOrDefault() > value.GetValueOrDefault() || float.IsNaN(value.GetValueOrDefault()))
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="float"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// <see cref="double"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static double Max(this IEnumerable<double> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double value;
            using (IEnumerator<double> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    double x = e.Current;
                    if (x > value)
                    {
                        value = x;
                    }
                    else if (double.IsNaN(value))
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="double"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static double Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// nullable <see cref="double"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <param name="source">최대값을 확인할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최대값이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static double? Max(this IEnumerable<double?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double? value = null;
            foreach (double? x in source)
            {
                if (x.HasValue)
                {
                    if (!value.HasValue || x.GetValueOrDefault() > value.GetValueOrDefault() || double.IsNaN(value.GetValueOrDefault()))
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="double"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// <see cref="decimal"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static decimal Max(this IEnumerable<decimal> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal value;
            using (IEnumerator<decimal> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    decimal x = e.Current;
                    if (x > value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 <see cref="decimal"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static decimal Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// nullable <see cref="decimal"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <param name="source">최대값을 확인할 nullable 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최대값이거나, 소스 시퀀스가 비어 있거나 null 값만 포함하면 null입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        public static decimal? Max(this IEnumerable<decimal?> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal? value = null;
            foreach (decimal? x in source)
            {
                if (x.HasValue)
                {
                    if (!value.HasValue || x.GetValueOrDefault() > value.GetValueOrDefault())
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 입력 시퀀스의 각 요소에 대해 변환 함수를 호출하여 얻은 nullable <see cref="decimal"/> 값 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            return source.Select(selector).Max();
        }

        /// <summary>
        /// 제네릭 시퀀스의 최대값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <returns>시퀀스의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>가 null인 경우.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/>에 요소가 없는 경우.</exception>
        public static TSource? Max<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparer<TSource> comparer = Comparer<TSource>.Default;
            TSource? value = default;
            if (value == null)
            {
                foreach (TSource x in source)
                {
                    if (x != null && (value == null || comparer.Compare(x, value) > 0))
                    {
                        value = x;
                    }
                }
                return value;
            }
            else
            {
                bool hasValue = false;
                foreach (TSource x in source)
                {
                    if (hasValue)
                    {
                        if (comparer.Compare(x, value) > 0)
                        {
                            value = x;
                        }
                    }
                    else
                    {
                        value = x;
                        hasValue = true;
                    }
                }

                if (hasValue)
                {
                    return value;
                }

                throw new InvalidOperationException("시퀀스에 요소가 없습니다.");
            }
        }

        /// <summary>
        /// 시퀀스의 각 요소에 대해 변환 함수를 호출하고 최대 결과 값을 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TResult">selector에서 반환하는 값의 형식입니다.</typeparam>
        /// <param name="source">최대값을 확인할 값의 시퀀스입니다.</param>
        /// <param name="selector">각 요소에 적용할 변환 함수입니다.</param>
        /// <returns>투영된 값의 최대값입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="selector"/>가 null인 경우.</exception>
        public static TResult? Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select(selector).Max();
        }

        #endregion
    }
}

#endif
