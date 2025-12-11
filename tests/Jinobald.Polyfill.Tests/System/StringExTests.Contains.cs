using Xunit;

namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Fact]
    public void Contains_WithChar_ShouldReturnTrue()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains('o');

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithChar_NotFound_ShouldReturnFalse()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains('x');

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_WithChar_EmptyString_ShouldReturnFalse()
    {
        // Arrange
        var text = "";

        // Act
        var result = text.Contains('x');

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_WithStringAndComparison_OrdinalShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains("WORLD", StringComparison.Ordinal);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_WithStringAndComparison_OrdinalIgnoreCaseShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains("WORLD", StringComparison.OrdinalIgnoreCase);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithStringAndComparison_CurrentCultureShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains("World", StringComparison.CurrentCulture);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithStringAndComparison_CurrentCultureIgnoreCaseShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains("world", StringComparison.CurrentCultureIgnoreCase);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithStringAndComparison_InvariantCultureShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains("World", StringComparison.InvariantCulture);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithStringAndComparison_InvariantCultureIgnoreCaseShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains("WORLD", StringComparison.InvariantCultureIgnoreCase);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithEmptyString_ShouldReturnTrue()
    {
        // Arrange
        var text = "Hello";

        // Act
        var result = text.Contains("", StringComparison.Ordinal);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_WithNullValue_ShouldThrow()
    {
        // Arrange
        var text = "Hello";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => text.Contains(null!, StringComparison.Ordinal));
    }
}
