namespace Jinobald.Polyfill.Tests.System;

using global::System;
using Xunit;

public class ReadOnlyMemoryTests
{
    [Fact]
    public void ReadOnlyMemory_FromArray_CreatesMemory()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var memory = new ReadOnlyMemory<int>(array);
        Assert.Equal(5, memory.Length);
        Assert.False(memory.IsEmpty);
    }

    [Fact]
    public void ReadOnlyMemory_FromArraySegment_CreatesMemory()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var memory = new ReadOnlyMemory<int>(array, 1, 3);
        Assert.Equal(3, memory.Length);
        var span = memory.Span;
        Assert.Equal(2, span[0]);
        Assert.Equal(3, span[1]);
        Assert.Equal(4, span[2]);
    }

    [Fact]
    public void ReadOnlyMemory_Span_ReturnsReadOnlySpan()
    {
        var array = new[] { 1, 2, 3 };
        var memory = new ReadOnlyMemory<int>(array);
        var span = memory.Span;

        Assert.Equal(3, span.Length);
        Assert.Equal(1, span[0]);
        Assert.Equal(2, span[1]);
        Assert.Equal(3, span[2]);
    }

    [Fact]
    public void ReadOnlyMemory_Slice_WithStart_CreatesSlice()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var memory = new ReadOnlyMemory<int>(array);
        var slice = memory.Slice(2);

        Assert.Equal(3, slice.Length);
        var span = slice.Span;
        Assert.Equal(3, span[0]);
        Assert.Equal(4, span[1]);
        Assert.Equal(5, span[2]);
    }

    [Fact]
    public void ReadOnlyMemory_Slice_WithStartAndLength_CreatesSlice()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var memory = new ReadOnlyMemory<int>(array);
        var slice = memory.Slice(1, 3);

        Assert.Equal(3, slice.Length);
        var span = slice.Span;
        Assert.Equal(2, span[0]);
        Assert.Equal(3, span[1]);
        Assert.Equal(4, span[2]);
    }

    [Fact]
    public void ReadOnlyMemory_Slice_OutOfRange_Throws()
    {
        var array = new[] { 1, 2, 3 };
        var memory = new ReadOnlyMemory<int>(array);
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Slice(5));
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Slice(1, 10));
    }

    [Fact]
    public void ReadOnlyMemory_ToArray_CreatesNewArray()
    {
        var array = new[] { 1, 2, 3 };
        var memory = new ReadOnlyMemory<int>(array);
        var newArray = memory.ToArray();

        Assert.Equal(array, newArray);
        Assert.NotSame(array, newArray);
    }

    [Fact]
    public void ReadOnlyMemory_ToArray_EmptyMemory_ReturnsEmptyArray()
    {
        var memory = new ReadOnlyMemory<int>(new int[0]);
        var array = memory.ToArray();

        Assert.Empty(array);
    }

    [Fact]
    public void ReadOnlyMemory_ImplicitConversion_FromArray()
    {
        var array = new[] { 1, 2, 3 };
        ReadOnlyMemory<int> memory = array;

        Assert.Equal(3, memory.Length);
        Assert.Equal(1, memory.Span[0]);
    }

    [Fact]
    public void ReadOnlyMemory_ImplicitConversion_FromMemory()
    {
        var array = new[] { 1, 2, 3 };
        var mutableMemory = new Memory<int>(array);
        ReadOnlyMemory<int> readOnlyMemory = mutableMemory;

        Assert.Equal(3, readOnlyMemory.Length);
        Assert.Equal(1, readOnlyMemory.Span[0]);
    }

    [Fact]
    public void ReadOnlyMemory_Empty_CreatesEmptyMemory()
    {
        var memory = ReadOnlyMemory<int>.Empty;
        Assert.Equal(0, memory.Length);
        Assert.True(memory.IsEmpty);
    }

    [Fact]
    public void ReadOnlyMemory_Equals_SameBackingArray_ReturnsTrue()
    {
        var array = new[] { 1, 2, 3 };
        var memory1 = new ReadOnlyMemory<int>(array);
        var memory2 = new ReadOnlyMemory<int>(array);

        Assert.True(memory1.Equals(memory2));
    }

    [Fact]
    public void ReadOnlyMemory_Equals_DifferentBackingArray_ReturnsFalse()
    {
        var array1 = new[] { 1, 2, 3 };
        var array2 = new[] { 1, 2, 3 };
        var memory1 = new ReadOnlyMemory<int>(array1);
        var memory2 = new ReadOnlyMemory<int>(array2);

        Assert.False(memory1.Equals(memory2));
    }

    [Fact]
    public void ReadOnlyMemory_GetHashCode_ReturnsHashCode()
    {
        var array = new[] { 1, 2, 3 };
        var memory = new ReadOnlyMemory<int>(array);
        var hashCode = memory.GetHashCode();

        Assert.NotEqual(0, hashCode);
    }

    [Fact]
    public void ReadOnlyMemory_FromNullArray_CreatesEmptyMemory()
    {
        var memory = new ReadOnlyMemory<int>(null);
        Assert.Equal(0, memory.Length);
        Assert.True(memory.IsEmpty);
    }

    [Fact]
    public void ReadOnlyMemory_WithString_WorksWithCharMemory()
    {
        var text = "Hello";
        var memory = new ReadOnlyMemory<char>(text.ToCharArray());
        var span = memory.Span;

        Assert.Equal(5, span.Length);
        Assert.Equal('H', span[0]);
        Assert.Equal('o', span[4]);
    }

    [Fact]
    public void ReadOnlyMemory_SlicePreservesData()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var memory = new ReadOnlyMemory<int>(array);
        var slice = memory.Slice(1, 3);

        // 원본 배열을 수정해도 슬라이스가 변경된 값을 참조함
        array[2] = 99;

        Assert.Equal(99, slice.Span[1]);
    }
}
