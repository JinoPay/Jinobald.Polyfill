namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Test]
    public void Trim_WithChar_EmptyString_ShouldReturnEmpty()
    {
        // Arrange
        string text = "";

        // Act
        string result = text.Trim('x');

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [Test]
    public void Trim_WithChar_MultipleChars_ShouldRemoveAll()
    {
        // Arrange
        string text = "xxxHello Worldxxx";

        // Act
        string result = text.Trim('x');

        // Assert
        Assert.AreEqual("Hello World", result);
    }

    [Test]
    public void Trim_WithChar_NoMatch_ShouldReturnOriginal()
    {
        // Arrange
        string text = "Hello World";

        // Act
        string result = text.Trim('x');

        // Assert
        Assert.AreEqual("Hello World", result);
    }

    [Test]
    public void Trim_WithChar_OnlyTrimChar_ShouldReturnEmpty()
    {
        // Arrange
        string text = "xxxx";

        // Act
        string result = text.Trim('x');

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [Test]
    public void Trim_WithChar_ShouldRemoveFromBothEnds()
    {
        // Arrange
        string text = "xHello Worldx";

        // Act
        string result = text.Trim('x');

        // Assert
        Assert.AreEqual("Hello World", result);
    }

    [Test]
    public void Trim_WithChar_WhitespaceNotAffected()
    {
        // Arrange
        string text = "x Hello World x";

        // Act
        string result = text.Trim('x');

        // Assert
        Assert.AreEqual(" Hello World ", result);
    }

    [Test]
    public void TrimEnd_WithChar_EmptyString_ShouldReturnEmpty()
    {
        // Arrange
        string text = "";

        // Act
        string result = text.TrimEnd('x');

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [Test]
    public void TrimEnd_WithChar_MultipleChars_ShouldRemoveAll()
    {
        // Arrange
        string text = "Hello Worldxxx";

        // Act
        string result = text.TrimEnd('x');

        // Assert
        Assert.AreEqual("Hello World", result);
    }

    [Test]
    public void TrimEnd_WithChar_NoMatch_ShouldReturnOriginal()
    {
        // Arrange
        string text = "Hello World";

        // Act
        string result = text.TrimEnd('x');

        // Assert
        Assert.AreEqual("Hello World", result);
    }

    [Test]
    public void TrimEnd_WithChar_OnlyTrimChar_ShouldReturnEmpty()
    {
        // Arrange
        string text = "xxxx";

        // Act
        string result = text.TrimEnd('x');

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [Test]
    public void TrimEnd_WithChar_ShouldRemoveFromEnd()
    {
        // Arrange
        string text = "xHello Worldx";

        // Act
        string result = text.TrimEnd('x');

        // Assert
        Assert.AreEqual("xHello World", result);
    }

    [Test]
    public void TrimEnd_WithChar_WhitespaceNotAffected()
    {
        // Arrange
        string text = "Hello World x";

        // Act
        string result = text.TrimEnd('x');

        // Assert
        Assert.AreEqual("Hello World ", result);
    }

    [Test]
    public void TrimStart_WithChar_EmptyString_ShouldReturnEmpty()
    {
        // Arrange
        string text = "";

        // Act
        string result = text.TrimStart('x');

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [Test]
    public void TrimStart_WithChar_MultipleChars_ShouldRemoveAll()
    {
        // Arrange
        string text = "xxxHello World";

        // Act
        string result = text.TrimStart('x');

        // Assert
        Assert.AreEqual("Hello World", result);
    }

    [Test]
    public void TrimStart_WithChar_NoMatch_ShouldReturnOriginal()
    {
        // Arrange
        string text = "Hello World";

        // Act
        string result = text.TrimStart('x');

        // Assert
        Assert.AreEqual("Hello World", result);
    }

    [Test]
    public void TrimStart_WithChar_OnlyTrimChar_ShouldReturnEmpty()
    {
        // Arrange
        string text = "xxxx";

        // Act
        string result = text.TrimStart('x');

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [Test]
    public void TrimStart_WithChar_ShouldRemoveFromStart()
    {
        // Arrange
        string text = "xHello Worldx";

        // Act
        string result = text.TrimStart('x');

        // Assert
        Assert.AreEqual("Hello Worldx", result);
    }

    [Test]
    public void TrimStart_WithChar_WhitespaceNotAffected()
    {
        // Arrange
        string text = "x Hello World";

        // Act
        string result = text.TrimStart('x');

        // Assert
        Assert.AreEqual(" Hello World", result);
    }
}
