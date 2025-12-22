using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Test]
    public void Contains_WithChar_ShouldReturnTrue()
    {
        // Arrange
        string text = "Hello World";

        // Act
        bool result = text.Contains('o');

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Contains_WithChar_NotFound_ShouldReturnFalse()
    {
        // Arrange
        string text = "Hello World";

        // Act
        bool result = text.Contains('x');

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void Contains_WithChar_EmptyString_ShouldReturnFalse()
    {
        // Arrange
        string text = "";

        // Act
        bool result = text.Contains('x');

        // Assert
        Assert.IsFalse(result);
    }

#if NETCOREAPP3_0_OR_GREATER
    [Test]
    public void Contains_WithStringAndComparison_OrdinalShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains("WORLD", StringComparison.Ordinal);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void Contains_WithStringAndComparison_OrdinalIgnoreCaseShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains("WORLD", StringComparison.OrdinalIgnoreCase);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Contains_WithStringAndComparison_CurrentCultureShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains("World", StringComparison.CurrentCulture);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Contains_WithStringAndComparison_CurrentCultureIgnoreCaseShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains("world", StringComparison.CurrentCultureIgnoreCase);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Contains_WithStringAndComparison_InvariantCultureShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains("World", StringComparison.InvariantCulture);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Contains_WithStringAndComparison_InvariantCultureIgnoreCaseShouldWork()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var result = text.Contains("WORLD", StringComparison.InvariantCultureIgnoreCase);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Contains_WithEmptyString_ShouldReturnTrue()
    {
        // Arrange
        var text = "Hello";

        // Act
        var result = text.Contains("", StringComparison.Ordinal);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Contains_WithNullValue_ShouldThrow()
    {
        // Arrange
        var text = "Hello";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => text.Contains(null!, StringComparison.Ordinal));
    }
#endif
}
