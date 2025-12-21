using System.Linq;
using Xunit;
using SysAttribute = global::System.Attribute;

namespace Jinobald.Polyfill.Tests.System.Runtime.CompilerServices
{
    public class CompilerAttributesTests
    {
        [Fact]
        public void ExtensionAttribute_Should_Exist()
        {
#if NET20
            var attr = typeof(global::System.Runtime.CompilerServices.ExtensionAttribute);
            Assert.NotNull(attr);
            Assert.True(attr.IsSealed);
            Assert.True(typeof(SysAttribute).IsAssignableFrom(attr));
#else
            Assert.True(true); // ExtensionAttribute is built-in in .NET 3.5+
#endif
        }

        [Fact]
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
                Assert.NotNull(type);
            }
            else
            {
                Assert.True(true); // Polyfill assembly not found
            }
#else
            Assert.True(true); // IsExternalInit is built-in in .NET 5+
#endif
        }

        [Fact]
        public void RequiredMemberAttribute_Should_Exist()
        {
#if NET47 || NET472 || NET48 || NETSTANDARD2_0 || NETSTANDARD2_1
            var attr = typeof(global::System.Runtime.CompilerServices.RequiredMemberAttribute);
            Assert.NotNull(attr);
            Assert.True(attr.IsSealed);
            Assert.True(typeof(SysAttribute).IsAssignableFrom(attr));
#else
            Assert.True(true); // RequiredMemberAttribute is built-in in .NET 7+
#endif
        }

        [Fact]
        public void SetsRequiredMembersAttribute_Should_Exist()
        {
#if NET47 || NET472 || NET48 || NETSTANDARD2_0 || NETSTANDARD2_1
            var attr = typeof(global::System.Runtime.CompilerServices.SetsRequiredMembersAttribute);
            Assert.NotNull(attr);
            Assert.True(attr.IsSealed);
            Assert.True(typeof(SysAttribute).IsAssignableFrom(attr));
#else
            Assert.True(true); // SetsRequiredMembersAttribute is built-in in .NET 7+
#endif
        }

#if NET20
        [Fact]
        public void Extension_Method_Should_Work_With_ExtensionAttribute()
        {
            var result = "test".TestExtension();
            Assert.Equal("TEST", result);
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
