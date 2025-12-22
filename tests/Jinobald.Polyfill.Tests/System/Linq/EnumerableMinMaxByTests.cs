// Jinobald.Polyfill - MinBy, MaxBy 메서드 테스트 (.NET 6.0+)

#if NET20
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jinobald.Polyfill.Tests.System.Linq
{
    [TestFixture]
    public class EnumerableMinMaxByTests
    {
        #region MinBy Tests

        [Test]
        public void MinBy_최소_키를_가진_요소_반환()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 30 },
                new Person { Name = "Bob", Age = 25 },
                new Person { Name = "Charlie", Age = 35 }
            };

            // Act
            var youngest = people.MinBy(p => p.Age);

            // Assert
            Assert.IsNotNull(youngest);
            Assert.AreEqual("Bob", youngest.Name);
            Assert.AreEqual(25, youngest.Age);
        }

        [Test]
        public void MinBy_단일_요소_해당_요소_반환()
        {
            // Arrange
            var people = new[] { new Person { Name = "Alice", Age = 25 } };

            // Act
            var result = people.MinBy(p => p.Age);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Alice", result.Name);
        }

        [Test]
        public void MinBy_동일_키_첫번째_요소_반환()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 25 },
                new Person { Name = "Charlie", Age = 30 }
            };

            // Act
            var result = people.MinBy(p => p.Age);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Alice", result.Name);
        }

        [Test]
        public void MinBy_빈_시퀀스_참조타입_null_반환()
        {
            // Arrange
            var empty = new Person[0];

            // Act
            var result = empty.MinBy(p => p.Age);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void MinBy_빈_시퀀스_값타입_InvalidOperationException()
        {
            // Arrange
            var empty = new int[0];

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => empty.MinBy(x => x));
        }

        [Test]
        public void MinBy_Null_소스_ArgumentNullException()
        {
            // Arrange
            Person[] people = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => people.MinBy(p => p.Age));
        }

        [Test]
        public void MinBy_사용자_정의_비교자()
        {
            // Arrange
            var words = new[] { "apple", "BANANA", "cherry" };

            // Act - 길이가 아닌 역순 비교 (최대값이 결과)
            var result = words.MinBy(
                w => w,
                Comparer<string>.Create((a, b) => StringComparer.OrdinalIgnoreCase.Compare(b, a)));

            // Assert
            Assert.AreEqual("cherry", result);
        }

        [Test]
        public void MinBy_문자열_키로_정렬()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Charlie", Age = 30 },
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 35 }
            };

            // Act
            var result = people.MinBy(p => p.Name);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Alice", result.Name);
        }

        #endregion

        #region MaxBy Tests

        [Test]
        public void MaxBy_최대_키를_가진_요소_반환()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 30 },
                new Person { Name = "Bob", Age = 25 },
                new Person { Name = "Charlie", Age = 35 }
            };

            // Act
            var oldest = people.MaxBy(p => p.Age);

            // Assert
            Assert.IsNotNull(oldest);
            Assert.AreEqual("Charlie", oldest.Name);
            Assert.AreEqual(35, oldest.Age);
        }

        [Test]
        public void MaxBy_단일_요소_해당_요소_반환()
        {
            // Arrange
            var people = new[] { new Person { Name = "Alice", Age = 25 } };

            // Act
            var result = people.MaxBy(p => p.Age);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Alice", result.Name);
        }

        [Test]
        public void MaxBy_동일_키_첫번째_요소_반환()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 35 },
                new Person { Name = "Bob", Age = 35 },
                new Person { Name = "Charlie", Age = 25 }
            };

            // Act
            var result = people.MaxBy(p => p.Age);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Alice", result.Name);
        }

        [Test]
        public void MaxBy_빈_시퀀스_참조타입_null_반환()
        {
            // Arrange
            var empty = new Person[0];

            // Act
            var result = empty.MaxBy(p => p.Age);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void MaxBy_빈_시퀀스_값타입_InvalidOperationException()
        {
            // Arrange
            var empty = new int[0];

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => empty.MaxBy(x => x));
        }

        [Test]
        public void MaxBy_Null_소스_ArgumentNullException()
        {
            // Arrange
            Person[] people = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => people.MaxBy(p => p.Age));
        }

        [Test]
        public void MaxBy_문자열_키로_정렬()
        {
            // Arrange
            var people = new[]
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Charlie", Age = 30 },
                new Person { Name = "Bob", Age = 35 }
            };

            // Act
            var result = people.MaxBy(p => p.Name);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Charlie", result.Name);
        }

        [Test]
        public void MinBy_MaxBy_음수_포함()
        {
            // Arrange
            var numbers = new[] { -5, 3, -10, 7, 2 };

            // Act
            var min = numbers.MinBy(x => x);
            var max = numbers.MaxBy(x => x);

            // Assert
            Assert.AreEqual(-10, min);
            Assert.AreEqual(7, max);
        }

        #endregion

        #region Helper Classes

        private class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }

        #endregion
    }
}

#endif
