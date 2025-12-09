using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Jinobald.Polyfill.Tests.System.Diagnostics.CodeAnalysis;

public class UnscopedRefAttributeTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance()
    {
        // Act
        var attribute = new UnscopedRefAttribute();

        // Assert
        Assert.NotNull(attribute);
    }

    [Fact]
    public void Attribute_ShouldBeApplicableToMethod()
    {
        // Arrange
        var attributeUsage = typeof(UnscopedRefAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.True((attributeUsage.ValidOn & AttributeTargets.Method) != 0);
    }

    [Fact]
    public void Attribute_ShouldBeApplicableToProperty()
    {
        // Arrange
        var attributeUsage = typeof(UnscopedRefAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.True((attributeUsage.ValidOn & AttributeTargets.Property) != 0);
    }

    [Fact]
    public void Attribute_ShouldBeApplicableToParameter()
    {
        // Arrange
        var attributeUsage = typeof(UnscopedRefAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.True((attributeUsage.ValidOn & AttributeTargets.Parameter) != 0);
    }

    [Fact]
    public void Attribute_ShouldNotBeInherited()
    {
        // Arrange
        var attributeUsage = typeof(UnscopedRefAttribute)
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
        var attribute = new UnscopedRefAttribute();

        // Assert
        Assert.IsAssignableFrom<Attribute>(attribute);
    }
}
