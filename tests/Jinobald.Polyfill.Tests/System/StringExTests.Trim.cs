using Xunit;

namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Fact]
    public void Trim_WithChar_ShouldRemoveFromBothEnds()
    {
        // Arrange
        string text = "xHello Worldx";

        // Act
        string result = text.Trim('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Trim_WithChar_NoMatch_ShouldReturnOriginal()
    {
        // Arrange
        string text = "Hello World";

        // Act
        string result = text.Trim('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Trim_WithChar_MultipleChars_ShouldRemoveAll()
    {
        // Arrange
        string text = "xxxHello Worldxxx";

        // Act
        string result = text.Trim('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Trim_WithChar_OnlyTrimChar_ShouldReturnEmpty()
    {
        // Arrange
        string text = "xxxx";

        // Act
        string result = text.Trim('x');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Trim_WithChar_EmptyString_ShouldReturnEmpty()
    {
        // Arrange
        string text = "";

        // Act
        string result = text.Trim('x');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void TrimStart_WithChar_ShouldRemoveFromStart()
    {
        // Arrange
        string text = "xHello Worldx";

        // Act
        string result = text.TrimStart('x');

        // Assert
        Assert.Equal("Hello Worldx", result);
    }

    [Fact]
    public void TrimStart_WithChar_NoMatch_ShouldReturnOriginal()
    {
        // Arrange
        string text = "Hello World";

        // Act
        string result = text.TrimStart('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void TrimStart_WithChar_MultipleChars_ShouldRemoveAll()
    {
        // Arrange
        string text = "xxxHello World";

        // Act
        string result = text.TrimStart('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void TrimStart_WithChar_OnlyTrimChar_ShouldReturnEmpty()
    {
        // Arrange
        string text = "xxxx";

        // Act
        string result = text.TrimStart('x');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void TrimStart_WithChar_EmptyString_ShouldReturnEmpty()
    {
        // Arrange
        string text = "";

        // Act
        string result = text.TrimStart('x');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void TrimEnd_WithChar_ShouldRemoveFromEnd()
    {
        // Arrange
        string text = "xHello Worldx";

        // Act
        string result = text.TrimEnd('x');

        // Assert
        Assert.Equal("xHello World", result);
    }

    [Fact]
    public void TrimEnd_WithChar_NoMatch_ShouldReturnOriginal()
    {
        // Arrange
        string text = "Hello World";

        // Act
        string result = text.TrimEnd('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void TrimEnd_WithChar_MultipleChars_ShouldRemoveAll()
    {
        // Arrange
        string text = "Hello Worldxxx";

        // Act
        string result = text.TrimEnd('x');

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void TrimEnd_WithChar_OnlyTrimChar_ShouldReturnEmpty()
    {
        // Arrange
        string text = "xxxx";

        // Act
        string result = text.TrimEnd('x');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void TrimEnd_WithChar_EmptyString_ShouldReturnEmpty()
    {
        // Arrange
        string text = "";

        // Act
        string result = text.TrimEnd('x');

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Trim_WithChar_WhitespaceNotAffected()
    {
        // Arrange
        string text = "x Hello World x";

        // Act
        string result = text.Trim('x');

        // Assert
        Assert.Equal(" Hello World ", result);
    }

    [Fact]
    public void TrimStart_WithChar_WhitespaceNotAffected()
    {
        // Arrange
        string text = "x Hello World";

        // Act
        string result = text.TrimStart('x');

        // Assert
        Assert.Equal(" Hello World", result);
    }

    [Fact]
    public void TrimEnd_WithChar_WhitespaceNotAffected()
    {
        // Arrange
        string text = "Hello World x";

        // Act
        string result = text.TrimEnd('x');

        // Assert
        Assert.Equal("Hello World ", result);
    }
}
