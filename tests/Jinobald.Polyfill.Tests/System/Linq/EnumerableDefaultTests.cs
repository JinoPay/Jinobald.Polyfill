// Jinobald.Polyfill - 기본값 지정 OrDefault 메서드 테스트 (.NET 6.0+)
// FirstOrDefault, LastOrDefault, SingleOrDefault with default value

#if NET20
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jinobald.Polyfill.Tests.System.Linq
{
    [TestFixture]
    public class EnumerableDefaultTests
    {
        #region FirstOrDefault with default value Tests

        [Test]
        public void FirstOrDefault_기본값_빈_시퀀스_기본값_반환()
        {
            // Arrange
            var empty = new int[0];

            // Act
            var result = empty.FirstOrDefault(-1);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void FirstOrDefault_기본값_요소_있으면_첫번째_반환()
        {
            // Arrange
            var numbers = new[] { 10, 20, 30 };

            // Act
            var result = numbers.FirstOrDefault(-1);

            // Assert
            Assert.AreEqual(10, result);
        }

        [Test]
        public void FirstOrDefault_조건_기본값_일치_없으면_기본값_반환()
        {
            // Arrange
            var numbers = new[] { 1, 2, 3 };

            // Act
            var result = numbers.FirstOrDefault(x => x > 10, -1);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void FirstOrDefault_조건_기본값_일치_있으면_첫번째_반환()
        {
            // Arrange
            var numbers = new[] { 1, 15, 20, 5 };

            // Act
            var result = numbers.FirstOrDefault(x => x > 10, -1);

            // Assert
            Assert.AreEqual(15, result);
        }

        [Test]
        public void FirstOrDefault_기본값_참조타입()
        {
            // Arrange
            var empty = new string[0];
            var defaultVal = "default";

            // Act
            var result = empty.FirstOrDefault(defaultVal);

            // Assert
            Assert.AreEqual("default", result);
        }

        #endregion

        #region LastOrDefault with default value Tests

        [Test]
        public void LastOrDefault_기본값_빈_시퀀스_기본값_반환()
        {
            // Arrange
            var empty = new int[0];

            // Act
            var result = empty.LastOrDefault(-1);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void LastOrDefault_기본값_요소_있으면_마지막_반환()
        {
            // Arrange
            var numbers = new[] { 10, 20, 30 };

            // Act
            var result = numbers.LastOrDefault(-1);

            // Assert
            Assert.AreEqual(30, result);
        }

        [Test]
        public void LastOrDefault_조건_기본값_일치_없으면_기본값_반환()
        {
            // Arrange
            var numbers = new[] { 1, 2, 3 };

            // Act
            var result = numbers.LastOrDefault(x => x > 10, -1);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void LastOrDefault_조건_기본값_일치_있으면_마지막_반환()
        {
            // Arrange
            var numbers = new[] { 1, 15, 20, 5 };

            // Act
            var result = numbers.LastOrDefault(x => x > 10, -1);

            // Assert
            Assert.AreEqual(20, result);
        }

        [Test]
        public void LastOrDefault_리스트가_아닌_IEnumerable()
        {
            // Arrange
            IEnumerable<int> source = GetEnumerable();

            // Act
            var result = source.LastOrDefault(-1);

            // Assert
            Assert.AreEqual(3, result);
        }

        #endregion

        #region SingleOrDefault with default value Tests

        [Test]
        public void SingleOrDefault_기본값_빈_시퀀스_기본값_반환()
        {
            // Arrange
            var empty = new int[0];

            // Act
            var result = empty.SingleOrDefault(-1);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void SingleOrDefault_기본값_단일_요소_해당_요소_반환()
        {
            // Arrange
            var numbers = new[] { 42 };

            // Act
            var result = numbers.SingleOrDefault(-1);

            // Assert
            Assert.AreEqual(42, result);
        }

        [Test]
        public void SingleOrDefault_기본값_복수_요소_예외_발생()
        {
            // Arrange
            var numbers = new[] { 1, 2 };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => numbers.SingleOrDefault(-1));
        }

        [Test]
        public void SingleOrDefault_조건_기본값_일치_없으면_기본값_반환()
        {
            // Arrange
            var numbers = new[] { 1, 2, 3 };

            // Act
            var result = numbers.SingleOrDefault(x => x > 10, -1);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void SingleOrDefault_조건_기본값_단일_일치_해당_요소_반환()
        {
            // Arrange
            var numbers = new[] { 1, 15, 3 };

            // Act
            var result = numbers.SingleOrDefault(x => x > 10, -1);

            // Assert
            Assert.AreEqual(15, result);
        }

        [Test]
        public void SingleOrDefault_조건_기본값_복수_일치_예외_발생()
        {
            // Arrange
            var numbers = new[] { 1, 15, 20, 3 };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                numbers.SingleOrDefault(x => x > 10, -1));
        }

        [Test]
        public void SingleOrDefault_리스트가_아닌_IEnumerable_빈_시퀀스()
        {
            // Arrange
            IEnumerable<int> source = GetEmptyEnumerable();

            // Act
            var result = source.SingleOrDefault(-1);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void SingleOrDefault_리스트가_아닌_IEnumerable_단일_요소()
        {
            // Arrange
            IEnumerable<int> source = GetSingleEnumerable();

            // Act
            var result = source.SingleOrDefault(-1);

            // Assert
            Assert.AreEqual(42, result);
        }

        #endregion

        #region Helper Methods

        private IEnumerable<int> GetEnumerable()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }

        private IEnumerable<int> GetEmptyEnumerable()
        {
            yield break;
        }

        private IEnumerable<int> GetSingleEnumerable()
        {
            yield return 42;
        }

        #endregion
    }
}

#endif
