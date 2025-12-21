using System;
using Xunit;

namespace Jinobald.Polyfill.Tests.System
{
    public class HashCodeTests
    {
// 폴리필 HashCode는 NET35-NET481에서 사용 가능, 테스트는 NET45 이상에서 실행
#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481
        [Fact]
        public void HashCode_Add_SingleValue()
        {
            var hc = new global::System.HashCode();
            hc.Add(42);
            var hash = hc.GetHashCode();

            Assert.NotEqual(0, hash);
        }

        [Fact]
        public void HashCode_Add_MultipleValues()
        {
            var hc = new global::System.HashCode();
            hc.Add(1);
            hc.Add(2);
            hc.Add(3);
            var hash = hc.GetHashCode();

            Assert.NotEqual(0, hash);
        }

        [Fact]
        public void HashCode_Add_DifferentTypes()
        {
            var hc1 = new global::System.HashCode();
            hc1.Add(42);
            hc1.Add("test");
            var hash1 = hc1.GetHashCode();

            var hc2 = new global::System.HashCode();
            hc2.Add(42);
            hc2.Add("test");
            var hash2 = hc2.GetHashCode();

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void HashCode_Add_NullValue()
        {
            var hc = new global::System.HashCode();
            hc.Add<string?>(null);
            var hash = hc.GetHashCode();

            Assert.NotEqual(0, hash);
        }

        [Fact]
        public void HashCode_Add_WithComparer()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            var hc = new global::System.HashCode();
            hc.Add("Test", comparer);
            var hash = hc.GetHashCode();

            Assert.NotEqual(0, hash);
        }

        [Fact]
        public void HashCode_Combine_OneValue()
        {
            var hash = global::System.HashCode.Combine(42);

            Assert.NotEqual(0, hash);
        }

        [Fact]
        public void HashCode_Combine_TwoValues()
        {
            var hash = global::System.HashCode.Combine(1, 2);

            Assert.NotEqual(0, hash);
        }

        [Fact]
        public void HashCode_Combine_ThreeValues()
        {
            var hash = global::System.HashCode.Combine(1, 2, 3);

            Assert.NotEqual(0, hash);
        }

        [Fact]
        public void HashCode_Combine_FourValues()
        {
            var hash = global::System.HashCode.Combine(1, 2, 3, 4);

            Assert.NotEqual(0, hash);
        }

        [Fact]
        public void HashCode_Combine_FiveValues()
        {
            var hash = global::System.HashCode.Combine(1, 2, 3, 4, 5);

            Assert.NotEqual(0, hash);
        }

        [Fact]
        public void HashCode_Combine_SixValues()
        {
            var hash = global::System.HashCode.Combine(1, 2, 3, 4, 5, 6);

            Assert.NotEqual(0, hash);
        }

        [Fact]
        public void HashCode_Combine_SevenValues()
        {
            var hash = global::System.HashCode.Combine(1, 2, 3, 4, 5, 6, 7);

            Assert.NotEqual(0, hash);
        }

        [Fact]
        public void HashCode_Combine_EightValues()
        {
            var hash = global::System.HashCode.Combine(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.NotEqual(0, hash);
        }

        [Fact]
        public void HashCode_Combine_SameValues_SameHash()
        {
            var hash1 = global::System.HashCode.Combine(1, 2, 3);
            var hash2 = global::System.HashCode.Combine(1, 2, 3);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void HashCode_Combine_DifferentValues_DifferentHash()
        {
            var hash1 = global::System.HashCode.Combine(1, 2, 3);
            var hash2 = global::System.HashCode.Combine(1, 2, 4);

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void HashCode_ToHashCode_ReturnsSameAsGetHashCode()
        {
            var hc = new global::System.HashCode();
            hc.Add(42);
            hc.Add("test");

            var hash1 = hc.GetHashCode();
            var hash2 = hc.ToHashCode();

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void HashCode_Consistency_MultipleValues()
        {
            var values = new[] { 1, 2, 3, 4, 5 };
            var hash1 = global::System.HashCode.Combine(1, 2, 3, 4, 5);

            var hc = new global::System.HashCode();
            foreach (var value in values)
            {
                hc.Add(value);
            }
            var hash2 = hc.GetHashCode();

            // Different values should produce different hashes
            Assert.NotEqual(hash1, hash2);
        }
#endif
    }
}
