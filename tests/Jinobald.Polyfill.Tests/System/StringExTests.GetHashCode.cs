namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Test]
    public void GetHashCode_FromSpan_ShouldReturnHashCode()
    {
        // Arrange
        var text = "test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text);

        // Assert
        Assert.AreEqual("test".GetHashCode(), hashCode);
    }

    [Test]
    public void GetHashCode_FromEmptySpan_ShouldReturnHashCode()
    {
        // Arrange
        var text = "".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text);

        // Assert
        Assert.AreEqual(string.Empty.GetHashCode(), hashCode);
    }

    [Test]
    public void GetHashCode_FromLongSpan_ShouldReturnHashCode()
    {
        // Arrange
        var longText = new string('a', 1000);
        var text = longText.AsSpan();

        // Act
        var hashCode = string.GetHashCode(text);

        // Assert
        Assert.AreEqual(longText.GetHashCode(), hashCode);
    }

    [Test]
    public void GetHashCode_WithComparison_OrdinalShouldReturnHashCode()
    {
        // Arrange
        var text = "Test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.Ordinal);

        // Assert
        Assert.AreEqual("Test".GetHashCode(StringComparison.Ordinal), hashCode);
    }

    [Test]
    public void GetHashCode_WithComparison_OrdinalIgnoreCaseShouldReturnHashCode()
    {
        // Arrange
        var text = "Test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.OrdinalIgnoreCase);

        // Assert
        Assert.AreEqual("Test".GetHashCode(StringComparison.OrdinalIgnoreCase), hashCode);
    }

    [Test]
    public void GetHashCode_WithComparison_CurrentCultureShouldReturnHashCode()
    {
        // Arrange
        var text = "Test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.CurrentCulture);

        // Assert
        Assert.AreEqual("Test".GetHashCode(StringComparison.CurrentCulture), hashCode);
    }

    [Test]
    public void GetHashCode_WithComparison_CurrentCultureIgnoreCaseShouldReturnHashCode()
    {
        // Arrange
        var text = "Test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.CurrentCultureIgnoreCase);

        // Assert
        Assert.AreEqual("Test".GetHashCode(StringComparison.CurrentCultureIgnoreCase), hashCode);
    }

    [Test]
    public void GetHashCode_WithComparison_InvariantCultureShouldReturnHashCode()
    {
        // Arrange
        var text = "Test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.InvariantCulture);

        // Assert
        Assert.AreEqual("Test".GetHashCode(StringComparison.InvariantCulture), hashCode);
    }

    [Test]
    public void GetHashCode_WithComparison_InvariantCultureIgnoreCaseShouldReturnHashCode()
    {
        // Arrange
        var text = "Test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.InvariantCultureIgnoreCase);

        // Assert
        Assert.AreEqual("Test".GetHashCode(StringComparison.InvariantCultureIgnoreCase), hashCode);
    }

    [Test]
    public void GetHashCode_SameContent_ShouldReturnSameHashCode()
    {
        // Arrange
        var text1 = "identical".AsSpan();
        var text2 = "identical".AsSpan();

        // Act
        var hashCode1 = string.GetHashCode(text1);
        var hashCode2 = string.GetHashCode(text2);

        // Assert
        Assert.AreEqual(hashCode1, hashCode2);
    }

    [Test]
    public void GetHashCode_DifferentContent_ShouldReturnDifferentHashCode()
    {
        // Arrange
        var text1 = "different1".AsSpan();
        var text2 = "different2".AsSpan();

        // Act
        var hashCode1 = string.GetHashCode(text1);
        var hashCode2 = string.GetHashCode(text2);

        // Assert
        Assert.AreNotEqual(hashCode1, hashCode2);
    }

    [Test]
    public void GetHashCode_WithComparison_EmptySpan_ShouldReturnHashCode()
    {
        // Arrange
        var text = "".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.Ordinal);

        // Assert
        Assert.AreEqual(string.Empty.GetHashCode(StringComparison.Ordinal), hashCode);
    }

    [Test]
    public void GetHashCode_IgnoreCase_SameContentDifferentCase_ShouldReturnSameHashCode()
    {
        // Arrange
        var text1 = "TEST".AsSpan();
        var text2 = "test".AsSpan();

        // Act
        var hashCode1 = string.GetHashCode(text1, StringComparison.OrdinalIgnoreCase);
        var hashCode2 = string.GetHashCode(text2, StringComparison.OrdinalIgnoreCase);

        // Assert
        Assert.AreEqual(hashCode1, hashCode2);
    }
}
