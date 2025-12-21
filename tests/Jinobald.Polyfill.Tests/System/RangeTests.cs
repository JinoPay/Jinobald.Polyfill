using System;
using Xunit;

namespace Jinobald.Polyfill.Tests.System;

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48

public class RangeTests
{
    [Fact]
    public void Range_Constructor_CreatesCorrectRange()
    {
        var start = Index.FromStart(2);
        var end = Index.FromStart(7);
        var range = new Range(start, end);

        Assert.Equal(start, range.Start);
        Assert.Equal(end, range.End);
    }

    [Fact]
    public void Range_StartAt_CreatesRangeFromStart()
    {
        var range = Range.StartAt(Index.FromStart(3));

        Assert.Equal(3, range.Start.Value);
        Assert.False(range.Start.IsFromEnd);
        Assert.True(range.End.IsFromEnd);
        Assert.Equal(0, range.End.Value);
    }

    [Fact]
    public void Range_EndAt_CreatesRangeToEnd()
    {
        var range = Range.EndAt(Index.FromStart(5));

        Assert.Equal(0, range.Start.Value);
        Assert.False(range.Start.IsFromEnd);
        Assert.Equal(5, range.End.Value);
        Assert.False(range.End.IsFromEnd);
    }

    [Fact]
    public void Range_All_CreatesFullRange()
    {
        var range = Range.All();

        Assert.Equal(0, range.Start.Value);
        Assert.False(range.Start.IsFromEnd);
        Assert.Equal(0, range.End.Value);
        Assert.True(range.End.IsFromEnd);
    }

    [Fact]
    public void Range_GetOffsetAndLength_FromStart()
    {
        var range = new Range(Index.FromStart(2), Index.FromStart(7));
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.Equal(2, offset);
        Assert.Equal(5, length);
    }

    [Fact]
    public void Range_GetOffsetAndLength_FromEnd()
    {
        var range = new Range(Index.FromEnd(5), Index.FromEnd(2));
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.Equal(5, offset);
        Assert.Equal(3, length);
    }

    [Fact]
    public void Range_GetOffsetAndLength_Mixed()
    {
        var range = new Range(Index.FromStart(2), Index.FromEnd(2));
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.Equal(2, offset);
        Assert.Equal(6, length);
    }

    [Fact]
    public void Range_GetOffsetAndLength_All()
    {
        var range = Range.All();
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.Equal(0, offset);
        Assert.Equal(10, length);
    }

    [Fact]
    public void Range_GetOffsetAndLength_Empty()
    {
        var range = new Range(Index.FromStart(5), Index.FromStart(5));
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.Equal(5, offset);
        Assert.Equal(0, length);
    }

    [Fact]
    public void Range_GetOffsetAndLength_InvalidRange_ThrowsException()
    {
        var range = new Range(Index.FromStart(7), Index.FromStart(2));
        Assert.Throws<ArgumentOutOfRangeException>(() => range.GetOffsetAndLength(10));
    }

    [Fact]
    public void Range_GetOffsetAndLength_OutOfBounds_ThrowsException()
    {
        var range = new Range(Index.FromStart(0), Index.FromStart(15));
        Assert.Throws<ArgumentOutOfRangeException>(() => range.GetOffsetAndLength(10));
    }

    [Fact]
    public void Range_ToString_FromStart()
    {
        var range = new Range(Index.FromStart(2), Index.FromStart(7));
        Assert.Equal("2..7", range.ToString());
    }

    [Fact]
    public void Range_ToString_FromEnd()
    {
        var range = new Range(Index.FromEnd(5), Index.FromEnd(2));
        Assert.Equal("^5..^2", range.ToString());
    }

    [Fact]
    public void Range_ToString_All()
    {
        var range = Range.All();
        Assert.Equal("0..^0", range.ToString());
    }

    [Fact]
    public void Range_Equals_SameRange()
    {
        var range1 = new Range(Index.FromStart(2), Index.FromStart(7));
        var range2 = new Range(Index.FromStart(2), Index.FromStart(7));

        Assert.True(range1.Equals(range2));
        Assert.True(range1 == range2);
        Assert.False(range1 != range2);
    }

    [Fact]
    public void Range_Equals_DifferentRange()
    {
        var range1 = new Range(Index.FromStart(2), Index.FromStart(7));
        var range2 = new Range(Index.FromStart(3), Index.FromStart(7));

        Assert.False(range1.Equals(range2));
        Assert.False(range1 == range2);
        Assert.True(range1 != range2);
    }

    [Fact]
    public void Range_GetHashCode_SameForEqual()
    {
        var range1 = new Range(Index.FromStart(2), Index.FromStart(7));
        var range2 = new Range(Index.FromStart(2), Index.FromStart(7));

        Assert.Equal(range1.GetHashCode(), range2.GetHashCode());
    }

    [Fact]
    public void Range_Equals_WithObject()
    {
        var range1 = new Range(Index.FromStart(2), Index.FromStart(7));
        object range2 = new Range(Index.FromStart(2), Index.FromStart(7));

        Assert.True(range1.Equals(range2));
    }

    [Fact]
    public void Range_Equals_WithNonRangeObject()
    {
        var range = new Range(Index.FromStart(2), Index.FromStart(7));
        object other = "not a range";

        Assert.False(range.Equals(other));
    }

    [Theory]
    [InlineData(0, 5, 10, 0, 5)]
    [InlineData(2, 8, 10, 2, 6)]
    [InlineData(0, 10, 10, 0, 10)]
    [InlineData(5, 5, 10, 5, 0)]
    public void Range_GetOffsetAndLength_VariousCases(int startValue, int endValue, int length, int expectedOffset, int expectedLength)
    {
        var range = new Range(Index.FromStart(startValue), Index.FromStart(endValue));
        var (offset, actualLength) = range.GetOffsetAndLength(length);

        Assert.Equal(expectedOffset, offset);
        Assert.Equal(expectedLength, actualLength);
    }

    [Fact]
    public void Range_WithImplicitIndexConversion()
    {
        // Using implicit conversion from int to Index
        Index start = 2;
        Index end = 7;
        var range = new Range(start, end);

        var (offset, length) = range.GetOffsetAndLength(10);
        Assert.Equal(2, offset);
        Assert.Equal(5, length);
    }

    [Fact]
    public void Range_StartAt_WithFromEnd()
    {
        var range = Range.StartAt(Index.FromEnd(3));
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.Equal(7, offset);
        Assert.Equal(3, length);
    }

    [Fact]
    public void Range_EndAt_WithFromEnd()
    {
        var range = Range.EndAt(Index.FromEnd(3));
        var (offset, length) = range.GetOffsetAndLength(10);

        Assert.Equal(0, offset);
        Assert.Equal(7, length);
    }
}

#endif
