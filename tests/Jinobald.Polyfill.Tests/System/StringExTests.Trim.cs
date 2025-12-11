using Xunit;

namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Fact]
    public void Trim_WithChar_ShouldRemoveFromBothEnds()
    {
        // Arrange
        var text = "xHello Worldx";

        // Act
        var result = text.Trim('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Trim_WithChar_NoMatch_ShouldReturnOriginal()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Trim('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Trim_WithChar_MultipleChars_ShouldRemoveAll()
    {
        // Arrange
        var text = "xxxHello Worldxxx";

        // Act
        var result = text.Trim('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Trim_WithChar_OnlyTrimChar_ShouldReturnEmpty()
    {
        // Arrange
        var text = "xxxx";

        // Act
        var result = text.Trim('x');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Trim_WithChar_EmptyString_ShouldReturnEmpty()
    {
        // Arrange
        var text = "";

        // Act
        var result = text.Trim('x');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void TrimStart_WithChar_ShouldRemoveFromStart()
    {
        // Arrange
        var text = "xHello Worldx";

        // Act
        var result = text.TrimStart('x');

        // Assert
        Assert.Equal("Hello Worldx", result);
    }

    [Fact]
    public void TrimStart_WithChar_NoMatch_ShouldReturnOriginal()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.TrimStart('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void TrimStart_WithChar_MultipleChars_ShouldRemoveAll()
    {
        // Arrange
        var text = "xxxHello World";

        // Act
        var result = text.TrimStart('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void TrimStart_WithChar_OnlyTrimChar_ShouldReturnEmpty()
    {
        // Arrange
        var text = "xxxx";

        // Act
        var result = text.TrimStart('x');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void TrimStart_WithChar_EmptyString_ShouldReturnEmpty()
    {
        // Arrange
        var text = "";

        // Act
        var result = text.TrimStart('x');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void TrimEnd_WithChar_ShouldRemoveFromEnd()
    {
        // Arrange
        var text = "xHello Worldx";

        // Act
        var result = text.TrimEnd('x');

        // Assert
        Assert.Equal("xHello World", result);
    }

    [Fact]
    public void TrimEnd_WithChar_NoMatch_ShouldReturnOriginal()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.TrimEnd('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void TrimEnd_WithChar_MultipleChars_ShouldRemoveAll()
    {
        // Arrange
        var text = "Hello Worldxxx";

        // Act
        var result = text.TrimEnd('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void TrimEnd_WithChar_OnlyTrimChar_ShouldReturnEmpty()
    {
        // Arrange
        var text = "xxxx";

        // Act
        var result = text.TrimEnd('x');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void TrimEnd_WithChar_EmptyString_ShouldReturnEmpty()
    {
        // Arrange
        var text = "";

        // Act
        var result = text.TrimEnd('x');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Trim_WithChar_WhitespaceNotAffected()
    {
        // Arrange
        var text = "x Hello World x";

        // Act
        var result = text.Trim('x');

        // Assert
        Assert.Equal(" Hello World ", result);
    }

    [Fact]
    public void TrimStart_WithChar_WhitespaceNotAffected()
    {
        // Arrange
        var text = "x Hello World";

        // Act
        var result = text.TrimStart('x');

        // Assert
        Assert.Equal(" Hello World", result);
    }

    [Fact]
    public void TrimEnd_WithChar_WhitespaceNotAffected()
    {
        // Arrange
        var text = "Hello World x";

        // Act
        var result = text.TrimEnd('x');

        // Assert
        Assert.Equal("Hello World ", result);
    }
}
