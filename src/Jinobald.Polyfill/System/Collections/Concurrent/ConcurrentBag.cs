#if NET35
using System.Diagnostics;

namespace System.Collections.Concurrent
{
    /// <summary>
    ///     스레드 안전한 순서가 지정되지 않은 개체 컬렉션을 나타냅니다.
    /// </summary>
    /// <typeparam name="T">백에 저장되는 요소의 타입입니다.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class ConcurrentBag<T> : IEnumerable<T>, ICollection
    {
        [ThreadStatic] private static ThreadLocalList? t_currentThreadList;

        private readonly object _lock = new();

        private volatile ThreadLocalList? _head;

        /// <summary>
        ///     ConcurrentBag 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public ConcurrentBag()
        {
        }

        /// <summary>
        ///     지정된 컬렉션의 요소를 포함하는 ConcurrentBag 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="collection">초기 요소로 사용할 컬렉션입니다.</param>
        public ConcurrentBag(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (T item in collection)
            {
                Add(item);
            }
        }

        /// <summary>
        ///     백이 비어 있는지 여부를 나타내는 값을 가져옵니다.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                ThreadLocalList? current = _head;
                while (current != null)
                {
                    lock (current._list)
                    {
                        if (current._list.Count > 0)
                        {
                            return false;
                        }
                    }

                    current = current._next;
                }

                return true;
            }
        }

        /// <summary>
        ///     백에 포함된 요소 수를 가져옵니다.
        /// </summary>
        /// <remarks>
        ///     이 속성은 스냅샷을 반환하며, 동시 작업 중에는 정확하지 않을 수 있습니다.
        /// </remarks>
        public int Count
        {
            get
            {
                int count = 0;
                ThreadLocalList? current = _head;

                while (current != null)
                {
                    lock (current._list)
                    {
                        count += current._list.Count;
                    }

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
            ThreadLocalList? current = _head;

            while (current != null)
            {
                T[] snapshot;
                lock (current._list)
                {
                    snapshot = current._list.ToArray();
                }

                foreach (T item in snapshot)
                {
                    yield return item;
                }

                current = current._next;
            }
        }

        /// <summary>
        ///     백에 있는 개체를 제거하지 않고 반환하려고 시도합니다.
        /// </summary>
        /// <param name="result">백에 있는 개체입니다.</param>
        /// <returns>요소가 성공적으로 반환되면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool TryPeek(out T result)
        {
            ThreadLocalList? list = t_currentThreadList;

            // 현재 스레드의 로컬 리스트에서 시도
            if (list != null && list._ownerThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                lock (list._list)
                {
                    int count = list._list.Count;
                    if (count > 0)
                    {
                        result = list._list[count - 1];
                        return true;
                    }
                }
            }

            // 다른 스레드의 리스트에서 확인
            ThreadLocalList? current = _head;
            while (current != null)
            {
                lock (current._list)
                {
                    int count = current._list.Count;
                    if (count > 0)
                    {
                        result = current._list[0];
                        return true;
                    }
                }

                current = current._next;
            }

            result = default!;
            return false;
        }

        /// <summary>
        ///     백에서 개체를 제거하고 반환하려고 시도합니다.
        /// </summary>
        /// <param name="result">제거된 개체입니다.</param>
        /// <returns>요소가 성공적으로 제거되면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool TryTake(out T result)
        {
            ThreadLocalList? list = t_currentThreadList;

            // 현재 스레드의 로컬 리스트에서 시도
            if (list != null && list._ownerThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                lock (list._list)
                {
                    int count = list._list.Count;
                    if (count > 0)
                    {
                        result = list._list[count - 1];
                        list._list.RemoveAt(count - 1);
                        return true;
                    }
                }
            }

            // Work-stealing: 다른 스레드의 리스트에서 가져오기
            return TrySteal(out result);
        }

        /// <summary>
        ///     백의 요소를 새 배열에 복사합니다.
        /// </summary>
        /// <returns>백의 요소를 포함하는 새 배열입니다.</returns>
        public T[] ToArray()
        {
            var result = new List<T>();
            ThreadLocalList? current = _head;

            while (current != null)
            {
                lock (current._list)
                {
                    result.AddRange(current._list);
                }

                current = current._next;
            }

            return result.ToArray();
        }

        /// <summary>
        ///     백에 개체를 추가합니다.
        /// </summary>
        /// <param name="item">백에 추가할 개체입니다.</param>
        public void Add(T item)
        {
            ThreadLocalList? list = GetThreadLocalList();
            lock (list._list)
            {
                list._list.Add(item);
            }
        }

        /// <summary>
        ///     백의 모든 개체를 기존 배열에 복사합니다.
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
        ///     다른 스레드의 리스트에서 항목을 가져옵니다 (work-stealing).
        /// </summary>
        private bool TrySteal(out T result)
        {
            ThreadLocalList? current = _head;

            while (current != null)
            {
                lock (current._list)
                {
                    int count = current._list.Count;
                    if (count > 0)
                    {
                        result = current._list[0];
                        current._list.RemoveAt(0);
                        current._stolen = true;
                        return true;
                    }
                }

                current = current._next;
            }

            result = default!;
            return false;
        }

        /// <summary>
        ///     현재 스레드의 로컬 리스트를 가져오거나 생성합니다.
        /// </summary>
        private ThreadLocalList GetThreadLocalList()
        {
            ThreadLocalList? list = t_currentThreadList;
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;

            if (list != null && list._ownerThreadId == currentThreadId)
            {
                return list;
            }

            // 새 스레드 로컬 리스트 생성
            list = new ThreadLocalList(currentThreadId);
            t_currentThreadList = list;

            // 전역 리스트에 추가
            lock (_lock)
            {
                list._next = _head;
                _head = list;
            }

            return list;
        }

        /// <summary>
        ///     스레드 로컬 리스트를 나타내는 클래스입니다.
        /// </summary>
        private class ThreadLocalList
        {
            internal readonly int _ownerThreadId;
            internal readonly List<T> _list;
            internal volatile bool _stolen;
            internal ThreadLocalList? _next;

            internal ThreadLocalList(int threadId)
            {
                _ownerThreadId = threadId;
                _list = new List<T>();
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

        object ICollection.SyncRoot => throw new NotSupportedException("ConcurrentBag는 SyncRoot를 지원하지 않습니다.");

        #endregion
    }
}
#endif
