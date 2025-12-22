#if NET35 || NET40
namespace System
{
    /// <summary>
    ///     Represents the method that compares two objects of the same type.
    /// </summary>
    /// <typeparam name="T">The type of the objects to compare.</typeparam>
    /// <param name="x">The first object to compare.</param>
    /// <param name="y">The second object to compare.</param>
    /// <returns>
    ///     A signed integer that indicates the relative values of x and y:
    ///     - Less than zero: x is less than y
    ///     - Zero: x equals y
    ///     - Greater than zero: x is greater than y
    /// </returns>
    public delegate int Comparison<in T>(T x, T y);
}
#endif
