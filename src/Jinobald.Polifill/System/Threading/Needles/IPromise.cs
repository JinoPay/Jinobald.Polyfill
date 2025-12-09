// Needed for NET40

namespace System.Threading.Needles;

public interface IPromise
{
    bool IsCompleted { get; }
}

public interface IPromise<out T> : IPromise, IReadOnlyNeedle<T>
{
    // Empty
}