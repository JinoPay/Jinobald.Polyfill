using Xunit;

namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Fact]
    public void Split_WithChar_ShouldSplitString()
    {
        // Arrange
        var text = "a,b,c";

        // Act
        var result = text.Split(',');

        // Assert
        Assert.Equal(new[] { "a", "b", "c" }, result);
    }

    [Fact]
    public void Split_WithChar_NoDelimiter_ShouldReturnOriginal()
    {
        // Arrange
        var text = "abc";

        // Act
        var result = text.Split(',');

        // Assert
        Assert.Equal(new[] { "abc" }, result);
    }

    [Fact]
    public void Split_WithChar_EmptyString_ShouldReturnEmptyArray()
    {
        // Arrange
        var text = "";

        // Act
        var result = text.Split(',');

        // Assert
        Assert.Equal(new[] { "" }, result);
    }

    [Fact]
    public void Split_WithChar_AndOptions_RemoveEmptyEntries()
    {
        // Arrange
        var text = "a,,b,c";

        // Act
        var result = text.Split(',', StringSplitOptions.RemoveEmptyEntries);

        // Assert
        Assert.Equal(new[] { "a", "b", "c" }, result);
    }

    [Fact]
    public void Split_WithChar_AndOptions_None()
    {
        // Arrange
        var text = "a,,b,c";

        // Act
        var result = text.Split(',', StringSplitOptions.None);

        // Assert
        Assert.Equal(new[] { "a", "", "b", "c" }, result);
    }

    [Fact]
    public void Split_WithChar_AndOptions_TrimEntries()
    {
        // Arrange
        var text = "a, b , c";

        // Act
        var result = text.Split(',', StringSplitOptions.TrimEntries);

        // Assert
        Assert.Equal(new[] { "a", "b", "c" }, result);
    }

    [Fact]
    public void Split_WithChar_AndOptions_RemoveEmptyAndTrim()
    {
        // Arrange
        var text = "a, , b , c";

        // Act
        var result = text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        // Assert
        Assert.Equal(new[] { "a", "b", "c" }, result);
    }

    [Fact]
    public void Split_WithChar_AndCount_ShouldLimitSplits()
    {
        // Arrange
        var text = "a,b,c,d";

        // Act
        var result = text.Split(',', 2);

        // Assert
        Assert.Equal(new[] { "a", "b,c,d" }, result);
    }

    [Fact]
    public void Split_WithChar_CountAndOptions_ShouldWork()
    {
        // Arrange
        var text = "a, ,b,c";

        // Act
        var result = text.Split(',', 3, StringSplitOptions.TrimEntries);

        // Assert
        Assert.Equal(new[] { "a", "", "b,c" }, result);
    }

    [Fact]
    public void Split_WithString_ShouldSplitString()
    {
        // Arrange
        var text = "a::b::c";

        // Act
        var result = text.Split("::");

        // Assert
        Assert.Equal(new[] { "a", "b", "c" }, result);
    }

    [Fact]
    public void Split_WithString_AndOptions_RemoveEmptyEntries()
    {
        // Arrange
        var text = "a::::b::c";

        // Act
        var result = text.Split("::", StringSplitOptions.RemoveEmptyEntries);

        // Assert
        Assert.Equal(new[] { "a", "b", "c" }, result);
    }

    [Fact]
    public void Split_WithString_AndOptions_TrimEntries()
    {
        // Arrange
        var text = "a:: b :: c";

        // Act
        var result = text.Split("::", StringSplitOptions.TrimEntries);

        // Assert
        Assert.Equal(new[] { "a", "b", "c" }, result);
    }

    [Fact]
    public void Split_WithString_AndCount_ShouldLimitSplits()
    {
        // Arrange
        var text = "a::b::c::d";

        // Act
        var result = text.Split("::", 2);

        // Assert
        Assert.Equal(new[] { "a", "b::c::d" }, result);
    }

    [Fact]
    public void Split_WithString_CountAndOptions_ShouldWork()
    {
        // Arrange
        var text = "a:: ::b::c";

        // Act
        var result = text.Split("::", 3, StringSplitOptions.TrimEntries);

        // Assert
        Assert.Equal(new[] { "a", "", "b::c" }, result);
    }

    [Fact]
    public void Split_WithCharArray_ShouldSplitOnAnyChar()
    {
        // Arrange
        var text = "a,b;c:d";

        // Act
        var result = text.Split(new[] { ',', ';', ':' });

        // Assert
        Assert.Equal(new[] { "a", "b", "c", "d" }, result);
    }

    [Fact]
    public void Split_WithStringArray_ShouldSplitOnAnyString()
    {
        // Arrange
        var text = "a::b;;c::d";

        // Act
        var result = text.Split(new[] { "::", ";;" }, StringSplitOptions.None);

        // Assert
        Assert.Equal(new[] { "a", "b", "c", "d" }, result);
    }
}
