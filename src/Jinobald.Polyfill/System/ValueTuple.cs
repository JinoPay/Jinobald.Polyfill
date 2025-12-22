// Jinobald.Polyfill - ValueTuple 폴리필
// .NET 3.5~4.6.2에서 ValueTuple을 사용할 수 있도록 하는 구현
// .NET 4.7 이상에서는 네이티브 ValueTuple이 존재하므로 제외

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462
using System.Collections;

namespace System;

/// <summary>
///     값 튜플 개체를 만들기 위한 정적 메서드를 제공합니다.
/// </summary>
public static class ValueTuple
{
    public static ValueTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(
        T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
    {
        return new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
    }

    public static ValueTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1,
        T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
    {
        return new ValueTuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
    }

    public static ValueTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2,
        T3 item3, T4 item4, T5 item5)
    {
        return new ValueTuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
    }

    public static ValueTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2,
        T3 item3, T4 item4)
    {
        return new ValueTuple<T1, T2, T3, T4>(item1, item2, item3, item4);
    }

    public static ValueTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
    {
        return new ValueTuple<T1, T2, T3>(item1, item2, item3);
    }

    public static ValueTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
    {
        return new ValueTuple<T1, T2>(item1, item2);
    }

    public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> Create<T1, T2, T3, T4, T5, T6,
        T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
    {
        return new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(item1, item2, item3, item4, item5, item6, item7,
            item8);
    }

    public static ValueTuple<T1> Create<T1>(T1 item1)
    {
        return new ValueTuple<T1>(item1);
    }
}

/// <summary>
///     1-튜플 또는 싱글톤을 값 형식으로 나타냅니다.
/// </summary>
public struct ValueTuple<T1> : IEquatable<ValueTuple<T1>>, IStructuralEquatable,
    IStructuralComparable, IComparable, ITuple
{
    public T1 Item1;

    public ValueTuple(T1 item1)
    {
        Item1 = item1;
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueTuple<T1> tuple && Equals(tuple);
    }

    public bool Equals(ValueTuple<T1> other)
    {
        return EqualityComparer<T1>.Default.Equals(Item1, other.Item1);
    }

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not ValueTuple<T1> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not ValueTuple<T1> objTuple)
        {
            throw new ArgumentException("other");
        }

        return comparer.Compare(Item1, objTuple.Item1);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is ValueTuple<T1> tuple
            ? ((IStructuralComparable)this).CompareTo(tuple, Comparer<object>.Default)
            : 1;
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        return comparer.GetHashCode(Item1!);
    }

    public override string ToString()
    {
        return $"({Item1})";
    }

    int ITuple.Length => 1;

    object? ITuple.this[int index]
    {
        get
        {
            if (index != 0)
            {
                throw new IndexOutOfRangeException();
            }

            return Item1;
        }
    }
}

/// <summary>
///     2-튜플 또는 쌍을 값 형식으로 나타냅니다.
/// </summary>
public struct ValueTuple<T1, T2> : IEquatable<ValueTuple<T1, T2>>, IStructuralEquatable,
    IStructuralComparable, IComparable, ITuple
{
    public T1 Item1;
    public T2 Item2;

    public ValueTuple(T1 item1, T2 item2)
    {
        Item1 = item1;
        Item2 = item2;
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueTuple<T1, T2> tuple && Equals(tuple);
    }

    public bool Equals(ValueTuple<T1, T2> other)
    {
        return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
               EqualityComparer<T2>.Default.Equals(Item2, other.Item2);
    }

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not ValueTuple<T1, T2> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1) &&
               comparer.Equals(Item2, objTuple.Item2);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not ValueTuple<T1, T2> objTuple)
        {
            throw new ArgumentException("other");
        }

        int c = comparer.Compare(Item1, objTuple.Item1);
        if (c != 0)
        {
            return c;
        }

        return comparer.Compare(Item2, objTuple.Item2);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is ValueTuple<T1, T2> tuple
            ? ((IStructuralComparable)this).CompareTo(tuple, Comparer<object>.Default)
            : 1;
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = (hash * 31) + comparer.GetHashCode(Item1!);
        hash = (hash * 31) + comparer.GetHashCode(Item2!);
        return hash;
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2})";
    }

    int ITuple.Length => 2;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => Item1,
            1 => Item2,
            _ => throw new IndexOutOfRangeException()
        };
}

