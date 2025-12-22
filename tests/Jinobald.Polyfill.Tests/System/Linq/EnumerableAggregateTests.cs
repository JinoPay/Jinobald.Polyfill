// Jinobald.Polyfill - LINQ 집계 연산자 테스트

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System.Linq;

/// <summary>
/// Aggregate, Sum, Average, Min, Max 연산자에 대한 테스트입니다.
/// </summary>
public class EnumerableAggregateTests
{
    #region Aggregate 테스트

    /// <summary>
    /// Aggregate가 시퀀스를 누적하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Aggregate_시퀀스누적_결합문자열()
    {
        var source = new[] { "a", "b", "c", "d" };

        var result = source.Aggregate((current, next) => current + next);

        Assert.AreEqual("abcd", result);
    }

    /// <summary>
    /// Aggregate가 시드값을 사용하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Aggregate_시드값사용_초기값포함()
    {
        var source = new[] { 1, 2, 3 };

        var result = source.Aggregate(10, (acc, x) => acc + x);

        Assert.AreEqual(16, result); // 10 + 1 + 2 + 3
    }

    /// <summary>
    /// Aggregate가 결과 선택기를 사용하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Aggregate_결과선택기_변환적용()
    {
        var source = new[] { 1, 2, 3, 4 };

        var result = source.Aggregate(0, (acc, x) => acc + x, sum => sum * 2);

        Assert.AreEqual(20, result); // (1+2+3+4) * 2
    }

    /// <summary>
    /// Aggregate가 빈 시퀀스에서 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Aggregate_빈시퀀스_예외발생()
    {
        var source = Array.Empty<int>();

        Assert.Throws<InvalidOperationException>(() => source.Aggregate((a, b) => a + b));
    }

    /// <summary>
    /// Aggregate가 단일 요소에 대해 작동하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Aggregate_단일요소_해당요소반환()
    {
        var source = new[] { 42 };

        var result = source.Aggregate((a, b) => a + b);

        Assert.AreEqual(42, result);
    }

    /// <summary>
    /// Aggregate가 null source에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Aggregate_NullSource_예외발생()
    {
        int[]? source = null;

        Assert.Throws<ArgumentNullException>(() => source!.Aggregate((a, b) => a + b));
    }

    #endregion

    #region Sum 테스트

    /// <summary>
    /// Sum이 int 시퀀스의 합을 계산하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Sum_Int_합계계산()
    {
        var source = new[] { 1, 2, 3, 4, 5 };

        var result = source.Sum();

        Assert.AreEqual(15, result);
    }

    /// <summary>
    /// Sum이 빈 시퀀스에 대해 0을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Sum_빈시퀀스_0반환()
    {
        var source = Array.Empty<int>();

        var result = source.Sum();

        Assert.AreEqual(0, result);
    }

    /// <summary>
    /// Sum이 선택기를 사용하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Sum_선택기_투영값합계()
    {
        var source = new[] { "a", "bb", "ccc" };

        var result = source.Sum(s => s.Length);

        Assert.AreEqual(6, result); // 1 + 2 + 3
    }

    /// <summary>
    /// Sum이 long 시퀀스의 합을 계산하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Sum_Long_합계계산()
    {
        var source = new[] { 1L, 2L, 3L };

        var result = source.Sum();

        Assert.AreEqual(6L, result);
    }

    /// <summary>
    /// Sum이 double 시퀀스의 합을 계산하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Sum_Double_합계계산()
    {
        var source = new[] { 1.5, 2.5, 3.0 };

        var result = source.Sum();

        Assert.AreEqual(7.0, result);
    }

    /// <summary>
    /// Sum이 decimal 시퀀스의 합을 계산하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Sum_Decimal_합계계산()
    {
        var source = new[] { 1.1m, 2.2m, 3.3m };

        var result = source.Sum();

        Assert.AreEqual(6.6m, result);
    }

