// Jinobald.Polyfill - Modern LINQ 확장 메서드 테스트 (.NET 6.0+)
// ToHashSet, Append, Prepend, DistinctBy, MinBy, MaxBy 등의 메서드 테스트

using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System.Linq;

/// <summary>
///     .NET 6.0+ Modern LINQ 확장 메서드에 대한 테스트입니다.
/// </summary>
[TestFixture]
public class EnumerableModernExtTests
{
    #region ToHashSet 테스트

    [Test]
    public void ToHashSet_시퀀스를_해시셋으로_변환()
    {
        // 준비
        int[] source = new[] { 1, 2, 3, 4, 5 };

        // 실행
        HashSet<int> result = source.ToHashSet();

        // 검증
        Assert.AreEqual(5, result.Count);
        Assert.IsTrue(result.Contains(1));
        Assert.IsTrue(result.Contains(5));
    }

    [Test]
    public void ToHashSet_중복_요소_제거()
    {
        // 준비
        int[] source = new[] { 1, 2, 2, 3, 3, 3, 4 };

        // 실행
        HashSet<int> result = source.ToHashSet();

        // 검증
        Assert.AreEqual(4, result.Count);
    }

    [Test]
    public void ToHashSet_커스텀_비교자_사용()
    {
        // 준비
        string[] source = new[] { "Apple", "apple", "BANANA", "banana" };

        // 실행
        HashSet<string> result = source.ToHashSet(StringComparer.OrdinalIgnoreCase);

        // 검증
        Assert.AreEqual(2, result.Count);
    }

    [Test]
    public void ToHashSet_빈_시퀀스_빈_해시셋_반환()
    {
        // 준비
        IEnumerable<int> source = Enumerable.Empty<int>();

        // 실행
        HashSet<int> result = source.ToHashSet();

        // 검증
        Assert.IsEmpty(result);
    }

    [Test]
    public void ToHashSet_Null_Source_예외_발생()
    {
        // 준비
        IEnumerable<int>? source = null;

        // 실행 및 검증
        Assert.Throws<ArgumentNullException>(() => source!.ToHashSet());
    }

    #endregion

    #region Append/Prepend 테스트

    [Test]
    public void Append_끝에_요소_추가()
    {
        // 준비
        int[] source = new[] { 1, 2, 3 };

        // 실행
        List<int> result = source.Append(4).ToList();

        // 검증
        Assert.AreEqual(new[] { 1, 2, 3, 4 }, result);
    }

    [Test]
    public void Prepend_앞에_요소_추가()
    {
        // 준비
        int[] source = new[] { 2, 3, 4 };

        // 실행
        List<int> result = source.Prepend(1).ToList();

        // 검증
        Assert.AreEqual(new[] { 1, 2, 3, 4 }, result);
    }

    [Test]
    public void Append_Prepend_체이닝()
    {
        // 준비
        int[] source = new[] { 2, 3 };

        // 실행
        List<int> result = source.Prepend(1).Append(4).ToList();

        // 검증
        Assert.AreEqual(new[] { 1, 2, 3, 4 }, result);
    }

    [Test]
    public void Append_빈_시퀀스에_요소_추가()
    {
        // 준비
        IEnumerable<int> source = Enumerable.Empty<int>();

        // 실행
        List<int> result = source.Append(1).ToList();

        // 검증
        Assert.AreEqual(new[] { 1 }, result);
    }

    #endregion

    #region DistinctBy 테스트

    [Test]
    public void DistinctBy_키에_따라_고유한_요소_반환()
    {
        // 준비
        var source = new[]
        {
            new { Name = "Alice", Age = 25 },
            new { Name = "Bob", Age = 30 },
            new { Name = "Charlie", Age = 25 },
            new { Name = "David", Age = 35 }
        };

        // 실행 - Age 기준으로 고유한 요소
        var result = source.DistinctBy(x => x.Age).ToList();

        // 검증
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("Alice", result[0].Name);
        Assert.AreEqual("Bob", result[1].Name);
        Assert.AreEqual("David", result[2].Name);
    }

    [Test]
    public void DistinctBy_커스텀_비교자_사용()
    {
        // 준비
        string[] source = new[] { "Apple", "apple", "Banana", "BANANA", "Cherry" };

        // 실행
        var result = source.DistinctBy(s => s, StringComparer.OrdinalIgnoreCase).ToList();

        // 검증
        Assert.AreEqual(3, result.Count);
    }

    #endregion

    #region MinBy/MaxBy 테스트

    [Test]
    public void MinBy_키에_따라_최소값_반환()
    {
        // 준비
        var source = new[]
        {
            new { Name = "Alice", Age = 25 },
            new { Name = "Bob", Age = 30 },
            new { Name = "Charlie", Age = 20 }
        };

        // 실행
        var result = source.MinBy(x => x.Age);

        // 검증
        Assert.IsNotNull(result);
        Assert.AreEqual("Charlie", result!.Name);
        Assert.AreEqual(20, result.Age);
    }

