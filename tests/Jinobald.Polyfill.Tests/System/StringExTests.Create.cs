#if !NETFRAMEWORK
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System;

public partial class StringExTests
{
    [Test]
    public void Create_WithValidLength_ShouldCreateString()
    {
        // Arrange
        var length = 5;

        // Act
        var result = string.Create(length, 'x', (span, state) =>
        {
            for (var i = 0; i < span.Length; i++)
            {
                span[i] = state;
            }
        });

        // Assert
        Assert.AreEqual("xxxxx", result);
    }

    [Test]
    public void Create_WithZeroLength_ShouldReturnEmpty()
    {
        // Act
        var result = string.Create(0, 'x', (span, state) => { });

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [Test]
    public void Create_WithNegativeLength_ShouldThrow()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            string.Create(-1, 'x', (span, state) => { }));
    }

    [Test]
    public void Create_WithComplexState_ShouldWork()
    {
        // Arrange
        var data = new { Prefix = "Hello", Suffix = "World" };

        // Act
        var result = string.Create(10, data, (span, state) =>
        {
            state.Prefix.AsSpan().CopyTo(span);
            state.Suffix.AsSpan().CopyTo(span.Slice(5));
        });

        // Assert
        Assert.AreEqual("HelloWorld", result);
    }

    [Test]
    public void Create_WithNullState_ShouldWork()
    {
        // Act
        var result = string.Create(3, (string?)null, (span, state) =>
        {
            span[0] = 'f';
            span[1] = 'o';
            span[2] = 'o';
        });

        // Assert
        Assert.AreEqual("foo", result);
    }

    [Test]
    public void Create_WithLargeString_ShouldWork()
    {
        // Arrange
        var length = 1000;

        // Act
        var result = string.Create(length, '*', (span, state) =>
        {
            for (var i = 0; i < span.Length; i++)
            {
                span[i] = state;
            }
        });

        // Assert
        Assert.AreEqual(length, result.Length);
        Assert.IsTrue(result.All(c => c == '*'));
    }

    [Test]
    public void Create_WithMultipleChars_ShouldWork()
    {
        // Act
        var result = string.Create(5, ('a', 0), (span, state) =>
        {
            for (var i = 0; i < span.Length; i++)
            {
                span[i] = (char)(state.Item1 + i);
            }
        });

        // Assert
        Assert.AreEqual("abcde", result);
    }

    [Test]
    public void Create_WithCustomClass_ShouldWork()
    {
        // Arrange
        var data = new TestData { Value = "Test" };

        // Act
        var result = string.Create(4, data, (span, state) =>
        {
            state.Value.AsSpan().CopyTo(span);
        });

        // Assert
        Assert.AreEqual("Test", result);
    }

    private class TestData
    {
        public string Value { get; set; } = string.Empty;
    }
}
#endif
