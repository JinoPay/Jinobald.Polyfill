namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
#if !NETFRAMEWORK
    [Test]
    public void Join_WithCharSeparator_ShouldJoinStrings()
    {
        // Arrange
        var values = new[] { "a", "b", "c" };

        // Act
        var result = string.Join(',', values);

        // Assert
        Assert.AreEqual("a,b,c", result);
    }

    [Test]
    public void Join_WithCharSeparator_EmptyArray_ShouldReturnEmpty()
    {
        // Arrange
        var values = Array.Empty<string>();

        // Act
        var result = string.Join(',', values);

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [Test]
    public void Join_WithCharSeparator_SingleElement_ShouldReturnElement()
    {
        // Arrange
        var values = new[] { "only" };

        // Act
        var result = string.Join(',', values);

        // Assert
        Assert.AreEqual("only", result);
    }

    [Test]
    public void Join_WithCharSeparator_Objects_ShouldJoinObjects()
    {
        // Arrange
        var values = new object[] { 1, 2, 3 };

        // Act
        var result = string.Join(',', values);

        // Assert
        Assert.AreEqual("1,2,3", result);
    }

    [Test]
    public void Join_WithCharSeparator_StartIndexAndCount_ShouldJoinSubset()
    {
        // Arrange
        var values = new[] { "a", "b", "c", "d" };

        // Act
        var result = string.Join(',', values, 1, 2);

        // Assert
        Assert.AreEqual("b,c", result);
    }

    [Test]
    public void Join_WithCharSeparator_IEnumerable_ShouldJoinElements()
    {
        // Arrange
        var values = new List<int> { 1, 2, 3 };

        // Act
        var result = string.Join(',', values);

        // Assert
        Assert.AreEqual("1,2,3", result);
    }
#endif
}
