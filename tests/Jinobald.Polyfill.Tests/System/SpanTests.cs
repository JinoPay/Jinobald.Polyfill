namespace Jinobald.Polyfill.Tests.System;

using global::System;
using Xunit;

public class SpanTests
{
    [Fact]
    public void Span_FromArray_CreatesSpan()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var span = new Span<int>(array);
        Assert.Equal(5, span.Length);
        Assert.False(span.IsEmpty);
    }

    [Fact]
    public void Span_FromArraySegment_CreatesSpan()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var span = new Span<int>(array, 1, 3);
        Assert.Equal(3, span.Length);
        Assert.Equal(2, span[0]);
        Assert.Equal(3, span[1]);
        Assert.Equal(4, span[2]);
    }

    [Fact]
    public void Span_Indexer_GetsAndSetsValues()
    {
        var array = new[] { 1, 2, 3 };
        var span = new Span<int>(array);
        Assert.Equal(1, span[0]);
        Assert.Equal(2, span[1]);
        Assert.Equal(3, span[2]);

        span[1] = 10;
        Assert.Equal(10, span[1]);
        Assert.Equal(10, array[1]);
    }

    [Fact]
    public void Span_Indexer_OutOfRange_Throws()
    {
        var array = new[] { 1, 2, 3 };
        var span = new Span<int>(array);

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
    public void Span_Fill_FillsAllElements()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var span = new Span<int>(array);
        span.Fill(99);
        Assert.All(array, x => Assert.Equal(99, x));
    }

    [Fact]
    public void Span_Slice_WithStart_CreatesSlice()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var span = new Span<int>(array);
        var slice = span.Slice(2);
        Assert.Equal(3, slice.Length);
        Assert.Equal(3, slice[0]);
        Assert.Equal(4, slice[1]);
        Assert.Equal(5, slice[2]);
    }

    [Fact]
    public void Span_Slice_WithStartAndLength_CreatesSlice()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var span = new Span<int>(array);
        var slice = span.Slice(1, 3);
        Assert.Equal(3, slice.Length);
        Assert.Equal(2, slice[0]);
        Assert.Equal(3, slice[1]);
        Assert.Equal(4, slice[2]);
    }

    [Fact]
    public void Span_Slice_OutOfRange_Throws()
    {
        var array = new[] { 1, 2, 3 };
        var span = new Span<int>(array);

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
    public void Span_CopyTo_CopiesElements()
    {
        var source = new[] { 1, 2, 3 };
        var destination = new int[3];
        var sourceSpan = new Span<int>(source);
        var destSpan = new Span<int>(destination);

        sourceSpan.CopyTo(destSpan);

        Assert.Equal(source, destination);
    }

    [Fact]
    public void Span_CopyTo_DestinationTooShort_Throws()
    {
        var source = new[] { 1, 2, 3, 4, 5 };
        var destination = new int[3];
        var sourceSpan = new Span<int>(source);
        var destSpan = new Span<int>(destination);

        var caught = false;
        try { sourceSpan.CopyTo(destSpan); }
        catch (ArgumentException) { caught = true; }
        Assert.True(caught);
    }

    [Fact]
    public void Span_TryCopyTo_Success_ReturnsTrue()
    {
        var source = new[] { 1, 2, 3 };
        var destination = new int[5];
        var sourceSpan = new Span<int>(source);
        var destSpan = new Span<int>(destination);

        var result = sourceSpan.TryCopyTo(destSpan);

        Assert.True(result);
        Assert.Equal(1, destination[0]);
        Assert.Equal(2, destination[1]);
        Assert.Equal(3, destination[2]);
    }

    [Fact]
    public void Span_TryCopyTo_DestinationTooShort_ReturnsFalse()
    {
        var source = new[] { 1, 2, 3, 4, 5 };
        var destination = new int[3];
        var sourceSpan = new Span<int>(source);
        var destSpan = new Span<int>(destination);

        var result = sourceSpan.TryCopyTo(destSpan);

        Assert.False(result);
    }

    [Fact]
    public void Span_ToArray_CreatesNewArray()
    {
        var array = new[] { 1, 2, 3 };
        var span = new Span<int>(array);
        var newArray = span.ToArray();

        Assert.Equal(array, newArray);
        Assert.NotSame(array, newArray);
    }

    [Fact]
    public void Span_ToArray_EmptySpan_ReturnsEmptyArray()
    {
        var span = new Span<int>(new int[0]);
        var array = span.ToArray();

        Assert.Empty(array);
    }

    [Fact]
    public void Span_ImplicitConversion_FromArray()
    {
        var array = new[] { 1, 2, 3 };
        Span<int> span = array;

        Assert.Equal(3, span.Length);
        Assert.Equal(1, span[0]);
    }

    [Fact]
    public void Span_Empty_CreatesEmptySpan()
    {
        var span = Span<int>.Empty;
        Assert.Equal(0, span.Length);
        Assert.True(span.IsEmpty);
    }

    [Fact]
    public void Span_Enumerator_IteratesElements()
    {
        var array = new[] { 1, 2, 3 };
        var span = new Span<int>(array);
        var sum = 0;

        foreach (ref var item in span)
        {
            sum += item;
        }

        Assert.Equal(6, sum);
    }

    [Fact]
    public void Span_Enumerator_CanModifyElements()
    {
        var array = new[] { 1, 2, 3 };
        var span = new Span<int>(array);

        foreach (ref var item in span)
        {
            item *= 2;
        }

        Assert.Equal(new[] { 2, 4, 6 }, array);
    }

    [Fact]
    public void Span_FromNullArray_CreatesEmptySpan()
    {
        var span = new Span<int>(null);
        Assert.Equal(0, span.Length);
        Assert.True(span.IsEmpty);
    }

    [Fact]
    public void Span_SliceModification_AffectsOriginal()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var span = new Span<int>(array);
        var slice = span.Slice(1, 3);

        slice[0] = 20;
        slice[1] = 30;

        Assert.Equal(20, array[1]);
        Assert.Equal(30, array[2]);
    }
}
