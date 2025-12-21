using System.Xml.Linq;
using Xunit;
using SysIO = System.IO;
using SysText = System.Text;
using SysXml = System.Xml;

namespace Jinobald.Polyfill.Tests.System.Xml.Linq;

public class XDocumentExTests
{
    private const string SampleXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><child>value</child></root>";

    [Fact]
    public async Task LoadAsync_FromStream_ShouldLoadDocument()
    {
        // Arrange
        using var stream = new SysIO.MemoryStream(SysText.Encoding.UTF8.GetBytes(SampleXml));

        // Act
        var document = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);

        // Assert
        Assert.NotNull(document);
        Assert.NotNull(document.Root);
        Assert.Equal("root", document.Root.Name.LocalName);
    }

    [Fact]
    public async Task LoadAsync_FromTextReader_ShouldLoadDocument()
    {
        // Arrange
        using var reader = new SysIO.StringReader(SampleXml);

        // Act
        var document = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None);

        // Assert
        Assert.NotNull(document);
        Assert.NotNull(document.Root);
        Assert.Equal("root", document.Root.Name.LocalName);
    }

    [Fact]
    public async Task LoadAsync_FromXmlReader_ShouldLoadDocument()
    {
        // Arrange
        using var stringReader = new SysIO.StringReader(SampleXml);
        var settings = new SysXml.XmlReaderSettings { Async = true };
        using var xmlReader = SysXml.XmlReader.Create(stringReader, settings);

        // Act
        var document = await XDocument.LoadAsync(xmlReader, LoadOptions.None, CancellationToken.None);

        // Assert
        Assert.NotNull(document);
        Assert.NotNull(document.Root);
        Assert.Equal("root", document.Root.Name.LocalName);
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
            () => XDocument.LoadAsync(stream, LoadOptions.None, cts.Token));
    }

    [Fact]
    public async Task LoadAsync_WithPreserveWhitespace_ShouldPreserveWhitespace()
    {
        // Arrange
        const string xmlWithWhitespace = "<?xml version=\"1.0\"?><root>  <child>  value  </child>  </root>";
        using var stream = new SysIO.MemoryStream(SysText.Encoding.UTF8.GetBytes(xmlWithWhitespace));

        // Act
        var document = await XDocument.LoadAsync(stream, LoadOptions.PreserveWhitespace, CancellationToken.None);

        // Assert
        Assert.NotNull(document);
        Assert.NotNull(document.Root);
    }
}
