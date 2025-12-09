using System.Runtime.InteropServices;
using Xunit;

namespace Jinobald.Polyfill.Tests.System.Runtime.InteropServices;

public class SuppressGCTransitionAttributeTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance()
    {
        // Act
        var attribute = new SuppressGCTransitionAttribute();

        // Assert
        Assert.NotNull(attribute);
    }

    [Fact]
    public void Attribute_ShouldBeApplicableToMethod()
    {
        // Arrange
        var attributeUsage = typeof(SuppressGCTransitionAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.True((attributeUsage.ValidOn & AttributeTargets.Method) != 0);
    }

    [Fact]
    public void Attribute_ShouldNotBeInherited()
    {
        // Arrange
        var attributeUsage = typeof(SuppressGCTransitionAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.False(attributeUsage.Inherited);
    }

    [Fact]
    public void Attribute_ShouldInheritFromAttribute()
    {
        // Act
        var attribute = new SuppressGCTransitionAttribute();

        // Assert
        Assert.IsAssignableFrom<Attribute>(attribute);
    }

    [Fact]
    public void Attribute_ShouldOnlyBeApplicableToMethod()
    {
        // Arrange
        var attributeUsage = typeof(SuppressGCTransitionAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.Equal(AttributeTargets.Method, attributeUsage.ValidOn);
    }
}
