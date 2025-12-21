namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    [Fact]
    public void GetHashCode_FromSpan_ShouldReturnHashCode()
    {
        // Arrange
        var text = "test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text);

        // Assert
        Assert.Equal("test".GetHashCode(), hashCode);
    }

    [Fact]
    public void GetHashCode_FromEmptySpan_ShouldReturnHashCode()
    {
        // Arrange
        var text = "".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text);

        // Assert
        Assert.Equal(string.Empty.GetHashCode(), hashCode);
    }

    [Fact]
    public void GetHashCode_FromLongSpan_ShouldReturnHashCode()
    {
        // Arrange
        var longText = new string('a', 1000);
        var text = longText.AsSpan();

        // Act
        var hashCode = string.GetHashCode(text);

        // Assert
        Assert.Equal(longText.GetHashCode(), hashCode);
    }
#endif

#if NETCOREAPP3_0_OR_GREATER
    [Fact]
    public void GetHashCode_WithComparison_OrdinalShouldReturnHashCode()
    {
        // Arrange
        var text = "Test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.Ordinal);

        // Assert
        Assert.Equal("Test".GetHashCode(StringComparison.Ordinal), hashCode);
    }

    [Fact]
    public void GetHashCode_WithComparison_OrdinalIgnoreCaseShouldReturnHashCode()
    {
        // Arrange
        var text = "Test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.OrdinalIgnoreCase);

        // Assert
        Assert.Equal("Test".GetHashCode(StringComparison.OrdinalIgnoreCase), hashCode);
    }

    [Fact]
    public void GetHashCode_WithComparison_CurrentCultureShouldReturnHashCode()
    {
        // Arrange
        var text = "Test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.CurrentCulture);

        // Assert
        Assert.Equal("Test".GetHashCode(StringComparison.CurrentCulture), hashCode);
    }

    [Fact]
    public void GetHashCode_WithComparison_CurrentCultureIgnoreCaseShouldReturnHashCode()
    {
        // Arrange
        var text = "Test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.CurrentCultureIgnoreCase);

        // Assert
        Assert.Equal("Test".GetHashCode(StringComparison.CurrentCultureIgnoreCase), hashCode);
    }

    [Fact]
    public void GetHashCode_WithComparison_InvariantCultureShouldReturnHashCode()
    {
        // Arrange
        var text = "Test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.InvariantCulture);

        // Assert
        Assert.Equal("Test".GetHashCode(StringComparison.InvariantCulture), hashCode);
    }

    [Fact]
    public void GetHashCode_WithComparison_InvariantCultureIgnoreCaseShouldReturnHashCode()
    {
        // Arrange
        var text = "Test".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.InvariantCultureIgnoreCase);

        // Assert
        Assert.Equal("Test".GetHashCode(StringComparison.InvariantCultureIgnoreCase), hashCode);
    }

    [Fact]
    public void GetHashCode_SameContent_ShouldReturnSameHashCode()
    {
        // Arrange
        var text1 = "identical".AsSpan();
        var text2 = "identical".AsSpan();

        // Act
        var hashCode1 = string.GetHashCode(text1);
        var hashCode2 = string.GetHashCode(text2);

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_DifferentContent_ShouldReturnDifferentHashCode()
    {
        // Arrange
        var text1 = "different1".AsSpan();
        var text2 = "different2".AsSpan();

        // Act
        var hashCode1 = string.GetHashCode(text1);
        var hashCode2 = string.GetHashCode(text2);

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithComparison_EmptySpan_ShouldReturnHashCode()
    {
        // Arrange
        var text = "".AsSpan();

        // Act
        var hashCode = string.GetHashCode(text, StringComparison.Ordinal);

        // Assert
        Assert.Equal(string.Empty.GetHashCode(StringComparison.Ordinal), hashCode);
    }

    [Fact]
    public void GetHashCode_IgnoreCase_SameContentDifferentCase_ShouldReturnSameHashCode()
    {
        // Arrange
        var text1 = "TEST".AsSpan();
        var text2 = "test".AsSpan();

        // Act
        var hashCode1 = string.GetHashCode(text1, StringComparison.OrdinalIgnoreCase);
        var hashCode2 = string.GetHashCode(text2, StringComparison.OrdinalIgnoreCase);

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }
#endif
}
