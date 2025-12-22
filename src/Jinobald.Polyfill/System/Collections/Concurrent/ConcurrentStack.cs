#if NET35 || NET40
using System.Diagnostics;

namespace System.Collections.Concurrent
{
    /// <summary>
    ///     스레드 안전한 LIFO(Last-In-First-Out) 컬렉션을 나타냅니다.
    /// </summary>
    /// <typeparam name="T">스택에 저장되는 요소의 타입입니다.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class ConcurrentStack<T> : IEnumerable<T>, ICollection
    {
        private volatile Node? _head;

        /// <summary>
        ///     ConcurrentStack 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public ConcurrentStack()
        {
        }

        /// <summary>
        ///     지정된 컬렉션의 요소를 포함하는 ConcurrentStack 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="collection">초기 요소로 사용할 컬렉션입니다.</param>
        public ConcurrentStack(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (T item in collection)
            {
                Push(item);
            }
        }

        /// <summary>
        ///     스택이 비어 있는지 여부를 나타내는 값을 가져옵니다.
        /// </summary>
        public bool IsEmpty => _head == null;

        /// <summary>
        ///     스택에 포함된 요소 수를 가져옵니다.
        /// </summary>
        /// <remarks>
        ///     이 속성은 스냅샷을 반환하며, 동시 작업 중에는 정확하지 않을 수 있습니다.
        /// </remarks>
        public int Count
        {
            get
            {
                int count = 0;
                Node? current = _head;

                while (current != null)
                {
                    count++;
                    current = current._next;
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
            Node? current = _head;

            while (current != null)
            {
                yield return current._value;
                current = current._next;
            }
        }

        /// <summary>
        ///     스택의 맨 위에 있는 개체를 제거하지 않고 반환하려고 시도합니다.
        /// </summary>
        /// <param name="result">스택의 맨 위에 있는 개체입니다.</param>
        /// <returns>요소가 성공적으로 반환되면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool TryPeek(out T result)
        {
            Node? head = _head;
            if (head == null)
            {
                result = default!;
                return false;
            }

            result = head._value;
            return true;
        }

        /// <summary>
        ///     스택의 맨 위에서 개체를 제거하고 반환하려고 시도합니다.
        /// </summary>
        /// <param name="result">제거된 개체입니다.</param>
        /// <returns>요소가 성공적으로 제거되면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool TryPop(out T result)
        {
            var spin = new SpinWait();

            while (true)
            {
                Node? oldHead = _head;
                if (oldHead == null)
                {
                    result = default!;
                    return false;
                }

                if (Interlocked.CompareExchange(ref _head, oldHead._next, oldHead) == oldHead)
                {
                    result = oldHead._value;
                    return true;
                }

                spin.SpinOnce();
            }
        }

        /// <summary>
        ///     여러 개체를 스택의 맨 위에서 원자적으로 제거하고 반환하려고 시도합니다.
        /// </summary>
        /// <param name="items">제거된 개체를 저장할 배열입니다.</param>
        /// <returns>제거된 요소의 개수입니다.</returns>
        public int TryPopRange(T[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            return TryPopRange(items, 0, items.Length);
        }

        /// <summary>
        ///     여러 개체를 스택의 맨 위에서 원자적으로 제거하고 반환하려고 시도합니다.
        /// </summary>
        /// <param name="items">제거된 개체를 저장할 배열입니다.</param>
        /// <param name="startIndex">배열에서 시작할 인덱스입니다.</param>
        /// <param name="count">제거할 최대 요소 개수입니다.</param>
        /// <returns>제거된 요소의 개수입니다.</returns>
        public int TryPopRange(T[] items, int startIndex, int count)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (startIndex < 0 || startIndex >= items.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (count < 0 || startIndex + count > items.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count == 0)
            {
                return 0;
            }

            var spin = new SpinWait();

            while (true)
            {
                Node? oldHead = _head;
                if (oldHead == null)
                {
                    return 0;
                }

                // 제거할 요소 개수 계산
                int numToRemove = 0;
                Node? current = oldHead;
                while (current != null && numToRemove < count)
                {
                    numToRemove++;
                    current = current._next;
                }

                // 원자적으로 노드 제거
                if (Interlocked.CompareExchange(ref _head, current, oldHead) == oldHead)
                {
                    // 배열에 복사
                    current = oldHead;
                    for (int i = 0; i < numToRemove; i++)
                    {
                        items[startIndex + i] = current!._value;
                        current = current._next;
                    }

                    return numToRemove;
                }

                spin.SpinOnce();
            }
        }

        /// <summary>
        ///     스택의 요소를 새 배열에 복사합니다.
        /// </summary>
        /// <returns>스택의 요소를 포함하는 새 배열입니다.</returns>
        public T[] ToArray()
        {
            var list = new List<T>();
            Node? current = _head;

            while (current != null)
            {
                list.Add(current._value);
                current = current._next;
            }

            return list.ToArray();
        }

        /// <summary>
        ///     스택의 모든 개체를 제거합니다.
        /// </summary>
        public void Clear()
        {
            _head = null;
        }

        /// <summary>
        ///     스택의 모든 개체를 기존 배열에 복사합니다.
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
        ///     스택의 맨 위에 개체를 삽입합니다.
        /// </summary>
        /// <param name="item">스택에 추가할 개체입니다.</param>
        public void Push(T item)
        {
            var newNode = new Node(item);
            var spin = new SpinWait();

            while (true)
            {
                Node? oldHead = _head;
                newNode._next = oldHead;

                if (Interlocked.CompareExchange(ref _head, newNode, oldHead) == oldHead)
                {
                    return;
                }

                spin.SpinOnce();
            }
        }

        /// <summary>
        ///     여러 개체를 스택의 맨 위에 원자적으로 삽입합니다.
        /// </summary>
        /// <param name="items">스택에 추가할 개체 배열입니다.</param>
        public void PushRange(T[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            PushRange(items, 0, items.Length);
        }

        /// <summary>
        ///     여러 개체를 스택의 맨 위에 원자적으로 삽입합니다.
        /// </summary>
        /// <param name="items">스택에 추가할 개체 배열입니다.</param>
        /// <param name="startIndex">배열에서 시작할 인덱스입니다.</param>
        /// <param name="count">추가할 요소의 개수입니다.</param>
        public void PushRange(T[] items, int startIndex, int count)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (startIndex < 0 || startIndex >= items.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (count < 0 || startIndex + count > items.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count == 0)
            {
                return;
            }

            // 노드 체인 생성
            Node? head = null;
            Node? tail = null;

            for (int i = startIndex; i < startIndex + count; i++)
            {
                var newNode = new Node(items[i]);
                if (head == null)
                {
                    head = tail = newNode;
                }
                else
                {
                    newNode._next = head;
                    head = newNode;
                }
            }

            // 원자적으로 체인을 스택에 추가
            var spin = new SpinWait();
            while (true)
            {
                Node? oldHead = _head;
                tail!._next = oldHead;

                if (Interlocked.CompareExchange(ref _head, head, oldHead) == oldHead)
                {
                    return;
                }

                spin.SpinOnce();
            }
        }

        /// <summary>
        ///     스택의 노드를 나타내는 클래스입니다.
        /// </summary>
        private class Node
        {
            internal readonly T _value;
            internal Node? _next;

            internal Node(T value)
            {
                _value = value;
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

        object ICollection.SyncRoot => throw new NotSupportedException("ConcurrentStack은 SyncRoot를 지원하지 않습니다.");

        #endregion
    }
}
#endif
