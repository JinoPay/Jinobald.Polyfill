namespace System;

using System.Collections;
using System.Collections.Generic;

#if NET35

/// <summary>
/// 튜플 개체를 만들기 위한 정적 메서드를 제공합니다.
/// </summary>
public static class Tuple
{
    public static Tuple<T1> Create<T1>(T1 item1) => new(item1);

    public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) => new(item1, item2);

    public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) =>
        new(item1, item2, item3);

    public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3,
        T4 item4) => new(item1, item2, item3, item4);

    public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2,
        T3 item3, T4 item4, T5 item5) => new(item1, item2, item3, item4, item5);

    public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1,
        T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) =>
        new(item1, item2, item3, item4, item5, item6);

    public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(
        T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) =>
        new(item1, item2, item3, item4, item5, item6, item7);

    public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Create<T1, T2, T3, T4, T5, T6, T7,
        T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) =>
        new(item1, item2, item3, item4, item5, item6, item7, item8);
}

/// <summary>
/// 1-튜플 또는 싱글톤을 값 형식으로 나타냅니다.
/// </summary>
/// <typeparam name="T1">튜플의 유일한 요소의 형식입니다.</typeparam>
public class Tuple<T1> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
{
    private readonly T1 m_Item1;

    public T1 Item1 => m_Item1;

    public Tuple(T1 item1)
    {
        m_Item1 = item1;
    }

    public override bool Equals(object? obj) =>
        ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);

    public override int GetHashCode() =>
        ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1> objTuple)
            return false;

        return comparer.Equals(m_Item1, objTuple.m_Item1);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1> objTuple)
            throw new ArgumentException("other");

        return comparer.Compare(m_Item1, objTuple.m_Item1);
    }

    int IComparable.CompareTo(object? obj) =>
        ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) =>
        comparer.GetHashCode(m_Item1!);

    public override string ToString() => $"({m_Item1})";

    int ITuple.Length => 1;

    object? ITuple.this[int index]
    {
        get
        {
            if (index != 0)
                throw new IndexOutOfRangeException();
            return m_Item1;
        }
    }
}

/// <summary>
/// 2-튜플 또는 쌍을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
{
    private readonly T1 m_Item1;
    private readonly T2 m_Item2;

    public T1 Item1 => m_Item1;
    public T2 Item2 => m_Item2;

    public Tuple(T1 item1, T2 item2)
    {
        m_Item1 = item1;
        m_Item2 = item2;
    }

    public override bool Equals(object? obj) =>
        ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);

    public override int GetHashCode() =>
        ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2> objTuple)
            return false;

        return comparer.Equals(m_Item1, objTuple.m_Item1) &&
               comparer.Equals(m_Item2, objTuple.m_Item2);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2> objTuple)
            throw new ArgumentException("other");

        int c = comparer.Compare(m_Item1, objTuple.m_Item1);
        if (c != 0) return c;

        return comparer.Compare(m_Item2, objTuple.m_Item2);
    }

    int IComparable.CompareTo(object? obj) =>
        ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = hash * 31 + comparer.GetHashCode(m_Item1!);
        hash = hash * 31 + comparer.GetHashCode(m_Item2!);
        return hash;
    }

    public override string ToString() => $"({m_Item1}, {m_Item2})";

    int ITuple.Length => 2;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => m_Item1,
            1 => m_Item2,
            _ => throw new IndexOutOfRangeException()
        };
}

