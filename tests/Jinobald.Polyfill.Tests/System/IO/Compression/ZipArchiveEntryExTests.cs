using System.IO.Compression;
using Xunit;

namespace Jinobald.Polyfill.Tests.System.IO.Compression;

public class ZipArchiveEntryExTests
{
    [Fact]
    public void ExternalAttributes_Get_ShouldReturnValue()
    {
        // Arrange
        using var memoryStream = new MemoryStream();
        using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);
        var entry = archive.CreateEntry("test.txt");

        // Act
        var attributes = entry.ExternalAttributes;

        // Assert
        Assert.True(attributes >= 0);
    }

    [Fact]
    public void ExternalAttributes_Set_ShouldNotThrow()
    {
        // Arrange
        using var memoryStream = new MemoryStream();
        using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);
        var entry = archive.CreateEntry("test.txt");

        // Act & Assert
        var exception = Record.Exception(() => entry.ExternalAttributes = 0x41A40000);
        Assert.Null(exception);
    }

    [Fact]
    public void ExternalAttributes_SetAndGet_ShouldWork()
    {
        // Arrange
        using var memoryStream = new MemoryStream();
        using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);
        var entry = archive.CreateEntry("test.txt");
        const int expectedAttributes = 0x41A40000;

        // Act
        entry.ExternalAttributes = expectedAttributes;
        var actualAttributes = entry.ExternalAttributes;

        // Assert
        // In .NET Framework 4.6.2, the polyfill returns 0 (no-op implementation)
        // In .NET Framework 4.7.2+ and .NET Core/.NET 5+, native implementation is used
#if NET462
        Assert.Equal(0, actualAttributes);
#else
        Assert.Equal(expectedAttributes, actualAttributes);
#endif
    }

    [Fact]
    public void CreateEntry_WithExternalAttributes_ShouldWork()
    {
        // Arrange & Act
        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            var entry = archive.CreateEntry("folder/test.txt");
            entry.ExternalAttributes = 0x41A40000;

            using var entryStream = entry.Open();
            using var writer = new StreamWriter(entryStream);
            writer.Write("test content");
        }

        // Assert
        memoryStream.Position = 0;
        using var readArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read);
        var readEntry = readArchive.GetEntry("folder/test.txt");
        Assert.NotNull(readEntry);
    }
}
