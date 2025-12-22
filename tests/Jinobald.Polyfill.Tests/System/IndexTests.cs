using System;
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System;

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48

public class IndexTests
{
    [Test]
    public void Index_FromStart_CreatesCorrectIndex()
    {
        var index = Index.FromStart(5);
        Assert.AreEqual(5, index.Value);
        Assert.IsFalse(index.IsFromEnd);
    }

    [Test]
    public void Index_FromEnd_CreatesCorrectIndex()
    {
        var index = Index.FromEnd(3);
        Assert.AreEqual(3, index.Value);
        Assert.IsTrue(index.IsFromEnd);
    }

    [Test]
    public void Index_Constructor_FromStart()
    {
        var index = new Index(7, fromEnd: false);
        Assert.AreEqual(7, index.Value);
        Assert.IsFalse(index.IsFromEnd);
    }

    [Test]
    public void Index_Constructor_FromEnd()
    {
        var index = new Index(4, fromEnd: true);
        Assert.AreEqual(4, index.Value);
        Assert.IsTrue(index.IsFromEnd);
    }

    [Test]
    public void Index_ImplicitConversion_FromInt()
    {
        Index index = 10;
        Assert.AreEqual(10, index.Value);
        Assert.IsFalse(index.IsFromEnd);
    }

    [Test]
    public void Index_GetOffset_FromStart()
    {
        var index = Index.FromStart(3);
        Assert.AreEqual(3, index.GetOffset(10));
    }

    [Test]
    public void Index_GetOffset_FromEnd()
    {
        var index = Index.FromEnd(2);
        Assert.AreEqual(8, index.GetOffset(10));
    }

    [Test]
    public void Index_GetOffset_End()
    {
        var index = Index.FromEnd(0);
        Assert.AreEqual(10, index.GetOffset(10));
    }

    [Test]
    public void Index_Start_Property()
    {
        var start = Index.Start;
        Assert.AreEqual(0, start.Value);
        Assert.IsFalse(start.IsFromEnd);
    }

    [Test]
    public void Index_End_Property()
    {
        var end = Index.End;
        Assert.AreEqual(0, end.Value);
        Assert.IsTrue(end.IsFromEnd);
    }

    [Test]
    public void Index_ToString_FromStart()
    {
        var index = Index.FromStart(5);
        Assert.AreEqual("5", index.ToString());
    }

    [Test]
    public void Index_ToString_FromEnd()
    {
        var index = Index.FromEnd(3);
        Assert.AreEqual("^3", index.ToString());
    }

    [Test]
    public void Index_Equals_SameValue()
    {
        var index1 = Index.FromStart(5);
        var index2 = Index.FromStart(5);
        Assert.IsTrue(index1.Equals(index2));
        Assert.IsTrue(index1 == index2);
        Assert.IsFalse(index1 != index2);
    }

    [Test]
    public void Index_Equals_DifferentValue()
    {
        var index1 = Index.FromStart(5);
        var index2 = Index.FromStart(3);
        Assert.IsFalse(index1.Equals(index2));
        Assert.IsFalse(index1 == index2);
        Assert.IsTrue(index1 != index2);
    }

    [Test]
    public void Index_Equals_DifferentDirection()
    {
        var index1 = Index.FromStart(5);
        var index2 = Index.FromEnd(5);
        Assert.IsFalse(index1.Equals(index2));
    }

    [Test]
    public void Index_GetHashCode_SameForEqual()
    {
        var index1 = Index.FromStart(5);
        var index2 = Index.FromStart(5);
        Assert.AreEqual(index1.GetHashCode(), index2.GetHashCode());
    }

    [Test]
    public void Index_FromStart_NegativeValue_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Index.FromStart(-1));
    }

    [Test]
    public void Index_FromEnd_NegativeValue_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Index.FromEnd(-1));
    }

    [Test]
    public void Index_Constructor_NegativeValue_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Index(-1, false));
    }

    [Test]
    public void Index_Equals_WithObject()
    {
        var index1 = Index.FromStart(5);
        object index2 = Index.FromStart(5);
        Assert.IsTrue(index1.Equals(index2));
    }

    [Test]
    public void Index_Equals_WithNonIndexObject()
    {
        var index = Index.FromStart(5);
        object other = 5;
        Assert.IsFalse(index.Equals(other));
    }

    [TestCase(0, 10, 0)]
    [TestCase(5, 10, 5)]
    [TestCase(9, 10, 9)]
    public void Index_GetOffset_VariousFromStartCases(int value, int length, int expectedOffset)
    {
        var index = Index.FromStart(value);
        Assert.AreEqual(expectedOffset, index.GetOffset(length));
    }

    [TestCase(1, 10, 9)]
    [TestCase(5, 10, 5)]
    [TestCase(10, 10, 0)]
    public void Index_GetOffset_VariousFromEndCases(int value, int length, int expectedOffset)
    {
        var index = Index.FromEnd(value);
        Assert.AreEqual(expectedOffset, index.GetOffset(length));
    }
}

#endif