/// <summary>
/// 3-튜플을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2, T3> : IStructuralEquatable, IStructuralComparable, IComparable,
    ITuple
{
    private readonly T1 m_Item1;
    private readonly T2 m_Item2;
    private readonly T3 m_Item3;

    public T1 Item1 => m_Item1;
    public T2 Item2 => m_Item2;
    public T3 Item3 => m_Item3;

    public Tuple(T1 item1, T2 item2, T3 item3)
    {
        m_Item1 = item1;
        m_Item2 = item2;
        m_Item3 = item3;
    }

    public override bool Equals(object? obj) =>
        ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);

    public override int GetHashCode() =>
        ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3> objTuple)
            return false;

        return comparer.Equals(m_Item1, objTuple.m_Item1) &&
               comparer.Equals(m_Item2, objTuple.m_Item2) &&
               comparer.Equals(m_Item3, objTuple.m_Item3);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3> objTuple)
            throw new ArgumentException("other");

        int c = comparer.Compare(m_Item1, objTuple.m_Item1);
        if (c != 0) return c;
        c = comparer.Compare(m_Item2, objTuple.m_Item2);
        if (c != 0) return c;

        return comparer.Compare(m_Item3, objTuple.m_Item3);
    }

    int IComparable.CompareTo(object? obj) =>
        ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = hash * 31 + comparer.GetHashCode(m_Item1!);
        hash = hash * 31 + comparer.GetHashCode(m_Item2!);
        hash = hash * 31 + comparer.GetHashCode(m_Item3!);
        return hash;
    }

    public override string ToString() => $"({m_Item1}, {m_Item2}, {m_Item3})";

    int ITuple.Length => 3;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => m_Item1,
            1 => m_Item2,
            2 => m_Item3,
            _ => throw new IndexOutOfRangeException()
        };
}

/// <summary>
/// 4-튜플을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2, T3, T4> : IStructuralEquatable, IStructuralComparable, IComparable,
    ITuple
{
    private readonly T1 m_Item1;
    private readonly T2 m_Item2;
    private readonly T3 m_Item3;
    private readonly T4 m_Item4;

    public T1 Item1 => m_Item1;
    public T2 Item2 => m_Item2;
    public T3 Item3 => m_Item3;
    public T4 Item4 => m_Item4;

    public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
    {
        m_Item1 = item1;
        m_Item2 = item2;
        m_Item3 = item3;
        m_Item4 = item4;
    }

    public override bool Equals(object? obj) =>
        ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);

    public override int GetHashCode() =>
        ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4> objTuple)
            return false;

        return comparer.Equals(m_Item1, objTuple.m_Item1) &&
               comparer.Equals(m_Item2, objTuple.m_Item2) &&
               comparer.Equals(m_Item3, objTuple.m_Item3) &&
               comparer.Equals(m_Item4, objTuple.m_Item4);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4> objTuple)
            throw new ArgumentException("other");

        int c = comparer.Compare(m_Item1, objTuple.m_Item1);
        if (c != 0) return c;
        c = comparer.Compare(m_Item2, objTuple.m_Item2);
        if (c != 0) return c;
        c = comparer.Compare(m_Item3, objTuple.m_Item3);
        if (c != 0) return c;

        return comparer.Compare(m_Item4, objTuple.m_Item4);
    }

    int IComparable.CompareTo(object? obj) =>
        ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = hash * 31 + comparer.GetHashCode(m_Item1!);
        hash = hash * 31 + comparer.GetHashCode(m_Item2!);
        hash = hash * 31 + comparer.GetHashCode(m_Item3!);
        hash = hash * 31 + comparer.GetHashCode(m_Item4!);
        return hash;
    }

    public override string ToString() => $"({m_Item1}, {m_Item2}, {m_Item3}, {m_Item4})";

    int ITuple.Length => 4;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => m_Item1,
            1 => m_Item2,
            2 => m_Item3,
            3 => m_Item4,
            _ => throw new IndexOutOfRangeException()
        };
}

