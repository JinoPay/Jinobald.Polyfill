using Xunit;

namespace Jinobald.Polyfill.Tests.System;

public class ValueTupleTests
{
    [Fact]
    public void ValueTuple_Create_OneTuple()
    {
        var tuple = ValueTuple.Create(42);
        Assert.Equal(42, tuple.Item1);
    }

    [Fact]
    public void ValueTuple_Create_TwoTuple()
    {
        var tuple = ValueTuple.Create(1, "hello");
        Assert.Equal(1, tuple.Item1);
        Assert.Equal("hello", tuple.Item2);
    }

    [Fact]
    public void ValueTuple_Create_ThreeTuple()
    {
        var tuple = ValueTuple.Create(1, 2, 3);
        Assert.Equal(1, tuple.Item1);
        Assert.Equal(2, tuple.Item2);
        Assert.Equal(3, tuple.Item3);
    }

    [Fact]
    public void ValueTuple_Equality_SameValues_AreEqual()
    {
        var tuple1 = ValueTuple.Create(1, 2);
        var tuple2 = ValueTuple.Create(1, 2);
        Assert.True(tuple1.Equals(tuple2));
        Assert.True(tuple1 == tuple2);
    }

    [Fact]
    public void ValueTuple_Equality_DifferentValues_AreNotEqual()
    {
        var tuple1 = ValueTuple.Create(1, 2);
        var tuple2 = ValueTuple.Create(1, 3);
        Assert.False(tuple1.Equals(tuple2));
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void ValueTuple_GetHashCode_SameValues_SameHashCode()
    {
        var tuple1 = ValueTuple.Create(1, 2, 3);
        var tuple2 = ValueTuple.Create(1, 2, 3);
        Assert.Equal(tuple1.GetHashCode(), tuple2.GetHashCode());
    }

    [Fact]
    public void ValueTuple_ToString_FormatsCorrectly()
    {
        var tuple = ValueTuple.Create(1, "hello", 3.14);
        string result = tuple.ToString();
        Assert.Equal("(1, hello, 3.14)", result);
    }

    [Fact]
    public void ValueTuple_CompareTo_Works()
    {
        var tuple1 = ValueTuple.Create(1, 2);
        var tuple2 = ValueTuple.Create(1, 3);
        var tuple3 = ValueTuple.Create(1, 2);

        Assert.True(tuple1.CompareTo(tuple2) < 0);
        Assert.True(tuple2.CompareTo(tuple1) > 0);
        Assert.Equal(0, tuple1.CompareTo(tuple3));
    }

    [Fact]
    public void ValueTuple_Deconstruction_Works()
    {
        var tuple = ValueTuple.Create(42, "test");
        var (num, str) = tuple;

        Assert.Equal(42, num);
        Assert.Equal("test", str);
    }

    [Fact]
    public void ValueTuple_SevenTuple_Works()
    {
        var tuple = ValueTuple.Create(1, 2, 3, 4, 5, 6, 7);
        Assert.Equal(1, tuple.Item1);
        Assert.Equal(7, tuple.Item7);
    }

    [Fact]
    public void ValueTuple_EightTuple_UsesRest()
    {
        var tuple = ValueTuple.Create(1, 2, 3, 4, 5, 6, 7, 8);
        Assert.Equal(1, tuple.Item1);
        Assert.Equal(7, tuple.Item7);
        Assert.Equal(8, tuple.Rest.Item1);
    }

    [Fact]
    public void ValueTuple_ITuple_Length_IsCorrect()
    {
        var tuple = ValueTuple.Create(1, 2, 3);
        ITuple iTuple = tuple;
        Assert.Equal(3, iTuple.Length);
    }

    [Fact]
    public void ValueTuple_ITuple_Indexer_Works()
    {
        var tuple = ValueTuple.Create("a", "b", "c");
        ITuple iTuple = tuple;

        Assert.Equal("a", iTuple[0]);
        Assert.Equal("b", iTuple[1]);
        Assert.Equal("c", iTuple[2]);
    }

    [Fact]
    public void ValueTuple_WithNullValues_HandlesCorrectly()
    {
        var tuple = ValueTuple.Create((string)null, (object)null);
        Assert.Null(tuple.Item1);
        Assert.Null(tuple.Item2);
    }

    [Fact]
    public void ValueTuple_NestedTuples_Work()
    {
        var inner = ValueTuple.Create(1, 2);
        var outer = ValueTuple.Create(inner, 3);

        Assert.Equal(1, outer.Item1.Item1);
        Assert.Equal(2, outer.Item1.Item2);
        Assert.Equal(3, outer.Item2);
    }
}
