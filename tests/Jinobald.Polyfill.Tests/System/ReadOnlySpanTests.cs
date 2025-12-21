namespace Jinobald.Polyfill.Tests.System;

using global::System;
using Xunit;

public class ReadOnlySpanTests
{
    [Fact]
    public void ReadOnlySpan_FromArray_CreatesSpan()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var span = new ReadOnlySpan<int>(array);
        Assert.Equal(5, span.Length);
        Assert.False(span.IsEmpty);
    }

    [Fact]
    public void ReadOnlySpan_FromArraySegment_CreatesSpan()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var span = new ReadOnlySpan<int>(array, 1, 3);
        Assert.Equal(3, span.Length);
        Assert.Equal(2, span[0]);
        Assert.Equal(3, span[1]);
        Assert.Equal(4, span[2]);
    }

    [Fact]
    public void ReadOnlySpan_Indexer_GetsValues()
    {
        var array = new[] { 1, 2, 3 };
        var span = new ReadOnlySpan<int>(array);
        Assert.Equal(1, span[0]);
        Assert.Equal(2, span[1]);
        Assert.Equal(3, span[2]);
    }

    [Fact]
    public void ReadOnlySpan_Indexer_OutOfRange_Throws()
    {
        var array = new[] { 1, 2, 3 };
        var span = new ReadOnlySpan<int>(array);

        var caught1 = false;
        try { var x = span[3]; }
        catch (IndexOutOfRangeException) { caught1 = true; }
        Assert.True(caught1);

        var caught2 = false;
        try { var x = span[-1]; }
        catch (IndexOutOfRangeException) { caught2 = true; }
        Assert.True(caught2);
    }

    [Fact]
    public void ReadOnlySpan_Slice_WithStart_CreatesSlice()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var span = new ReadOnlySpan<int>(array);
        var slice = span.Slice(2);
        Assert.Equal(3, slice.Length);
        Assert.Equal(3, slice[0]);
        Assert.Equal(4, slice[1]);
        Assert.Equal(5, slice[2]);
    }

    [Fact]
    public void ReadOnlySpan_Slice_WithStartAndLength_CreatesSlice()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var span = new ReadOnlySpan<int>(array);
        var slice = span.Slice(1, 3);
        Assert.Equal(3, slice.Length);
        Assert.Equal(2, slice[0]);
        Assert.Equal(3, slice[1]);
        Assert.Equal(4, slice[2]);
    }

    [Fact]
    public void ReadOnlySpan_Slice_OutOfRange_Throws()
    {
        var array = new[] { 1, 2, 3 };
        var span = new ReadOnlySpan<int>(array);

        var caught1 = false;
        try { var x = span.Slice(5); }
        catch (ArgumentOutOfRangeException) { caught1 = true; }
        Assert.True(caught1);

        var caught2 = false;
        try { var x = span.Slice(1, 10); }
        catch (ArgumentOutOfRangeException) { caught2 = true; }
        Assert.True(caught2);
    }

    [Fact]
    public void ReadOnlySpan_CopyTo_CopiesElements()
    {
        var source = new[] { 1, 2, 3 };
        var destination = new int[3];
        var sourceSpan = new ReadOnlySpan<int>(source);
        var destSpan = new Span<int>(destination);

        sourceSpan.CopyTo(destSpan);

        Assert.Equal(source, destination);
    }

    [Fact]
    public void ReadOnlySpan_CopyTo_DestinationTooShort_Throws()
    {
        var source = new[] { 1, 2, 3, 4, 5 };
        var destination = new int[3];
        var sourceSpan = new ReadOnlySpan<int>(source);
        var destSpan = new Span<int>(destination);

        var caught = false;
        try { sourceSpan.CopyTo(destSpan); }
        catch (ArgumentException) { caught = true; }
        Assert.True(caught);
    }

    [Fact]
    public void ReadOnlySpan_TryCopyTo_Success_ReturnsTrue()
    {
        var source = new[] { 1, 2, 3 };
        var destination = new int[5];
        var sourceSpan = new ReadOnlySpan<int>(source);
        var destSpan = new Span<int>(destination);

        var result = sourceSpan.TryCopyTo(destSpan);

        Assert.True(result);
        Assert.Equal(1, destination[0]);
        Assert.Equal(2, destination[1]);
        Assert.Equal(3, destination[2]);
    }

    [Fact]
    public void ReadOnlySpan_TryCopyTo_DestinationTooShort_ReturnsFalse()
    {
        var source = new[] { 1, 2, 3, 4, 5 };
        var destination = new int[3];
        var sourceSpan = new ReadOnlySpan<int>(source);
        var destSpan = new Span<int>(destination);

        var result = sourceSpan.TryCopyTo(destSpan);

        Assert.False(result);
    }

    [Fact]
    public void ReadOnlySpan_ToArray_CreatesNewArray()
    {
        var array = new[] { 1, 2, 3 };
        var span = new ReadOnlySpan<int>(array);
        var newArray = span.ToArray();

        Assert.Equal(array, newArray);
        Assert.NotSame(array, newArray);
    }

    [Fact]
    public void ReadOnlySpan_ToArray_EmptySpan_ReturnsEmptyArray()
    {
        var span = new ReadOnlySpan<int>(new int[0]);
        var array = span.ToArray();

        Assert.Empty(array);
    }

    [Fact]
    public void ReadOnlySpan_ImplicitConversion_FromArray()
    {
        var array = new[] { 1, 2, 3 };
        ReadOnlySpan<int> span = array;

        Assert.Equal(3, span.Length);
        Assert.Equal(1, span[0]);
    }

    [Fact]
    public void ReadOnlySpan_ImplicitConversion_FromSpan()
    {
        var array = new[] { 1, 2, 3 };
        var mutableSpan = new Span<int>(array);
        ReadOnlySpan<int> readOnlySpan = mutableSpan;

        Assert.Equal(3, readOnlySpan.Length);
        Assert.Equal(1, readOnlySpan[0]);
    }

    [Fact]
    public void ReadOnlySpan_Empty_CreatesEmptySpan()
    {
        var span = ReadOnlySpan<int>.Empty;
        Assert.Equal(0, span.Length);
        Assert.True(span.IsEmpty);
    }

    [Fact]
    public void ReadOnlySpan_Enumerator_IteratesElements()
    {
        var array = new[] { 1, 2, 3 };
        var span = new ReadOnlySpan<int>(array);
        var sum = 0;

        foreach (ref readonly var item in span)
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
        var text = "Hello";
        var span = new ReadOnlySpan<char>(text.ToCharArray());

        Assert.Equal(5, span.Length);
        Assert.Equal('H', span[0]);
        Assert.Equal('o', span[4]);
    }
}
