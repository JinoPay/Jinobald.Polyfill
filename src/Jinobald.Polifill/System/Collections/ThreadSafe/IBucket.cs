#pragma warning disable CA1716 // Identifiers should not match keywords

using System.Diagnostics.CodeAnalysis;

namespace System.Collections.ThreadSafe;

public interface IBucket<T> : IEnumerable<T>
{
    int Count { get; }

    bool Exchange(int index, T item, [MaybeNullWhen(true)] out T previous);

    bool Insert(int index, T item);

    bool Insert(int index, T item, out T previous);

    bool RemoveAt(int index);

    bool RemoveAt(int index, out T previous);

    bool RemoveAt(int index, Predicate<T> check);

    bool TryGet(int index, out T value);

    bool Update(int index, Func<T, T> itemUpdateFactory, Predicate<T> check, out bool isEmpty);

    IEnumerable<KeyValuePair<int, T>> WhereIndexed(Predicate<T> check);

    IEnumerable<T> Where(Predicate<T> check);

    void CopyTo(T[] array, int arrayIndex);

    void Set(int index, T item, out bool isNew);
}