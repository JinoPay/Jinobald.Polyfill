// Jinobald.Polyfill - ValueTuple 테스트
// ValueTuple 구조체에 대한 단위 테스트

#if !NET462
using System.Runtime.CompilerServices;
#endif
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System;

public class ValueTupleTests
{
#if !NET462
    [Test]
    public void ValueTuple_Create_OneTuple()
    {
        var tuple = ValueTuple.Create(42);
        Assert.AreEqual(42, tuple.Item1);
    }
#endif

#if !NET462
    [Test]
    public void ValueTuple_Create_TwoTuple()
    {
        var tuple = ValueTuple.Create(1, "hello");
        Assert.AreEqual(1, tuple.Item1);
        Assert.AreEqual("hello", tuple.Item2);
    }

    [Test]
    public void ValueTuple_Create_ThreeTuple()
    {
        var tuple = ValueTuple.Create(1, 2, 3);
        Assert.AreEqual(1, tuple.Item1);
        Assert.AreEqual(2, tuple.Item2);
        Assert.AreEqual(3, tuple.Item3);
    }

    [Test]
    public void ValueTuple_Equality_SameValues_AreEqual()
    {
        var tuple1 = ValueTuple.Create(1, 2);
        var tuple2 = ValueTuple.Create(1, 2);
        Assert.IsTrue(tuple1.Equals(tuple2));
        Assert.IsTrue(tuple1 == tuple2);
    }

    [Test]
    public void ValueTuple_Equality_DifferentValues_AreNotEqual()
    {
        var tuple1 = ValueTuple.Create(1, 2);
        var tuple2 = ValueTuple.Create(1, 3);
        Assert.IsFalse(tuple1.Equals(tuple2));
        Assert.IsTrue(tuple1 != tuple2);
    }

    [Test]
    public void ValueTuple_GetHashCode_SameValues_SameHashCode()
    {
        var tuple1 = ValueTuple.Create(1, 2, 3);
        var tuple2 = ValueTuple.Create(1, 2, 3);
        Assert.AreEqual(tuple1.GetHashCode(), tuple2.GetHashCode());
    }

    [Test]
    public void ValueTuple_ToString_FormatsCorrectly()
    {
        var tuple = ValueTuple.Create(1, "hello", 3.14);
        string result = tuple.ToString();
        Assert.AreEqual("(1, hello, 3.14)", result);
    }

    [Test]
    public void ValueTuple_CompareTo_Works()
    {
        var tuple1 = ValueTuple.Create(1, 2);
        var tuple2 = ValueTuple.Create(1, 3);
        var tuple3 = ValueTuple.Create(1, 2);

        Assert.IsTrue(tuple1.CompareTo(tuple2) < 0);
        Assert.IsTrue(tuple2.CompareTo(tuple1) > 0);
        Assert.AreEqual(0, tuple1.CompareTo(tuple3));
    }

    [Test]
    public void ValueTuple_Deconstruction_Works()
    {
        var tuple = ValueTuple.Create(42, "test");
        var (num, str) = tuple;

        Assert.AreEqual(42, num);
        Assert.AreEqual("test", str);
    }

    [Test]
    public void ValueTuple_SevenTuple_Works()
    {
        var tuple = ValueTuple.Create(1, 2, 3, 4, 5, 6, 7);
        Assert.AreEqual(1, tuple.Item1);
        Assert.AreEqual(7, tuple.Item7);
    }

    [Test]
    public void ValueTuple_EightTuple_UsesRest()
    {
        var tuple = ValueTuple.Create(1, 2, 3, 4, 5, 6, 7, 8);
        Assert.AreEqual(1, tuple.Item1);
        Assert.AreEqual(7, tuple.Item7);
        Assert.AreEqual(8, tuple.Rest.Item1);
    }
#endif

#if !NET462 && !NET47
    [Test]
    public void ValueTuple_ITuple_Length_IsCorrect()
    {
        var tuple = ValueTuple.Create(1, 2, 3);
        ITuple iTuple = tuple;
        Assert.AreEqual(3, iTuple.Length);
    }

    [Test]
    public void ValueTuple_ITuple_Indexer_Works()
    {
        var tuple = ValueTuple.Create("a", "b", "c");
        ITuple iTuple = tuple;

        Assert.AreEqual("a", iTuple[0]);
        Assert.AreEqual("b", iTuple[1]);
        Assert.AreEqual("c", iTuple[2]);
    }
#endif

#if !NET462
    [Test]
    public void ValueTuple_WithNullValues_HandlesCorrectly()
    {
        var tuple = ValueTuple.Create((string?)null, (object?)null);
        Assert.IsNull(tuple.Item1);
        Assert.IsNull(tuple.Item2);
    }

    [Test]
    public void ValueTuple_NestedTuples_Work()
    {
        var inner = ValueTuple.Create(1, 2);
        var outer = ValueTuple.Create(inner, 3);

        Assert.AreEqual(1, outer.Item1.Item1);
        Assert.AreEqual(2, outer.Item1.Item2);
        Assert.AreEqual(3, outer.Item2);
    }
#endif
}
