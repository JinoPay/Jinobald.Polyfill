using System.Runtime.InteropServices;
using Xunit;

namespace Jinobald.Polyfill.Tests.System.Runtime.InteropServices;

public class UnmanagedCallersOnlyAttributeTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance()
    {
        // Act
        var attribute = new UnmanagedCallersOnlyAttribute();

        // Assert
        Assert.NotNull(attribute);
    }

    [Fact]
    public void EntryPoint_ShouldBeNullByDefault()
    {
        // Act
        var attribute = new UnmanagedCallersOnlyAttribute();

        // Assert
        Assert.Null(attribute.EntryPoint);
    }

    [Fact]
    public void EntryPoint_ShouldBeSettable()
    {
        // Arrange
        var attribute = new UnmanagedCallersOnlyAttribute();

        // Act
        attribute.EntryPoint = "MyEntryPoint";

        // Assert
        Assert.Equal("MyEntryPoint", attribute.EntryPoint);
    }

    [Fact]
    public void CallConvs_ShouldBeNullByDefault()
    {
        // Act
        var attribute = new UnmanagedCallersOnlyAttribute();

        // Assert
        Assert.Null(attribute.CallConvs);
    }

    [Fact]
    public void CallConvs_ShouldBeSettable()
    {
        // Arrange
        var attribute = new UnmanagedCallersOnlyAttribute();
        var callConvs = new[] { typeof(object) };

        // Act
        attribute.CallConvs = callConvs;

        // Assert
        Assert.Same(callConvs, attribute.CallConvs);
    }

    [Fact]
    public void Attribute_ShouldBeApplicableToMethod()
    {
        // Arrange
        var attributeUsage = typeof(UnmanagedCallersOnlyAttribute)
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
        var attributeUsage = typeof(UnmanagedCallersOnlyAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.False(attributeUsage.Inherited);
    }

    [Fact]
    public void Attribute_ShouldOnlyBeApplicableToMethod()
    {
        // Arrange
        var attributeUsage = typeof(UnmanagedCallersOnlyAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.Equal(AttributeTargets.Method, attributeUsage.ValidOn);
    }

    [Fact]
    public void Attribute_ShouldInheritFromAttribute()
    {
        // Act
        var attribute = new UnmanagedCallersOnlyAttribute();

        // Assert
        Assert.IsAssignableFrom<Attribute>(attribute);
    }
}
