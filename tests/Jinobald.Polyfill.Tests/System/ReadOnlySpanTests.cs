using Xunit;

namespace Jinobald.Polyfill.Tests.System;

public class ReadOnlySpanTests
{
    [Fact]
    public void ReadOnlySpan_FromArray_CreatesSpan()
    {
        int[] array = new[] { 1, 2, 3, 4, 5 };
        var span = new ReadOnlySpan<int>(array);
        Assert.Equal(5, span.Length);
        Assert.False(span.IsEmpty);
    }

    [Fact]
    public void ReadOnlySpan_FromArraySegment_CreatesSpan()
    {
        int[] array = new[] { 1, 2, 3, 4, 5 };
        var span = new ReadOnlySpan<int>(array, 1, 3);
        Assert.Equal(3, span.Length);
        Assert.Equal(2, span[0]);
        Assert.Equal(3, span[1]);
        Assert.Equal(4, span[2]);
    }

    [Fact]
    public void ReadOnlySpan_Indexer_GetsValues()
    {
        int[] array = new[] { 1, 2, 3 };
        var span = new ReadOnlySpan<int>(array);
        Assert.Equal(1, span[0]);
        Assert.Equal(2, span[1]);
        Assert.Equal(3, span[2]);
    }

    [Fact]
    public void ReadOnlySpan_Indexer_OutOfRange_Throws()
    {
        int[] array = new[] { 1, 2, 3 };
        var span = new ReadOnlySpan<int>(array);

        bool caught1 = false;
        try
        {
            int x = span[3];
        }
        catch (IndexOutOfRangeException) { caught1 = true; }

        Assert.True(caught1);

        bool caught2 = false;
        try
        {
            int x = span[-1];
        }
        catch (IndexOutOfRangeException) { caught2 = true; }

        Assert.True(caught2);
    }

    [Fact]
    public void ReadOnlySpan_Slice_WithStart_CreatesSlice()
    {
        int[] array = new[] { 1, 2, 3, 4, 5 };
        var span = new ReadOnlySpan<int>(array);
        ReadOnlySpan<int> slice = span.Slice(2);
        Assert.Equal(3, slice.Length);
        Assert.Equal(3, slice[0]);
        Assert.Equal(4, slice[1]);
        Assert.Equal(5, slice[2]);
    }

    [Fact]
    public void ReadOnlySpan_Slice_WithStartAndLength_CreatesSlice()
    {
        int[] array = new[] { 1, 2, 3, 4, 5 };
        var span = new ReadOnlySpan<int>(array);
        ReadOnlySpan<int> slice = span.Slice(1, 3);
        Assert.Equal(3, slice.Length);
        Assert.Equal(2, slice[0]);
        Assert.Equal(3, slice[1]);
        Assert.Equal(4, slice[2]);
    }

    [Fact]
    public void ReadOnlySpan_Slice_OutOfRange_Throws()
    {
        int[] array = new[] { 1, 2, 3 };
        var span = new ReadOnlySpan<int>(array);

        bool caught1 = false;
        try
        {
            ReadOnlySpan<int> x = span.Slice(5);
        }
        catch (ArgumentOutOfRangeException) { caught1 = true; }

        Assert.True(caught1);

        bool caught2 = false;
        try
        {
            ReadOnlySpan<int> x = span.Slice(1, 10);
        }
        catch (ArgumentOutOfRangeException) { caught2 = true; }

        Assert.True(caught2);
    }

    [Fact]
    public void ReadOnlySpan_CopyTo_CopiesElements()
    {
        int[] source = new[] { 1, 2, 3 };
        int[] destination = new int[3];
        var sourceSpan = new ReadOnlySpan<int>(source);
        var destSpan = new Span<int>(destination);

        sourceSpan.CopyTo(destSpan);

        Assert.Equal(source, destination);
    }

