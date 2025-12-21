namespace System;

/// <summary>
/// Provides static methods for creating tuple objects.
/// </summary>
public interface ITuple
{
    /// <summary>
    /// Gets the number of elements in the tuple.
    /// </summary>
    int Length { get; }

    /// <summary>
    /// Gets the value of the element at the specified index.
    /// </summary>
    /// <param name="index">The index of the element to retrieve.</param>
    /// <returns>The value of the element at the specified index.</returns>
    object? this[int index] { get; }
}
