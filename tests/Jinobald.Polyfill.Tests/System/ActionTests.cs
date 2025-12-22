using System;
using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System
{
    public class ActionTests
    {
        [Test]
        public void Action_NoParameters_ExecutesSuccessfully()
        {
            bool executed = false;
            Action action = () => { executed = true; };

            action();

            Assert.IsTrue(executed);
        }

        [Test]
        public void Action_OneParameter_ExecutesWithParameter()
        {
            int result = 0;
            Action<int> action = (x) => { result = x; };

            action(42);

            Assert.AreEqual(42, result);
        }

        [Test]
        public void Action_TwoParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int> action = (x, y) => { result = x + y; };

            action(10, 20);

            Assert.AreEqual(30, result);
        }

        [Test]
        public void Action_ThreeParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int> action = (x, y, z) => { result = x + y + z; };

            action(10, 20, 30);

            Assert.AreEqual(60, result);
        }

        [Test]
        public void Action_FourParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int> action = (a, b, c, d) => { result = a + b + c + d; };

            action(1, 2, 3, 4);

            Assert.AreEqual(10, result);
        }

        [Test]
        public void Action_FiveParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int, int> action = (a, b, c, d, e) => { result = a + b + c + d + e; };

            action(1, 2, 3, 4, 5);

            Assert.AreEqual(15, result);
        }

        [Test]
        public void Action_SixParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int, int, int> action = (a, b, c, d, e, f) => { result = a + b + c + d + e + f; };

            action(1, 2, 3, 4, 5, 6);

            Assert.AreEqual(21, result);
        }

        [Test]
        public void Action_SevenParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int, int, int, int> action = (a, b, c, d, e, f, g) => { result = a + b + c + d + e + f + g; };

            action(1, 2, 3, 4, 5, 6, 7);

            Assert.AreEqual(28, result);
        }

        [Test]
        public void Action_EightParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int, int, int, int, int> action = (a, b, c, d, e, f, g, h) => { result = a + b + c + d + e + f + g + h; };

            action(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.AreEqual(36, result);
        }

        [Test]
        public void Action_NineParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int, int, int, int, int, int> action = (a, b, c, d, e, f, g, h, i) => { result = a + b + c + d + e + f + g + h + i; };

            action(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual(45, result);
        }

        [Test]
        public void Action_TenParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int, int, int, int, int, int, int> action = (a, b, c, d, e, f, g, h, i, j) => { result = a + b + c + d + e + f + g + h + i + j; };

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.AreEqual(55, result);
        }

        [Test]
        public void Action_ElevenParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int, int, int, int, int, int, int, int> action = (a, b, c, d, e, f, g, h, i, j, k) => { result = a + b + c + d + e + f + g + h + i + j + k; };

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

            Assert.AreEqual(66, result);
        }

        [Test]
        public void Action_TwelveParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int, int, int, int, int, int, int, int, int> action = (a, b, c, d, e, f, g, h, i, j, k, l) => { result = a + b + c + d + e + f + g + h + i + j + k + l; };

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Assert.AreEqual(78, result);
        }

        [Test]
        public void Action_ThirteenParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> action = (a, b, c, d, e, f, g, h, i, j, k, l, m) => { result = a + b + c + d + e + f + g + h + i + j + k + l + m; };

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            Assert.AreEqual(91, result);
        }

        [Test]
        public void Action_FourteenParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { result = a + b + c + d + e + f + g + h + i + j + k + l + m + n; };

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

            Assert.AreEqual(105, result);
        }

        [Test]
        public void Action_FifteenParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { result = a + b + c + d + e + f + g + h + i + j + k + l + m + n + o; };

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.AreEqual(120, result);
        }

        [Test]
        public void Action_SixteenParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { result = a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p; };

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.AreEqual(136, result);
        }

        [Test]
        public void Func_NoParameters_ReturnsValue()
        {
            Func<int> func = () => 42;

            int result = func();

            Assert.AreEqual(42, result);
        }

        [Test]
        public void Func_OneParameter_ReturnsTransformedValue()
        {
            Func<int, int> func = (x) => x * 2;

            int result = func(21);

            Assert.AreEqual(42, result);
        }

        [Test]
        public void Func_TwoParameters_ReturnsSum()
        {
            Func<int, int, int> func = (x, y) => x + y;

            int result = func(20, 22);

            Assert.AreEqual(42, result);
        }

        [Test]
        public void Predicate_ValidCondition_ReturnsTrue()
        {
            Predicate<int> predicate = (x) => x > 40;

            bool result = predicate(42);

            Assert.IsTrue(result);
        }

        [Test]
        public void Predicate_InvalidCondition_ReturnsFalse()
        {
            Predicate<int> predicate = (x) => x > 40;

            bool result = predicate(10);

            Assert.IsFalse(result);
        }

        [Test]
        public void Lazy_ValueNotCreatedInitially()
        {
            var lazy = new Lazy<int>(() => 42);

            Assert.IsFalse(lazy.IsValueCreated);
        }

        [Test]
        public void Lazy_ValueCreatedOnFirstAccess()
        {
            var lazy = new Lazy<int>(() => 42);

            int value = lazy.Value;

            Assert.IsTrue(lazy.IsValueCreated);
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Lazy_ValueCachedAfterFirstAccess()
        {
            int callCount = 0;
            var lazy = new Lazy<int>(() => { callCount++; return 42; });

            int value1 = lazy.Value;
            int value2 = lazy.Value;

            Assert.AreEqual(1, callCount);
            Assert.AreEqual(42, value1);
            Assert.AreEqual(42, value2);
        }

        [Test]
        public void Lazy_WithDirectValue()
        {
            var lazy = new Lazy<int>(() => 42);

            int value = lazy.Value;

            Assert.IsTrue(lazy.IsValueCreated);
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Lazy_DefaultConstructor()
        {
            var lazy = new Lazy<int>();

            int value = lazy.Value;

            Assert.IsTrue(lazy.IsValueCreated);
            Assert.AreEqual(0, value);
        }
    }
}
