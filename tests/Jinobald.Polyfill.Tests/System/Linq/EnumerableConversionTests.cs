// Jinobald.Polyfill - LINQ Enumerable 변환 연산자 테스트
// ToArray, ToList, ToDictionary, Cast, OfType 등 변환 연산자에 대한 단위 테스트

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System.Linq
{
    /// <summary>
    /// Enumerable 클래스의 변환 연산자에 대한 테스트입니다.
    /// </summary>
    public class EnumerableConversionTests
    {
        #region ToArray 테스트

        [Test]
        public void ToArray_리스트를_배열로_변환()
        {
            // 준비
            var source = new List<int> { 1, 2, 3, 4, 5 };

            // 실행
            var result = source.ToArray();

            // 검증
            Assert.IsInstanceOf<int[]>(result);
            Assert.AreEqual(new[] { 1, 2, 3, 4, 5 }, result);
        }

        [Test]
        public void ToArray_빈_시퀀스_빈_배열_반환()
        {
            // 준비
            var source = Enumerable.Empty<int>();

            // 실행
            var result = source.ToArray();

            // 검증
            Assert.IsEmpty(result);
        }

        [Test]
        public void ToArray_지연_실행된_시퀀스_변환()
        {
            // 준비
            var source = Enumerable.Range(1, 5).Where(x => x % 2 == 0);

            // 실행
            var result = source.ToArray();

            // 검증
            Assert.AreEqual(new[] { 2, 4 }, result);
        }

        [Test]
        public void ToArray_Null_Source_예외_발생()
        {
            // 준비
            IEnumerable<int>? source = null;

            // 실행 및 검증
            Assert.Throws<ArgumentNullException>(() => source!.ToArray());
        }

        #endregion

        #region ToList 테스트

        [Test]
        public void ToList_배열을_리스트로_변환()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행
            var result = source.ToList();

            // 검증
            Assert.IsInstanceOf<List<int>>(result);
            Assert.AreEqual(new[] { 1, 2, 3, 4, 5 }, result);
        }

        [Test]
        public void ToList_빈_시퀀스_빈_리스트_반환()
        {
            // 준비
            var source = Enumerable.Empty<string>();

            // 실행
            var result = source.ToList();

            // 검증
            Assert.IsEmpty(result);
            Assert.IsInstanceOf<List<string>>(result);
        }

        [Test]
        public void ToList_수정_가능한_리스트_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.ToList();
            result.Add(4);

            // 검증
            Assert.AreEqual(4, result.Count);
            Assert.Contains(4, result);
        }

        #endregion

        #region ToDictionary 테스트

        [Test]
        public void ToDictionary_키_선택기만_사용()
        {
            // 준비
            var source = new[] { "apple", "banana", "cherry" };

            // 실행
            var result = source.ToDictionary(x => x[0]);

            // 검증
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("apple", result['a']);
            Assert.AreEqual("banana", result['b']);
            Assert.AreEqual("cherry", result['c']);
        }

        [Test]
        public void ToDictionary_키와_요소_선택기_사용()
        {
            // 준비
            var source = new[] { "apple", "banana", "cherry" };

            // 실행
            var result = source.ToDictionary(
                x => x[0],
                x => x.Length
            );

            // 검증
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(5, result['a']); // "apple".Length
            Assert.AreEqual(6, result['b']); // "banana".Length
            Assert.AreEqual(6, result['c']); // "cherry".Length
        }

        [Test]
        public void ToDictionary_커스텀_비교자_사용()
        {
            // 준비
            var source = new[] { "Apple", "Banana" };

            // 실행 - 대소문자 무시 비교자
            var result = source.ToDictionary(
                x => x,
                StringComparer.OrdinalIgnoreCase
            );

            // 검증
            Assert.IsTrue(result.ContainsKey("APPLE"));
            Assert.IsTrue(result.ContainsKey("banana"));
        }

        [Test]
        public void ToDictionary_중복_키_예외_발생()
        {
            // 준비
            var source = new[] { "apple", "apricot" }; // 둘 다 'a'로 시작

            // 실행 및 검증
            Assert.Throws<ArgumentException>(() => source.ToDictionary(x => x[0]));
        }

        [Test]
        public void ToDictionary_Null_Source_예외_발생()
        {
            // 준비
            IEnumerable<string>? source = null;

            // 실행 및 검증
            Assert.Throws<ArgumentNullException>(() => source!.ToDictionary(x => x[0]));
        }

        #endregion

        #region Cast 테스트

        [Test]
        public void Cast_성공적으로_형식_변환()
        {
            // 준비
            var source = new ArrayList { 1, 2, 3, 4, 5 };

            // 실행
            var result = source.Cast<int>().ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3, 4, 5 }, result);
        }

        [Test]
        public void Cast_이미_올바른_형식이면_그대로_반환()
        {
            // 준비
            IEnumerable<int> source = new[] { 1, 2, 3 };

            // 실행
            var result = source.Cast<int>();

            // 검증 - 같은 참조여야 함
            Assert.AreSame(source, result);
        }

        [Test]
        public void Cast_잘못된_형식_예외_발생()
        {
            // 준비
            var source = new ArrayList { 1, "two", 3 };

            // 실행 - 열거 시 예외 발생
            var query = source.Cast<int>();

            // 검증
            Assert.Throws<InvalidCastException>(() => query.ToList());
        }

        #endregion

        #region OfType 테스트

        [Test]
        public void OfType_지정된_형식만_필터링()
        {
            // 준비
            var source = new ArrayList { 1, "two", 3, "four", 5 };

            // 실행 - int만 선택
            var intResult = source.OfType<int>().ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 3, 5 }, intResult);
        }

        [Test]
        public void OfType_문자열만_필터링()
        {
            // 준비
            var source = new ArrayList { 1, "two", 3, "four", 5 };

            // 실행 - string만 선택
            var stringResult = source.OfType<string>().ToList();

            // 검증
            Assert.AreEqual(new[] { "two", "four" }, stringResult);
        }

        [Test]
        public void OfType_일치하는_형식_없음_빈_시퀀스_반환()
        {
            // 준비
            var source = new ArrayList { 1, 2, 3 };

            // 실행
            var result = source.OfType<string>().ToList();

            // 검증
            Assert.IsEmpty(result);
        }

        [Test]
        public void OfType_Null_값_건너뛰기()
        {
            // 준비
            var source = new ArrayList { "one", null, "two", null, "three" };

            // 실행
            var result = source.OfType<string>().ToList();

            // 검증 - null은 제외됨
            Assert.AreEqual(new[] { "one", "two", "three" }, result);
        }

        #endregion

        #region Empty 테스트

        [Test]
        public void Empty_빈_시퀀스_반환()
        {
            // 실행
            var result = Enumerable.Empty<int>();

            // 검증
            Assert.IsEmpty(result);
        }

        [Test]
        public void Empty_같은_형식에_대해_동일한_인스턴스_반환()
        {
            // 실행
            var empty1 = Enumerable.Empty<string>();
            var empty2 = Enumerable.Empty<string>();

            // 검증 - 같은 참조여야 함 (캐시됨)
            Assert.AreSame(empty1, empty2);
        }

        #endregion

        #region Range 테스트

        [Test]
        public void Range_정수_시퀀스_생성()
        {
            // 실행
            var result = Enumerable.Range(1, 5).ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3, 4, 5 }, result);
        }

        [Test]
        public void Range_음수_시작값()
        {
            // 실행
            var result = Enumerable.Range(-2, 5).ToList();

            // 검증
            Assert.AreEqual(new[] { -2, -1, 0, 1, 2 }, result);
        }

        [Test]
        public void Range_0개_요소()
        {
            // 실행
            var result = Enumerable.Range(10, 0).ToList();

            // 검증
            Assert.IsEmpty(result);
        }

        [Test]
        public void Range_음수_Count_예외_발생()
        {
            // 실행 및 검증
            Assert.Throws<ArgumentOutOfRangeException>(() => Enumerable.Range(0, -1));
        }

        #endregion

        #region Repeat 테스트

        [Test]
        public void Repeat_값_반복()
        {
            // 실행
            var result = Enumerable.Repeat("hello", 3).ToList();

            // 검증
            Assert.AreEqual(new[] { "hello", "hello", "hello" }, result);
        }

        [Test]
        public void Repeat_0회_반복_빈_시퀀스()
        {
            // 실행
            var result = Enumerable.Repeat(42, 0).ToList();

            // 검증
            Assert.IsEmpty(result);
        }

        [Test]
        public void Repeat_Null_값_반복()
        {
            // 실행
            var result = Enumerable.Repeat<string?>(null, 3).ToList();

            // 검증
            Assert.AreEqual(3, result.Count);
            foreach (var item in result)
            {
                Assert.IsNull(item);
            }
        }

        [Test]
        public void Repeat_음수_Count_예외_발생()
        {
            // 실행 및 검증
            Assert.Throws<ArgumentOutOfRangeException>(() => Enumerable.Repeat("x", -1));
        }

        #endregion

        #region Concat 테스트

        [Test]
        public void Concat_두_시퀀스_연결()
        {
            // 준비
            var first = new[] { 1, 2, 3 };
            var second = new[] { 4, 5, 6 };

            // 실행
            var result = first.Concat(second).ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3, 4, 5, 6 }, result);
        }

        [Test]
        public void Concat_첫_번째_시퀀스_빈_경우()
        {
            // 준비
            var first = Enumerable.Empty<int>();
            var second = new[] { 1, 2, 3 };

            // 실행
            var result = first.Concat(second).ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        [Test]
        public void Concat_두_번째_시퀀스_빈_경우()
        {
            // 준비
            var first = new[] { 1, 2, 3 };
            var second = Enumerable.Empty<int>();

            // 실행
            var result = first.Concat(second).ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        #endregion

        #region Distinct 테스트

        [Test]
        public void Distinct_중복_제거()
        {
            // 준비
            var source = new[] { 1, 2, 2, 3, 3, 3, 4 };

            // 실행
            var result = source.Distinct().ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3, 4 }, result);
        }

        [Test]
        public void Distinct_중복_없음_원본_그대로()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행
            var result = source.Distinct().ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3, 4, 5 }, result);
        }

        [Test]
        public void Distinct_커스텀_비교자_사용()
        {
            // 준비
            var source = new[] { "Apple", "apple", "BANANA", "banana" };

            // 실행 - 대소문자 무시
            var result = source.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            // 검증
            Assert.AreEqual(2, result.Count);
        }

        #endregion

        #region Reverse 테스트

        [Test]
        public void Reverse_시퀀스_역순()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행 - 명시적으로 LINQ Reverse 호출
            var result = source.AsEnumerable().Reverse().ToList();

            // 검증
            Assert.AreEqual(new[] { 5, 4, 3, 2, 1 }, result);
        }

        [Test]
        public void Reverse_빈_시퀀스()
        {
            // 준비
            var source = Enumerable.Empty<int>();

            // 실행
            var result = source.AsEnumerable().Reverse().ToList();

            // 검증
            Assert.IsEmpty(result);
        }

        [Test]
        public void Reverse_단일_요소()
        {
            // 준비
            var source = new[] { 42 };

            // 실행
            var result = source.AsEnumerable().Reverse().ToList();

            // 검증
            Assert.AreEqual(new[] { 42 }, result);
        }

        #endregion

        #region Skip / Take 테스트

        [Test]
        public void Skip_지정된_수만큼_건너뛰기()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행
            var result = source.Skip(2).ToList();

            // 검증
            Assert.AreEqual(new[] { 3, 4, 5 }, result);
        }

        [Test]
        public void Skip_전체_요소_수보다_많이_건너뛰기()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.Skip(10).ToList();

            // 검증
            Assert.IsEmpty(result);
        }

        [Test]
        public void Take_지정된_수만큼_가져오기()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행
            var result = source.Take(3).ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        [Test]
        public void Take_전체_요소_수보다_많이_가져오기()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.Take(10).ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        [Test]
        public void SkipWhile_조건이_false가_될_때까지_건너뛰기()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5, 1, 2 };

            // 실행
            var result = source.SkipWhile(x => x < 4).ToList();

            // 검증
            Assert.AreEqual(new[] { 4, 5, 1, 2 }, result);
        }

        [Test]
        public void TakeWhile_조건이_true인_동안_가져오기()
        {
            // 준비
            var source = new[] { 1, 2, 3, 4, 5 };

            // 실행
            var result = source.TakeWhile(x => x < 4).ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        #endregion

        #region SequenceEqual 테스트

        [Test]
        public void SequenceEqual_동일한_시퀀스_True()
        {
            // 준비
            var first = new[] { 1, 2, 3 };
            var second = new[] { 1, 2, 3 };

            // 실행 및 검증
            Assert.IsTrue(first.SequenceEqual(second));
        }

        [Test]
        public void SequenceEqual_다른_시퀀스_False()
        {
            // 준비
            var first = new[] { 1, 2, 3 };
            var second = new[] { 1, 2, 4 };

            // 실행 및 검증
            Assert.IsFalse(first.SequenceEqual(second));
        }

        [Test]
        public void SequenceEqual_길이_다름_False()
        {
            // 준비
            var first = new[] { 1, 2, 3 };
            var second = new[] { 1, 2 };

            // 실행 및 검증
            Assert.IsFalse(first.SequenceEqual(second));
        }

        [Test]
        public void SequenceEqual_빈_시퀀스_True()
        {
            // 준비
            var first = Enumerable.Empty<int>();
            var second = Enumerable.Empty<int>();

            // 실행 및 검증
            Assert.IsTrue(first.SequenceEqual(second));
        }

        #endregion

        #region DefaultIfEmpty 테스트

        [Test]
        public void DefaultIfEmpty_비어있지_않은_시퀀스_그대로_반환()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.DefaultIfEmpty().ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        [Test]
        public void DefaultIfEmpty_빈_시퀀스_기본값_반환()
        {
            // 준비
            var source = Enumerable.Empty<int>();

            // 실행
            var result = source.DefaultIfEmpty().ToList();

            // 검증
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(0, result[0]);
        }

        [Test]
        public void DefaultIfEmpty_지정된_기본값_사용()
        {
            // 준비
            var source = Enumerable.Empty<int>();

            // 실행
            var result = source.DefaultIfEmpty(42).ToList();

            // 검증
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(42, result[0]);
        }

        #endregion

        #region Append / Prepend 테스트

// Append/Prepend는 .NET Framework 4.7.1부터 BCL에 추가됨
// net462, net47에서는 이 테스트를 실행하지 않음
#if NET471 || NET472 || NET48 || NET481 || NET6_0_OR_GREATER

        [Test]
        public void Append_끝에_요소_추가()
        {
            // 준비
            var source = new[] { 1, 2, 3 };

            // 실행
            var result = source.Append(4).ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3, 4 }, result);
        }

        [Test]
        public void Prepend_앞에_요소_추가()
        {
            // 준비
            var source = new[] { 2, 3, 4 };

            // 실행
            var result = source.Prepend(1).ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3, 4 }, result);
        }

        [Test]
        public void Append_Prepend_체이닝()
        {
            // 준비
            var source = new[] { 2, 3 };

            // 실행
            var result = source.Prepend(1).Append(4).ToList();

            // 검증
            Assert.AreEqual(new[] { 1, 2, 3, 4 }, result);
        }

#endif

        #endregion
    }
}