/// <summary>
///     3-튜플을 값 형식으로 나타냅니다.
/// </summary>
public struct ValueTuple<T1, T2, T3> : IEquatable<ValueTuple<T1, T2, T3>>,
    IStructuralEquatable, IStructuralComparable, IComparable, ITuple
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;

    public ValueTuple(T1 item1, T2 item2, T3 item3)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueTuple<T1, T2, T3> tuple && Equals(tuple);
    }

    public bool Equals(ValueTuple<T1, T2, T3> other)
    {
        return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
               EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
               EqualityComparer<T3>.Default.Equals(Item3, other.Item3);
    }

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not ValueTuple<T1, T2, T3> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1) &&
               comparer.Equals(Item2, objTuple.Item2) &&
               comparer.Equals(Item3, objTuple.Item3);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not ValueTuple<T1, T2, T3> objTuple)
        {
            throw new ArgumentException("other");
        }

        int c = comparer.Compare(Item1, objTuple.Item1);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item2, objTuple.Item2);
        if (c != 0)
        {
            return c;
        }

        return comparer.Compare(Item3, objTuple.Item3);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is ValueTuple<T1, T2, T3> tuple
            ? ((IStructuralComparable)this).CompareTo(tuple, Comparer<object>.Default)
            : 1;
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = (hash * 31) + comparer.GetHashCode(Item1!);
        hash = (hash * 31) + comparer.GetHashCode(Item2!);
        hash = (hash * 31) + comparer.GetHashCode(Item3!);
        return hash;
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2}, {Item3})";
    }

    int ITuple.Length => 3;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => Item1,
            1 => Item2,
            2 => Item3,
            _ => throw new IndexOutOfRangeException()
        };
}

/// <summary>
///     4-튜플을 값 형식으로 나타냅니다.
/// </summary>
public struct ValueTuple<T1, T2, T3, T4> : IEquatable<ValueTuple<T1, T2, T3, T4>>,
    IStructuralEquatable, IStructuralComparable, IComparable, ITuple
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
    public T4 Item4;

    public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        Item4 = item4;
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueTuple<T1, T2, T3, T4> tuple && Equals(tuple);
    }

    public bool Equals(ValueTuple<T1, T2, T3, T4> other)
    {
        return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
               EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
               EqualityComparer<T3>.Default.Equals(Item3, other.Item3) &&
               EqualityComparer<T4>.Default.Equals(Item4, other.Item4);
    }

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not ValueTuple<T1, T2, T3, T4> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1) &&
               comparer.Equals(Item2, objTuple.Item2) &&
               comparer.Equals(Item3, objTuple.Item3) &&
               comparer.Equals(Item4, objTuple.Item4);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not ValueTuple<T1, T2, T3, T4> objTuple)
        {
            throw new ArgumentException("other");
        }

        int c = comparer.Compare(Item1, objTuple.Item1);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item2, objTuple.Item2);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item3, objTuple.Item3);
        if (c != 0)
        {
            return c;
        }

        return comparer.Compare(Item4, objTuple.Item4);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is ValueTuple<T1, T2, T3, T4> tuple
            ? ((IStructuralComparable)this).CompareTo(tuple, Comparer<object>.Default)
            : 1;
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = (hash * 31) + comparer.GetHashCode(Item1!);
        hash = (hash * 31) + comparer.GetHashCode(Item2!);
        hash = (hash * 31) + comparer.GetHashCode(Item3!);
        hash = (hash * 31) + comparer.GetHashCode(Item4!);
        return hash;
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2}, {Item3}, {Item4})";
    }

    int ITuple.Length => 4;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => Item1,
            1 => Item2,
            2 => Item3,
            3 => Item4,
            _ => throw new IndexOutOfRangeException()
        };
}

