// Jinobald.Polyfill - LINQ Zip 확장 메서드 테스트
// Zip 오버로드 (ValueTuple 반환, 3개 시퀀스)에 대한 단위 테스트

namespace Jinobald.Polyfill.Tests.System.Linq;

/// <summary>
///     EnumerableEx.Zip 메서드에 대한 테스트입니다.
/// </summary>
public class EnumerableZipTests
{
    #region Zip (ValueTuple 반환) 테스트

    [Test]
    public void Zip_ValueTuple_두_시퀀스_동일한_길이()
    {
        // 준비
        int[] first = new[] { 1, 2, 3 };
        string[] second = new[] { "a", "b", "c" };

        // 실행
        var result = first.Zip(second).ToArray();

        // 검증
        Assert.AreEqual(3, result.Length);
        Assert.AreEqual((1, "a"), result[0]);
        Assert.AreEqual((2, "b"), result[1]);
        Assert.AreEqual((3, "c"), result[2]);
    }

    [Test]
    public void Zip_ValueTuple_첫_번째_시퀀스가_더_짧음()
    {
        // 준비
        int[] first = new[] { 1, 2 };
        string[] second = new[] { "a", "b", "c", "d" };

        // 실행
        var result = first.Zip(second).ToArray();

        // 검증
        Assert.AreEqual(2, result.Length);
        Assert.AreEqual((1, "a"), result[0]);
        Assert.AreEqual((2, "b"), result[1]);
    }

    [Test]
    public void Zip_ValueTuple_두_번째_시퀀스가_더_짧음()
    {
        // 준비
        int[] first = new[] { 1, 2, 3, 4 };
        string[] second = new[] { "a", "b" };

        // 실행
        var result = first.Zip(second).ToArray();

        // 검증
        Assert.AreEqual(2, result.Length);
        Assert.AreEqual((1, "a"), result[0]);
        Assert.AreEqual((2, "b"), result[1]);
    }

    [Test]
    public void Zip_ValueTuple_빈_시퀀스()
    {
        // 준비
        int[] first = Array.Empty<int>();
        string[] second = new[] { "a", "b", "c" };

        // 실행
        var result = first.Zip(second).ToArray();

        // 검증
        Assert.AreEqual(0, result.Length);
    }

    [Test]
    public void Zip_ValueTuple_null_첫_번째_시퀀스_예외()
    {
        // 준비
        int[]? first = null;
        string[] second = new[] { "a", "b" };

        // 실행 및 검증
        Assert.Throws<ArgumentNullException>(() => first!.Zip(second).ToArray());
    }

    [Test]
    public void Zip_ValueTuple_null_두_번째_시퀀스_예외()
    {
        // 준비
        int[] first = new[] { 1, 2 };
        string[]? second = null;

        // 실행 및 검증
        Assert.Throws<ArgumentNullException>(() => first.Zip(second!).ToArray());
    }

    [Test]
    public void Zip_ValueTuple_튜플_구조_분해_사용()
    {
        // 준비
        int[] numbers = new[] { 1, 2, 3 };
        string[] letters = new[] { "one", "two", "three" };

        // 실행
        var result = new List<string>();
        foreach (var (num, letter) in numbers.Zip(letters))
        {
            result.Add($"{num}={letter}");
        }

        // 검증
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("1=one", result[0]);
        Assert.AreEqual("2=two", result[1]);
        Assert.AreEqual("3=three", result[2]);
    }

    #endregion

    #region Zip (3개 시퀀스) 테스트

    [Test]
    public void Zip_Three_세_시퀀스_동일한_길이()
    {
        // 준비
        int[] first = new[] { 1, 2, 3 };
        string[] second = new[] { "a", "b", "c" };
        double[] third = new[] { 1.1, 2.2, 3.3 };

        // 실행
        var result = first.Zip(second, third).ToArray();

        // 검증
        Assert.AreEqual(3, result.Length);
        Assert.AreEqual((1, "a", 1.1), result[0]);
        Assert.AreEqual((2, "b", 2.2), result[1]);
        Assert.AreEqual((3, "c", 3.3), result[2]);
    }

    [Test]
    public void Zip_Three_첫_번째_시퀀스가_가장_짧음()
    {
        // 준비
        int[] first = new[] { 1 };
        string[] second = new[] { "a", "b", "c" };
        double[] third = new[] { 1.1, 2.2, 3.3, 4.4 };

        // 실행
        var result = first.Zip(second, third).ToArray();

        // 검증
        Assert.AreEqual(1, result.Length);
        Assert.AreEqual((1, "a", 1.1), result[0]);
    }

