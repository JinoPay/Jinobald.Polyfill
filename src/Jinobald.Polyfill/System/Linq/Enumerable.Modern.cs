// Jinobald.Polyfill - Modern LINQ 확장 메서드 (.NET 6.0+)
// .NET 6.0 이상에서 추가된 LINQ 메서드들을 하위 버전에서 사용 가능하도록 폴리필

#if NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETSTANDARD2_0

using System.Collections;

namespace System.Linq
{
    public static partial class Enumerable
    {
        #region TryGetNonEnumeratedCount - .NET 6.0+

        /// <summary>
        ///     시퀀스를 강제로 열거하지 않고 요소의 개수를 가져오려고 시도합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">요소 개수를 확인할 시퀀스입니다.</param>
        /// <param name="count">
        ///     이 메서드가 true를 반환하면 <paramref name="source" />의 요소 개수를 포함합니다.
        ///     그렇지 않으면 0입니다.
        /// </param>
        /// <returns>
        ///     시퀀스를 열거하지 않고 개수를 확인할 수 있으면 true이고, 그렇지 않으면 false입니다.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
        /// <remarks>
        ///     이 메서드는 <paramref name="source" />가 <see cref="ICollection{T}" /> 또는
        ///     <see cref="ICollection" />을 구현하는 경우에만 true를 반환합니다.
        /// </remarks>
        public static bool TryGetNonEnumeratedCount<TSource>(
            this IEnumerable<TSource> source,
            out int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            // ICollection<T> 확인
            if (source is ICollection<TSource> collectionT)
            {
                count = collectionT.Count;
                return true;
            }

            // ICollection 확인 (non-generic)
            if (source is ICollection collection)
            {
                count = collection.Count;
                return true;
            }

            // 배열 확인
            if (source is TSource[] array)
            {
                count = array.Length;
                return true;
            }

            count = 0;
            return false;
        }

        #endregion

        #region Chunk - .NET 6.0+

        /// <summary>
        ///     시퀀스의 요소를 최대 <paramref name="size" /> 크기의 청크로 분할합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">청크로 분할할 시퀀스입니다.</param>
        /// <param name="size">각 청크의 최대 크기입니다.</param>
        /// <returns>각 청크를 배열로 포함하는 시퀀스입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" />가 1보다 작은 경우.</exception>
        /// <remarks>
        ///     마지막 청크는 <paramref name="size" />보다 작은 요소를 포함할 수 있습니다.
        ///     각 청크는 새로운 배열로 구체화됩니다.
        /// </remarks>
        public static IEnumerable<TSource[]> Chunk<TSource>(
            this IEnumerable<TSource> source,
            int size)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException("size", "Chunk size must be greater than 0.");
            }

            return ChunkIterator(source, size);
        }

        private static IEnumerable<TSource[]> ChunkIterator<TSource>(
            IEnumerable<TSource> source,
            int size)
        {
            TSource[]? chunk = null;
            int count = 0;

            foreach (TSource item in source)
            {
                if (chunk == null)
                {
                    chunk = new TSource[size];
                }

                chunk[count++] = item;

                if (count == size)
                {
                    yield return chunk;
                    chunk = null;
                    count = 0;
                }
            }

            // 마지막 청크가 부분적으로 채워진 경우
            if (count > 0 && chunk != null)
            {
                var lastChunk = new TSource[count];
                Array.Copy(chunk, 0, lastChunk, 0, count);
                yield return lastChunk;
            }
        }

        #endregion

        #region Index - .NET 6.0+

        /// <summary>
        ///     시퀀스의 각 요소와 해당 인덱스를 튜플로 반환합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <param name="source">인덱스를 추가할 시퀀스입니다.</param>
        /// <returns>각 요소와 해당 인덱스를 포함하는 튜플의 시퀀스입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" />가 null인 경우.</exception>
        /// <remarks>
        ///     인덱스는 0부터 시작합니다.
        ///     이 메서드는 Select((item, index) => (index, item))의 간편한 대안입니다.
        /// </remarks>
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETSTANDARD2_0
        public static IEnumerable<ValueTuple<int, TSource>> Index<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return IndexIterator(source);
        }

        private static IEnumerable<ValueTuple<int, TSource>> IndexIterator<TSource>(
            IEnumerable<TSource> source)
        {
            int index = 0;
            foreach (TSource item in source)
            {
                yield return new ValueTuple<int, TSource>(index, item);
                checked { index++; }
            }
        }
#else
        // .NET 2.0/3.0/3.5에서는 ValueTuple이 없으므로 Tuple 사용
        public static IEnumerable<Tuple<int, TSource>> Index<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return IndexIterator(source);
        }

        private static IEnumerable<Tuple<int, TSource>> IndexIterator<TSource>(
            IEnumerable<TSource> source)
        {
            int index = 0;
            foreach (TSource item in source)
            {
                yield return Tuple.Create(index, item);
                checked { index++; }
            }
        }
#endif

        #endregion
    }
}

#endif