/// <summary>
///     5-튜플을 값 형식으로 나타냅니다.
/// </summary>
public struct ValueTuple<T1, T2, T3, T4, T5> : IEquatable<ValueTuple<T1, T2, T3, T4, T5>>,
    IStructuralEquatable, IStructuralComparable, IComparable, ITuple
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
    public T4 Item4;
    public T5 Item5;

    public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        Item4 = item4;
        Item5 = item5;
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueTuple<T1, T2, T3, T4, T5> tuple && Equals(tuple);
    }

    public bool Equals(ValueTuple<T1, T2, T3, T4, T5> other)
    {
        return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
               EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
               EqualityComparer<T3>.Default.Equals(Item3, other.Item3) &&
               EqualityComparer<T4>.Default.Equals(Item4, other.Item4) &&
               EqualityComparer<T5>.Default.Equals(Item5, other.Item5);
    }

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not ValueTuple<T1, T2, T3, T4, T5> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1) &&
               comparer.Equals(Item2, objTuple.Item2) &&
               comparer.Equals(Item3, objTuple.Item3) &&
               comparer.Equals(Item4, objTuple.Item4) &&
               comparer.Equals(Item5, objTuple.Item5);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not ValueTuple<T1, T2, T3, T4, T5> objTuple)
        {
            throw new ArgumentException("other");
        }

        int c = comparer.Compare(Item1, objTuple.Item1);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item2, objTuple.Item2);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item3, objTuple.Item3);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item4, objTuple.Item4);
        if (c != 0)
        {
            return c;
        }

        return comparer.Compare(Item5, objTuple.Item5);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is ValueTuple<T1, T2, T3, T4, T5> tuple
            ? ((IStructuralComparable)this).CompareTo(tuple, Comparer<object>.Default)
            : 1;
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = (hash * 31) + comparer.GetHashCode(Item1!);
        hash = (hash * 31) + comparer.GetHashCode(Item2!);
        hash = (hash * 31) + comparer.GetHashCode(Item3!);
        hash = (hash * 31) + comparer.GetHashCode(Item4!);
        hash = (hash * 31) + comparer.GetHashCode(Item5!);
        return hash;
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5})";
    }

    int ITuple.Length => 5;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => Item1,
            1 => Item2,
            2 => Item3,
            3 => Item4,
            4 => Item5,
            _ => throw new IndexOutOfRangeException()
        };
}

