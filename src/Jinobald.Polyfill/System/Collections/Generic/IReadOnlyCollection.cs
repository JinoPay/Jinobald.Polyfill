#if NET35 || NET40
namespace System.Collections.Generic
{
    public interface IReadOnlyCollection<T> : IEnumerable<T>
    {
        int Count { get; }
    }
}
#endif
