using System.Diagnostics;
using Xunit;

namespace Jinobald.Polyfill.Tests.System.Diagnostics;

public class UnreachableExceptionTests
{
    [Fact]
    public void Constructor_Default_ShouldSetDefaultMessage()
    {
        // Act
        var exception = new UnreachableException();

        // Assert
        Assert.NotNull(exception.Message);
        Assert.Contains("unreachable", exception.Message.ToLowerInvariant());
    }

    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "Custom unreachable message";

        // Act
        var exception = new UnreachableException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithNullMessage_ShouldAcceptNull()
    {
        // Act
        var exception = new UnreachableException(null);

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldSetBoth()
    {
        // Arrange
        var message = "Outer message";
        var innerException = new InvalidOperationException("Inner message");

        // Act
        var exception = new UnreachableException(message, innerException);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }

    [Fact]
    public void Constructor_WithNullInnerException_ShouldAcceptNull()
    {
        // Act
        var exception = new UnreachableException("message", null);

        // Assert
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Exception_ShouldBeThrowable()
    {
        // Act & Assert
        var thrown = Assert.Throws<UnreachableException>(() =>
        {
            throw new UnreachableException("Test throw");
        });

        Assert.Equal("Test throw", thrown.Message);
    }

    [Fact]
    public void Exception_ShouldInheritFromException()
    {
        // Act
        var exception = new UnreachableException();

        // Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void Exception_ShouldBeCatchableAsException()
    {
        // Arrange
        Exception? caught = null;

        // Act
        try
        {
            throw new UnreachableException("test");
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        // Assert
        Assert.NotNull(caught);
        Assert.IsType<UnreachableException>(caught);
    }
}
