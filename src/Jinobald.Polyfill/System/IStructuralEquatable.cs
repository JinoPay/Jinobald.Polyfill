namespace System;

using System.Collections;

#if NET35

/// <summary>
/// Defines methods for supporting the comparison of objects for structural equality.
/// </summary>
public interface IStructuralEquatable
{
    /// <summary>
    /// Determines whether an object is structurally equal to the current object, using a specified comparison method.
    /// </summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <param name="comparer">An object that determines whether the current object and other are equal.</param>
    /// <returns>true if the current object and other are equal; otherwise, false.</returns>
    bool Equals(object? other, IEqualityComparer comparer);

    /// <summary>
    /// Returns a hash code for the current object.
    /// </summary>
    /// <param name="comparer">An object that computes the hash code of the current object.</param>
    /// <returns>The hash code for the current instance.</returns>
    int GetHashCode(IEqualityComparer comparer);
}

#endif
