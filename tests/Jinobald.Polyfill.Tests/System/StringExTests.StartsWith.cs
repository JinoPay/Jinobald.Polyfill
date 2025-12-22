#if NETCOREAPP3_0_OR_GREATER
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Test]
    public void StartsWith_WithChar_ShouldReturnTrue()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.StartsWith('H');

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void StartsWith_WithChar_NotMatching_ShouldReturnFalse()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.StartsWith('W');

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void StartsWith_WithChar_EmptyString_ShouldReturnFalse()
    {
        // Arrange
        var text = "";

        // Act
        var result = text.StartsWith('H');

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void StartsWith_WithChar_CaseSensitive()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.StartsWith('h');

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void StartsWith_WithChar_SingleChar_ShouldReturnTrue()
    {
        // Arrange
        var text = "H";

        // Act
        var result = text.StartsWith('H');

        // Assert
        Assert.IsTrue(result);
    }
}
#endif