    /// <summary>
    /// Sum이 nullable int 시퀀스의 합을 계산하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Sum_NullableInt_Null무시()
    {
        var source = new int?[] { 1, null, 2, null, 3 };

        var result = source.Sum();

        Assert.AreEqual(6, result);
    }

    /// <summary>
    /// Sum이 null source에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Sum_NullSource_예외발생()
    {
        int[]? source = null;

        Assert.Throws<ArgumentNullException>(() => source!.Sum());
    }

    #endregion

    #region Average 테스트

    /// <summary>
    /// Average가 int 시퀀스의 평균을 계산하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Average_Int_평균계산()
    {
        var source = new[] { 1, 2, 3, 4, 5 };

        var result = source.Average();

        Assert.AreEqual(3.0, result);
    }

    /// <summary>
    /// Average가 빈 시퀀스에서 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Average_빈시퀀스_예외발생()
    {
        var source = Array.Empty<int>();

        Assert.Throws<InvalidOperationException>(() => source.Average());
    }

    /// <summary>
    /// Average가 선택기를 사용하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Average_선택기_투영값평균()
    {
        var source = new[] { "a", "bb", "ccc", "dddd" };

        var result = source.Average(s => s.Length);

        Assert.AreEqual(2.5, result); // (1+2+3+4) / 4
    }

    /// <summary>
    /// Average가 double 시퀀스의 평균을 계산하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Average_Double_평균계산()
    {
        var source = new[] { 1.0, 2.0, 3.0 };

        var result = source.Average();

        Assert.AreEqual(2.0, result);
    }

    /// <summary>
    /// Average가 decimal 시퀀스의 평균을 계산하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Average_Decimal_평균계산()
    {
        var source = new[] { 1.0m, 2.0m, 3.0m };

        var result = source.Average();

        Assert.AreEqual(2.0m, result);
    }

    /// <summary>
    /// Average가 nullable int 시퀀스의 평균을 계산하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Average_NullableInt_Null무시()
    {
        var source = new int?[] { 1, null, 2, null, 3 };

        var result = source.Average();

        Assert.AreEqual(2.0, result); // (1+2+3) / 3
    }

    /// <summary>
    /// Average가 null만 있는 nullable 시퀀스에서 null을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Average_NullableOnly_Null반환()
    {
        var source = new int?[] { null, null };

        var result = source.Average();

        Assert.IsNull(result);
    }

    /// <summary>
    /// Average가 null source에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Average_NullSource_예외발생()
    {
        int[]? source = null;

        Assert.Throws<ArgumentNullException>(() => source!.Average());
    }

    #endregion

    #region Min 테스트

    /// <summary>
    /// Min이 int 시퀀스의 최소값을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Min_Int_최소값반환()
    {
        var source = new[] { 5, 2, 8, 1, 9 };

        var result = source.Min();

        Assert.AreEqual(1, result);
    }

    /// <summary>
    /// Min이 빈 시퀀스에서 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Min_빈시퀀스_예외발생()
    {
        var source = Array.Empty<int>();

        Assert.Throws<InvalidOperationException>(() => source.Min());
    }

    /// <summary>
    /// Min이 선택기를 사용하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Min_선택기_투영값최소()
    {
        var source = new[] { "aaa", "b", "cc" };

        var result = source.Min(s => s.Length);

        Assert.AreEqual(1, result);
    }

    /// <summary>
    /// Min이 double 시퀀스의 최소값을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Min_Double_최소값반환()
    {
        var source = new[] { 3.5, 1.2, 4.8 };

        var result = source.Min();

        Assert.AreEqual(1.2, result);
    }

    /// <summary>
    /// Min이 nullable int 시퀀스에서 null을 무시하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Min_NullableInt_Null무시()
    {
        var source = new int?[] { 5, null, 2, null, 8 };

        var result = source.Min();

        Assert.AreEqual(2, result);
    }

    /// <summary>
    /// Min이 null만 있는 nullable 시퀀스에서 null을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Min_NullableOnly_Null반환()
    {
        var source = new int?[] { null, null };

        var result = source.Min();

        Assert.IsNull(result);
    }