/// <summary>
///     6-튜플을 값 형식으로 나타냅니다.
/// </summary>
public struct ValueTuple<T1, T2, T3, T4, T5, T6> : IEquatable<ValueTuple<T1, T2, T3, T4, T5,
    T6>>, IStructuralEquatable, IStructuralComparable, IComparable, ITuple
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
    public T4 Item4;
    public T5 Item5;
    public T6 Item6;

    public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        Item4 = item4;
        Item5 = item5;
        Item6 = item6;
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueTuple<T1, T2, T3, T4, T5, T6> tuple && Equals(tuple);
    }

    public bool Equals(ValueTuple<T1, T2, T3, T4, T5, T6> other)
    {
        return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
               EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
               EqualityComparer<T3>.Default.Equals(Item3, other.Item3) &&
               EqualityComparer<T4>.Default.Equals(Item4, other.Item4) &&
               EqualityComparer<T5>.Default.Equals(Item5, other.Item5) &&
               EqualityComparer<T6>.Default.Equals(Item6, other.Item6);
    }

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not ValueTuple<T1, T2, T3, T4, T5, T6> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1) &&
               comparer.Equals(Item2, objTuple.Item2) &&
               comparer.Equals(Item3, objTuple.Item3) &&
               comparer.Equals(Item4, objTuple.Item4) &&
               comparer.Equals(Item5, objTuple.Item5) &&
               comparer.Equals(Item6, objTuple.Item6);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not ValueTuple<T1, T2, T3, T4, T5, T6> objTuple)
        {
            throw new ArgumentException("other");
        }

        int c = comparer.Compare(Item1, objTuple.Item1);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item2, objTuple.Item2);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item3, objTuple.Item3);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item4, objTuple.Item4);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item5, objTuple.Item5);
        if (c != 0)
        {
            return c;
        }

        return comparer.Compare(Item6, objTuple.Item6);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is ValueTuple<T1, T2, T3, T4, T5, T6> tuple
            ? ((IStructuralComparable)this).CompareTo(tuple, Comparer<object>.Default)
            : 1;
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = (hash * 31) + comparer.GetHashCode(Item1!);
        hash = (hash * 31) + comparer.GetHashCode(Item2!);
        hash = (hash * 31) + comparer.GetHashCode(Item3!);
        hash = (hash * 31) + comparer.GetHashCode(Item4!);
        hash = (hash * 31) + comparer.GetHashCode(Item5!);
        hash = (hash * 31) + comparer.GetHashCode(Item6!);
        return hash;
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6})";
    }

    int ITuple.Length => 6;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => Item1,
            1 => Item2,
            2 => Item3,
            3 => Item4,
            4 => Item5,
            5 => Item6,
            _ => throw new IndexOutOfRangeException()
        };
}

/// <summary>
///     7-튜플을 값 형식으로 나타냅니다.
/// </summary>
public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7> : IEquatable<ValueTuple<T1, T2, T3, T4,
    T5, T6, T7>>, IStructuralEquatable, IStructuralComparable, IComparable, ITuple
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
    public T4 Item4;
    public T5 Item5;
    public T6 Item6;
    public T7 Item7;

    public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        Item4 = item4;
        Item5 = item5;
        Item6 = item6;
        Item7 = item7;
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueTuple<T1, T2, T3, T4, T5, T6, T7> tuple && Equals(tuple);
    }

    public bool Equals(ValueTuple<T1, T2, T3, T4, T5, T6, T7> other)
    {
        return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
               EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
               EqualityComparer<T3>.Default.Equals(Item3, other.Item3) &&
               EqualityComparer<T4>.Default.Equals(Item4, other.Item4) &&
               EqualityComparer<T5>.Default.Equals(Item5, other.Item5) &&
               EqualityComparer<T6>.Default.Equals(Item6, other.Item6) &&
               EqualityComparer<T7>.Default.Equals(Item7, other.Item7);
    }

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not ValueTuple<T1, T2, T3, T4, T5, T6, T7> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1) &&
               comparer.Equals(Item2, objTuple.Item2) &&
               comparer.Equals(Item3, objTuple.Item3) &&
               comparer.Equals(Item4, objTuple.Item4) &&
               comparer.Equals(Item5, objTuple.Item5) &&
               comparer.Equals(Item6, objTuple.Item6) &&
               comparer.Equals(Item7, objTuple.Item7);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not ValueTuple<T1, T2, T3, T4, T5, T6, T7> objTuple)
        {
            throw new ArgumentException("other");
        }

        int c = comparer.Compare(Item1, objTuple.Item1);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item2, objTuple.Item2);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item3, objTuple.Item3);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item4, objTuple.Item4);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item5, objTuple.Item5);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item6, objTuple.Item6);
        if (c != 0)
        {
            return c;
        }

        return comparer.Compare(Item7, objTuple.Item7);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is ValueTuple<T1, T2, T3, T4, T5, T6, T7> tuple
            ? ((IStructuralComparable)this).CompareTo(tuple, Comparer<object>.Default)
            : 1;
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = (hash * 31) + comparer.GetHashCode(Item1!);
        hash = (hash * 31) + comparer.GetHashCode(Item2!);
        hash = (hash * 31) + comparer.GetHashCode(Item3!);
        hash = (hash * 31) + comparer.GetHashCode(Item4!);
        hash = (hash * 31) + comparer.GetHashCode(Item5!);
        hash = (hash * 31) + comparer.GetHashCode(Item6!);
        hash = (hash * 31) + comparer.GetHashCode(Item7!);
        return hash;
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7})";
    }

    int ITuple.Length => 7;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => Item1,
            1 => Item2,
            2 => Item3,
            3 => Item4,
            4 => Item5,
            5 => Item6,
            6 => Item7,
            _ => throw new IndexOutOfRangeException()
        };
}

