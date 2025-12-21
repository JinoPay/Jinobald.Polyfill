// Jinobald.Polyfill - LINQ 집합 연산자 테스트

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Jinobald.Polyfill.Tests.System.Linq;

/// <summary>
/// Union, Intersect, Except, Zip 연산자에 대한 테스트입니다.
/// </summary>
public class EnumerableSetTests
{
    #region Union 테스트

    /// <summary>
    /// Union이 두 시퀀스의 합집합을 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Union_두시퀀스_합집합반환()
    {
        var first = new[] { 1, 2, 3 };
        var second = new[] { 3, 4, 5 };

        var result = first.Union(second).ToArray();

        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, result);
    }

    /// <summary>
    /// Union이 중복을 제거하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Union_중복요소_제거()
    {
        var first = new[] { 1, 1, 2 };
        var second = new[] { 2, 3, 3 };

        var result = first.Union(second).ToArray();

        Assert.Equal(new[] { 1, 2, 3 }, result);
    }

    /// <summary>
    /// Union이 비교자를 사용하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Union_비교자_대소문자무시()
    {
        var first = new[] { "a", "B" };
        var second = new[] { "A", "c" };

        var result = first.Union(second, StringComparer.OrdinalIgnoreCase).ToArray();

        Assert.Equal(3, result.Length);
        Assert.Contains("a", result);
        Assert.Contains("B", result);
        Assert.Contains("c", result);
    }

    /// <summary>
    /// Union이 빈 시퀀스를 처리하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Union_빈시퀀스_다른시퀀스반환()
    {
        var first = Array.Empty<int>();
        var second = new[] { 1, 2, 3 };

        var result = first.Union(second).ToArray();

        Assert.Equal(new[] { 1, 2, 3 }, result);
    }

    /// <summary>
    /// Union이 null source에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Union_NullFirst_예외발생()
    {
        int[]? first = null;
        var second = new[] { 1, 2, 3 };

        Assert.Throws<ArgumentNullException>(() => first!.Union(second).ToArray());
    }

    /// <summary>
    /// Union이 null second에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Union_NullSecond_예외발생()
    {
        var first = new[] { 1, 2, 3 };
        int[]? second = null;

        Assert.Throws<ArgumentNullException>(() => first.Union(second!).ToArray());
    }

    #endregion

    #region Intersect 테스트

    /// <summary>
    /// Intersect가 두 시퀀스의 교집합을 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Intersect_두시퀀스_교집합반환()
    {
        var first = new[] { 1, 2, 3, 4 };
        var second = new[] { 3, 4, 5, 6 };

        var result = first.Intersect(second).ToArray();

        Assert.Equal(new[] { 3, 4 }, result);
    }

    /// <summary>
    /// Intersect가 중복을 제거하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Intersect_중복요소_한번만반환()
    {
        var first = new[] { 1, 1, 2, 2 };
        var second = new[] { 1, 2, 2, 3 };

        var result = first.Intersect(second).ToArray();

        Assert.Equal(new[] { 1, 2 }, result);
    }

    /// <summary>
    /// Intersect가 비교자를 사용하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Intersect_비교자_대소문자무시()
    {
        var first = new[] { "a", "B", "c" };
        var second = new[] { "A", "d" };

        var result = first.Intersect(second, StringComparer.OrdinalIgnoreCase).ToArray();

        Assert.Single(result);
        Assert.Equal("a", result[0]);
    }

    /// <summary>
    /// Intersect가 공통 요소가 없을 때 빈 시퀀스를 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Intersect_공통요소없음_빈시퀀스반환()
    {
        var first = new[] { 1, 2, 3 };
        var second = new[] { 4, 5, 6 };

        var result = first.Intersect(second).ToArray();

        Assert.Empty(result);
    }

    /// <summary>
    /// Intersect가 null source에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Intersect_NullFirst_예외발생()
    {
        int[]? first = null;
        var second = new[] { 1, 2, 3 };

        Assert.Throws<ArgumentNullException>(() => first!.Intersect(second).ToArray());
    }

    #endregion

    #region Except 테스트

    /// <summary>
    /// Except가 두 시퀀스의 차집합을 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Except_두시퀀스_차집합반환()
    {
        var first = new[] { 1, 2, 3, 4 };
        var second = new[] { 3, 4, 5, 6 };

        var result = first.Except(second).ToArray();

        Assert.Equal(new[] { 1, 2 }, result);
    }

    /// <summary>
    /// Except가 중복을 제거하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Except_중복요소_한번만반환()
    {
        var first = new[] { 1, 1, 2, 2, 3 };
        var second = new[] { 3 };

        var result = first.Except(second).ToArray();

        Assert.Equal(new[] { 1, 2 }, result);
    }

    /// <summary>
    /// Except가 비교자를 사용하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Except_비교자_대소문자무시()
    {
        var first = new[] { "a", "B", "c" };
        var second = new[] { "A", "C" };

        var result = first.Except(second, StringComparer.OrdinalIgnoreCase).ToArray();

        Assert.Single(result);
        Assert.Equal("B", result[0]);
    }

    /// <summary>
    /// Except가 second가 비어있을 때 first의 고유 요소를 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Except_빈Second_고유요소반환()
    {
        var first = new[] { 1, 2, 2, 3 };
        var second = Array.Empty<int>();

        var result = first.Except(second).ToArray();

        Assert.Equal(new[] { 1, 2, 3 }, result);
    }

    /// <summary>
    /// Except가 모든 요소가 제거될 때 빈 시퀀스를 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Except_모든요소제거_빈시퀀스반환()
    {
        var first = new[] { 1, 2, 3 };
        var second = new[] { 1, 2, 3, 4, 5 };

        var result = first.Except(second).ToArray();

        Assert.Empty(result);
    }

    /// <summary>
    /// Except가 null source에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Except_NullFirst_예외발생()
    {
        int[]? first = null;
        var second = new[] { 1, 2, 3 };

        Assert.Throws<ArgumentNullException>(() => first!.Except(second).ToArray());
    }

    #endregion

    #region Zip 테스트

    /// <summary>
    /// Zip이 두 시퀀스를 병합하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Zip_두시퀀스_병합()
    {
        var first = new[] { 1, 2, 3 };
        var second = new[] { "a", "b", "c" };

        var result = first.Zip(second, (x, y) => $"{x}{y}").ToArray();

        Assert.Equal(new[] { "1a", "2b", "3c" }, result);
    }

    /// <summary>
    /// Zip이 첫 번째 시퀀스가 짧을 때 해당 길이까지만 병합하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Zip_첫번째짧음_짧은길이까지병합()
    {
        var first = new[] { 1, 2 };
        var second = new[] { "a", "b", "c", "d" };

        var result = first.Zip(second, (x, y) => $"{x}{y}").ToArray();

        Assert.Equal(new[] { "1a", "2b" }, result);
    }

    /// <summary>
    /// Zip이 두 번째 시퀀스가 짧을 때 해당 길이까지만 병합하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Zip_두번째짧음_짧은길이까지병합()
    {
        var first = new[] { 1, 2, 3, 4 };
        var second = new[] { "a", "b" };

        var result = first.Zip(second, (x, y) => $"{x}{y}").ToArray();

        Assert.Equal(new[] { "1a", "2b" }, result);
    }

    /// <summary>
    /// Zip이 빈 시퀀스를 처리하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Zip_빈시퀀스_빈결과반환()
    {
        var first = Array.Empty<int>();
        var second = new[] { "a", "b", "c" };

        var result = first.Zip(second, (x, y) => $"{x}{y}").ToArray();

        Assert.Empty(result);
    }

    /// <summary>
    /// Zip이 결과 선택기를 올바르게 적용하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Zip_결과선택기_객체생성()
    {
        var numbers = new[] { 1, 2, 3 };
        var names = new[] { "one", "two", "three" };

        var result = numbers.Zip(names, (n, s) => new { Number = n, Name = s }).ToArray();

        Assert.Equal(3, result.Length);
        Assert.Equal(1, result[0].Number);
        Assert.Equal("one", result[0].Name);
        Assert.Equal(2, result[1].Number);
        Assert.Equal("two", result[1].Name);
    }

    /// <summary>
    /// Zip이 null first에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Zip_NullFirst_예외발생()
    {
        int[]? first = null;
        var second = new[] { "a", "b" };

        Assert.Throws<ArgumentNullException>(() => first!.Zip(second, (x, y) => $"{x}{y}").ToArray());
    }

    /// <summary>
    /// Zip이 null second에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Zip_NullSecond_예외발생()
    {
        var first = new[] { 1, 2, 3 };
        string[]? second = null;

        Assert.Throws<ArgumentNullException>(() => first.Zip(second!, (x, y) => $"{x}{y}").ToArray());
    }

    /// <summary>
    /// Zip이 null resultSelector에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Zip_NullResultSelector_예외발생()
    {
        var first = new[] { 1, 2, 3 };
        var second = new[] { "a", "b", "c" };
        Func<int, string, string>? selector = null;

        Assert.Throws<ArgumentNullException>(() => first.Zip(second, selector!).ToArray());
    }

    #endregion
}
