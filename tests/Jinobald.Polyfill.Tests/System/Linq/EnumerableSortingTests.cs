// Jinobald.Polyfill - LINQ 정렬 연산자 테스트

using System.Linq;
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System.Linq;

/// <summary>
/// OrderBy, OrderByDescending, ThenBy, ThenByDescending 연산자에 대한 테스트입니다.
/// </summary>
public class EnumerableSortingTests
{
    #region OrderBy 테스트

    /// <summary>
    /// OrderBy가 오름차순으로 정렬하는지 테스트합니다.
    /// </summary>
    [Test]
    public void OrderBy_정수배열_오름차순정렬()
    {
        var source = new[] { 5, 2, 8, 1, 9, 3 };
        var result = source.OrderBy(x => x).ToArray();
        Assert.AreEqual(new[] { 1, 2, 3, 5, 8, 9 }, result);
    }

    /// <summary>
    /// OrderBy가 문자열 길이로 정렬하는지 테스트합니다.
    /// </summary>
    [Test]
    public void OrderBy_문자열길이_오름차순정렬()
    {
        var source = new[] { "apple", "cat", "banana", "ox" };
        var result = source.OrderBy(x => x.Length).ToArray();
        Assert.AreEqual(new[] { "ox", "cat", "apple", "banana" }, result);
    }

    /// <summary>
    /// OrderBy가 빈 시퀀스를 처리하는지 테스트합니다.
    /// </summary>
    [Test]
    public void OrderBy_빈시퀀스_빈결과반환()
    {
        var source = Enumerable.Empty<int>();
        var result = source.OrderBy(x => x).ToArray();
        Assert.IsEmpty(result);
    }

    /// <summary>
    /// OrderBy가 null source에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void OrderBy_NullSource_예외발생()
    {
        int[]? source = null;
        Assert.Throws<ArgumentNullException>(() => source!.OrderBy(x => x).ToArray());
    }

    #endregion

    #region OrderByDescending 테스트

    /// <summary>
    /// OrderByDescending이 내림차순으로 정렬하는지 테스트합니다.
    /// </summary>
    [Test]
    public void OrderByDescending_정수배열_내림차순정렬()
    {
        var source = new[] { 5, 2, 8, 1, 9, 3 };
        var result = source.OrderByDescending(x => x).ToArray();
        Assert.AreEqual(new[] { 9, 8, 5, 3, 2, 1 }, result);
    }

    /// <summary>
    /// OrderByDescending이 문자열로 내림차순 정렬하는지 테스트합니다.
    /// </summary>
    [Test]
    public void OrderByDescending_문자열_내림차순정렬()
    {
        var source = new[] { "apple", "cherry", "banana" };
        var result = source.OrderByDescending(x => x).ToArray();
        Assert.AreEqual(new[] { "cherry", "banana", "apple" }, result);
    }

    #endregion

    #region ThenBy 테스트

    /// <summary>
    /// ThenBy가 2차 정렬을 수행하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ThenBy_복합정렬_1차2차오름차순()
    {
        var source = new[]
        {
            new { Name = "Kim", Age = 30 },
            new { Name = "Lee", Age = 25 },
            new { Name = "Kim", Age = 20 },
            new { Name = "Lee", Age = 35 }
        };

        var result = source
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Age)
            .ToArray();

        Assert.AreEqual("Kim", result[0].Name);
        Assert.AreEqual(20, result[0].Age);
        Assert.AreEqual("Kim", result[1].Name);
        Assert.AreEqual(30, result[1].Age);
        Assert.AreEqual("Lee", result[2].Name);
        Assert.AreEqual(25, result[2].Age);
        Assert.AreEqual("Lee", result[3].Name);
        Assert.AreEqual(35, result[3].Age);
    }

    #endregion

    #region ThenByDescending 테스트

    /// <summary>
    /// ThenByDescending이 2차 내림차순 정렬을 수행하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ThenByDescending_복합정렬_1차오름차순2차내림차순()
    {
        var source = new[]
        {
            new { Category = "A", Price = 100 },
            new { Category = "B", Price = 200 },
            new { Category = "A", Price = 150 },
            new { Category = "B", Price = 50 }
        };

        var result = source
            .OrderBy(x => x.Category)
            .ThenByDescending(x => x.Price)
            .ToArray();

        Assert.AreEqual("A", result[0].Category);
        Assert.AreEqual(150, result[0].Price);
        Assert.AreEqual("A", result[1].Category);
        Assert.AreEqual(100, result[1].Price);
        Assert.AreEqual("B", result[2].Category);
        Assert.AreEqual(200, result[2].Price);
        Assert.AreEqual("B", result[3].Category);
        Assert.AreEqual(50, result[3].Price);
    }

    #endregion

    #region 정렬 안정성 테스트

    /// <summary>
    /// 정렬이 안정적인지 (동일 키 요소의 상대적 순서 유지) 테스트합니다.
    /// </summary>
    [Test]
    public void OrderBy_정렬안정성_동일키순서유지()
    {
        var source = new[]
        {
            new { Id = 1, Key = "A" },
            new { Id = 2, Key = "B" },
            new { Id = 3, Key = "A" },
            new { Id = 4, Key = "B" },
            new { Id = 5, Key = "A" }
        };

        var result = source.OrderBy(x => x.Key).ToArray();

        // Key가 "A"인 요소들의 Id 순서: 1, 3, 5
        var aItems = result.Where(x => x.Key == "A").ToArray();
        Assert.AreEqual(1, aItems[0].Id);
        Assert.AreEqual(3, aItems[1].Id);
        Assert.AreEqual(5, aItems[2].Id);
    }

    #endregion
}
