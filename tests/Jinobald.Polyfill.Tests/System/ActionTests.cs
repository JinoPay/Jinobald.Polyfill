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
