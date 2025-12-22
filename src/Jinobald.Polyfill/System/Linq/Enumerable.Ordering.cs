// Jinobald.Polyfill - LINQ 정렬 연산자
// OrderBy, OrderByDescending, ThenBy, ThenByDescending 및 지원 클래스

#if NET20
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class Enumerable
    {
        #region OrderBy / OrderByDescending - 정렬

        /// <summary>
        /// 시퀀스의 요소를 키에 따라 오름차순으로 정렬합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">정렬할 값의 시퀀스입니다.</param>
        /// <param name="keySelector">요소에서 키를 추출하는 함수입니다.</param>
        /// <returns>요소가 키에 따라 정렬된 <see cref="IOrderedEnumerable{TElement}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return new OrderedEnumerable<TSource, TKey>(source, keySelector, null, false, null);
        }

        /// <summary>
        /// 지정된 비교자를 사용하여 시퀀스의 요소를 오름차순으로 정렬합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">정렬할 값의 시퀀스입니다.</param>
        /// <param name="keySelector">요소에서 키를 추출하는 함수입니다.</param>
        /// <param name="comparer">키를 비교할 <see cref="IComparer{TKey}"/>입니다.</param>
        /// <returns>요소가 키에 따라 정렬된 <see cref="IOrderedEnumerable{TElement}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer)
        {
            return new OrderedEnumerable<TSource, TKey>(source, keySelector, comparer, false, null);
        }

        /// <summary>
        /// 시퀀스의 요소를 키에 따라 내림차순으로 정렬합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">정렬할 값의 시퀀스입니다.</param>
        /// <param name="keySelector">요소에서 키를 추출하는 함수입니다.</param>
        /// <returns>요소가 키에 따라 내림차순으로 정렬된 <see cref="IOrderedEnumerable{TElement}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return new OrderedEnumerable<TSource, TKey>(source, keySelector, null, true, null);
        }

        /// <summary>
        /// 지정된 비교자를 사용하여 시퀀스의 요소를 내림차순으로 정렬합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">정렬할 값의 시퀀스입니다.</param>
        /// <param name="keySelector">요소에서 키를 추출하는 함수입니다.</param>
        /// <param name="comparer">키를 비교할 <see cref="IComparer{TKey}"/>입니다.</param>
        /// <returns>요소가 키에 따라 내림차순으로 정렬된 <see cref="IOrderedEnumerable{TElement}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer)
        {
            return new OrderedEnumerable<TSource, TKey>(source, keySelector, comparer, true, null);
        }

        #endregion

        #region ThenBy / ThenByDescending - 후속 정렬

        /// <summary>
        /// 시퀀스의 요소에 대해 키를 기준으로 후속 오름차순 정렬을 수행합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">정렬 작업을 수행할 <see cref="IOrderedEnumerable{TElement}"/>입니다.</param>
        /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
        /// <returns>요소가 키에 따라 정렬된 <see cref="IOrderedEnumerable{TElement}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.CreateOrderedEnumerable(keySelector, null, false);
        }

        /// <summary>
        /// 지정된 비교자를 사용하여 시퀀스의 요소에 대해 후속 오름차순 정렬을 수행합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">정렬 작업을 수행할 <see cref="IOrderedEnumerable{TElement}"/>입니다.</param>
        /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
        /// <param name="comparer">키를 비교할 <see cref="IComparer{TKey}"/>입니다.</param>
        /// <returns>요소가 키에 따라 정렬된 <see cref="IOrderedEnumerable{TElement}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.CreateOrderedEnumerable(keySelector, comparer, false);
        }

        /// <summary>
        /// 시퀀스의 요소에 대해 키를 기준으로 후속 내림차순 정렬을 수행합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">정렬 작업을 수행할 <see cref="IOrderedEnumerable{TElement}"/>입니다.</param>
        /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
        /// <returns>요소가 키에 따라 내림차순으로 정렬된 <see cref="IOrderedEnumerable{TElement}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.CreateOrderedEnumerable(keySelector, null, true);
        }

        /// <summary>
        /// 지정된 비교자를 사용하여 시퀀스의 요소에 대해 후속 내림차순 정렬을 수행합니다.
        /// </summary>
        /// <typeparam name="TSource">소스 요소의 형식입니다.</typeparam>
        /// <typeparam name="TKey">keySelector에서 반환하는 키의 형식입니다.</typeparam>
        /// <param name="source">정렬 작업을 수행할 <see cref="IOrderedEnumerable{TElement}"/>입니다.</param>
        /// <param name="keySelector">각 요소에서 키를 추출하는 함수입니다.</param>
        /// <param name="comparer">키를 비교할 <see cref="IComparer{TKey}"/>입니다.</param>
        /// <returns>요소가 키에 따라 내림차순으로 정렬된 <see cref="IOrderedEnumerable{TElement}"/>입니다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 또는 <paramref name="keySelector"/>가 null인 경우.</exception>
        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.CreateOrderedEnumerable(keySelector, comparer, true);
        }

        #endregion
    }

    #region OrderedEnumerable 구현 클래스

    /// <summary>
    /// 정렬된 시퀀스의 내부 구현 클래스입니다.
    /// </summary>
    internal abstract class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>
    {
        internal IEnumerable<TElement> _source;

        protected OrderedEnumerable(IEnumerable<TElement> source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            Buffer<TElement> buffer = new Buffer<TElement>(_source);
            if (buffer._count > 0)
            {
                int[] map = SortedMap(buffer);
                for (int i = 0; i < buffer._count; i++)
                {
                    yield return buffer._items[map[i]];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal abstract EnumerableSorter<TElement> GetEnumerableSorter(EnumerableSorter<TElement>? next);

        private int[] SortedMap(Buffer<TElement> buffer)
        {
            return GetEnumerableSorter(null).Sort(buffer._items, buffer._count);
        }

        public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(
            Func<TElement, TKey> keySelector,
            IComparer<TKey>? comparer,
            bool descending)
        {
            return new OrderedEnumerable<TElement, TKey>(_source, keySelector, comparer, descending, this);
        }
    }

    /// <summary>
    /// 특정 키 형식에 대한 정렬된 시퀀스 구현입니다.
    /// </summary>
    internal sealed class OrderedEnumerable<TElement, TKey> : OrderedEnumerable<TElement>
    {
        private readonly OrderedEnumerable<TElement>? _parent;
        private readonly Func<TElement, TKey> _keySelector;
        private readonly IComparer<TKey> _comparer;
        private readonly bool _descending;

        internal OrderedEnumerable(
            IEnumerable<TElement> source,
            Func<TElement, TKey> keySelector,
            IComparer<TKey>? comparer,
            bool descending,
            OrderedEnumerable<TElement>? parent)
            : base(source)
        {
            _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
            _comparer = comparer ?? Comparer<TKey>.Default;
            _descending = descending;
            _parent = parent;
        }

        internal override EnumerableSorter<TElement> GetEnumerableSorter(EnumerableSorter<TElement>? next)
        {
            EnumerableSorter<TElement> sorter = new EnumerableSorter<TElement, TKey>(
                _keySelector, _comparer, _descending, next);
            return _parent != null ? _parent.GetEnumerableSorter(sorter) : sorter;
        }
    }

    /// <summary>
    /// 시퀀스 정렬을 수행하는 추상 클래스입니다.
    /// </summary>
    internal abstract class EnumerableSorter<TElement>
    {
        internal abstract void ComputeKeys(TElement[] elements, int count);
        internal abstract int CompareAnyKeys(int index1, int index2);

        internal int[] Sort(TElement[] elements, int count)
        {
            ComputeKeys(elements, count);
            int[] map = new int[count];
            for (int i = 0; i < count; i++) map[i] = i;
            QuickSort(map, 0, count - 1);
            return map;
        }

        private void QuickSort(int[] map, int left, int right)
        {
            do
            {
                int i = left;
                int j = right;
                int x = map[i + ((j - i) >> 1)];
                do
                {
                    while (i < map.Length && CompareAnyKeys(x, map[i]) > 0) i++;
                    while (j >= 0 && CompareAnyKeys(x, map[j]) < 0) j--;
                    if (i > j) break;
                    if (i < j)
                    {
                        int temp = map[i];
                        map[i] = map[j];
                        map[j] = temp;
                    }
                    i++;
                    j--;
                } while (i <= j);
                if (j - left <= right - i)
                {
                    if (left < j) QuickSort(map, left, j);
                    left = i;
                }
                else
                {
                    if (i < right) QuickSort(map, i, right);
                    right = j;
                }
            } while (left < right);
        }
    }

    /// <summary>
    /// 특정 키 형식에 대한 정렬기 구현입니다.
    /// </summary>
    internal sealed class EnumerableSorter<TElement, TKey> : EnumerableSorter<TElement>
    {
        private readonly Func<TElement, TKey> _keySelector;
        private readonly IComparer<TKey> _comparer;
        private readonly bool _descending;
        private readonly EnumerableSorter<TElement>? _next;
        private TKey[]? _keys;

        internal EnumerableSorter(
            Func<TElement, TKey> keySelector,
            IComparer<TKey> comparer,
            bool descending,
            EnumerableSorter<TElement>? next)
        {
            _keySelector = keySelector;
            _comparer = comparer;
            _descending = descending;
            _next = next;
        }

        internal override void ComputeKeys(TElement[] elements, int count)
        {
            _keys = new TKey[count];
            for (int i = 0; i < count; i++)
            {
                _keys[i] = _keySelector(elements[i]);
            }
            _next?.ComputeKeys(elements, count);
        }

        internal override int CompareAnyKeys(int index1, int index2)
        {
            int c = _comparer.Compare(_keys![index1], _keys[index2]);
            if (c == 0)
            {
                return _next == null ? index1 - index2 : _next.CompareAnyKeys(index1, index2);
            }
            return _descending ? -c : c;
        }
    }

    /// <summary>
    /// 시퀀스 버퍼링을 위한 내부 구조체입니다.
    /// </summary>
    internal struct Buffer<TElement>
    {
        internal TElement[] _items;
        internal int _count;

        internal Buffer(IEnumerable<TElement> source)
        {
            if (source is ICollection<TElement> collection)
            {
                _count = collection.Count;
                if (_count > 0)
                {
                    _items = new TElement[_count];
                    collection.CopyTo(_items, 0);
                }
                else
                {
                    _items = Array.Empty<TElement>();
                }
            }
            else
            {
                _items = Array.Empty<TElement>();
                _count = 0;
                foreach (TElement item in source)
                {
                    if (_items.Length == _count)
                    {
                        Array.Resize(ref _items, _count == 0 ? 4 : _count * 2);
                    }
                    _items[_count++] = item;
                }
            }
        }
    }

    #endregion
}

#endif
