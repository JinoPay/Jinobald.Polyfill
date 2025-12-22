namespace Jinobald.Polyfill.Tests.System;

public class MemoryTests
{
    [Test]
    public void Memory_Empty_CreatesEmptyMemory()
    {
        Memory<int> memory = Memory<int>.Empty;
        Assert.AreEqual(0, memory.Length);
        Assert.IsTrue(memory.IsEmpty);
    }

    [Test]
    public void Memory_Equals_DifferentBackingArray_ReturnsFalse()
    {
        int[] array1 = new[] { 1, 2, 3 };
        int[] array2 = new[] { 1, 2, 3 };
        var memory1 = new Memory<int>(array1);
        var memory2 = new Memory<int>(array2);

        Assert.IsFalse(memory1.Equals(memory2));
    }

    [Test]
    public void Memory_Equals_SameBackingArray_ReturnsTrue()
    {
        int[] array = new[] { 1, 2, 3 };
        var memory1 = new Memory<int>(array);
        var memory2 = new Memory<int>(array);

        Assert.IsTrue(memory1.Equals(memory2));
    }

    [Test]
    public void Memory_FromArray_CreatesMemory()
    {
        int[] array = new[] { 1, 2, 3, 4, 5 };
        var memory = new Memory<int>(array);
        Assert.AreEqual(5, memory.Length);
        Assert.IsFalse(memory.IsEmpty);
    }

    [Test]
    public void Memory_FromArraySegment_CreatesMemory()
    {
        int[] array = new[] { 1, 2, 3, 4, 5 };
        var memory = new Memory<int>(array, 1, 3);
        Assert.AreEqual(3, memory.Length);
        Span<int> span = memory.Span;
        Assert.AreEqual(2, span[0]);
        Assert.AreEqual(3, span[1]);
        Assert.AreEqual(4, span[2]);
    }

    [Test]
    public void Memory_FromNullArray_CreatesEmptyMemory()
    {
        var memory = new Memory<int>(null);
        Assert.AreEqual(0, memory.Length);
        Assert.IsTrue(memory.IsEmpty);
    }

    [Test]
    public void Memory_GetHashCode_ReturnsHashCode()
    {
        int[] array = new[] { 1, 2, 3 };
        var memory = new Memory<int>(array);
        int hashCode = memory.GetHashCode();

        Assert.AreNotEqual(0, hashCode);
    }

    [Test]
    public void Memory_ImplicitConversion_FromArray()
    {
        int[] array = new[] { 1, 2, 3 };
        Memory<int> memory = array;

        Assert.AreEqual(3, memory.Length);
        Assert.AreEqual(1, memory.Span[0]);
    }

    [Test]
    public void Memory_ImplicitConversion_ToReadOnlyMemory()
    {
        int[] array = new[] { 1, 2, 3 };
        var memory = new Memory<int>(array);
        ReadOnlyMemory<int> readOnlyMemory = memory;

        Assert.AreEqual(3, readOnlyMemory.Length);
        Assert.AreEqual(1, readOnlyMemory.Span[0]);
    }

    [Test]
    public void Memory_Slice_OutOfRange_Throws()
    {
        int[] array = new[] { 1, 2, 3 };
        var memory = new Memory<int>(array);
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Slice(5));
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Slice(1, 10));
    }

    [Test]
    public void Memory_Slice_WithStart_CreatesSlice()
    {
        int[] array = new[] { 1, 2, 3, 4, 5 };
        var memory = new Memory<int>(array);
        Memory<int> slice = memory.Slice(2);

        Assert.AreEqual(3, slice.Length);
        Span<int> span = slice.Span;
        Assert.AreEqual(3, span[0]);
        Assert.AreEqual(4, span[1]);
        Assert.AreEqual(5, span[2]);
    }

    [Test]
    public void Memory_Slice_WithStartAndLength_CreatesSlice()
    {
        int[] array = new[] { 1, 2, 3, 4, 5 };
        var memory = new Memory<int>(array);
        Memory<int> slice = memory.Slice(1, 3);

        Assert.AreEqual(3, slice.Length);
        Span<int> span = slice.Span;
        Assert.AreEqual(2, span[0]);
        Assert.AreEqual(3, span[1]);
        Assert.AreEqual(4, span[2]);
    }

    [Test]
    public void Memory_SliceModification_AffectsOriginal()
    {
        int[] array = new[] { 1, 2, 3, 4, 5 };
        var memory = new Memory<int>(array);
        Memory<int> slice = memory.Slice(1, 3);
        Span<int> span = slice.Span;

        span[0] = 20;
        span[1] = 30;

        Assert.AreEqual(20, array[1]);
        Assert.AreEqual(30, array[2]);
    }

    [Test]
    public void Memory_Span_CanModifyElements()
    {
        int[] array = new[] { 1, 2, 3 };
        var memory = new Memory<int>(array);
        Span<int> span = memory.Span;

        span[1] = 20;

        Assert.AreEqual(20, array[1]);
    }

    [Test]
    public void Memory_Span_ReturnsSpan()
    {
        int[] array = new[] { 1, 2, 3 };
        var memory = new Memory<int>(array);
        Span<int> span = memory.Span;

        Assert.AreEqual(3, span.Length);
        Assert.AreEqual(1, span[0]);
        Assert.AreEqual(2, span[1]);
        Assert.AreEqual(3, span[2]);
    }

    [Test]
    public void Memory_ToArray_CreatesNewArray()
    {
        int[] array = new[] { 1, 2, 3 };
        var memory = new Memory<int>(array);
        int[]? newArray = memory.ToArray();

        Assert.AreEqual(array, newArray);
        Assert.AreNotSame(array, newArray);
    }

    [Test]
    public void Memory_ToArray_EmptyMemory_ReturnsEmptyArray()
    {
        var memory = new Memory<int>(new int[0]);
        int[]? array = memory.ToArray();

        Assert.IsEmpty(array);
    }
}
