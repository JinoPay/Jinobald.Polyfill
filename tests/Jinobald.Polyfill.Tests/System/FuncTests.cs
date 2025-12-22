using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System;

public class FuncTests
{
    [Test]
    public void Func_NoParameters_ReturnsValue()
    {
        Func<int> func = () => 42;
        Assert.AreEqual(42, func());
    }

    [Test]
    public void Func_OneParameter_ReturnsValue()
    {
        Func<int, int> func = x => x * 2;
        Assert.AreEqual(10, func(5));
    }

    [Test]
    public void Func_TwoParameters_ReturnsValue()
    {
        Func<int, int, int> func = (x, y) => x + y;
        Assert.AreEqual(7, func(3, 4));
    }

    [Test]
    public void Func_ThreeParameters_ReturnsValue()
    {
        Func<int, int, int, int> func = (x, y, z) => x + y + z;
        Assert.AreEqual(15, func(5, 6, 4));
    }

    [Test]
    public void Func_FourParameters_ReturnsValue()
    {
        Func<int, int, int, int, int> func = (a, b, c, d) => a + b + c + d;
        Assert.AreEqual(10, func(1, 2, 3, 4));
    }

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET472 || NET48
    [Test]
    public void Func_FiveParameters_ReturnsValue()
    {
        Func<int, int, int, int, int, int> func = (a, b, c, d, e) => a + b + c + d + e;
        Assert.AreEqual(15, func(1, 2, 3, 4, 5));
    }
#endif

#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET472 || NET48
    [Test]
    public void Func_SixteenParameters_ReturnsValue()
    {
        Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func =
            (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) =>
                a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p;
        Assert.AreEqual(136, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
    }
#endif

    [Test]
    public void Func_WithString_ReturnsLength()
    {
        Func<string, int> func = s => s.Length;
        Assert.AreEqual(5, func("Hello"));
    }

    [Test]
    public void Func_Chaining_Works()
    {
        Func<int, int> double_it = x => x * 2;
        Func<int, int> square_it = x => x * x;

        int result = square_it(double_it(3)); // (3 * 2)^2 = 36
        Assert.AreEqual(36, result);
    }

    [Test]
    public void Func_NullParameter_ThrowsNullReferenceException()
    {
        Func<string?, int> func = s => s!.Length;
        Assert.Throws<NullReferenceException>(() => func(null));
    }
}
