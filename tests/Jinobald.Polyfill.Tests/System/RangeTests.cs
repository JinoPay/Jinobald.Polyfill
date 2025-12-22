using System;
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System;

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48

public class RangeTests
{
    [Test]
    public void Range_Constructor_CreatesCorrectRange()
    {
        var start = Index.FromStart(2);
        var end = Index.FromStart(7);
        var range = new Range(start, end);

        Assert.AreEqual(start, range.Start);
        Assert.AreEqual(end, range.End);
    }

    [Test]
    public void Range_StartAt_CreatesRangeFromStart()
    {
        var range = Range.StartAt(Index.FromStart(3));

        Assert.AreEqual(3, range.Start.Value);
        Assert.IsFalse(range.Start.IsFromEnd);
        Assert.IsTrue(range.End.IsFromEnd);
        Assert.AreEqual(0, range.End.Value);
    }

    [Test]
    public void Range_EndAt_CreatesRangeToEnd()
    {
        var range = Range.EndAt(Index.FromStart(5));

        Assert.AreEqual(0, range.Start.Value);
        Assert.IsFalse(range.Start.IsFromEnd);
        Assert.AreEqual(5, range.End.Value);
        Assert.IsFalse(range.End.IsFromEnd);
    }

    [Test]
    public void Range_All_CreatesFullRange()
    {
        var range = Range.All();

        Assert.AreEqual(0, range.Start.Value);
        Assert.IsFalse(range.Start.IsFromEnd);
        Assert.AreEqual(0, range.End.Value);
        Assert.IsTrue(range.End.IsFromEnd);
    }

    [Test]
    public void Range_GetOffsetAndLength_FromStart()
    {
        var range = new Range(Index.FromStart(2), Index.FromStart(7));
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.AreEqual(2, offset);
        Assert.AreEqual(5, length);
    }

    [Test]
    public void Range_GetOffsetAndLength_FromEnd()
    {
        var range = new Range(Index.FromEnd(5), Index.FromEnd(2));
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.AreEqual(5, offset);
        Assert.AreEqual(3, length);
    }

    [Test]
    public void Range_GetOffsetAndLength_Mixed()
    {
        var range = new Range(Index.FromStart(2), Index.FromEnd(2));
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.AreEqual(2, offset);
        Assert.AreEqual(6, length);
    }

    [Test]
    public void Range_GetOffsetAndLength_All()
    {
        var range = Range.All();
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.AreEqual(0, offset);
        Assert.AreEqual(10, length);
    }

    [Test]
    public void Range_GetOffsetAndLength_Empty()
    {
        var range = new Range(Index.FromStart(5), Index.FromStart(5));
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.AreEqual(5, offset);
        Assert.AreEqual(0, length);
    }

    [Test]
    public void Range_GetOffsetAndLength_InvalidRange_ThrowsException()
    {
        var range = new Range(Index.FromStart(7), Index.FromStart(2));
        Assert.Throws<ArgumentOutOfRangeException>(() => range.GetOffsetAndLength(10));
    }

    [Test]
    public void Range_GetOffsetAndLength_OutOfBounds_ThrowsException()
    {
        var range = new Range(Index.FromStart(0), Index.FromStart(15));
        Assert.Throws<ArgumentOutOfRangeException>(() => range.GetOffsetAndLength(10));
    }

    [Test]
    public void Range_ToString_FromStart()
    {
        var range = new Range(Index.FromStart(2), Index.FromStart(7));
        Assert.AreEqual("2..7", range.ToString());
    }

    [Test]
    public void Range_ToString_FromEnd()
    {
        var range = new Range(Index.FromEnd(5), Index.FromEnd(2));
        Assert.AreEqual("^5..^2", range.ToString());
    }

    [Test]
    public void Range_ToString_All()
    {
        var range = Range.All();
        Assert.AreEqual("0..^0", range.ToString());
    }

    [Test]
    public void Range_Equals_SameRange()
    {
        var range1 = new Range(Index.FromStart(2), Index.FromStart(7));
        var range2 = new Range(Index.FromStart(2), Index.FromStart(7));

        Assert.IsTrue(range1.Equals(range2));
        Assert.IsTrue(range1 == range2);
        Assert.IsFalse(range1 != range2);
    }

    [Test]
    public void Range_Equals_DifferentRange()
    {
        var range1 = new Range(Index.FromStart(2), Index.FromStart(7));
        var range2 = new Range(Index.FromStart(3), Index.FromStart(7));

        Assert.IsFalse(range1.Equals(range2));
        Assert.IsFalse(range1 == range2);
        Assert.IsTrue(range1 != range2);
    }

    [Test]
    public void Range_GetHashCode_SameForEqual()
    {
        var range1 = new Range(Index.FromStart(2), Index.FromStart(7));
        var range2 = new Range(Index.FromStart(2), Index.FromStart(7));

        Assert.AreEqual(range1.GetHashCode(), range2.GetHashCode());
    }

    [Test]
    public void Range_Equals_WithObject()
    {
        var range1 = new Range(Index.FromStart(2), Index.FromStart(7));
        object range2 = new Range(Index.FromStart(2), Index.FromStart(7));

        Assert.IsTrue(range1.Equals(range2));
    }

    [Test]
    public void Range_Equals_WithNonRangeObject()
    {
        var range = new Range(Index.FromStart(2), Index.FromStart(7));
        object other = "not a range";

        Assert.IsFalse(range.Equals(other));
    }

    [TestCase(0, 5, 10, 0, 5)]
    [TestCase(2, 8, 10, 2, 6)]
    [TestCase(0, 10, 10, 0, 10)]
    [TestCase(5, 5, 10, 5, 0)]
    public void Range_GetOffsetAndLength_VariousCases(int startValue, int endValue, int length, int expectedOffset, int expectedLength)
    {
        var range = new Range(Index.FromStart(startValue), Index.FromStart(endValue));
        var (offset, actualLength) = range.GetOffsetAndLength(length);

        Assert.AreEqual(expectedOffset, offset);
        Assert.AreEqual(expectedLength, actualLength);
    }

    [Test]
    public void Range_WithImplicitIndexConversion()
    {
        // Using implicit conversion from int to Index
        Index start = 2;
        Index end = 7;
        var range = new Range(start, end);

        var (offset, length) = range.GetOffsetAndLength(10);
        Assert.AreEqual(2, offset);
        Assert.AreEqual(5, length);
    }

    [Test]
    public void Range_StartAt_WithFromEnd()
    {
        var range = Range.StartAt(Index.FromEnd(3));
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.AreEqual(7, offset);
        Assert.AreEqual(3, length);
    }

    [Test]
    public void Range_EndAt_WithFromEnd()
    {
        var range = Range.EndAt(Index.FromEnd(3));
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.AreEqual(0, offset);
        Assert.AreEqual(7, length);
    }
}

#endif
