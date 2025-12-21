#if NETCOREAPP2_1_OR_GREATER
using Xunit;

namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Fact]
    public void Replace_WithStringAndComparison_OrdinalShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Replace("World", "Universe", StringComparison.Ordinal);

        // Assert
        Assert.Equal("Hello Universe", result);
    }

    [Fact]
    public void Replace_WithStringAndComparison_OrdinalIgnoreCaseShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Replace("WORLD", "Universe", StringComparison.OrdinalIgnoreCase);

        // Assert
        Assert.Equal("Hello Universe", result);
    }

    [Fact]
    public void Replace_WithStringAndComparison_NoMatch_ShouldReturnOriginal()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Replace("xyz", "abc", StringComparison.Ordinal);

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Replace_WithStringAndComparison_CurrentCultureShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Replace("World", "Universe", StringComparison.CurrentCulture);

        // Assert
        Assert.Equal("Hello Universe", result);
    }

    [Fact]
    public void Replace_WithStringAndComparison_CurrentCultureIgnoreCaseShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Replace("world", "Universe", StringComparison.CurrentCultureIgnoreCase);

        // Assert
        Assert.Equal("Hello Universe", result);
    }

    [Fact]
    public void Replace_WithStringAndComparison_InvariantCultureShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Replace("World", "Universe", StringComparison.InvariantCulture);

        // Assert
        Assert.Equal("Hello Universe", result);
    }

    [Fact]
    public void Replace_WithStringAndComparison_InvariantCultureIgnoreCaseShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Replace("WORLD", "Universe", StringComparison.InvariantCultureIgnoreCase);

        // Assert
        Assert.Equal("Hello Universe", result);
    }

    [Fact]
    public void Replace_WithStringAndComparison_MultipleOccurrences_ShouldReplaceAll()
    {
        // Arrange
        var text = "Hello Hello Hello";

        // Act
        var result = text.Replace("Hello", "Hi", StringComparison.Ordinal);

        // Assert
        Assert.Equal("Hi Hi Hi", result);
    }

    [Fact]
    public void Replace_WithStringAndComparison_EmptyOldValue_ShouldThrow()
    {
        // Arrange
        var text = "Hello";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => text.Replace("", "X", StringComparison.Ordinal));
    }

    [Fact]
    public void Replace_WithStringAndComparison_NullOldValue_ShouldThrow()
    {
        // Arrange
        var text = "Hello";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => text.Replace(null!, "X", StringComparison.Ordinal));
    }

    [Fact]
    public void Replace_WithStringAndComparison_NullNewValue_ShouldRemove()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Replace("World", null, StringComparison.Ordinal);

        // Assert
        Assert.Equal("Hello ", result);
    }

    [Fact]
    public void Replace_WithStringAndComparison_OverlappingMatches()
    {
        // Arrange
        var text = "aaa";

        // Act
        var result = text.Replace("aa", "b", StringComparison.Ordinal);

        // Assert
        Assert.Equal("ba", result);
    }
}
#endif
