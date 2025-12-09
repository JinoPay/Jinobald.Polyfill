using Xunit;

namespace Jinobald.Polyfill.Tests.System;

public class StringExTests
{
#if NETSTANDARD2_0 || NETFRAMEWORK
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

#if NETFRAMEWORK || NETSTANDARD2_0 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2
    [Fact]
    public void Create_WithValidLength_ShouldCreateString()
    {
        // Arrange
        var length = 5;

        // Act
        var result = string.Create(length, 'x', (span, state) =>
        {
            for (var i = 0; i < span.Length; i++)
            {
                span[i] = state;
            }
        });

        // Assert
        Assert.Equal("xxxxx", result);
    }

    [Fact]
    public void Create_WithZeroLength_ShouldReturnEmpty()
    {
        // Act
        var result = string.Create(0, 'x', (span, state) => { });

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Create_WithNegativeLength_ShouldThrow()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            string.Create(-1, 'x', (span, state) => { }));
    }

    [Fact]
    public void Create_WithComplexState_ShouldWork()
    {
        // Arrange
        var data = new { Prefix = "Hello", Suffix = "World" };

        // Act
        var result = string.Create(10, data, (span, state) =>
        {
            state.Prefix.AsSpan().CopyTo(span);
            state.Suffix.AsSpan().CopyTo(span.Slice(5));
        });

        // Assert
        Assert.Equal("HelloWorld", result);
    }
#endif

#if !NETCOREAPP3_0_OR_GREATER && (NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2)
    [Fact]
    public void GetHashCode_FromSpan_ShouldReturnHashCode()
    {
        // Arrange
        var text = "test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text);

        // Assert
        Assert.Equal("test".GetHashCode(), hashCode);
    }
#endif

#if !NET9_0_OR_GREATER && (NETSTANDARD2_1 || NETCOREAPP)
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
