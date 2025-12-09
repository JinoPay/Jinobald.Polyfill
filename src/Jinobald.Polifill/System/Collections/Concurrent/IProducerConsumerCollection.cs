#if NET35

namespace System.Collections.Concurrent;

public interface IProducerConsumerCollection<T> : IEnumerable<T>, ICollection
{
    bool TryAdd(T item);

    bool TryTake(out T item);

    T[] ToArray();
    void CopyTo(T[] array, int index);
}

#endif