    [Test]
    public void MaxBy_키에_따라_최대값_반환()
    {
        // 준비
        var source = new[]
        {
            new { Name = "Alice", Age = 25 },
            new { Name = "Bob", Age = 30 },
            new { Name = "Charlie", Age = 20 }
        };

        // 실행
        var result = source.MaxBy(x => x.Age);

        // 검증
        Assert.IsNotNull(result);
        Assert.AreEqual("Bob", result!.Name);
        Assert.AreEqual(30, result.Age);
    }

    [Test]
    public void MinBy_빈_시퀀스_참조_타입_Null_반환()
    {
        // 준비
        var source = Enumerable.Empty<object>();

        // 실행
        var result = source.MinBy(x => x?.GetHashCode() ?? 0);

        // 검증
        Assert.IsNull(result);
    }

    #endregion

    #region UnionBy/IntersectBy/ExceptBy 테스트

    [Test]
    public void UnionBy_키에_따라_합집합_생성()
    {
        // 준비
        var first = new[]
        {
            new { Id = 1, Name = "Alice" },
            new { Id = 2, Name = "Bob" }
        };
        var second = new[]
        {
            new { Id = 2, Name = "Bobby" },
            new { Id = 3, Name = "Charlie" }
        };

        // 실행
        var result = first.UnionBy(second, x => x.Id).ToList();

        // 검증
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("Alice", result[0].Name);
        Assert.AreEqual("Bob", result[1].Name);
        Assert.AreEqual("Charlie", result[2].Name);
    }

    [Test]
    public void IntersectBy_키에_따라_교집합_생성()
    {
        // 준비
        var first = new[]
        {
            new { Id = 1, Name = "Alice" },
            new { Id = 2, Name = "Bob" },
            new { Id = 3, Name = "Charlie" }
        };
        int[] secondKeys = new[] { 2, 3, 4 };

        // 실행
        var result = first.IntersectBy(secondKeys, x => x.Id).ToList();

        // 검증
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Bob", result[0].Name);
        Assert.AreEqual("Charlie", result[1].Name);
    }

    [Test]
    public void ExceptBy_키에_따라_차집합_생성()
    {
        // 준비
        var first = new[]
        {
            new { Id = 1, Name = "Alice" },
            new { Id = 2, Name = "Bob" },
            new { Id = 3, Name = "Charlie" }
        };
        int[] excludeKeys = new[] { 2 };

        // 실행
        var result = first.ExceptBy(excludeKeys, x => x.Id).ToList();

        // 검증
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Alice", result[0].Name);
        Assert.AreEqual("Charlie", result[1].Name);
    }

    #endregion

    #region FirstOrDefault/LastOrDefault/SingleOrDefault 기본값 오버로드 테스트

    [Test]
    public void FirstOrDefault_기본값_빈_시퀀스_기본값_반환()
    {
        // 준비
        IEnumerable<int> source = Enumerable.Empty<int>();

        // 실행
        int result = source.FirstOrDefault(-1);

        // 검증
        Assert.AreEqual(-1, result);
    }

    [Test]
    public void FirstOrDefault_기본값_조건과_함께_사용()
    {
        // 준비
        int[] source = new[] { 1, 2, 3, 4, 5 };

        // 실행
        int result = source.FirstOrDefault(x => x > 10, -1);

        // 검증
        Assert.AreEqual(-1, result);
    }

    [Test]
    public void LastOrDefault_기본값_빈_시퀀스_기본값_반환()
    {
        // 준비
        IEnumerable<string> source = Enumerable.Empty<string>();

        // 실행
        string result = source.LastOrDefault("default");

        // 검증
        Assert.AreEqual("default", result);
    }

    [Test]
    public void SingleOrDefault_기본값_빈_시퀀스_기본값_반환()
    {
        // 준비
        IEnumerable<int> source = Enumerable.Empty<int>();

        // 실행
        int result = source.SingleOrDefault(42);

        // 검증
        Assert.AreEqual(42, result);
    }

    [Test]
    public void SingleOrDefault_기본값_여러_요소_예외_발생()
    {
        // 준비
        int[] source = new[] { 1, 2, 3 };

        // 실행 및 검증
        Assert.Throws<InvalidOperationException>(() => source.SingleOrDefault(0));
    }

    #endregion

    #region TakeLast/SkipLast 테스트

    [Test]
    public void TakeLast_마지막_N개_요소_반환()
    {
        // 준비
        int[] source = new[] { 1, 2, 3, 4, 5 };

        // 실행
        List<int> result = source.TakeLast(3).ToList();

        // 검증
        Assert.AreEqual(new[] { 3, 4, 5 }, result);
    }

    [Test]
    public void TakeLast_Count가_시퀀스보다_클_때_전체_반환()
    {
        // 준비
        int[] source = new[] { 1, 2, 3 };

        // 실행
        List<int> result = source.TakeLast(10).ToList();

        // 검증
        Assert.AreEqual(new[] { 1, 2, 3 }, result);
    }

