// Jinobald.Polyfill - 키 기반 LINQ 메서드 테스트 (.NET 6.0+)
// DistinctBy, ExceptBy, IntersectBy, UnionBy 메서드 테스트

#if NET20
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jinobald.Polyfill.Tests.System.Linq
{
    [TestFixture]
    public class EnumerableByKeyTests
    {
        #region DistinctBy Tests

        [Test]
        public void DistinctBy_키로_고유_요소_반환()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 30 },
                new Person { Name = "Charlie", Age = 25 },
                new Person { Name = "David", Age = 35 }
            };

            // Act
            var distinctByAge = people.DistinctBy(p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(3, distinctByAge.Length);
            Assert.AreEqual("Alice", distinctByAge[0].Name); // 첫 번째 Age=25
            Assert.AreEqual("Bob", distinctByAge[1].Name);
            Assert.AreEqual("David", distinctByAge[2].Name);
        }

        [Test]
        public void DistinctBy_빈_시퀀스_빈_결과_반환()
        {
            // Arrange
            var empty = new Person[0];

            // Act
            var result = empty.DistinctBy(p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void DistinctBy_사용자_정의_비교자_사용()
        {
            // Arrange
            var words = new[] { "Apple", "apple", "BANANA", "banana", "Cherry" };

            // Act
            var distinct = words.DistinctBy(
                w => w,
                StringComparer.OrdinalIgnoreCase).ToArray();

            // Assert
            Assert.AreEqual(3, distinct.Length);
            Assert.AreEqual("Apple", distinct[0]);
            Assert.AreEqual("BANANA", distinct[1]);
            Assert.AreEqual("Cherry", distinct[2]);
        }

        [Test]
        public void DistinctBy_Null_소스_ArgumentNullException_발생()
        {
            // Arrange
            Person[] people = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => people.DistinctBy(p => p.Age).ToArray());
        }

        [Test]
        public void DistinctBy_Null_키선택자_ArgumentNullException_발생()
        {
            // Arrange
            var people = new[] { new Person { Name = "Alice", Age = 25 } };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                people.DistinctBy<Person, int>(null).ToArray());
        }

        [Test]
        public void DistinctBy_지연_실행()
        {
            // Arrange
            bool executed = false;
            IEnumerable<int> source = GetSourceWithSideEffect(() => executed = true);

            // Act
            var distinct = source.DistinctBy(x => x % 2);

            // Assert - 아직 실행되지 않음
            Assert.IsFalse(executed);

            // 실제 열거
            distinct.ToArray();
            Assert.IsTrue(executed);
        }

        #endregion

        #region ExceptBy Tests

        [Test]
        public void ExceptBy_차집합_반환()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 30 },
                new Person { Name = "Charlie", Age = 35 }
            };
            var excludeAges = new[] { 30, 35 };

            // Act
            var result = people.ExceptBy(excludeAges, p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("Alice", result[0].Name);
        }

        [Test]
        public void ExceptBy_모든_요소_제외시_빈_결과()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 30 }
            };
            var excludeAges = new[] { 25, 30, 35 };

            // Act
            var result = people.ExceptBy(excludeAges, p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void ExceptBy_제외할_키가_비어있으면_원본_반환()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 30 }
            };
            var excludeAges = new int[0];

            // Act
            var result = people.ExceptBy(excludeAges, p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public void ExceptBy_중복_요소_한번만_반환()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 25 }, // 같은 Age
                new Person { Name = "Charlie", Age = 30 }
            };
            var excludeAges = new[] { 30 };

            // Act
            var result = people.ExceptBy(excludeAges, p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(1, result.Length); // Age=25인 첫 번째 요소만
            Assert.AreEqual("Alice", result[0].Name);
        }

        #endregion

        #region IntersectBy Tests

        [Test]
        public void IntersectBy_교집합_반환()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 30 },
                new Person { Name = "Charlie", Age = 35 }
            };
            var includedAges = new[] { 25, 35, 40 };

            // Act
            var result = people.IntersectBy(includedAges, p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("Alice", result[0].Name);
            Assert.AreEqual("Charlie", result[1].Name);
        }

        [Test]
        public void IntersectBy_교집합이_없으면_빈_결과()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 30 }
            };
            var includedAges = new[] { 35, 40 };

            // Act
            var result = people.IntersectBy(includedAges, p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void IntersectBy_같은_키의_첫번째_요소만_반환()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 25 }, // 같은 Age
                new Person { Name = "Charlie", Age = 30 }
            };
            var includedAges = new[] { 25 };

            // Act
            var result = people.IntersectBy(includedAges, p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("Alice", result[0].Name);
        }

        #endregion

        #region UnionBy Tests

        [Test]
        public void UnionBy_합집합_반환()
        {
            // Arrange
            var first = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 30 }
            };
            var second = new[]
            {
                new Person { Name = "Charlie", Age = 30 }, // 같은 Age
                new Person { Name = "David", Age = 35 }
            };

            // Act
            var result = first.UnionBy(second, p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("Alice", result[0].Name);
            Assert.AreEqual("Bob", result[1].Name);
            Assert.AreEqual("David", result[2].Name);
        }

        [Test]
        public void UnionBy_첫번째_시퀀스가_비어있으면_두번째_반환()
        {
            // Arrange
            var first = new Person[0];
            var second = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 30 }
            };

            // Act
            var result = first.UnionBy(second, p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public void UnionBy_두번째_시퀀스가_비어있으면_첫번째_반환()
        {
            // Arrange
            var first = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 30 }
            };
            var second = new Person[0];

            // Act
            var result = first.UnionBy(second, p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public void UnionBy_완전히_중복되면_첫번째_시퀀스만()
        {
            // Arrange
            var first = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 30 }
            };
            var second = new[]
            {
                new Person { Name = "Charlie", Age = 25 },
                new Person { Name = "David", Age = 30 }
            };

            // Act
            var result = first.UnionBy(second, p => p.Age).ToArray();

            // Assert
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("Alice", result[0].Name);
            Assert.AreEqual("Bob", result[1].Name);
        }

        #endregion

        #region Helper Classes and Methods

        private class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }

        private IEnumerable<int> GetSourceWithSideEffect(Action sideEffect)
        {
            sideEffect();
            yield return 1;
            yield return 2;
            yield return 3;
        }

        #endregion
    }
}

#endif