    [Fact]
    public void ReadOnlySpan_CopyTo_DestinationTooShort_Throws()
    {
        int[] source = new[] { 1, 2, 3, 4, 5 };
        int[] destination = new int[3];
        var sourceSpan = new ReadOnlySpan<int>(source);
        var destSpan = new Span<int>(destination);

        bool caught = false;
        try { sourceSpan.CopyTo(destSpan); }
        catch (ArgumentException) { caught = true; }

        Assert.True(caught);
    }

    [Fact]
    public void ReadOnlySpan_TryCopyTo_Success_ReturnsTrue()
    {
        int[] source = new[] { 1, 2, 3 };
        int[] destination = new int[5];
        var sourceSpan = new ReadOnlySpan<int>(source);
        var destSpan = new Span<int>(destination);

        bool result = sourceSpan.TryCopyTo(destSpan);

        Assert.True(result);
        Assert.Equal(1, destination[0]);
        Assert.Equal(2, destination[1]);
        Assert.Equal(3, destination[2]);
    }

    [Fact]
    public void ReadOnlySpan_TryCopyTo_DestinationTooShort_ReturnsFalse()
    {
        int[] source = new[] { 1, 2, 3, 4, 5 };
        int[] destination = new int[3];
        var sourceSpan = new ReadOnlySpan<int>(source);
        var destSpan = new Span<int>(destination);

        bool result = sourceSpan.TryCopyTo(destSpan);

        Assert.False(result);
    }

    [Fact]
    public void ReadOnlySpan_ToArray_CreatesNewArray()
    {
        int[] array = new[] { 1, 2, 3 };
        var span = new ReadOnlySpan<int>(array);
        int[]? newArray = span.ToArray();

        Assert.Equal(array, newArray);
        Assert.NotSame(array, newArray);
    }

    [Fact]
    public void ReadOnlySpan_ToArray_EmptySpan_ReturnsEmptyArray()
    {
        var span = new ReadOnlySpan<int>(new int[0]);
        int[]? array = span.ToArray();

        Assert.Empty(array);
    }

    [Fact]
    public void ReadOnlySpan_ImplicitConversion_FromArray()
    {
        int[] array = new[] { 1, 2, 3 };
        ReadOnlySpan<int> span = array;

        Assert.Equal(3, span.Length);
        Assert.Equal(1, span[0]);
    }

    [Fact]
    public void ReadOnlySpan_ImplicitConversion_FromSpan()
    {
        int[] array = new[] { 1, 2, 3 };
        var mutableSpan = new Span<int>(array);
        ReadOnlySpan<int> readOnlySpan = mutableSpan;

        Assert.Equal(3, readOnlySpan.Length);
        Assert.Equal(1, readOnlySpan[0]);
    }

    [Fact]
    public void ReadOnlySpan_Empty_CreatesEmptySpan()
    {
        ReadOnlySpan<int> span = ReadOnlySpan<int>.Empty;
        Assert.Equal(0, span.Length);
        Assert.True(span.IsEmpty);
    }

    [Fact]
    public void ReadOnlySpan_Enumerator_IteratesElements()
    {
        int[] array = new[] { 1, 2, 3 };
        var span = new ReadOnlySpan<int>(array);
        int sum = 0;

        foreach (ref readonly int item in span)
        {
            sum += item;
        }

        Assert.Equal(6, sum);
    }

    [Fact]
    public void ReadOnlySpan_FromNullArray_CreatesEmptySpan()
    {
        var span = new ReadOnlySpan<int>(null);
        Assert.Equal(0, span.Length);
        Assert.True(span.IsEmpty);
    }

    [Fact]
    public void ReadOnlySpan_FromString_WorksWithCharSpan()
    {
        string text = "Hello";
        var span = new ReadOnlySpan<char>(text.ToCharArray());

        Assert.Equal(5, span.Length);
        Assert.Equal('H', span[0]);
        Assert.Equal('o', span[4]);
    }
}