/// <summary>
/// 5-튜플을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2, T3, T4, T5> : IStructuralEquatable, IStructuralComparable,
    IComparable, ITuple
{
    private readonly T1 m_Item1;
    private readonly T2 m_Item2;
    private readonly T3 m_Item3;
    private readonly T4 m_Item4;
    private readonly T5 m_Item5;

    public T1 Item1 => m_Item1;
    public T2 Item2 => m_Item2;
    public T3 Item3 => m_Item3;
    public T4 Item4 => m_Item4;
    public T5 Item5 => m_Item5;

    public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
    {
        m_Item1 = item1;
        m_Item2 = item2;
        m_Item3 = item3;
        m_Item4 = item4;
        m_Item5 = item5;
    }

    public override bool Equals(object? obj) =>
        ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);

    public override int GetHashCode() =>
        ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5> objTuple)
            return false;

        return comparer.Equals(m_Item1, objTuple.m_Item1) &&
               comparer.Equals(m_Item2, objTuple.m_Item2) &&
               comparer.Equals(m_Item3, objTuple.m_Item3) &&
               comparer.Equals(m_Item4, objTuple.m_Item4) &&
               comparer.Equals(m_Item5, objTuple.m_Item5);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5> objTuple)
            throw new ArgumentException("other");

        int c = comparer.Compare(m_Item1, objTuple.m_Item1);
        if (c != 0) return c;
        c = comparer.Compare(m_Item2, objTuple.m_Item2);
        if (c != 0) return c;
        c = comparer.Compare(m_Item3, objTuple.m_Item3);
        if (c != 0) return c;
        c = comparer.Compare(m_Item4, objTuple.m_Item4);
        if (c != 0) return c;

        return comparer.Compare(m_Item5, objTuple.m_Item5);
    }

    int IComparable.CompareTo(object? obj) =>
        ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = hash * 31 + comparer.GetHashCode(m_Item1!);
        hash = hash * 31 + comparer.GetHashCode(m_Item2!);
        hash = hash * 31 + comparer.GetHashCode(m_Item3!);
        hash = hash * 31 + comparer.GetHashCode(m_Item4!);
        hash = hash * 31 + comparer.GetHashCode(m_Item5!);
        return hash;
    }

    public override string ToString() =>
        $"({m_Item1}, {m_Item2}, {m_Item3}, {m_Item4}, {m_Item5})";

    int ITuple.Length => 5;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => m_Item1,
            1 => m_Item2,
            2 => m_Item3,
            3 => m_Item4,
            4 => m_Item5,
            _ => throw new IndexOutOfRangeException()
        };
}

/// <summary>
/// 6-튜플을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2, T3, T4, T5, T6> : IStructuralEquatable, IStructuralComparable,
    IComparable, ITuple
{
    private readonly T1 m_Item1;
    private readonly T2 m_Item2;
    private readonly T3 m_Item3;
    private readonly T4 m_Item4;
    private readonly T5 m_Item5;
    private readonly T6 m_Item6;

    public T1 Item1 => m_Item1;
    public T2 Item2 => m_Item2;
    public T3 Item3 => m_Item3;
    public T4 Item4 => m_Item4;
    public T5 Item5 => m_Item5;
    public T6 Item6 => m_Item6;

    public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
    {
        m_Item1 = item1;
        m_Item2 = item2;
        m_Item3 = item3;
        m_Item4 = item4;
        m_Item5 = item5;
        m_Item6 = item6;
    }

    public override bool Equals(object? obj) =>
        ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);

    public override int GetHashCode() =>
        ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5, T6> objTuple)
            return false;

        return comparer.Equals(m_Item1, objTuple.m_Item1) &&
               comparer.Equals(m_Item2, objTuple.m_Item2) &&
               comparer.Equals(m_Item3, objTuple.m_Item3) &&
               comparer.Equals(m_Item4, objTuple.m_Item4) &&
               comparer.Equals(m_Item5, objTuple.m_Item5) &&
               comparer.Equals(m_Item6, objTuple.m_Item6);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5, T6> objTuple)
            throw new ArgumentException("other");

        int c = comparer.Compare(m_Item1, objTuple.m_Item1);
        if (c != 0) return c;
        c = comparer.Compare(m_Item2, objTuple.m_Item2);
        if (c != 0) return c;
        c = comparer.Compare(m_Item3, objTuple.m_Item3);
        if (c != 0) return c;
        c = comparer.Compare(m_Item4, objTuple.m_Item4);
        if (c != 0) return c;
        c = comparer.Compare(m_Item5, objTuple.m_Item5);
        if (c != 0) return c;

        return comparer.Compare(m_Item6, objTuple.m_Item6);
    }

    int IComparable.CompareTo(object? obj) =>
        ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = hash * 31 + comparer.GetHashCode(m_Item1!);
        hash = hash * 31 + comparer.GetHashCode(m_Item2!);
        hash = hash * 31 + comparer.GetHashCode(m_Item3!);
        hash = hash * 31 + comparer.GetHashCode(m_Item4!);
        hash = hash * 31 + comparer.GetHashCode(m_Item5!);
        hash = hash * 31 + comparer.GetHashCode(m_Item6!);
        return hash;
    }

    public override string ToString() =>
        $"({m_Item1}, {m_Item2}, {m_Item3}, {m_Item4}, {m_Item5}, {m_Item6})";

    int ITuple.Length => 6;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => m_Item1,
            1 => m_Item2,
            2 => m_Item3,
            3 => m_Item4,
            4 => m_Item5,
            5 => m_Item6,
            _ => throw new IndexOutOfRangeException()
        };
}

