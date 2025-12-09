// Needed for NET40

namespace System.Threading.Needles;

public interface ICacheNeedle<T> : INeedle<T>, IPromise<T>
{
    bool TryGetValue(out T value);
}