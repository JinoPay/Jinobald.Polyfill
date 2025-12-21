// Jinobald.Polyfill - LINQ 조인 연산자 테스트

using System.Linq;
using Xunit;

namespace Jinobald.Polyfill.Tests.System.Linq;

/// <summary>
/// Join, GroupJoin 연산자에 대한 테스트입니다.
/// </summary>
public class EnumerableJoinTests
{
    #region Join 테스트

    /// <summary>
    /// Join이 일치하는 키로 두 시퀀스를 조인하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Join_일치하는키_조인결과반환()
    {
        var categories = new[]
        {
            new { Id = 1, Name = "과일" },
            new { Id = 2, Name = "채소" }
        };
        var products = new[]
        {
            new { Name = "사과", CategoryId = 1 },
            new { Name = "바나나", CategoryId = 1 },
            new { Name = "당근", CategoryId = 2 }
        };

        var result = categories.Join(
            products,
            c => c.Id,
            p => p.CategoryId,
            (c, p) => new { Category = c.Name, Product = p.Name }
        ).ToArray();

        Assert.Equal(3, result.Length);
        Assert.Contains(result, r => r.Category == "과일" && r.Product == "사과");
        Assert.Contains(result, r => r.Category == "과일" && r.Product == "바나나");
        Assert.Contains(result, r => r.Category == "채소" && r.Product == "당근");
    }

    /// <summary>
    /// Join이 일치하지 않는 요소를 제외하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Join_일치하지않는키_제외()
    {
        var outer = new[] { 1, 2, 3 };
        var inner = new[] { 2, 3, 4 };

        var result = outer.Join(inner, o => o, i => i, (o, i) => o).ToArray();

        Assert.Equal(2, result.Length);
        Assert.Contains(2, result);
        Assert.Contains(3, result);
    }

    /// <summary>
    /// Join이 빈 시퀀스를 처리하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Join_빈시퀀스_빈결과()
    {
        var outer = new[] { 1, 2, 3 };
        var inner = Enumerable.Empty<int>();

        var result = outer.Join(inner, o => o, i => i, (o, i) => o).ToArray();

        Assert.Empty(result);
    }

    /// <summary>
    /// Join이 null source에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Join_NullOuter_예외발생()
    {
        int[]? outer = null;
        var inner = new[] { 1, 2 };

        Assert.Throws<ArgumentNullException>(() =>
            outer!.Join(inner, o => o, i => i, (o, i) => o).ToArray());
    }

    #endregion

    #region GroupJoin 테스트

    /// <summary>
    /// GroupJoin이 일치하는 요소를 그룹화하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void GroupJoin_일치하는키_그룹화()
    {
        var categories = new[]
        {
            new { Id = 1, Name = "과일" },
            new { Id = 2, Name = "채소" }
        };
        var products = new[]
        {
            new { Name = "사과", CategoryId = 1 },
            new { Name = "바나나", CategoryId = 1 },
            new { Name = "당근", CategoryId = 2 }
        };

        var result = categories.GroupJoin(
            products,
            c => c.Id,
            p => p.CategoryId,
            (c, ps) => new { Category = c.Name, Count = ps.Count() }
        ).ToArray();

        Assert.Equal(2, result.Length);
        Assert.Equal(2, result.First(r => r.Category == "과일").Count);
        Assert.Equal(1, result.First(r => r.Category == "채소").Count);
    }

    /// <summary>
    /// GroupJoin이 일치하지 않는 외부 요소에 빈 그룹을 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void GroupJoin_일치하지않는키_빈그룹()
    {
        var outer = new[] { 1, 2, 3 };
        var inner = new[] { 1, 1, 2 };

        var result = outer.GroupJoin(
            inner,
            o => o,
            i => i,
            (o, items) => new { Key = o, Count = items.Count() }
        ).ToArray();

        Assert.Equal(3, result.Length);
        Assert.Equal(2, result.First(r => r.Key == 1).Count);
        Assert.Equal(1, result.First(r => r.Key == 2).Count);
        Assert.Equal(0, result.First(r => r.Key == 3).Count);
    }

    /// <summary>
    /// GroupJoin이 Left Outer Join처럼 동작하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void GroupJoin_LeftOuterJoin패턴()
    {
        var departments = new[] { "개발", "영업", "인사" };
        var employees = new[]
        {
            new { Name = "김철수", Dept = "개발" },
            new { Name = "이영희", Dept = "개발" },
            new { Name = "박민수", Dept = "영업" }
        };

        var result = departments.GroupJoin(
            employees,
            d => d,
            e => e.Dept,
            (d, emps) => new { Dept = d, Employees = emps.Select(e => e.Name).ToArray() }
        ).ToArray();

        Assert.Equal(3, result.Length);
        Assert.Equal(2, result.First(r => r.Dept == "개발").Employees.Length);
        Assert.Single(result.First(r => r.Dept == "영업").Employees);
        Assert.Empty(result.First(r => r.Dept == "인사").Employees);
    }

    #endregion
}
