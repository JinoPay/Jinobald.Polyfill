using System.Xml.Linq;
using Xunit;
using SysIO = System.IO;
using SysText = System.Text;
using SysXml = System.Xml;

namespace Jinobald.Polyfill.Tests.System.Xml.Linq;

public class XElementExTests
{
    private const string SampleXml = "<root><child>value</child></root>";

    [Fact]
    public async Task LoadAsync_FromStream_ShouldLoadElement()
    {
        // Arrange
        using var stream = new SysIO.MemoryStream(SysText.Encoding.UTF8.GetBytes(SampleXml));

        // Act
        var element = await XElement.LoadAsync(stream, LoadOptions.None, CancellationToken.None);

        // Assert
        Assert.NotNull(element);
        Assert.Equal("root", element.Name.LocalName);
    }

    [Fact]
    public async Task LoadAsync_FromTextReader_ShouldLoadElement()
    {
        // Arrange
        using var reader = new SysIO.StringReader(SampleXml);

        // Act
        var element = await XElement.LoadAsync(reader, LoadOptions.None, CancellationToken.None);

        // Assert
        Assert.NotNull(element);
        Assert.Equal("root", element.Name.LocalName);
    }

    [Fact]
    public async Task LoadAsync_FromXmlReader_ShouldLoadElement()
    {
        // Arrange
        using var stringReader = new SysIO.StringReader(SampleXml);
        var settings = new SysXml.XmlReaderSettings { Async = true };
        using var xmlReader = SysXml.XmlReader.Create(stringReader, settings);

        // Act
        var element = await XElement.LoadAsync(xmlReader, LoadOptions.None, CancellationToken.None);

        // Assert
        Assert.NotNull(element);
        Assert.Equal("root", element.Name.LocalName);
    }

    [Fact]
    public async Task LoadAsync_WithCancelledToken_ShouldThrowOperationCanceledException()
    {
        // Arrange
        using var stream = new SysIO.MemoryStream(SysText.Encoding.UTF8.GetBytes(SampleXml));
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert (TaskCanceledException inherits from OperationCanceledException)
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => XElement.LoadAsync(stream, LoadOptions.None, cts.Token));
    }

    [Fact]
    public async Task LoadAsync_ShouldLoadNestedElements()
    {
        // Arrange
        const string nestedXml = "<root><parent><child>value</child></parent></root>";
        using var stream = new SysIO.MemoryStream(SysText.Encoding.UTF8.GetBytes(nestedXml));

        // Act
        var element = await XElement.LoadAsync(stream, LoadOptions.None, CancellationToken.None);

        // Assert
        Assert.NotNull(element);
        var child = element.Element("parent")?.Element("child");
        Assert.NotNull(child);
        Assert.Equal("value", child.Value);
    }
}