/// <summary>
/// 7-튜플을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2, T3, T4, T5, T6, T7> : IStructuralEquatable,
    IStructuralComparable, IComparable, ITuple
{
    private readonly T1 m_Item1;
    private readonly T2 m_Item2;
    private readonly T3 m_Item3;
    private readonly T4 m_Item4;
    private readonly T5 m_Item5;
    private readonly T6 m_Item6;
    private readonly T7 m_Item7;

    public T1 Item1 => m_Item1;
    public T2 Item2 => m_Item2;
    public T3 Item3 => m_Item3;
    public T4 Item4 => m_Item4;
    public T5 Item5 => m_Item5;
    public T6 Item6 => m_Item6;
    public T7 Item7 => m_Item7;

    public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
    {
        m_Item1 = item1;
        m_Item2 = item2;
        m_Item3 = item3;
        m_Item4 = item4;
        m_Item5 = item5;
        m_Item6 = item6;
        m_Item7 = item7;
    }

    public override bool Equals(object? obj) =>
        ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);

    public override int GetHashCode() =>
        ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5, T6, T7> objTuple)
            return false;

        return comparer.Equals(m_Item1, objTuple.m_Item1) &&
               comparer.Equals(m_Item2, objTuple.m_Item2) &&
               comparer.Equals(m_Item3, objTuple.m_Item3) &&
               comparer.Equals(m_Item4, objTuple.m_Item4) &&
               comparer.Equals(m_Item5, objTuple.m_Item5) &&
               comparer.Equals(m_Item6, objTuple.m_Item6) &&
               comparer.Equals(m_Item7, objTuple.m_Item7);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5, T6, T7> objTuple)
            throw new ArgumentException("other");

        int c = comparer.Compare(m_Item1, objTuple.m_Item1);
        if (c != 0) return c;
        c = comparer.Compare(m_Item2, objTuple.m_Item2);
        if (c != 0) return c;
        c = comparer.Compare(m_Item3, objTuple.m_Item3);
        if (c != 0) return c;
        c = comparer.Compare(m_Item4, objTuple.m_Item4);
        if (c != 0) return c;
        c = comparer.Compare(m_Item5, objTuple.m_Item5);
        if (c != 0) return c;
        c = comparer.Compare(m_Item6, objTuple.m_Item6);
        if (c != 0) return c;

        return comparer.Compare(m_Item7, objTuple.m_Item7);
    }

    int IComparable.CompareTo(object? obj) =>
        ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = hash * 31 + comparer.GetHashCode(m_Item1!);
        hash = hash * 31 + comparer.GetHashCode(m_Item2!);
        hash = hash * 31 + comparer.GetHashCode(m_Item3!);
        hash = hash * 31 + comparer.GetHashCode(m_Item4!);
        hash = hash * 31 + comparer.GetHashCode(m_Item5!);
        hash = hash * 31 + comparer.GetHashCode(m_Item6!);
        hash = hash * 31 + comparer.GetHashCode(m_Item7!);
        return hash;
    }

    public override string ToString() =>
        $"({m_Item1}, {m_Item2}, {m_Item3}, {m_Item4}, {m_Item5}, {m_Item6}, {m_Item7})";

    int ITuple.Length => 7;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => m_Item1,
            1 => m_Item2,
            2 => m_Item3,
            3 => m_Item4,
            4 => m_Item5,
            5 => m_Item6,
            6 => m_Item7,
            _ => throw new IndexOutOfRangeException()
        };
}

