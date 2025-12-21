using Xunit;

namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
#if !NETFRAMEWORK
    [Fact]
    public void Join_WithCharSeparator_ShouldJoinStrings()
    {
        // Arrange
        var values = new[] { "a", "b", "c" };

        // Act
        var result = string.Join(',', values);

        // Assert
        Assert.Equal("a,b,c", result);
    }

    [Fact]
    public void Join_WithCharSeparator_EmptyArray_ShouldReturnEmpty()
    {
        // Arrange
        var values = Array.Empty<string>();

        // Act
        var result = string.Join(',', values);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Join_WithCharSeparator_SingleElement_ShouldReturnElement()
    {
        // Arrange
        var values = new[] { "only" };

        // Act
        var result = string.Join(',', values);

        // Assert
        Assert.Equal("only", result);
    }

    [Fact]
    public void Join_WithCharSeparator_Objects_ShouldJoinObjects()
    {
        // Arrange
        var values = new object[] { 1, 2, 3 };

        // Act
        var result = string.Join(',', values);

        // Assert
        Assert.Equal("1,2,3", result);
    }

    [Fact]
    public void Join_WithCharSeparator_StartIndexAndCount_ShouldJoinSubset()
    {
        // Arrange
        var values = new[] { "a", "b", "c", "d" };

        // Act
        var result = string.Join(',', values, 1, 2);

        // Assert
        Assert.Equal("b,c", result);
    }

    [Fact]
    public void Join_WithCharSeparator_IEnumerable_ShouldJoinElements()
    {
        // Arrange
        var values = new List<int> { 1, 2, 3 };

        // Act
        var result = string.Join(',', values);

        // Assert
        Assert.Equal("1,2,3", result);
    }
#endif

#if NETCOREAPP && !NET8_0_OR_GREATER
    [Fact]
    public void Join_WithCharSeparator_ReadOnlySpan_ShouldJoinStrings()
    {
        // Arrange
        ReadOnlySpan<string?> values = new[] { "a", "b", "c" };

        // Act
        var result = string.Join(',', values);

        // Assert
        Assert.Equal("a,b,c", result);
    }

    [Fact]
    public void Join_WithStringSeparator_ReadOnlySpan_ShouldJoinStrings()
    {
        // Arrange
        ReadOnlySpan<string?> values = new[] { "a", "b", "c" };

        // Act
        var result = string.Join(", ", values);

        // Assert
        Assert.Equal("a, b, c", result);
    }

    [Fact]
    public void Join_WithCharSeparator_ReadOnlySpanObjects_ShouldJoinObjects()
    {
        // Arrange
        ReadOnlySpan<object?> values = new object[] { 1, 2, 3 };

        // Act
        var result = string.Join(',', values);

        // Assert
        Assert.Equal("1,2,3", result);
    }

    [Fact]
    public void Join_WithStringSeparator_ReadOnlySpanObjects_ShouldJoinObjects()
    {
        // Arrange
        ReadOnlySpan<object?> values = new object[] { 1, 2, 3 };

        // Act
        var result = string.Join(", ", values);

        // Assert
        Assert.Equal("1, 2, 3", result);
    }

    [Fact]
    public void Join_WithNullSeparator_ShouldTreatAsEmpty()
    {
        // Arrange
        ReadOnlySpan<string?> values = new[] { "a", "b", "c" };

        // Act
        var result = string.Join((string?)null, values);

        // Assert
        Assert.Equal("abc", result);
    }

    [Fact]
    public void Join_WithNullValues_ShouldHandleNulls()
    {
        // Arrange
        ReadOnlySpan<string?> values = new[] { "a", null, "c" };

        // Act
        var result = string.Join(",", values);

        // Assert
        Assert.Equal("a,,c", result);
    }
#endif
}
