namespace Jinobald.Polyfill.Tests.System;

using global::System;
using Xunit;

public class MemoryTests
{
    [Fact]
    public void Memory_FromArray_CreatesMemory()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var memory = new Memory<int>(array);
        Assert.Equal(5, memory.Length);
        Assert.False(memory.IsEmpty);
    }

    [Fact]
    public void Memory_FromArraySegment_CreatesMemory()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var memory = new Memory<int>(array, 1, 3);
        Assert.Equal(3, memory.Length);
        var span = memory.Span;
        Assert.Equal(2, span[0]);
        Assert.Equal(3, span[1]);
        Assert.Equal(4, span[2]);
    }

    [Fact]
    public void Memory_Span_ReturnsSpan()
    {
        var array = new[] { 1, 2, 3 };
        var memory = new Memory<int>(array);
        var span = memory.Span;

        Assert.Equal(3, span.Length);
        Assert.Equal(1, span[0]);
        Assert.Equal(2, span[1]);
        Assert.Equal(3, span[2]);
    }

    [Fact]
    public void Memory_Span_CanModifyElements()
    {
        var array = new[] { 1, 2, 3 };
        var memory = new Memory<int>(array);
        var span = memory.Span;

        span[1] = 20;

        Assert.Equal(20, array[1]);
    }

    [Fact]
    public void Memory_Slice_WithStart_CreatesSlice()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var memory = new Memory<int>(array);
        var slice = memory.Slice(2);

        Assert.Equal(3, slice.Length);
        var span = slice.Span;
        Assert.Equal(3, span[0]);
        Assert.Equal(4, span[1]);
        Assert.Equal(5, span[2]);
    }

    [Fact]
    public void Memory_Slice_WithStartAndLength_CreatesSlice()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var memory = new Memory<int>(array);
        var slice = memory.Slice(1, 3);

        Assert.Equal(3, slice.Length);
        var span = slice.Span;
        Assert.Equal(2, span[0]);
        Assert.Equal(3, span[1]);
        Assert.Equal(4, span[2]);
    }

    [Fact]
    public void Memory_Slice_OutOfRange_Throws()
    {
        var array = new[] { 1, 2, 3 };
        var memory = new Memory<int>(array);
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Slice(5));
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Slice(1, 10));
    }

    [Fact]
    public void Memory_ToArray_CreatesNewArray()
    {
        var array = new[] { 1, 2, 3 };
        var memory = new Memory<int>(array);
        var newArray = memory.ToArray();

        Assert.Equal(array, newArray);
        Assert.NotSame(array, newArray);
    }

    [Fact]
    public void Memory_ToArray_EmptyMemory_ReturnsEmptyArray()
    {
        var memory = new Memory<int>(new int[0]);
        var array = memory.ToArray();

        Assert.Empty(array);
    }

    [Fact]
    public void Memory_ImplicitConversion_FromArray()
    {
        var array = new[] { 1, 2, 3 };
        Memory<int> memory = array;

        Assert.Equal(3, memory.Length);
        Assert.Equal(1, memory.Span[0]);
    }

    [Fact]
    public void Memory_ImplicitConversion_ToReadOnlyMemory()
    {
        var array = new[] { 1, 2, 3 };
        var memory = new Memory<int>(array);
        ReadOnlyMemory<int> readOnlyMemory = memory;

        Assert.Equal(3, readOnlyMemory.Length);
        Assert.Equal(1, readOnlyMemory.Span[0]);
    }

    [Fact]
    public void Memory_Empty_CreatesEmptyMemory()
    {
        var memory = Memory<int>.Empty;
        Assert.Equal(0, memory.Length);
        Assert.True(memory.IsEmpty);
    }

    [Fact]
    public void Memory_Equals_SameBackingArray_ReturnsTrue()
    {
        var array = new[] { 1, 2, 3 };
        var memory1 = new Memory<int>(array);
        var memory2 = new Memory<int>(array);

        Assert.True(memory1.Equals(memory2));
    }

    [Fact]
    public void Memory_Equals_DifferentBackingArray_ReturnsFalse()
    {
        var array1 = new[] { 1, 2, 3 };
        var array2 = new[] { 1, 2, 3 };
        var memory1 = new Memory<int>(array1);
        var memory2 = new Memory<int>(array2);

        Assert.False(memory1.Equals(memory2));
    }

    [Fact]
    public void Memory_GetHashCode_ReturnsHashCode()
    {
        var array = new[] { 1, 2, 3 };
        var memory = new Memory<int>(array);
        var hashCode = memory.GetHashCode();

        Assert.NotEqual(0, hashCode);
    }

    [Fact]
    public void Memory_FromNullArray_CreatesEmptyMemory()
    {
        var memory = new Memory<int>(null);
        Assert.Equal(0, memory.Length);
        Assert.True(memory.IsEmpty);
    }

    [Fact]
    public void Memory_SliceModification_AffectsOriginal()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var memory = new Memory<int>(array);
        var slice = memory.Slice(1, 3);
        var span = slice.Span;

        span[0] = 20;
        span[1] = 30;

        Assert.Equal(20, array[1]);
        Assert.Equal(30, array[2]);
    }
}
