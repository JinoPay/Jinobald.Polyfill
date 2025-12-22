using System.Runtime.CompilerServices;

namespace Jinobald.Polyfill.Tests.System.Runtime.CompilerServices;

public class CallerInfoTests
{
    private string? GetCallerFilePath([CallerFilePath] string? filePath = null)
    {
        return filePath;
    }

    private int GetCallerLineNumber([CallerLineNumber] int lineNumber = 0)
    {
        return lineNumber;
    }

    private string? GetCallerMemberName([CallerMemberName] string? memberName = null)
    {
        return memberName;
    }

    [Test]
    public void CallerFilePath_Should_Be_Filled_By_Compiler()
    {
        string? result = GetCallerFilePath();
        Assert.Contains("CallerInfoTests.cs", result);
    }

    [Test]
    public void CallerInfo_Attributes_Should_Exist()
    {
        Type memberNameAttr = typeof(CallerMemberNameAttribute);
        Type filePathAttr = typeof(CallerFilePathAttribute);
        Type lineNumberAttr = typeof(CallerLineNumberAttribute);

        Assert.IsNotNull(memberNameAttr);
        Assert.IsNotNull(filePathAttr);
        Assert.IsNotNull(lineNumberAttr);

        Assert.IsTrue(memberNameAttr.IsSealed);
        Assert.IsTrue(filePathAttr.IsSealed);
        Assert.IsTrue(lineNumberAttr.IsSealed);
    }

    [Test]
    public void CallerInfo_Should_Use_Default_When_Provided()
    {
        string? memberName = GetCallerMemberName("CustomMember");
        string? filePath = GetCallerFilePath("CustomPath");
        int lineNumber = GetCallerLineNumber(999);

        Assert.AreEqual("CustomMember", memberName);
        Assert.AreEqual("CustomPath", filePath);
        Assert.AreEqual(999, lineNumber);
    }

    [Test]
    public void CallerLineNumber_Should_Be_Filled_By_Compiler()
    {
        int result = GetCallerLineNumber();
        Assert.IsTrue(result > 0);
    }

    [Test]
    public void CallerMemberName_Should_Be_Filled_By_Compiler()
    {
        string? result = GetCallerMemberName();
        Assert.AreEqual(nameof(CallerMemberName_Should_Be_Filled_By_Compiler), result);
    }
}