    [Test]
    public void SkipLast_마지막_N개_요소_건너뛰기()
    {
        // 준비
        int[] source = new[] { 1, 2, 3, 4, 5 };

        // 실행
        List<int> result = source.SkipLast(2).ToList();

        // 검증
        Assert.AreEqual(new[] { 1, 2, 3 }, result);
    }

    [Test]
    public void SkipLast_Count가_시퀀스보다_클_때_빈_시퀀스_반환()
    {
        // 준비
        int[] source = new[] { 1, 2, 3 };

        // 실행
        List<int> result = source.SkipLast(10).ToList();

        // 검증
        Assert.IsEmpty(result);
    }

    #endregion

    #region Order/OrderDescending 테스트

    [Test]
    public void Order_오름차순_정렬()
    {
        // 준비
        int[] source = new[] { 3, 1, 4, 1, 5, 9, 2, 6 };

        // 실행
        List<int> result = source.Order().ToList();

        // 검증
        Assert.AreEqual(new[] { 1, 1, 2, 3, 4, 5, 6, 9 }, result);
    }

    [Test]
    public void OrderDescending_내림차순_정렬()
    {
        // 준비
        int[] source = new[] { 3, 1, 4, 1, 5, 9, 2, 6 };

        // 실행
        List<int> result = source.OrderDescending().ToList();

        // 검증
        Assert.AreEqual(new[] { 9, 6, 5, 4, 3, 2, 1, 1 }, result);
    }

    [Test]
    public void Order_문자열_대소문자_무시_정렬()
    {
        // 준비
        string[] source = new[] { "Banana", "apple", "Cherry", "APPLE" };

        // 실행
        List<string> result = source.Order(StringComparer.OrdinalIgnoreCase).ToList();

        // 검증 - 첫 번째 요소는 "apple" 또는 "APPLE" (대소문자 무시 정렬에서는 동등함)
        Assert.That(result[0], Is.EqualTo("apple").IgnoreCase);
    }

    #endregion

    #region CountBy 테스트

    [Test]
    public void CountBy_키별_개수_계산()
    {
        // 준비
        string[] source = new[] { "apple", "banana", "apple", "cherry", "banana", "apple" };

        // 실행
        var result = source.CountBy(x => x).ToList();

        // 검증
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("apple", result[0].Key);
        Assert.AreEqual(3, result[0].Value);
        Assert.AreEqual("banana", result[1].Key);
        Assert.AreEqual(2, result[1].Value);
        Assert.AreEqual("cherry", result[2].Key);
        Assert.AreEqual(1, result[2].Value);
    }

    #endregion

    #region AggregateBy 테스트

    [Test]
    public void AggregateBy_키별_집계()
    {
        // 준비
        var source = new[]
        {
            new { Category = "Fruit", Price = 10 },
            new { Category = "Vegetable", Price = 5 },
            new { Category = "Fruit", Price = 20 },
            new { Category = "Vegetable", Price = 8 }
        };

        // 실행 - 카테고리별 가격 합계
        var result = source.AggregateBy(
            x => x.Category,
            0,
            (acc, item) => acc + item.Price).ToList();

        // 검증
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Fruit", result[0].Key);
        Assert.AreEqual(30, result[0].Value);
        Assert.AreEqual("Vegetable", result[1].Key);
        Assert.AreEqual(13, result[1].Value);
    }

    #endregion

    #region ElementAt(Index) 테스트

    [Test]
    public void ElementAt_Index_시작부터_인덱싱()
    {
        // 준비
        int[] source = new[] { 10, 20, 30, 40, 50 };

        // 실행
        int result = source.ElementAt(new Index(2));

        // 검증
        Assert.AreEqual(30, result);
    }

    [Test]
    public void ElementAt_Index_끝부터_인덱싱()
    {
        // 준비
        int[] source = new[] { 10, 20, 30, 40, 50 };

        // 실행
        int result = source.ElementAt(Index.FromEnd(2));

        // 검증
        Assert.AreEqual(40, result);
    }

    [Test]
    public void ElementAtOrDefault_Index_범위_초과시_기본값()
    {
        // 준비
        int[] source = new[] { 10, 20, 30 };

        // 실행
        int result = source.ElementAtOrDefault(Index.FromEnd(10));

        // 검증
        Assert.AreEqual(0, result);
    }

    #endregion

    #region Take(Range) 테스트

    [Test]
    public void Take_Range_시작부터_끝까지_범위_지정()
    {
        // 준비
        int[] source = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        // 실행 - 인덱스 2부터 5까지 (5 미포함)
        List<int> result = source.Take(new Range(2, 5)).ToList();

        // 검증
        Assert.AreEqual(new[] { 2, 3, 4 }, result);
    }

    [Test]
    public void Take_Range_끝에서부터_범위_지정()
    {
        // 준비
        int[] source = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        // 실행 - 마지막 3개 요소
        List<int> result = source.Take(Range.StartAt(Index.FromEnd(3))).ToList();

        // 검증
        Assert.AreEqual(new[] { 7, 8, 9 }, result);
    }

    #endregion
}
