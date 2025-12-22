// Jinobald.Polyfill - LINQ 그룹화 연산자 테스트

using System.Linq;
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System.Linq;

/// <summary>
/// GroupBy, ToLookup 연산자에 대한 테스트입니다.
/// </summary>
public class EnumerableGroupingTests
{
    #region GroupBy 테스트

    /// <summary>
    /// GroupBy가 키로 그룹화하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GroupBy_키선택기_그룹화()
    {
        var source = new[] { "apple", "banana", "apricot", "blueberry", "avocado" };
        var groups = source.GroupBy(x => x[0]).ToArray();

        Assert.AreEqual(2, groups.Length);
        Assert.AreEqual('a', groups[0].Key);
        Assert.AreEqual(3, groups[0].Count());
        Assert.AreEqual('b', groups[1].Key);
        Assert.AreEqual(2, groups[1].Count());
    }

    /// <summary>
    /// GroupBy가 요소 선택기를 적용하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GroupBy_요소선택기_변환적용()
    {
        var source = new[] { "apple", "banana", "apricot" };
        var groups = source.GroupBy(x => x[0], x => x.Length).ToArray();

        Assert.AreEqual('a', groups[0].Key);
        Assert.Contains(5, groups[0]);  // "apple"
        Assert.Contains(7, groups[0]);  // "apricot"
    }

    /// <summary>
    /// GroupBy가 결과 선택기를 적용하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GroupBy_결과선택기_집계()
    {
        var source = new[] { 1, 2, 3, 4, 5, 6 };
        var results = source.GroupBy(
            x => x % 2,
            (key, items) => new { IsEven = key == 0, Sum = items.Sum() }
        ).ToArray();

        Assert.AreEqual(2, results.Length);
        var odd = results.First(r => !r.IsEven);
        var even = results.First(r => r.IsEven);
        Assert.AreEqual(9, odd.Sum);   // 1+3+5
        Assert.AreEqual(12, even.Sum); // 2+4+6
    }

    /// <summary>
    /// GroupBy가 null source에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GroupBy_NullSource_예외발생()
    {
        int[]? source = null;
        Assert.Throws<ArgumentNullException>(() => source!.GroupBy(x => x).ToArray());
    }

    #endregion

    #region ToLookup 테스트

    /// <summary>
    /// ToLookup이 키로 그룹화하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ToLookup_키선택기_Lookup생성()
    {
        var source = new[] { "apple", "banana", "apricot", "blueberry" };
        var lookup = source.ToLookup(x => x[0]);

        Assert.AreEqual(2, lookup.Count);
        Assert.AreEqual(2, lookup['a'].Count());
        Assert.AreEqual(2, lookup['b'].Count());
    }

    /// <summary>
    /// ToLookup이 없는 키에 대해 빈 시퀀스를 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ToLookup_없는키_빈시퀀스반환()
    {
        var source = new[] { "apple", "banana" };
        var lookup = source.ToLookup(x => x[0]);

        Assert.IsEmpty(lookup['z']);
    }

    /// <summary>
    /// ToLookup이 Contains를 올바르게 처리하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ToLookup_Contains_키존재여부()
    {
        var source = new[] { "apple", "banana" };
        var lookup = source.ToLookup(x => x[0]);

        Assert.IsTrue(lookup.Contains('a'));
        Assert.IsTrue(lookup.Contains('b'));
        Assert.IsFalse(lookup.Contains('c'));
    }

    #endregion
}