/// <summary>
///     8-튜플을 값 형식으로 나타냅니다.
/// </summary>
public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> : IEquatable<ValueTuple<T1, T2, T3,
    T4, T5, T6, T7, T8>>, IStructuralEquatable, IStructuralComparable, IComparable, ITuple
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
    public T4 Item4;
    public T5 Item5;
    public T6 Item6;
    public T7 Item7;
    public T8 Item8;

    public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7,
        T8 item8)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        Item4 = item4;
        Item5 = item5;
        Item6 = item6;
        Item7 = item7;
        Item8 = item8;
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple && Equals(tuple);
    }

    public bool Equals(ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> other)
    {
        return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
               EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
               EqualityComparer<T3>.Default.Equals(Item3, other.Item3) &&
               EqualityComparer<T4>.Default.Equals(Item4, other.Item4) &&
               EqualityComparer<T5>.Default.Equals(Item5, other.Item5) &&
               EqualityComparer<T6>.Default.Equals(Item6, other.Item6) &&
               EqualityComparer<T7>.Default.Equals(Item7, other.Item7) &&
               EqualityComparer<T8>.Default.Equals(Item8, other.Item8);
    }

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1) &&
               comparer.Equals(Item2, objTuple.Item2) &&
               comparer.Equals(Item3, objTuple.Item3) &&
               comparer.Equals(Item4, objTuple.Item4) &&
               comparer.Equals(Item5, objTuple.Item5) &&
               comparer.Equals(Item6, objTuple.Item6) &&
               comparer.Equals(Item7, objTuple.Item7) &&
               comparer.Equals(Item8, objTuple.Item8);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> objTuple)
        {
            throw new ArgumentException("other");
        }

        int c = comparer.Compare(Item1, objTuple.Item1);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item2, objTuple.Item2);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item3, objTuple.Item3);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item4, objTuple.Item4);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item5, objTuple.Item5);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item6, objTuple.Item6);
        if (c != 0)
        {
            return c;
        }

        c = comparer.Compare(Item7, objTuple.Item7);
        if (c != 0)
        {
            return c;
        }

        return comparer.Compare(Item8, objTuple.Item8);
    }

    int IComparable.CompareTo(object? obj)
    {
        return obj is ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple
            ? ((IStructuralComparable)this).CompareTo(tuple, Comparer<object>.Default)
            : 1;
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = (hash * 31) + comparer.GetHashCode(Item1!);
        hash = (hash * 31) + comparer.GetHashCode(Item2!);
        hash = (hash * 31) + comparer.GetHashCode(Item3!);
        hash = (hash * 31) + comparer.GetHashCode(Item4!);
        hash = (hash * 31) + comparer.GetHashCode(Item5!);
        hash = (hash * 31) + comparer.GetHashCode(Item6!);
        hash = (hash * 31) + comparer.GetHashCode(Item7!);
        hash = (hash * 31) + comparer.GetHashCode(Item8!);
        return hash;
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7}, {Item8})";
    }

    int ITuple.Length => 8;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => Item1,
            1 => Item2,
            2 => Item3,
            3 => Item4,
            4 => Item5,
            5 => Item6,
            6 => Item7,
            7 => Item8,
            _ => throw new IndexOutOfRangeException()
        };
}

#endif
