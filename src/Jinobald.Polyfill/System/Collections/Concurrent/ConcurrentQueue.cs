#if NET35
using System.Diagnostics;

namespace System.Collections.Concurrent
{
    /// <summary>
    ///     스레드 안전한 FIFO(First-In-First-Out) 컬렉션을 나타냅니다.
    /// </summary>
    /// <typeparam name="T">큐에 저장되는 요소의 타입입니다.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class ConcurrentQueue<T> : IEnumerable<T>, ICollection
    {
        private const int SEGMENT_SIZE = 32;

        private volatile Segment _head;
        private volatile Segment _tail;

        /// <summary>
        ///     ConcurrentQueue 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public ConcurrentQueue()
        {
            _head = _tail = new Segment(SEGMENT_SIZE);
        }

        /// <summary>
        ///     지정된 컬렉션의 요소를 포함하는 ConcurrentQueue 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="collection">초기 요소로 사용할 컬렉션입니다.</param>
        public ConcurrentQueue(IEnumerable<T> collection)
            : this()
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (T item in collection)
            {
                Enqueue(item);
            }
        }

        /// <summary>
        ///     큐가 비어 있는지 여부를 나타내는 값을 가져옵니다.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                Segment head = _head;
                if (head._low > head._high)
                {
                    return true;
                }

                if (head._low < head._high)
                {
                    return false;
                }

                // head._low == head._high
                return head._next == null;
            }
        }

        /// <summary>
        ///     큐에 포함된 요소 수를 가져옵니다.
        /// </summary>
        /// <remarks>
        ///     이 속성은 스냅샷을 반환하며, 동시 작업 중에는 정확하지 않을 수 있습니다.
        /// </remarks>
        public int Count
        {
            get
            {
                int count = 0;
                Segment? segment = _head;

                while (segment != null)
                {
                    int low = segment._low;
                    int high = segment._high;
                    count += high - low;
                    segment = segment._next;
                }

                return count;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     컬렉션을 반복하는 열거자를 반환합니다.
        /// </summary>
        /// <returns>컬렉션을 반복하는 데 사용할 수 있는 열거자입니다.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            Segment? segment = _head;

            while (segment != null)
            {
                int low = segment._low;
                int high = segment._high;

                for (int i = low; i < high; i++)
                {
                    yield return segment._array[i % SEGMENT_SIZE];
                }

                segment = segment._next;
            }
        }

        /// <summary>
        ///     큐의 시작 부분에서 개체를 제거하고 반환하려고 시도합니다.
        /// </summary>
        /// <param name="result">제거된 개체입니다.</param>
        /// <returns>요소가 성공적으로 제거되면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool TryDequeue(out T result)
        {
            var spin = new SpinWait();

            while (true)
            {
                Segment head = _head;
                int low = head._low;
                int high = head._high;

                if (low < high)
                {
                    // 현재 세그먼트에 요소가 있음
                    if (Interlocked.CompareExchange(ref head._low, low + 1, low) == low)
                    {
                        result = head._array[low % SEGMENT_SIZE];
                        head._array[low % SEGMENT_SIZE] = default!;
                        return true;
                    }
                }
                else if (low == high)
                {
                    // 세그먼트가 비어있을 수 있음
                    Segment? next = head._next;
                    if (next == null)
                    {
                        // 큐가 비어있음
                        result = default!;
                        return false;
                    }

                    // 다음 세그먼트로 이동
                    Interlocked.CompareExchange(ref _head, next, head);
                }
                else
                {
                    // 다른 스레드가 모든 요소를 제거함
                    result = default!;
                    return false;
                }

                spin.SpinOnce();
            }
        }

        /// <summary>
        ///     큐의 시작 부분에 있는 개체를 제거하지 않고 반환하려고 시도합니다.
        /// </summary>
        /// <param name="result">큐의 시작 부분에 있는 개체입니다.</param>
        /// <returns>요소가 성공적으로 반환되면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool TryPeek(out T result)
        {
            while (true)
            {
                Segment head = _head;
                int low = head._low;
                int high = head._high;

                if (low < high)
                {
                    result = head._array[low % SEGMENT_SIZE];
                    return true;
                }
                else if (low == high)
                {
                    Segment? next = head._next;
                    if (next == null)
                    {
                        result = default!;
                        return false;
                    }

                    // 다음 세그먼트로 이동 시도
                    Interlocked.CompareExchange(ref _head, next, head);
                }
                else
                {
                    result = default!;
                    return false;
                }
            }
        }

        /// <summary>
        ///     큐의 요소를 새 배열에 복사합니다.
        /// </summary>
        /// <returns>큐의 요소를 포함하는 새 배열입니다.</returns>
        public T[] ToArray()
        {
            var list = new List<T>();
            Segment? segment = _head;

            while (segment != null)
            {
                int low = segment._low;
                int high = segment._high;

                for (int i = low; i < high; i++)
                {
                    list.Add(segment._array[i % SEGMENT_SIZE]);
                }

                segment = segment._next;
            }

            return list.ToArray();
        }

        /// <summary>
        ///     큐의 모든 개체를 기존 배열에 복사합니다.
        /// </summary>
        /// <param name="array">대상 배열입니다.</param>
        /// <param name="index">복사를 시작할 인덱스입니다.</param>
        public void CopyTo(T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            T[] elements = ToArray();
            Array.Copy(elements, 0, array, index, elements.Length);
        }

        /// <summary>
        ///     큐의 끝에 개체를 추가합니다.
        /// </summary>
        /// <param name="item">큐에 추가할 개체입니다.</param>
        public void Enqueue(T item)
        {
            var spin = new SpinWait();

            while (true)
            {
                Segment tail = _tail;
                int high = tail._high;
                int low = tail._low;

                if (high - low < SEGMENT_SIZE)
                {
                    // 현재 세그먼트에 공간이 있음
                    if (Interlocked.CompareExchange(ref tail._high, high + 1, high) == high)
                    {
                        tail._array[high % SEGMENT_SIZE] = item;
                        return;
                    }
                }
                else
                {
                    // 현재 세그먼트가 가득 참 - 새 세그먼트 할당
                    Segment? next = tail._next;
                    if (next == null)
                    {
                        var newSegment = new Segment(SEGMENT_SIZE);
                        if (Interlocked.CompareExchange(ref tail._next, newSegment, null) == null)
                        {
                            // 새 세그먼트를 tail로 설정
                            Interlocked.CompareExchange(ref _tail, newSegment, tail);
                        }
                    }
                    else
                    {
                        // 다른 스레드가 이미 새 세그먼트를 만듦
                        Interlocked.CompareExchange(ref _tail, next, tail);
                    }
                }

                spin.SpinOnce();
            }
        }

        /// <summary>
        ///     큐의 세그먼트를 나타내는 클래스입니다.
        /// </summary>
        private class Segment
        {
            internal readonly T[] _array;
            internal volatile Segment? _next;
            internal volatile int _high;
            internal volatile int _low;

            internal Segment(int size)
            {
                _array = new T[size];
            }
        }

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            T[] elements = ToArray();
            Array.Copy(elements, 0, array, index, elements.Length);
        }

        int ICollection.Count => Count;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => throw new NotSupportedException("ConcurrentQueue는 SyncRoot를 지원하지 않습니다.");

        #endregion
    }
}
#endif
