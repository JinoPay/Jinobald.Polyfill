using System.IO;
using Xunit;

namespace Jinobald.Polyfill.Tests.System.IO;

public class UnixFileModeTests
{
    [Fact]
    public void None_ShouldBeZero()
    {
        Assert.Equal(0, (int)UnixFileMode.None);
    }

    [Fact]
    public void OtherExecute_ShouldBe1()
    {
        Assert.Equal(1, (int)UnixFileMode.OtherExecute);
    }

    [Fact]
    public void OtherWrite_ShouldBe2()
    {
        Assert.Equal(2, (int)UnixFileMode.OtherWrite);
    }

    [Fact]
    public void OtherRead_ShouldBe4()
    {
        Assert.Equal(4, (int)UnixFileMode.OtherRead);
    }

    [Fact]
    public void GroupExecute_ShouldBe8()
    {
        Assert.Equal(8, (int)UnixFileMode.GroupExecute);
    }

    [Fact]
    public void GroupWrite_ShouldBe16()
    {
        Assert.Equal(16, (int)UnixFileMode.GroupWrite);
    }

    [Fact]
    public void GroupRead_ShouldBe32()
    {
        Assert.Equal(32, (int)UnixFileMode.GroupRead);
    }

    [Fact]
    public void UserExecute_ShouldBe64()
    {
        Assert.Equal(64, (int)UnixFileMode.UserExecute);
    }

    [Fact]
    public void UserWrite_ShouldBe128()
    {
        Assert.Equal(128, (int)UnixFileMode.UserWrite);
    }

    [Fact]
    public void UserRead_ShouldBe256()
    {
        Assert.Equal(256, (int)UnixFileMode.UserRead);
    }

    [Fact]
    public void StickyBit_ShouldBe512()
    {
        Assert.Equal(512, (int)UnixFileMode.StickyBit);
    }

    [Fact]
    public void SetGroup_ShouldBe1024()
    {
        Assert.Equal(1024, (int)UnixFileMode.SetGroup);
    }

    [Fact]
    public void SetUser_ShouldBe2048()
    {
        Assert.Equal(2048, (int)UnixFileMode.SetUser);
    }

    [Fact]
    public void Enum_ShouldHaveFlagsAttribute()
    {
        // Arrange
        var flagsAttribute = typeof(UnixFileMode)
            .GetCustomAttributes(typeof(FlagsAttribute), false)
            .FirstOrDefault();

        // Assert
        Assert.NotNull(flagsAttribute);
    }

    [Fact]
    public void Flags_ShouldBeCombineable()
    {
        // Arrange
        var rwxUser = UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute;

        // Assert
        Assert.Equal(448, (int)rwxUser); // 256 + 128 + 64 = 448
    }

    [Fact]
    public void Flags_ShouldSupportHasFlag()
    {
        // Arrange
        var mode = UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.GroupRead;

        // Assert
        Assert.True(mode.HasFlag(UnixFileMode.UserRead));
        Assert.True(mode.HasFlag(UnixFileMode.UserWrite));
        Assert.True(mode.HasFlag(UnixFileMode.GroupRead));
        Assert.False(mode.HasFlag(UnixFileMode.UserExecute));
        Assert.False(mode.HasFlag(UnixFileMode.OtherRead));
    }

    [Fact]
    public void StandardPermissions_ShouldCalculateCorrectly()
    {
        // 755 permission: rwxr-xr-x
        var mode755 = UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
                      UnixFileMode.GroupRead | UnixFileMode.GroupExecute |
                      UnixFileMode.OtherRead | UnixFileMode.OtherExecute;

        // 256 + 128 + 64 + 32 + 8 + 4 + 1 = 493
        Assert.Equal(493, (int)mode755);
    }

    [Fact]
    public void StandardPermissions644_ShouldCalculateCorrectly()
    {
        // 644 permission: rw-r--r--
        var mode644 = UnixFileMode.UserRead | UnixFileMode.UserWrite |
                      UnixFileMode.GroupRead |
                      UnixFileMode.OtherRead;

        // 256 + 128 + 32 + 4 = 420
        Assert.Equal(420, (int)mode644);
    }
}
