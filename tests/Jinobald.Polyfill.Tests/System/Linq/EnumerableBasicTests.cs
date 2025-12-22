// Jinobald.Polyfill - LINQ Enumerable 기본 연산자 테스트
// Where, Select, SelectMany, First, Last, Single 등 기본 연산자에 대한 단위 테스트

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System.Linq
{
    /// <summary>
    /// Enumerable 클래스의 기본 연산자에 대한 테스트입니다.
    /// </summary>
    public class EnumerableBasicTests
    {
        #region Where 테스트

        [Test]
        public void Where_조건을_만족하는_요소만_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행
            var result = source.Where(x => x > 2).ToList();

            // 검증
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new[] { 3, 4, 5 }, result);
        }

        [Test]
        public void Where_인덱스와_함께_필터링()
        {
            // 준비
            var source = new[] { "a", "b", "c", "d", "e" };

            // 실행 - 짝수 인덱스 요소만 선택
            var result = source.Where((x, i) => i % 2 == 0).ToList();

            // 검증
            Assert.AreEqual(new[] { "a", "c", "e" }, result);
        }

        [Test]
        public void Where_빈_시퀀스_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.Where(x => x > 10).ToList();

            // 검증
            Assert.IsEmpty(result);
        }

        [Test]
        public void Where_Null_Source_예외_발생()
        {
            // 준비
            IEnumerable<int>? source = null;

            // 실행 및 검증
            Assert.Throws<ArgumentNullException>(() => source!.Where(x => true));
        }

        [Test]
        public void Where_Null_Predicate_예외_발생()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행 및 검증
            Assert.Throws<ArgumentNullException>(() => source.Where((Func<int, bool>)null!));
        }

        #endregion

        #region Select 테스트

        [Test]
        public void Select_요소를_변환()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.Select(x => x * 2).ToList();

            // 검증
            Assert.AreEqual(new[] { 2, 4, 6 }, result);
        }

        [Test]
        public void Select_인덱스와_함께_변환()
        {
            // 준비
            var source = new[] { "a", "b", "c" };

            // 실행
            var result = source.Select((x, i) => $"{i}:{x}").ToList();

            // 검증
            Assert.AreEqual(new[] { "0:a", "1:b", "2:c" }, result);
        }

        [Test]
        public void Select_타입_변환()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.Select(x => x.ToString()).ToList();

            // 검증
            Assert.AreEqual(new[] { "1", "2", "3" }, result);
        }

        #endregion

        #region SelectMany 테스트

        [Test]
        public void SelectMany_중첩_컬렉션_평탄화()
        {
            // 준비
            var source = new[] { new[] { 1, 2 }, new[] { 3, 4 } };

            // 실행
            var result = source.SelectMany(x => x).ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3, 4 }, result);
        }

        [Test]
        public void SelectMany_인덱스_사용()
        {
            // 준비
            var source = new[] { "ab", "cd" };

            // 실행
            var result = source.SelectMany((s, i) => s.Select(c => $"{i}:{c}")).ToList();

            // 검증
            Assert.AreEqual(new[] { "0:a", "0:b", "1:c", "1:d" }, result);
        }

        [Test]
        public void SelectMany_결과_선택기_사용()
        {
            // 준비
            var source = new[]
            {
                new { Name = "Alice", Pets = new[] { "Cat", "Dog" } },
                new { Name = "Bob", Pets = new[] { "Bird" } }
            };

            // 실행
            var result = source.SelectMany(
                owner => owner.Pets,
                (owner, pet) => $"{owner.Name}:{pet}"
            ).ToList();

            // 검증
            Assert.AreEqual(new[] { "Alice:Cat", "Alice:Dog", "Bob:Bird" }, result);
        }

        #endregion

        #region First / FirstOrDefault 테스트

        [Test]
        public void First_첫_번째_요소_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.First();

            // 검증
            Assert.AreEqual(1, result);
        }

        [Test]
        public void First_조건을_만족하는_첫_번째_요소()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행
            var result = source.First(x => x > 3);

            // 검증
            Assert.AreEqual(4, result);
        }

        [Test]
        public void First_빈_시퀀스_예외_발생()
        {
            // 준비
            var source = Array.Empty<int>();

            // 실행 및 검증
            Assert.Throws<InvalidOperationException>(() => source.First());
        }

        [Test]
        public void FirstOrDefault_첫_번째_요소_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.FirstOrDefault();

            // 검증
            Assert.AreEqual(1, result);
        }

        [Test]
        public void FirstOrDefault_빈_시퀀스_기본값_반환()
        {
            // 준비
            var source = Array.Empty<int>();

            // 실행
            var result = source.FirstOrDefault();

            // 검증
            Assert.AreEqual(0, result);
        }

        [Test]
        public void FirstOrDefault_조건을_만족하는_요소_없음_기본값_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.FirstOrDefault(x => x > 10);

            // 검증
            Assert.AreEqual(0, result);
        }

        #endregion

        #region Last / LastOrDefault 테스트

        [Test]
        public void Last_마지막_요소_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.Last();

            // 검증
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Last_조건을_만족하는_마지막_요소()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행
            var result = source.Last(x => x < 4);

            // 검증
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Last_빈_시퀀스_예외_발생()
        {
            // 준비
            var source = Array.Empty<int>();

            // 실행 및 검증
            Assert.Throws<InvalidOperationException>(() => source.Last());
        }

        [Test]
        public void LastOrDefault_마지막_요소_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.LastOrDefault();

            // 검증
            Assert.AreEqual(3, result);
        }

        [Test]
        public void LastOrDefault_빈_시퀀스_기본값_반환()
        {
            // 준비
            var source = Array.Empty<int>();

            // 실행
            var result = source.LastOrDefault();

            // 검증
            Assert.AreEqual(0, result);
        }

        #endregion

        #region Single / SingleOrDefault 테스트

        [Test]
        public void Single_유일한_요소_반환()
        {
            // 준비
            var source = new[] { 42 };

            // 실행
            var result = source.Single();

            // 검증
            Assert.AreEqual(42, result);
        }

        [Test]
        public void Single_여러_요소_예외_발생()
        {
            // 준비
            var source = new[] { 1, 2 };

            // 실행 및 검증
            Assert.Throws<InvalidOperationException>(() => source.Single());
        }

        [Test]
        public void Single_빈_시퀀스_예외_발생()
        {
            // 준비
            var source = Array.Empty<int>();

            // 실행 및 검증
            Assert.Throws<InvalidOperationException>(() => source.Single());
        }

        [Test]
        public void SingleOrDefault_유일한_요소_반환()
        {
            // 준비
            var source = new[] { 42 };

            // 실행
            var result = source.SingleOrDefault();

            // 검증
            Assert.AreEqual(42, result);
        }

        [Test]
        public void SingleOrDefault_빈_시퀀스_기본값_반환()
        {
            // 준비
            var source = Array.Empty<int>();

            // 실행
            var result = source.SingleOrDefault();

            // 검증
            Assert.AreEqual(0, result);
        }

        [Test]
        public void SingleOrDefault_여러_요소_예외_발생()
        {
            // 준비
            var source = new[] { 1, 2 };

            // 실행 및 검증
            Assert.Throws<InvalidOperationException>(() => source.SingleOrDefault());
        }

        #endregion

        #region Any / All 테스트

        [Test]
        public void Any_요소가_있으면_True()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행 및 검증
            Assert.IsTrue(source.Any());
        }

        [Test]
        public void Any_빈_시퀀스_False()
        {
            // 준비
            var source = Array.Empty<int>();

            // 실행 및 검증
            Assert.IsFalse(source.Any());
        }

        [Test]
        public void Any_조건을_만족하는_요소가_있으면_True()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행 및 검증
            Assert.IsTrue(source.Any(x => x > 4));
        }

        [Test]
        public void Any_조건을_만족하는_요소가_없으면_False()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행 및 검증
            Assert.IsFalse(source.Any(x => x > 10));
        }

        [Test]
        public void All_모든_요소가_조건_만족_True()
        {
            // 준비
            var source = new[] { 2, 4, 6, 8 };

            // 실행 및 검증
            Assert.IsTrue(source.All(x => x % 2 == 0));
        }

        [Test]
        public void All_일부_요소가_조건_불만족_False()
        {
            // 준비
            var source = new[] { 2, 3, 4, 6 };

            // 실행 및 검증
            Assert.IsFalse(source.All(x => x % 2 == 0));
        }

        [Test]
        public void All_빈_시퀀스_True()
        {
            // 준비
            var source = Array.Empty<int>();

            // 실행 및 검증
            Assert.IsTrue(source.All(x => x > 0));
        }

        #endregion

        #region Contains 테스트

        [Test]
        public void Contains_요소_포함_True()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행 및 검증
            Assert.IsTrue(source.Contains(3));
        }

        [Test]
        public void Contains_요소_미포함_False()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행 및 검증
            Assert.IsFalse(source.Contains(10));
        }

        [Test]
        public void Contains_커스텀_비교자_사용()
        {
            // 준비
            var source = new[] { "Apple", "Banana", "Cherry" };

            // 실행 및 검증 - 대소문자 무시 비교
            Assert.IsTrue(source.Contains("apple", StringComparer.OrdinalIgnoreCase));
        }

        #endregion

        #region Count / LongCount 테스트

        [Test]
        public void Count_요소_수_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행 및 검증
            Assert.AreEqual(5, source.Count());
        }

        [Test]
        public void Count_조건을_만족하는_요소_수_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행 및 검증
            Assert.AreEqual(3, source.Count(x => x > 2));
        }

        [Test]
        public void Count_빈_시퀀스_0_반환()
        {
            // 준비
            var source = Array.Empty<int>();

            // 실행 및 검증
            Assert.AreEqual(0, source.Count());
        }

        [Test]
        public void LongCount_큰_시퀀스_요소_수_반환()
        {
            // 준비
            var source = Enumerable.Range(0, 1000);

            // 실행 및 검증
            Assert.AreEqual(1000L, source.LongCount());
        }

        #endregion

        #region ElementAt / ElementAtOrDefault 테스트

        [Test]
        public void ElementAt_인덱스로_요소_반환()
        {
            // 준비
            var source = new[] { "a", "b", "c", "d" };

            // 실행 및 검증
            Assert.AreEqual("c", source.ElementAt(2));
        }

        [Test]
        public void ElementAt_범위_초과_예외_발생()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행 및 검증
            Assert.Throws<ArgumentOutOfRangeException>(() => source.ElementAt(10));
        }

        [Test]
        public void ElementAt_음수_인덱스_예외_발생()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행 및 검증
            Assert.Throws<ArgumentOutOfRangeException>(() => source.ElementAt(-1));
        }

        [Test]
        public void ElementAtOrDefault_인덱스로_요소_반환()
        {
            // 준비
            var source = new[] { "a", "b", "c" };

            // 실행 및 검증
            Assert.AreEqual("b", source.ElementAtOrDefault(1));
        }

        [Test]
        public void ElementAtOrDefault_범위_초과_기본값_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행 및 검증
            Assert.AreEqual(0, source.ElementAtOrDefault(10));
        }

        [Test]
        public void ElementAtOrDefault_음수_인덱스_기본값_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행 및 검증
            Assert.AreEqual(0, source.ElementAtOrDefault(-1));
        }

        #endregion

        #region 지연 실행 테스트

        [Test]
        public void Where_지연_실행_확인()
        {
            // 준비
            var executionCount = 0;
            IEnumerable<int> Source()
            {
                for (int i = 1; i <= 5; i++)
                {
                    executionCount++;
                    yield return i;
                }
            }

            // 실행 - 쿼리 정의만 (실행 아님)
            var query = Source().Where(x => x > 2);

            // 검증 - 아직 실행되지 않음
            Assert.AreEqual(0, executionCount);

            // 실행 - ToList로 실제 실행
            var result = query.ToList();

            // 검증 - 모든 요소가 평가됨
            Assert.AreEqual(5, executionCount);
            Assert.AreEqual(new[] { 3, 4, 5 }, result);
        }

        [Test]
        public void Select_지연_실행_확인()
        {
            // 준비
            var transformCount = 0;

            // 실행 - 쿼리 정의
            var query = new[] { 1, 2, 3 }.Select(x =>
            {
                transformCount++;
                return x * 2;
            });

            // 검증 - 아직 변환되지 않음
            Assert.AreEqual(0, transformCount);

            // 실행 - 첫 번째 요소만 가져옴
            var first = query.First();

            // 검증 - 하나의 요소만 변환됨
            Assert.AreEqual(1, transformCount);
            Assert.AreEqual(2, first);
        }

        #endregion
    }
}
