using System;
using System.Collections.Generic;
using Xunit;

namespace Jinobald.Polyfill.Tests.System;

#if NET35

public class TupleTests
{
    [Fact]
    public void Tuple_Create_SingleItem()
    {
        var tuple = Tuple.Create(42);
        Assert.Equal(42, tuple.Item1);
    }

    [Fact]
    public void Tuple_Create_TwoItems()
    {
        var tuple = Tuple.Create(1, "hello");
        Assert.Equal(1, tuple.Item1);
        Assert.Equal("hello", tuple.Item2);
    }

    [Fact]
    public void Tuple_Create_ThreeItems()
    {
        var tuple = Tuple.Create(1, 2, 3);
        Assert.Equal(1, tuple.Item1);
        Assert.Equal(2, tuple.Item2);
        Assert.Equal(3, tuple.Item3);
    }

    [Fact]
    public void Tuple_Create_EightItems()
    {
        var tuple = Tuple.Create(1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Equal(1, tuple.Item1);
        Assert.Equal(2, tuple.Item2);
        Assert.Equal(3, tuple.Item3);
        Assert.Equal(4, tuple.Item4);
        Assert.Equal(5, tuple.Item5);
        Assert.Equal(6, tuple.Item6);
        Assert.Equal(7, tuple.Item7);
        Assert.Equal(8, tuple.Item8);
    }

    [Fact]
    public void Tuple_Equals()
    {
        var tuple1 = Tuple.Create(1, "hello");
        var tuple2 = Tuple.Create(1, "hello");
        var tuple3 = Tuple.Create(1, "world");

        Assert.Equal(tuple1, tuple2);
        Assert.NotEqual(tuple1, tuple3);
    }

    [Fact]
    public void Tuple_GetHashCode()
    {
        var tuple1 = Tuple.Create(1, "hello");
        var tuple2 = Tuple.Create(1, "hello");

        Assert.Equal(tuple1.GetHashCode(), tuple2.GetHashCode());
    }

    [Fact]
    public void Tuple_ToString()
    {
        var tuple = Tuple.Create(1, "hello");
        Assert.Equal("(1, hello)", tuple.ToString());
    }

    [Fact]
    public void Tuple_ITuple_Length()
    {
        var tuple1 = Tuple.Create(1);
        var tuple2 = Tuple.Create(1, 2);
        var tuple3 = Tuple.Create(1, 2, 3);
        var tuple8 = Tuple.Create(1, 2, 3, 4, 5, 6, 7, 8);

        var iTuple1 = (ITuple)tuple1;
        var iTuple2 = (ITuple)tuple2;
        var iTuple3 = (ITuple)tuple3;
        var iTuple8 = (ITuple)tuple8;

        Assert.Equal(1, iTuple1.Length);
        Assert.Equal(2, iTuple2.Length);
        Assert.Equal(3, iTuple3.Length);
        Assert.Equal(8, iTuple8.Length);
    }

    [Fact]
    public void Tuple_ITuple_Indexer()
    {
        var tuple = Tuple.Create(1, "hello", 3.14);
        var iTuple = (ITuple)tuple;

        Assert.Equal(1, iTuple[0]);
        Assert.Equal("hello", iTuple[1]);
        Assert.Equal(3.14, iTuple[2]);
    }

    [Fact]
    public void Tuple_IStructuralComparable()
    {
        var tuple1 = Tuple.Create(1, 2);
        var tuple2 = Tuple.Create(1, 2);
        var tuple3 = Tuple.Create(1, 3);

        var comparable = (IComparable)tuple1;
        Assert.Equal(0, comparable.CompareTo(tuple2));
        Assert.True(comparable.CompareTo(tuple3) < 0);
    }

    [Fact]
    public void Tuple_Direct_Construction()
    {
        var tuple = new Tuple<int, string>(42, "test");
        Assert.Equal(42, tuple.Item1);
        Assert.Equal("test", tuple.Item2);
    }

    [Fact]
    public void Tuple_ITuple_IndexOutOfRange()
    {
        var tuple = Tuple.Create(1, 2);
        var iTuple = (ITuple)tuple;

        Assert.Throws<IndexOutOfRangeException>(() => iTuple[2]);
    }
}

#endif