    [Test]
    public void Zip_Three_두_번째_시퀀스가_가장_짧음()
    {
        // 준비
        int[] first = new[] { 1, 2, 3 };
        string[] second = new[] { "a" };
        double[] third = new[] { 1.1, 2.2, 3.3 };

        // 실행
        var result = first.Zip(second, third).ToArray();

        // 검증
        Assert.AreEqual(1, result.Length);
        Assert.AreEqual((1, "a", 1.1), result[0]);
    }

    [Test]
    public void Zip_Three_세_번째_시퀀스가_가장_짧음()
    {
        // 준비
        int[] first = new[] { 1, 2, 3 };
        string[] second = new[] { "a", "b", "c" };
        double[] third = new[] { 1.1, 2.2 };

        // 실행
        var result = first.Zip(second, third).ToArray();

        // 검증
        Assert.AreEqual(2, result.Length);
        Assert.AreEqual((1, "a", 1.1), result[0]);
        Assert.AreEqual((2, "b", 2.2), result[1]);
    }

    [Test]
    public void Zip_Three_빈_시퀀스_포함()
    {
        // 준비
        int[] first = new[] { 1, 2, 3 };
        string[] second = Array.Empty<string>();
        double[] third = new[] { 1.1, 2.2, 3.3 };

        // 실행
        var result = first.Zip(second, third).ToArray();

        // 검증
        Assert.AreEqual(0, result.Length);
    }

    [Test]
    public void Zip_Three_null_첫_번째_시퀀스_예외()
    {
        // 준비
        int[]? first = null;
        string[] second = new[] { "a", "b" };
        double[] third = new[] { 1.1, 2.2 };

        // 실행 및 검증
        Assert.Throws<ArgumentNullException>(() => first!.Zip(second, third).ToArray());
    }

    [Test]
    public void Zip_Three_null_두_번째_시퀀스_예외()
    {
        // 준비
        int[] first = new[] { 1, 2 };
        string[]? second = null;
        double[] third = new[] { 1.1, 2.2 };

        // 실행 및 검증
        Assert.Throws<ArgumentNullException>(() => first.Zip(second!, third).ToArray());
    }

    [Test]
    public void Zip_Three_null_세_번째_시퀀스_예외()
    {
        // 준비
        int[] first = new[] { 1, 2 };
        string[] second = new[] { "a", "b" };
        double[]? third = null;

        // 실행 및 검증
        Assert.Throws<ArgumentNullException>(() => first.Zip(second, third!).ToArray());
    }

    [Test]
    public void Zip_Three_튜플_구조_분해_사용()
    {
        // 준비
        int[] ids = new[] { 1, 2 };
        string[] names = new[] { "Alice", "Bob" };
        int[] ages = new[] { 25, 30 };

        // 실행
        var result = new List<string>();
        foreach (var (id, name, age) in ids.Zip(names, ages))
        {
            result.Add($"{id}: {name}, {age}세");
        }

        // 검증
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("1: Alice, 25세", result[0]);
        Assert.AreEqual("2: Bob, 30세", result[1]);
    }

    #endregion

    #region 지연 평가 테스트

    [Test]
    public void Zip_ValueTuple_지연_평가()
    {
        // 준비
        int enumerationCount = 0;
        IEnumerable<int> CreateSequence()
        {
            enumerationCount++;
            yield return 1;
            yield return 2;
        }

        var first = CreateSequence();
        var second = new[] { "a", "b" };

        // 실행 - Zip 호출만으로는 열거되지 않음
        var zipped = first.Zip(second);
        Assert.AreEqual(0, enumerationCount);

        // ToArray 호출 시 열거됨
        var result = zipped.ToArray();
        Assert.AreEqual(1, enumerationCount);
        Assert.AreEqual(2, result.Length);
    }

    [Test]
    public void Zip_Three_지연_평가()
    {
        // 준비
        int enumerationCount = 0;
        IEnumerable<int> CreateSequence()
        {
            enumerationCount++;
            yield return 1;
            yield return 2;
        }

        var first = CreateSequence();
        var second = new[] { "a", "b" };
        var third = new[] { 1.0, 2.0 };

        // 실행 - Zip 호출만으로는 열거되지 않음
        var zipped = first.Zip(second, third);
        Assert.AreEqual(0, enumerationCount);

        // ToArray 호출 시 열거됨
        var result = zipped.ToArray();
        Assert.AreEqual(1, enumerationCount);
        Assert.AreEqual(2, result.Length);
    }

    #endregion
}
