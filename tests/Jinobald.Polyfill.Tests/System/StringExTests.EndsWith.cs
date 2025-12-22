using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Test]
    public void EndsWith_WithChar_ShouldReturnTrue()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.EndsWith('d');

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void EndsWith_WithChar_NotMatching_ShouldReturnFalse()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.EndsWith('o');

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void EndsWith_WithChar_EmptyString_ShouldReturnFalse()
    {
        // Arrange
        var text = "";

        // Act
        var result = text.EndsWith('d');

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void EndsWith_WithChar_CaseSensitive()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.EndsWith('D');

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void EndsWith_WithChar_SingleChar_ShouldReturnTrue()
    {
        // Arrange
        var text = "d";

        // Act
        var result = text.EndsWith('d');

        // Assert
        Assert.IsTrue(result);
    }
}
