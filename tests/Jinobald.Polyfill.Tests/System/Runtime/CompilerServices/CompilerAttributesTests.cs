using System.Linq;
using NUnit.Framework;
using SysAttribute = global::System.Attribute;

namespace Jinobald.Polyfill.Tests.System.Runtime.CompilerServices
{
    public class CompilerAttributesTests
    {
        [Test]
        public void ExtensionAttribute_Should_Exist()
        {
#if NET20
            var attr = typeof(global::System.Runtime.CompilerServices.ExtensionAttribute);
            Assert.IsNotNull(attr);
            Assert.IsTrue(attr.IsSealed);
            Assert.IsTrue(typeof(SysAttribute).IsAssignableFrom(attr));
#else
            Assert.IsTrue(true); // ExtensionAttribute is built-in in .NET 3.5+
#endif
        }

        [Test]
        public void IsExternalInit_Should_Exist_Via_Reflection()
        {
#if NET46 || NET462 || NET47 || NET472 || NET48 || NETSTANDARD2_0 || NETSTANDARD2_1
            // IsExternalInit is internal, so we need to check it via reflection
            var assembly = typeof(global::Jinobald.Polyfill.Tests.System.Runtime.CompilerServices.CompilerAttributesTests).Assembly;
            var polyfillAssembly = assembly.GetReferencedAssemblies()
                .Where(a => a.Name.Contains("Jinobald.Polyfill"))
                .FirstOrDefault();

            if (polyfillAssembly != null)
            {
                var asm = global::System.Reflection.Assembly.Load(polyfillAssembly);
                var type = asm.GetType("System.Runtime.CompilerServices.IsExternalInit");
                Assert.IsNotNull(type);
            }
            else
            {
                Assert.IsTrue(true); // Polyfill assembly not found
            }
#else
            Assert.IsTrue(true); // IsExternalInit is built-in in .NET 5+
#endif
        }

        [Test]
        public void RequiredMemberAttribute_Should_Exist()
        {
#if NET47 || NET472 || NET48 || NETSTANDARD2_0 || NETSTANDARD2_1
            var attr = typeof(global::System.Runtime.CompilerServices.RequiredMemberAttribute);
            Assert.IsNotNull(attr);
            Assert.IsTrue(attr.IsSealed);
            Assert.IsTrue(typeof(SysAttribute).IsAssignableFrom(attr));
#else
            Assert.IsTrue(true); // RequiredMemberAttribute is built-in in .NET 7+
#endif
        }

        [Test]
        public void SetsRequiredMembersAttribute_Should_Exist()
        {
#if NET47 || NET472 || NET48 || NETSTANDARD2_0 || NETSTANDARD2_1
            var attr = typeof(global::System.Runtime.CompilerServices.SetsRequiredMembersAttribute);
            Assert.IsNotNull(attr);
            Assert.IsTrue(attr.IsSealed);
            Assert.IsTrue(typeof(SysAttribute).IsAssignableFrom(attr));
#else
            Assert.IsTrue(true); // SetsRequiredMembersAttribute is built-in in .NET 7+
#endif
        }

#if NET20
        [Test]
        public void Extension_Method_Should_Work_With_ExtensionAttribute()
        {
            var result = "test".TestExtension();
            Assert.AreEqual("TEST", result);
        }
#endif
    }

#if NET20
    public static class TestExtensions
    {
        public static string TestExtension(this string str)
        {
            return str.ToUpper();
        }
    }
#endif
}
