// Jinobald.Polyfill - Lookup 클래스
// 각 키가 하나 이상의 값에 매핑되는 컬렉션을 나타냅니다.

#if NET20
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Linq
{
    /// <summary>
    /// 각 키가 하나 이상의 값에 매핑된 키의 컬렉션을 나타냅니다.
    /// </summary>
    /// <typeparam name="TKey"><see cref="Lookup{TKey, TElement}"/>의 키 형식입니다.</typeparam>
    /// <typeparam name="TElement"><see cref="Lookup{TKey, TElement}"/>의 각 <see cref="IEnumerable{TElement}"/> 값의 요소 형식입니다.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class Lookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private Grouping<TKey, TElement>[] _groupings;
        private Grouping<TKey, TElement>? _lastGrouping;
        private int _count;

        /// <summary>
        /// 소스에서 Lookup을 생성합니다.
        /// </summary>
        internal static Lookup<TKey, TElement> Create<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));

            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            foreach (TSource item in source)
            {
                lookup.GetGrouping(keySelector(item), create: true)!.Add(elementSelector(item));
            }
            return lookup;
        }

        /// <summary>
        /// Join 연산을 위한 Lookup을 생성합니다.
        /// </summary>
        internal static Lookup<TKey, TElement> CreateForJoin(
            IEnumerable<TElement> source,
            Func<TElement, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            foreach (TElement item in source)
            {
                TKey key = keySelector(item);
                if (key != null)
                {
                    lookup.GetGrouping(key, create: true)!.Add(item);
                }
            }
            return lookup;
        }

        private Lookup(IEqualityComparer<TKey>? comparer)
        {
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _groupings = new Grouping<TKey, TElement>[7];
        }

        /// <summary>
        /// <see cref="Lookup{TKey, TElement}"/>에 있는 키/값 컬렉션 쌍의 수를 가져옵니다.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// 지정된 키로 인덱싱된 값의 컬렉션을 가져옵니다.
        /// </summary>
        /// <param name="key">원하는 값 컬렉션의 키입니다.</param>
        /// <returns>지정된 키로 인덱싱된 값의 컬렉션입니다.</returns>
        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                Grouping<TKey, TElement>? grouping = GetGrouping(key, create: false);
                return grouping ?? Enumerable.Empty<TElement>();
            }
        }

        /// <summary>
        /// 지정된 키가 <see cref="Lookup{TKey, TElement}"/>에 있는지 확인합니다.
        /// </summary>
        /// <param name="key"><see cref="Lookup{TKey, TElement}"/>에서 찾을 키입니다.</param>
        /// <returns><paramref name="key"/>가 <see cref="Lookup{TKey, TElement}"/>에 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool Contains(TKey key) => GetGrouping(key, create: false) != null;

        /// <summary>
        /// <see cref="Lookup{TKey, TElement}"/>를 반복하는 제네릭 열거자를 반환합니다.
        /// </summary>
        /// <returns><see cref="Lookup{TKey, TElement}"/>에 대한 열거자입니다.</returns>
        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            Grouping<TKey, TElement>? g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    yield return g!;
                }
                while (g != _lastGrouping);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// 각 키와 연결된 값에 변환 함수를 적용하고 결과를 반환합니다.
        /// </summary>
        /// <typeparam name="TResult"><paramref name="resultSelector"/>에서 생성하는 결과 값의 형식입니다.</typeparam>
        /// <param name="resultSelector">각 키와 연결된 값에서 결과 값을 투영하는 함수입니다.</param>
        /// <returns><see cref="Lookup{TKey, TElement}"/>의 각 키/값 컬렉션 쌍에 대해 하나의 값이 포함된 컬렉션입니다.</returns>
        public IEnumerable<TResult> ApplyResultSelector<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            Grouping<TKey, TElement>? g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    yield return resultSelector(g!._key, g);
                }
                while (g != _lastGrouping);
            }
        }

        /// <summary>
        /// 지정된 키에 대한 그룹을 가져오거나 생성합니다.
        /// </summary>
        internal Grouping<TKey, TElement>? GetGrouping(TKey key, bool create)
        {
            int hashCode = InternalGetHashCode(key);
            for (Grouping<TKey, TElement>? g = _groupings[hashCode % _groupings.Length]; g != null; g = g._hashNext)
            {
                if (g._hashCode == hashCode && _comparer.Equals(g._key, key))
                {
                    return g;
                }
            }

            if (create)
            {
                if (_count == _groupings.Length)
                {
                    Resize();
                }

                int index = hashCode % _groupings.Length;
                Grouping<TKey, TElement> g = new Grouping<TKey, TElement>(key, hashCode);
                g._hashNext = _groupings[index];
                _groupings[index] = g;
                if (_lastGrouping == null)
                {
                    g._next = g;
                }
                else
                {
                    g._next = _lastGrouping._next;
                    _lastGrouping._next = g;
                }
                _lastGrouping = g;
                _count++;
                return g;
            }

            return null;
        }

        /// <summary>
        /// 키에 대한 해시 코드를 계산합니다 (null 안전).
        /// </summary>
        private int InternalGetHashCode(TKey key)
        {
            // null 키를 우아하게 처리
            return (key == null) ? 0 : _comparer.GetHashCode(key) & 0x7FFFFFFF;
        }

        /// <summary>
        /// 내부 해시 테이블의 크기를 조정합니다.
        /// </summary>
        private void Resize()
        {
            int newSize = checked((_count * 2) + 1);
            Grouping<TKey, TElement>[] newGroupings = new Grouping<TKey, TElement>[newSize];
            Grouping<TKey, TElement>? g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    int index = g!._hashCode % newSize;
                    g._hashNext = newGroupings[index];
                    newGroupings[index] = g;
                }
                while (g != _lastGrouping);
            }
            _groupings = newGroupings;
        }
    }

    /// <summary>
    /// 공통 키를 가진 요소의 그룹을 나타냅니다.
    /// </summary>
    /// <typeparam name="TKey">키의 형식입니다.</typeparam>
    /// <typeparam name="TElement">요소의 형식입니다.</typeparam>
    [DebuggerDisplay("Key = {Key}, Count = {Count}")]
    internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IList<TElement>
    {
        internal readonly TKey _key;
        internal readonly int _hashCode;
        internal TElement[] _elements;
        internal int _count;
        internal Grouping<TKey, TElement>? _hashNext;
        internal Grouping<TKey, TElement>? _next;

        internal Grouping(TKey key, int hashCode)
        {
            _key = key;
            _hashCode = hashCode;
            _elements = new TElement[1];
        }

        /// <summary>
        /// 그룹의 키를 가져옵니다.
        /// </summary>
        public TKey Key => _key;

        /// <summary>
        /// 그룹의 요소 수를 가져옵니다.
        /// </summary>
        public int Count => _count;

        bool ICollection<TElement>.IsReadOnly => true;

        TElement IList<TElement>.this[int index]
        {
            get
            {
                if (index < 0 || index >= _count) throw new ArgumentOutOfRangeException(nameof(index));
                return _elements[index];
            }
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// 그룹에 요소를 추가합니다.
        /// </summary>
        internal void Add(TElement element)
        {
            if (_elements.Length == _count)
            {
                Array.Resize(ref _elements, checked(_count * 2));
            }
            _elements[_count] = element;
            _count++;
        }

        /// <summary>
        /// 그룹의 요소를 반복하는 열거자를 반환합니다.
        /// </summary>
        public IEnumerator<TElement> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _elements[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<TElement>.Add(TElement item) => throw new NotSupportedException();

        void ICollection<TElement>.Clear() => throw new NotSupportedException();

        bool ICollection<TElement>.Contains(TElement item) =>
            Array.IndexOf(_elements, item, 0, _count) >= 0;

        void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex) =>
            Array.Copy(_elements, 0, array, arrayIndex, _count);

        bool ICollection<TElement>.Remove(TElement item) => throw new NotSupportedException();

        int IList<TElement>.IndexOf(TElement item) =>
            Array.IndexOf(_elements, item, 0, _count);

        void IList<TElement>.Insert(int index, TElement item) => throw new NotSupportedException();

        void IList<TElement>.RemoveAt(int index) => throw new NotSupportedException();
    }
}

#endif
