using System;
using Xunit;

namespace Jinobald.Polyfill.Tests.System;

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48

public class IndexTests
{
    [Fact]
    public void Index_FromStart_CreatesCorrectIndex()
    {
        var index = Index.FromStart(5);
        Assert.Equal(5, index.Value);
        Assert.False(index.IsFromEnd);
    }

    [Fact]
    public void Index_FromEnd_CreatesCorrectIndex()
    {
        var index = Index.FromEnd(3);
        Assert.Equal(3, index.Value);
        Assert.True(index.IsFromEnd);
    }

    [Fact]
    public void Index_Constructor_FromStart()
    {
        var index = new Index(7, fromEnd: false);
        Assert.Equal(7, index.Value);
        Assert.False(index.IsFromEnd);
    }

    [Fact]
    public void Index_Constructor_FromEnd()
    {
        var index = new Index(4, fromEnd: true);
        Assert.Equal(4, index.Value);
        Assert.True(index.IsFromEnd);
    }

    [Fact]
    public void Index_ImplicitConversion_FromInt()
    {
        Index index = 10;
        Assert.Equal(10, index.Value);
        Assert.False(index.IsFromEnd);
    }

    [Fact]
    public void Index_GetOffset_FromStart()
    {
        var index = Index.FromStart(3);
        Assert.Equal(3, index.GetOffset(10));
    }

    [Fact]
    public void Index_GetOffset_FromEnd()
    {
        var index = Index.FromEnd(2);
        Assert.Equal(8, index.GetOffset(10));
    }

    [Fact]
    public void Index_GetOffset_End()
    {
        var index = Index.FromEnd(0);
        Assert.Equal(10, index.GetOffset(10));
    }

    [Fact]
    public void Index_Start_Property()
    {
        var start = Index.Start;
        Assert.Equal(0, start.Value);
        Assert.False(start.IsFromEnd);
    }

    [Fact]
    public void Index_End_Property()
    {
        var end = Index.End;
        Assert.Equal(0, end.Value);
        Assert.True(end.IsFromEnd);
    }

    [Fact]
    public void Index_ToString_FromStart()
    {
        var index = Index.FromStart(5);
        Assert.Equal("5", index.ToString());
    }

    [Fact]
    public void Index_ToString_FromEnd()
    {
        var index = Index.FromEnd(3);
        Assert.Equal("^3", index.ToString());
    }

    [Fact]
    public void Index_Equals_SameValue()
    {
        var index1 = Index.FromStart(5);
        var index2 = Index.FromStart(5);
        Assert.True(index1.Equals(index2));
        Assert.True(index1 == index2);
        Assert.False(index1 != index2);
    }

    [Fact]
    public void Index_Equals_DifferentValue()
    {
        var index1 = Index.FromStart(5);
        var index2 = Index.FromStart(3);
        Assert.False(index1.Equals(index2));
        Assert.False(index1 == index2);
        Assert.True(index1 != index2);
    }

    [Fact]
    public void Index_Equals_DifferentDirection()
    {
        var index1 = Index.FromStart(5);
        var index2 = Index.FromEnd(5);
        Assert.False(index1.Equals(index2));
    }

    [Fact]
    public void Index_GetHashCode_SameForEqual()
    {
        var index1 = Index.FromStart(5);
        var index2 = Index.FromStart(5);
        Assert.Equal(index1.GetHashCode(), index2.GetHashCode());
    }

    [Fact]
    public void Index_FromStart_NegativeValue_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Index.FromStart(-1));
    }

    [Fact]
    public void Index_FromEnd_NegativeValue_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Index.FromEnd(-1));
    }

    [Fact]
    public void Index_Constructor_NegativeValue_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Index(-1, false));
    }

    [Fact]
    public void Index_Equals_WithObject()
    {
        var index1 = Index.FromStart(5);
        object index2 = Index.FromStart(5);
        Assert.True(index1.Equals(index2));
    }

    [Fact]
    public void Index_Equals_WithNonIndexObject()
    {
        var index = Index.FromStart(5);
        object other = 5;
        Assert.False(index.Equals(other));
    }

    [Theory]
    [InlineData(0, 10, 0)]
    [InlineData(5, 10, 5)]
    [InlineData(9, 10, 9)]
    public void Index_GetOffset_VariousFromStartCases(int value, int length, int expectedOffset)
    {
        var index = Index.FromStart(value);
        Assert.Equal(expectedOffset, index.GetOffset(length));
    }

    [Theory]
    [InlineData(1, 10, 9)]
    [InlineData(5, 10, 5)]
    [InlineData(10, 10, 0)]
    public void Index_GetOffset_VariousFromEndCases(int value, int length, int expectedOffset)
    {
        var index = Index.FromEnd(value);
        Assert.Equal(expectedOffset, index.GetOffset(length));
    }
}

#endif