    /// <summary>
    /// Min이 제네릭 시퀀스의 최소값을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Min_제네릭_최소값반환()
    {
        var source = new[] { "banana", "apple", "cherry" };

        var result = source.Min();

        Assert.AreEqual("apple", result);
    }

    /// <summary>
    /// Min이 null source에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Min_NullSource_예외발생()
    {
        int[]? source = null;

        Assert.Throws<ArgumentNullException>(() => source!.Min());
    }

    #endregion

    #region Max 테스트

    /// <summary>
    /// Max가 int 시퀀스의 최대값을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Max_Int_최대값반환()
    {
        var source = new[] { 5, 2, 8, 1, 9 };

        var result = source.Max();

        Assert.AreEqual(9, result);
    }

    /// <summary>
    /// Max가 빈 시퀀스에서 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Max_빈시퀀스_예외발생()
    {
        var source = Array.Empty<int>();

        Assert.Throws<InvalidOperationException>(() => source.Max());
    }

    /// <summary>
    /// Max가 선택기를 사용하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Max_선택기_투영값최대()
    {
        var source = new[] { "aaa", "b", "cc" };

        var result = source.Max(s => s.Length);

        Assert.AreEqual(3, result);
    }

    /// <summary>
    /// Max가 double 시퀀스의 최대값을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Max_Double_최대값반환()
    {
        var source = new[] { 3.5, 1.2, 4.8 };

        var result = source.Max();

        Assert.AreEqual(4.8, result);
    }

    /// <summary>
    /// Max가 nullable int 시퀀스에서 null을 무시하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Max_NullableInt_Null무시()
    {
        var source = new int?[] { 5, null, 2, null, 8 };

        var result = source.Max();

        Assert.AreEqual(8, result);
    }

    /// <summary>
    /// Max가 null만 있는 nullable 시퀀스에서 null을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Max_NullableOnly_Null반환()
    {
        var source = new int?[] { null, null };

        var result = source.Max();

        Assert.IsNull(result);
    }

    /// <summary>
    /// Max가 제네릭 시퀀스의 최대값을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Max_제네릭_최대값반환()
    {
        var source = new[] { "banana", "apple", "cherry" };

        var result = source.Max();

        Assert.AreEqual("cherry", result);
    }

    /// <summary>
    /// Max가 null source에 대해 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Max_NullSource_예외발생()
    {
        int[]? source = null;

        Assert.Throws<ArgumentNullException>(() => source!.Max());
    }

    #endregion

    #region 추가 테스트

    /// <summary>
    /// Sum이 음수를 포함한 시퀀스를 처리하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Sum_음수포함_정확한합계()
    {
        var source = new[] { -5, 3, -2, 4 };

        var result = source.Sum();

        Assert.AreEqual(0, result);
    }

    /// <summary>
    /// Average가 소수점 결과를 올바르게 계산하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Average_소수점결과_정확한평균()
    {
        var source = new[] { 1, 2 };

        var result = source.Average();

        Assert.AreEqual(1.5, result);
    }

    /// <summary>
    /// Min이 단일 요소에 대해 해당 요소를 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Min_단일요소_해당요소반환()
    {
        var source = new[] { 42 };

        var result = source.Min();

        Assert.AreEqual(42, result);
    }

    /// <summary>
    /// Max가 단일 요소에 대해 해당 요소를 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Max_단일요소_해당요소반환()
    {
        var source = new[] { 42 };

        var result = source.Max();

        Assert.AreEqual(42, result);
    }

    /// <summary>
    /// Min과 Max가 동일한 값을 가진 시퀀스에서 작동하는지 테스트합니다.
    /// </summary>
    [Test]
    public void MinMax_동일값시퀀스_해당값반환()
    {
        var source = new[] { 5, 5, 5, 5 };

        Assert.AreEqual(5, source.Min());
        Assert.AreEqual(5, source.Max());
    }

    #endregion
}
