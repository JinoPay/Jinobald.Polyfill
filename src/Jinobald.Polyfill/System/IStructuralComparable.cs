using System.Collections;

namespace System;

#if NET35

/// <summary>
///     Provides support for structural comparison of collection objects.
/// </summary>
public interface IStructuralComparable
{
    /// <summary>
    ///     Determines the relationship of the current collection object to a second object of the same type.
    /// </summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <param name="comparer">An object that provides custom rules for comparison of collection elements.</param>
    /// <returns>
    ///     An integer that indicates the relationship of the current object to other, as shown in the following table.
    ///     Value Meaning
    ///     -1 The current instance comes before other in the sort order.
    ///     0 The current instance occurs in the same position in the sort order as other.
    ///     1 The current instance comes after other in the sort order.
    /// </returns>
    int CompareTo(object? other, IComparer comparer);
}

#endif
