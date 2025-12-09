// Needed for NET40

namespace System.Threading.Needles;

public interface INeedle<T> : IReadOnlyNeedle<T>
{
    new T Value { get; set; }
}