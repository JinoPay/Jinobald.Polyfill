using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System.Runtime.CompilerServices
{
    public class CallerInfoTests
    {
#if NET40 || NET35 || NET20
        [Test]
        public void CallerMemberName_Should_Be_Filled_By_Compiler()
        {
            var result = GetCallerMemberName();
            Assert.AreEqual(nameof(CallerMemberName_Should_Be_Filled_By_Compiler), result);
        }

        [Test]
        public void CallerFilePath_Should_Be_Filled_By_Compiler()
        {
            var result = GetCallerFilePath();
            Assert.Contains("CallerInfoTests.cs", result);
        }

        [Test]
        public void CallerLineNumber_Should_Be_Filled_By_Compiler()
        {
            var result = GetCallerLineNumber();
            Assert.IsTrue(result > 0);
        }

        [Test]
        public void CallerInfo_Should_Use_Default_When_Provided()
        {
            var memberName = GetCallerMemberName("CustomMember");
            var filePath = GetCallerFilePath("CustomPath");
            var lineNumber = GetCallerLineNumber(999);

            Assert.AreEqual("CustomMember", memberName);
            Assert.AreEqual("CustomPath", filePath);
            Assert.AreEqual(999, lineNumber);
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

        [Test]
        public void CallerInfo_Attributes_Should_Exist()
        {
#if NET40 || NET35 || NET20
            var memberNameAttr = typeof(CallerMemberNameAttribute);
            var filePathAttr = typeof(CallerFilePathAttribute);
            var lineNumberAttr = typeof(CallerLineNumberAttribute);

            Assert.IsNotNull(memberNameAttr);
            Assert.IsNotNull(filePathAttr);
            Assert.IsNotNull(lineNumberAttr);

            Assert.IsTrue(memberNameAttr.IsSealed);
            Assert.IsTrue(filePathAttr.IsSealed);
            Assert.IsTrue(lineNumberAttr.IsSealed);
#else
            Assert.IsTrue(true); // Attributes are built-in in newer frameworks
#endif
        }
    }
}
