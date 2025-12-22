using System.Collections;

namespace System;

#if NET35

/// <summary>
///     튜플 개체를 만들기 위한 정적 메서드를 제공합니다.
/// </summary>
public static class Tuple
{
    public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Create<T1, T2, T3, T4, T5, T6, T7,
        T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
    {
        return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>(item1, item2, item3, item4, item5, item6, item7, item8);
    }

    public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(
        T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
    {
        return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
    }

    public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1,
        T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
    {
        return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
    }

    public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2,
        T3 item3, T4 item4, T5 item5)
    {
        return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
    }

    public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3,
        T4 item4)
    {
        return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
    }

    public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
    {
        return new Tuple<T1, T2, T3>(item1, item2, item3);
    }

    public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
    {
        return new Tuple<T1, T2>(item1, item2);
    }

    public static Tuple<T1> Create<T1>(T1 item1)
    {
        return new Tuple<T1>(item1);
    }
}

/// <summary>
///     1-튜플 또는 싱글톤을 값 형식으로 나타냅니다.
/// </summary>
/// <typeparam name="T1">튜플의 유일한 요소의 형식입니다.</typeparam>
public class Tuple<T1> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
{
    public Tuple(T1 item1)
    {
        Item1 = item1;
    }

    public T1 Item1 { get; }

    int IComparable.CompareTo(object? obj)
    {
        return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1> objTuple)
        {
            throw new ArgumentException("other");
        }

        return comparer.Compare(Item1, objTuple.Item1);
    }

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1);
    }

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        return comparer.GetHashCode(Item1!);
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

    public override bool Equals(object? obj)
    {
        return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    public override string ToString()
    {
        return $"({Item1})";
    }
}

/// <summary>
///     2-튜플 또는 쌍을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
{
    public Tuple(T1 item1, T2 item2)
    {
        Item1 = item1;
        Item2 = item2;
    }

    public T1 Item1 { get; }

    public T2 Item2 { get; }

    int IComparable.CompareTo(object? obj)
    {
        return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2> objTuple)
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

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1) &&
               comparer.Equals(Item2, objTuple.Item2);
    }

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = (hash * 31) + comparer.GetHashCode(Item1!);
        hash = (hash * 31) + comparer.GetHashCode(Item2!);
        return hash;
    }

    int ITuple.Length => 2;

    object? ITuple.this[int index] =>
        index switch
        {
            0 => Item1,
            1 => Item2,
            _ => throw new IndexOutOfRangeException()
        };

    public override bool Equals(object? obj)
    {
        return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2})";
    }
}

/// <summary>
///     3-튜플을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2, T3> : IStructuralEquatable, IStructuralComparable, IComparable,
    ITuple
{
    public Tuple(T1 item1, T2 item2, T3 item3)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
    }

    public T1 Item1 { get; }

    public T2 Item2 { get; }

    public T3 Item3 { get; }

    int IComparable.CompareTo(object? obj)
    {
        return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3> objTuple)
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

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1) &&
               comparer.Equals(Item2, objTuple.Item2) &&
               comparer.Equals(Item3, objTuple.Item3);
    }

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
        int hash = 17;
        hash = (hash * 31) + comparer.GetHashCode(Item1!);
        hash = (hash * 31) + comparer.GetHashCode(Item2!);
        hash = (hash * 31) + comparer.GetHashCode(Item3!);
        return hash;
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

    public override bool Equals(object? obj)
    {
        return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2}, {Item3})";
    }
}

/// <summary>
///     4-튜플을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2, T3, T4> : IStructuralEquatable, IStructuralComparable, IComparable,
    ITuple
{
    public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        Item4 = item4;
    }

    public T1 Item1 { get; }

    public T2 Item2 { get; }

    public T3 Item3 { get; }

    public T4 Item4 { get; }

    int IComparable.CompareTo(object? obj)
    {
        return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4> objTuple)
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

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1) &&
               comparer.Equals(Item2, objTuple.Item2) &&
               comparer.Equals(Item3, objTuple.Item3) &&
               comparer.Equals(Item4, objTuple.Item4);
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

    public override bool Equals(object? obj)
    {
        return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2}, {Item3}, {Item4})";
    }
}

/// <summary>
///     5-튜플을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2, T3, T4, T5> : IStructuralEquatable, IStructuralComparable,
    IComparable, ITuple
{
    public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        Item4 = item4;
        Item5 = item5;
    }

    public T1 Item1 { get; }

    public T2 Item2 { get; }

    public T3 Item3 { get; }

    public T4 Item4 { get; }

    public T5 Item5 { get; }

    int IComparable.CompareTo(object? obj)
    {
        return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5> objTuple)
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

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5> objTuple)
        {
            return false;
        }

        return comparer.Equals(Item1, objTuple.Item1) &&
               comparer.Equals(Item2, objTuple.Item2) &&
               comparer.Equals(Item3, objTuple.Item3) &&
               comparer.Equals(Item4, objTuple.Item4) &&
               comparer.Equals(Item5, objTuple.Item5);
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

    public override bool Equals(object? obj)
    {
        return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5})";
    }
}

/// <summary>
///     6-튜플을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2, T3, T4, T5, T6> : IStructuralEquatable, IStructuralComparable,
    IComparable, ITuple
{
    public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        Item4 = item4;
        Item5 = item5;
        Item6 = item6;
    }

    public T1 Item1 { get; }

    public T2 Item2 { get; }

    public T3 Item3 { get; }

    public T4 Item4 { get; }

    public T5 Item5 { get; }

    public T6 Item6 { get; }

    int IComparable.CompareTo(object? obj)
    {
        return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5, T6> objTuple)
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

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5, T6> objTuple)
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

    public override bool Equals(object? obj)
    {
        return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6})";
    }
}

/// <summary>
///     7-튜플을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2, T3, T4, T5, T6, T7> : IStructuralEquatable,
    IStructuralComparable, IComparable, ITuple
{
    public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        Item4 = item4;
        Item5 = item5;
        Item6 = item6;
        Item7 = item7;
    }

    public T1 Item1 { get; }

    public T2 Item2 { get; }

    public T3 Item3 { get; }

    public T4 Item4 { get; }

    public T5 Item5 { get; }

    public T6 Item6 { get; }

    public T7 Item7 { get; }

    int IComparable.CompareTo(object? obj)
    {
        return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5, T6, T7> objTuple)
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

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5, T6, T7> objTuple)
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

    public override bool Equals(object? obj)
    {
        return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7})";
    }
}

/// <summary>
///     8-튜플을 값 형식으로 나타냅니다.
/// </summary>
public class Tuple<T1, T2, T3, T4, T5, T6, T7, T8> : IStructuralEquatable,
    IStructuralComparable, IComparable, ITuple
{
    public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7,
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

    public T1 Item1 { get; }

    public T2 Item2 { get; }

    public T3 Item3 { get; }

    public T4 Item4 { get; }

    public T5 Item5 { get; }

    public T6 Item6 { get; }

    public T7 Item7 { get; }

    public T8 Item8 { get; }

    int IComparable.CompareTo(object? obj)
    {
        return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
    }

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5, T6, T7, T8> objTuple)
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

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Tuple<T1, T2, T3, T4, T5, T6, T7, T8> objTuple)
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

    public override bool Equals(object? obj)
    {
        return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
    }

    public override int GetHashCode()
    {
        return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
    }

    public override string ToString()
    {
        return $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7}, {Item8})";
    }
}

#endif
