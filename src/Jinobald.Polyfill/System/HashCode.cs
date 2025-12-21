// Jinobald.Polyfill - HashCode 폴리필
// .NET Core 2.1 이전 버전을 위한 System.HashCode 구현
#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481
namespace System;

/// <summary>
/// 형식에 대한 해시 코드를 제공합니다.
/// </summary>
public struct HashCode
{
    private uint _hash;
    private const uint FnvPrime = 16777619;
    private const uint FnvOffsetBasis = 2166136261u;

    /// <summary>
    /// HashCode 구조체의 새 인스턴스를 초기화합니다.
    /// </summary>
    public HashCode()
    {
        _hash = FnvOffsetBasis;
    }

    /// <summary>
    /// 해시 코드에 단일 값을 추가합니다.
    /// </summary>
    /// <typeparam name="T">추가할 값의 형식입니다.</typeparam>
    /// <param name="value">해시 코드에 추가할 값입니다.</param>
    public void Add<T>(T value)
    {
        var hashCode = value?.GetHashCode() ?? 0;
        _hash = unchecked((_hash ^ (uint)hashCode) * FnvPrime);
    }

    /// <summary>
    /// 비교자를 사용하여 해시 코드에 단일 값을 추가합니다.
    /// </summary>
    /// <typeparam name="T">추가할 값의 형식입니다.</typeparam>
    /// <param name="value">해시 코드에 추가할 값입니다.</param>
    /// <param name="comparer">값을 해시할 때 사용할 비교자입니다.</param>
    public void Add<T>(T value, IEqualityComparer<T> comparer)
    {
        var hashCode = value == null ? 0 : comparer.GetHashCode(value);
        _hash = unchecked((_hash ^ (uint)hashCode) * FnvPrime);
    }

    /// <summary>
    /// 현재 인스턴스에 대한 해시 코드를 반환합니다.
    /// </summary>
    /// <returns>현재 인스턴스에 대한 해시 코드입니다.</returns>
    public override int GetHashCode()
    {
        return (int)_hash;
    }

    /// <summary>
    /// 최대 8개 값의 해시 코드를 결합합니다.
    /// </summary>
    public static int Combine<T1>(T1 value1)
    {
        var hc = new HashCode();
        hc.Add(value1);
        return hc.GetHashCode();
    }

    /// <summary>
    /// 최대 8개 값의 해시 코드를 결합합니다.
    /// </summary>
    public static int Combine<T1, T2>(T1 value1, T2 value2)
    {
        var hc = new HashCode();
        hc.Add(value1);
        hc.Add(value2);
        return hc.GetHashCode();
    }

    /// <summary>
    /// 최대 8개 값의 해시 코드를 결합합니다.
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
    /// 최대 8개 값의 해시 코드를 결합합니다.
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
    /// 최대 8개 값의 해시 코드를 결합합니다.
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
    /// 최대 8개 값의 해시 코드를 결합합니다.
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
    /// 최대 8개 값의 해시 코드를 결합합니다.
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
    /// 최대 8개 값의 해시 코드를 결합합니다.
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
    /// 현재 인스턴스에 대한 해시 코드를 반환합니다.
    /// </summary>
    public int ToHashCode()
    {
        return GetHashCode();
    }
}
#endif
