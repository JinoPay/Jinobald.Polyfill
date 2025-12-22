// Jinobald.Polyfill - Modern LINQ 확장 메서드 테스트 (.NET 6.0+)
// Chunk, Index, TryGetNonEnumeratedCount 메서드 테스트

#if NET35
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jinobald.Polyfill.Tests.System.Linq
{
    [TestFixture]
    public class EnumerableModernTests
    {
        #region Chunk Tests

        [Test]
        public void Chunk_WithValidSize_ReturnsChunks()
        {
            // Arrange
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Act
            var chunks = numbers.Chunk(3).ToArray();

            // Assert
            Assert.AreEqual(3, chunks.Length);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, chunks[0]);
            CollectionAssert.AreEqual(new[] { 4, 5, 6 }, chunks[1]);
            CollectionAssert.AreEqual(new[] { 7, 8, 9 }, chunks[2]);
        }

        [Test]
        public void Chunk_WithPartialLastChunk_ReturnsCorrectChunks()
        {
            // Arrange
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7 };

            // Act
            var chunks = numbers.Chunk(3).ToArray();

            // Assert
            Assert.AreEqual(3, chunks.Length);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, chunks[0]);
            CollectionAssert.AreEqual(new[] { 4, 5, 6 }, chunks[1]);
            CollectionAssert.AreEqual(new[] { 7 }, chunks[2]);
        }

        [Test]
        public void Chunk_WithSizeLargerThanSequence_ReturnsSingleChunk()
        {
            // Arrange
            int[] numbers = { 1, 2, 3 };

            // Act
            var chunks = numbers.Chunk(10).ToArray();

            // Assert
            Assert.AreEqual(1, chunks.Length);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, chunks[0]);
        }

        [Test]
        public void Chunk_WithEmptySequence_ReturnsEmpty()
        {
            // Arrange
            int[] numbers = new int[0];

            // Act
            var chunks = numbers.Chunk(3).ToArray();

            // Assert
            Assert.AreEqual(0, chunks.Length);
        }

        [Test]
        public void Chunk_WithSize1_ReturnsSingleElementChunks()
        {
            // Arrange
            int[] numbers = { 1, 2, 3 };

            // Act
            var chunks = numbers.Chunk(1).ToArray();

            // Assert
            Assert.AreEqual(3, chunks.Length);
            CollectionAssert.AreEqual(new[] { 1 }, chunks[0]);
            CollectionAssert.AreEqual(new[] { 2 }, chunks[1]);
            CollectionAssert.AreEqual(new[] { 3 }, chunks[2]);
        }

        [Test]
        public void Chunk_WithNullSource_ThrowsArgumentNullException()
        {
            // Arrange
            int[] numbers = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => numbers.Chunk(3).ToArray());
        }

        [Test]
        public void Chunk_WithZeroSize_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int[] numbers = { 1, 2, 3 };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => numbers.Chunk(0).ToArray());
        }

        [Test]
        public void Chunk_WithNegativeSize_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int[] numbers = { 1, 2, 3 };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => numbers.Chunk(-1).ToArray());
        }

        [Test]
        public void Chunk_IsDeferredExecution()
        {
            // Arrange
            bool executed = false;
            IEnumerable<int> source = GetSourceWithSideEffect(() => executed = true);

            // Act
            var chunks = source.Chunk(2);

            // Assert - 아직 실행되지 않음
            Assert.IsFalse(executed);

            // 실제 열거
            chunks.ToArray();
            Assert.IsTrue(executed);
        }

        #endregion

        #region Index Tests

        [Test]
        public void Index_WithValidSequence_ReturnsIndexedElements()
        {
            // Arrange
            string[] words = { "apple", "banana", "cherry" };

            // Act
            var indexed = words.Index().ToArray();

            // Assert
            Assert.AreEqual(3, indexed.Length);

            Assert.AreEqual(0, GetIndexFromTuple(indexed[0]));
            Assert.AreEqual("apple", GetItemFromTuple(indexed[0]));

            Assert.AreEqual(1, GetIndexFromTuple(indexed[1]));
            Assert.AreEqual("banana", GetItemFromTuple(indexed[1]));

            Assert.AreEqual(2, GetIndexFromTuple(indexed[2]));
            Assert.AreEqual("cherry", GetItemFromTuple(indexed[2]));
        }

        [Test]
        public void Index_WithEmptySequence_ReturnsEmpty()
        {
            // Arrange
            int[] numbers = new int[0];

            // Act
            var indexed = numbers.Index().ToArray();

            // Assert
            Assert.AreEqual(0, indexed.Length);
        }

        [Test]
        public void Index_WithSingleElement_ReturnsIndexZero()
        {
            // Arrange
            string[] words = { "solo" };

            // Act
            var indexed = words.Index().ToArray();

            // Assert
            Assert.AreEqual(1, indexed.Length);
            Assert.AreEqual(0, GetIndexFromTuple(indexed[0]));
            Assert.AreEqual("solo", GetItemFromTuple(indexed[0]));
        }

        [Test]
        public void Index_WithNullSource_ThrowsArgumentNullException()
        {
            // Arrange
            int[] numbers = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => numbers.Index().ToArray());
        }

        [Test]
        public void Index_IsDeferredExecution()
        {
            // Arrange
            bool executed = false;
            IEnumerable<int> source = GetSourceWithSideEffect(() => executed = true);

            // Act
            var indexed = source.Index();

            // Assert - 아직 실행되지 않음
            Assert.IsFalse(executed);

            // 실제 열거
            indexed.ToArray();
            Assert.IsTrue(executed);
        }

        [Test]
        public void Index_CanBeUsedInLinq()
        {
            // Arrange
            string[] words = { "apple", "banana", "cherry", "date" };

            // Act - 짝수 인덱스의 요소만 선택
            var evenIndexed = words.Index()
                .Where(pair => GetIndexFromTuple(pair) % 2 == 0)
                .Select(pair => GetItemFromTuple(pair))
                .ToArray();

            // Assert
            CollectionAssert.AreEqual(new[] { "apple", "cherry" }, evenIndexed);
        }

        #endregion

        #region TryGetNonEnumeratedCount Tests

        [Test]
        public void TryGetNonEnumeratedCount_WithArray_ReturnsTrue()
        {
            // Arrange
            int[] numbers = { 1, 2, 3, 4, 5 };

            // Act
            bool result = numbers.TryGetNonEnumeratedCount(out int count);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(5, count);
        }

        [Test]
        public void TryGetNonEnumeratedCount_WithList_ReturnsTrue()
        {
            // Arrange
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            bool result = numbers.TryGetNonEnumeratedCount(out int count);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(5, count);
        }

        [Test]
        public void TryGetNonEnumeratedCount_WithHashSet_ReturnsTrue()
        {
            // Arrange
            HashSet<int> numbers = new HashSet<int> { 1, 2, 3, 4, 5 };

            // Act
            bool result = numbers.TryGetNonEnumeratedCount(out int count);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(5, count);
        }

        [Test]
        public void TryGetNonEnumeratedCount_WithEnumerable_ReturnsFalse()
        {
            // Arrange
            IEnumerable<int> numbers = GetEnumerable();

            // Act
            bool result = numbers.TryGetNonEnumeratedCount(out int count);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, count);
        }

        [Test]
        public void TryGetNonEnumeratedCount_WithWhereClause_ReturnsFalse()
        {
            // Arrange
            int[] numbers = { 1, 2, 3, 4, 5 };
            var filtered = numbers.Where(n => n > 2);

            // Act
            bool result = filtered.TryGetNonEnumeratedCount(out int count);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, count);
        }

        [Test]
        public void TryGetNonEnumeratedCount_WithEmptyArray_ReturnsTrue()
        {
            // Arrange
            int[] numbers = new int[0];

            // Act
            bool result = numbers.TryGetNonEnumeratedCount(out int count);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, count);
        }

        [Test]
        public void TryGetNonEnumeratedCount_WithNullSource_ThrowsArgumentNullException()
        {
            // Arrange
            int[] numbers = null;
            int count;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => numbers.TryGetNonEnumeratedCount(out count));
        }

        [Test]
        public void TryGetNonEnumeratedCount_DoesNotEnumerateSequence()
        {
            // Arrange
            bool enumerated = false;
            IEnumerable<int> source = GetSourceWithSideEffect(() => enumerated = true);

            // Act
            source.TryGetNonEnumeratedCount(out int count);

            // Assert - 열거되지 않아야 함
            Assert.IsFalse(enumerated);
        }

        #endregion

        #region Helper Methods

        private IEnumerable<int> GetSourceWithSideEffect(Action sideEffect)
        {
            sideEffect();
            yield return 1;
            yield return 2;
            yield return 3;
        }

        private IEnumerable<int> GetEnumerable()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;
        }

        // Tuple/ValueTuple 호환을 위한 헬퍼 메서드
        private static int GetIndexFromTuple<T>(object tuple)
        {
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481 || NETSTANDARD2_0
            var vt = (ValueTuple<int, T>)tuple;
            return vt.Item1;
#else
            var t = (Tuple<int, T>)tuple;
            return t.Item1;
#endif
        }

        private static T GetItemFromTuple<T>(object tuple)
        {
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481 || NETSTANDARD2_0
            var vt = (ValueTuple<int, T>)tuple;
            return vt.Item2;
#else
            var t = (Tuple<int, T>)tuple;
            return t.Item2;
#endif
        }

        #endregion
    }
}

#endif
