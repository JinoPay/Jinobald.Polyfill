// Jinobald.Polyfill - HashCode 폴리필
// .NET Core 2.1 이전 버전을 위한 System.HashCode 구현
#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481
namespace System;

/// <summary>
/// Provides a hash code for a type.
/// </summary>
public struct HashCode
{
    private uint _hash;
    private const uint FnvPrime = 16777619;
    private const uint FnvOffsetBasis = 2166136261u;

    /// <summary>
    /// Initializes a new instance of the HashCode struct.
    /// </summary>
    public HashCode()
    {
        _hash = FnvOffsetBasis;
    }

    /// <summary>
    /// Adds a single value to the hash code.
    /// </summary>
    /// <typeparam name="T">The type of the value to add.</typeparam>
    /// <param name="value">The value to add to the hash code.</param>
    public void Add<T>(T value)
    {
        var hashCode = value?.GetHashCode() ?? 0;
        _hash = unchecked((_hash ^ (uint)hashCode) * FnvPrime);
    }

    /// <summary>
    /// Adds a single value with a comparer to the hash code.
    /// </summary>
    /// <typeparam name="T">The type of the value to add.</typeparam>
    /// <param name="value">The value to add to the hash code.</param>
    /// <param name="comparer">The comparer to use when hashing the value.</param>
    public void Add<T>(T value, IEqualityComparer<T> comparer)
    {
        var hashCode = value == null ? 0 : comparer.GetHashCode(value);
        _hash = unchecked((_hash ^ (uint)hashCode) * FnvPrime);
    }

    /// <summary>
    /// Returns the hash code for the current instance.
    /// </summary>
    /// <returns>The hash code for the current instance.</returns>
    public override int GetHashCode()
    {
        return (int)_hash;
    }

    /// <summary>
    /// Combines the hash codes of up to eight values.
    /// </summary>
    public static int Combine<T1>(T1 value1)
    {
        var hc = new HashCode();
        hc.Add(value1);
        return hc.GetHashCode();
    }

    /// <summary>
    /// Combines the hash codes of up to eight values.
    /// </summary>
    public static int Combine<T1, T2>(T1 value1, T2 value2)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        return hc.GetHashCode();
    }

    /// <summary>
    /// Combines the hash codes of up to eight values.
    /// </summary>
    public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        hc.Add(value3);
        return hc.GetHashCode();
    }

    /// <summary>
    /// Combines the hash codes of up to eight values.
    /// </summary>
    public static int Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        hc.Add(value3);
        hc.Add(value4);
        return hc.GetHashCode();
    }

    /// <summary>
    /// Combines the hash codes of up to eight values.
    /// </summary>
    public static int Combine<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        hc.Add(value3);
        hc.Add(value4);
        hc.Add(value5);
        return hc.GetHashCode();
    }

    /// <summary>
    /// Combines the hash codes of up to eight values.
    /// </summary>
    public static int Combine<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        hc.Add(value3);
        hc.Add(value4);
        hc.Add(value5);
        hc.Add(value6);
        return hc.GetHashCode();
    }

    /// <summary>
    /// Combines the hash codes of up to eight values.
    /// </summary>
    public static int Combine<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        hc.Add(value3);
        hc.Add(value4);
        hc.Add(value5);
        hc.Add(value6);
        hc.Add(value7);
        return hc.GetHashCode();
    }

    /// <summary>
    /// Combines the hash codes of up to eight values.
    /// </summary>
    public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        hc.Add(value3);
        hc.Add(value4);
        hc.Add(value5);
        hc.Add(value6);
        hc.Add(value7);
        hc.Add(value8);
        return hc.GetHashCode();
    }

    /// <summary>
    /// Returns the hash code for the current instance.
    /// </summary>
    public int ToHashCode()
    {
        return GetHashCode();
    }
}
#endif