/// <summary>
/// 8-튜플을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8> : IStructuralEquatable,
    IStructuralComparable, IComparable, ITuple
{
    private readonly T1 m_Item1;
    private readonly T2 m_Item2;
    private readonly T3 m_Item3;
    private readonly T4 m_Item4;
    private readonly T5 m_Item5;
    private readonly T6 m_Item6;
    private readonly T7 m_Item7;
    private readonly T8 m_Item8;

    public T1 Item1 => m_Item1;
    public T2 Item2 => m_Item2;
    public T3 Item3 => m_Item3;
    public T4 Item4 => m_Item4;
    public T5 Item5 => m_Item5;
    public T6 Item6 => m_Item6;
    public T7 Item7 => m_Item7;
    public T8 Item8 => m_Item8;

    public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7,
        T8 item8)
    {
        m_Item1 = item1;
        m_Item2 = item2;
        m_Item3 = item3;
        m_Item4 = item4;
        m_Item5 = item5;
        m_Item6 = item6;
        m_Item7 = item7;
        m_Item8 = item8;
    }

    public override bool Equals(object? obj) =>
        ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);

    public override int GetHashCode() =>
        ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5, T6, T7, T8> objTuple)
            return false;

        return comparer.Equals(m_Item1, objTuple.m_Item1) &&
               comparer.Equals(m_Item2, objTuple.m_Item2) &&
               comparer.Equals(m_Item3, objTuple.m_Item3) &&
               comparer.Equals(m_Item4, objTuple.m_Item4) &&
               comparer.Equals(m_Item5, objTuple.m_Item5) &&
               comparer.Equals(m_Item6, objTuple.m_Item6) &&
               comparer.Equals(m_Item7, objTuple.m_Item7) &&
               comparer.Equals(m_Item8, objTuple.m_Item8);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5, T6, T7, T8> objTuple)
            throw new ArgumentException("other");

        int c = comparer.Compare(m_Item1, objTuple.m_Item1);
        if (c != 0) return c;
        c = comparer.Compare(m_Item2, objTuple.m_Item2);
        if (c != 0) return c;
        c = comparer.Compare(m_Item3, objTuple.m_Item3);
        if (c != 0) return c;
        c = comparer.Compare(m_Item4, objTuple.m_Item4);
        if (c != 0) return c;
        c = comparer.Compare(m_Item5, objTuple.m_Item5);
        if (c != 0) return c;
        c = comparer.Compare(m_Item6, objTuple.m_Item6);
        if (c != 0) return c;
        c = comparer.Compare(m_Item7, objTuple.m_Item7);
        if (c != 0) return c;

        return comparer.Compare(m_Item8, objTuple.m_Item8);
    }

    int IComparable.CompareTo(object? obj) =>
        ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = hash * 31 + comparer.GetHashCode(m_Item1!);
        hash = hash * 31 + comparer.GetHashCode(m_Item2!);
        hash = hash * 31 + comparer.GetHashCode(m_Item3!);
        hash = hash * 31 + comparer.GetHashCode(m_Item4!);
        hash = hash * 31 + comparer.GetHashCode(m_Item5!);
        hash = hash * 31 + comparer.GetHashCode(m_Item6!);
        hash = hash * 31 + comparer.GetHashCode(m_Item7!);
        hash = hash * 31 + comparer.GetHashCode(m_Item8!);
        return hash;
    }

    public override string ToString() =>
        $"({m_Item1}, {m_Item2}, {m_Item3}, {m_Item4}, {m_Item5}, {m_Item6}, {m_Item7}, {m_Item8})";

    int ITuple.Length => 8;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => m_Item1,
            1 => m_Item2,
            2 => m_Item3,
            3 => m_Item4,
            4 => m_Item5,
            5 => m_Item6,
            6 => m_Item7,
            7 => m_Item8,
            _ => throw new IndexOutOfRangeException()
        };
}

#endif
