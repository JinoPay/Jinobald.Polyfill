using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Jinobald.Polyfill.Tests.System.Diagnostics.CodeAnalysis;

public class StringSyntaxAttributeTests
{
    [Fact]
    public void Constructor_WithSyntax_ShouldSetSyntax()
    {
        // Arrange & Act
        var attribute = new StringSyntaxAttribute(StringSyntaxAttribute.Regex);

        // Assert
        Assert.Equal(StringSyntaxAttribute.Regex, attribute.Syntax);
        Assert.Empty(attribute.Arguments);
    }

    [Fact]
    public void Constructor_WithSyntaxAndArguments_ShouldSetBoth()
    {
        // Arrange
        var args = new object[] { "arg1", 123 };

        // Act
        var attribute = new StringSyntaxAttribute(StringSyntaxAttribute.CompositeFormat, args);

        // Assert
        Assert.Equal(StringSyntaxAttribute.CompositeFormat, attribute.Syntax);
        Assert.Equal(2, attribute.Arguments.Length);
        Assert.Equal("arg1", attribute.Arguments[0]);
        Assert.Equal(123, attribute.Arguments[1]);
    }

    [Fact]
    public void SyntaxConstants_ShouldHaveCorrectValues()
    {
        // Assert
        Assert.Equal("CompositeFormat", StringSyntaxAttribute.CompositeFormat);
        Assert.Equal("DateOnlyFormat", StringSyntaxAttribute.DateOnlyFormat);
        Assert.Equal("DateTimeFormat", StringSyntaxAttribute.DateTimeFormat);
        Assert.Equal("EnumFormat", StringSyntaxAttribute.EnumFormat);
        Assert.Equal("GuidFormat", StringSyntaxAttribute.GuidFormat);
        Assert.Equal("Json", StringSyntaxAttribute.Json);
        Assert.Equal("NumericFormat", StringSyntaxAttribute.NumericFormat);
        Assert.Equal("Regex", StringSyntaxAttribute.Regex);
        Assert.Equal("TimeOnlyFormat", StringSyntaxAttribute.TimeOnlyFormat);
        Assert.Equal("TimeSpanFormat", StringSyntaxAttribute.TimeSpanFormat);
        Assert.Equal("Uri", StringSyntaxAttribute.Uri);
        Assert.Equal("Xml", StringSyntaxAttribute.Xml);
    }

    [Fact]
    public void Attribute_ShouldBeApplicableToParameter()
    {
        // Arrange
        var attributeUsage = typeof(StringSyntaxAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.True((attributeUsage.ValidOn & AttributeTargets.Parameter) != 0);
    }

    [Fact]
    public void Attribute_ShouldBeApplicableToField()
    {
        // Arrange
        var attributeUsage = typeof(StringSyntaxAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.True((attributeUsage.ValidOn & AttributeTargets.Field) != 0);
    }

    [Fact]
    public void Attribute_ShouldBeApplicableToProperty()
    {
        // Arrange
        var attributeUsage = typeof(StringSyntaxAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.True((attributeUsage.ValidOn & AttributeTargets.Property) != 0);
    }

    [Fact]
    public void Constructor_WithNullArguments_ShouldWork()
    {
        // Act
        var attribute = new StringSyntaxAttribute(StringSyntaxAttribute.Json, null!, null!);

        // Assert
        Assert.Equal(2, attribute.Arguments.Length);
    }
}
