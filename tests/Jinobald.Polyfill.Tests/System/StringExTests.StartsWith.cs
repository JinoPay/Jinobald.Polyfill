#if NETCOREAPP3_0_OR_GREATER
using Xunit;

namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Fact]
    public void StartsWith_WithChar_ShouldReturnTrue()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.StartsWith('H');

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void StartsWith_WithChar_NotMatching_ShouldReturnFalse()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.StartsWith('W');

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void StartsWith_WithChar_EmptyString_ShouldReturnFalse()
    {
        // Arrange
        var text = "";

        // Act
        var result = text.StartsWith('H');

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void StartsWith_WithChar_CaseSensitive()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.StartsWith('h');

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void StartsWith_WithChar_SingleChar_ShouldReturnTrue()
    {
        // Arrange
        var text = "H";

        // Act
        var result = text.StartsWith('H');

        // Assert
        Assert.True(result);
    }
}
#endif
