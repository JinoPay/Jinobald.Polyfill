using Xunit;

namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Fact]
    public void EndsWith_WithChar_ShouldReturnTrue()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.EndsWith('d');

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EndsWith_WithChar_NotMatching_ShouldReturnFalse()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.EndsWith('o');

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EndsWith_WithChar_EmptyString_ShouldReturnFalse()
    {
        // Arrange
        var text = "";

        // Act
        var result = text.EndsWith('d');

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EndsWith_WithChar_CaseSensitive()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.EndsWith('D');

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EndsWith_WithChar_SingleChar_ShouldReturnTrue()
    {
        // Arrange
        var text = "d";

        // Act
        var result = text.EndsWith('d');

        // Assert
        Assert.True(result);
    }
}
