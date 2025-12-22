namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Test]
    public void Join_WithCharSeparator_EmptyArray_ShouldReturnEmpty()
    {
        // Arrange
        string[] values = Array.Empty<string>();

        // Act
        string result = string.Join(',', values);

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [Test]
    public void Join_WithCharSeparator_IEnumerable_ShouldJoinElements()
    {
        // Arrange
        var values = new List<int> { 1, 2, 3 };

        // Act
        string result = string.Join(',', values);

        // Assert
        Assert.AreEqual("1,2,3", result);
    }

    [Test]
    public void Join_WithCharSeparator_Objects_ShouldJoinObjects()
    {
        // Arrange
        object[] values = [1, 2, 3];

        // Act
        string result = string.Join(',', values);

        // Assert
        Assert.AreEqual("1,2,3", result);
    }

    [Test]
    public void Join_WithCharSeparator_ShouldJoinStrings()
    {
        // Arrange
        string[] values = ["a", "b", "c"];

        // Act
        string result = string.Join(',', values);

        // Assert
        Assert.AreEqual("a,b,c", result);
    }

    [Test]
    public void Join_WithCharSeparator_SingleElement_ShouldReturnElement()
    {
        // Arrange
        string[] values = ["only"];

        // Act
        string result = string.Join(',', values);

        // Assert
        Assert.AreEqual("only", result);
    }

    [Test]
    public void Join_WithCharSeparator_StartIndexAndCount_ShouldJoinSubset()
    {
        // Arrange
        string[] values = ["a", "b", "c", "d"];

        // Act
        string result = string.Join(',', values, 1, 2);

        // Assert
        Assert.AreEqual("b,c", result);
    }
}
