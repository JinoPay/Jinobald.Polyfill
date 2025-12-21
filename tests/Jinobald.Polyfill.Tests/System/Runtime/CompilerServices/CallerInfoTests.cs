using System.Runtime.CompilerServices;
using Xunit;

namespace Jinobald.Polyfill.Tests.System.Runtime.CompilerServices
{
    public class CallerInfoTests
    {
#if NET40 || NET35 || NET20
        [Fact]
        public void CallerMemberName_Should_Be_Filled_By_Compiler()
        {
            var result = GetCallerMemberName();
            Assert.Equal(nameof(CallerMemberName_Should_Be_Filled_By_Compiler), result);
        }

        [Fact]
        public void CallerFilePath_Should_Be_Filled_By_Compiler()
        {
            var result = GetCallerFilePath();
            Assert.Contains("CallerInfoTests.cs", result);
        }

        [Fact]
        public void CallerLineNumber_Should_Be_Filled_By_Compiler()
        {
            var result = GetCallerLineNumber();
            Assert.True(result > 0);
        }

        [Fact]
        public void CallerInfo_Should_Use_Default_When_Provided()
        {
            var memberName = GetCallerMemberName("CustomMember");
            var filePath = GetCallerFilePath("CustomPath");
            var lineNumber = GetCallerLineNumber(999);

            Assert.Equal("CustomMember", memberName);
            Assert.Equal("CustomPath", filePath);
            Assert.Equal(999, lineNumber);
        }

        private string GetCallerMemberName([CallerMemberName] string memberName = null)
        {
            return memberName;
        }

        private string GetCallerFilePath([CallerFilePath] string filePath = null)
        {
            return filePath;
        }

        private int GetCallerLineNumber([CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }
#endif

        [Fact]
        public void CallerInfo_Attributes_Should_Exist()
        {
#if NET40 || NET35 || NET20
            var memberNameAttr = typeof(CallerMemberNameAttribute);
            var filePathAttr = typeof(CallerFilePathAttribute);
            var lineNumberAttr = typeof(CallerLineNumberAttribute);

            Assert.NotNull(memberNameAttr);
            Assert.NotNull(filePathAttr);
            Assert.NotNull(lineNumberAttr);

            Assert.True(memberNameAttr.IsSealed);
            Assert.True(filePathAttr.IsSealed);
            Assert.True(lineNumberAttr.IsSealed);
#else
            Assert.True(true); // Attributes are built-in in newer frameworks
#endif
        }
    }
}
