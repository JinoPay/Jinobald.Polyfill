using System;
using Xunit;

namespace Jinobald.Polyfill.Tests.System
{
    public class ActionTests
    {
        [Fact]
        public void Action_NoParameters_ExecutesSuccessfully()
        {
            bool executed = false;
            Action action = () => { executed = true; };

            action();

            Assert.True(executed);
        }

        [Fact]
        public void Action_OneParameter_ExecutesWithParameter()
        {
            int result = 0;
            Action<int> action = (x) => { result = x; };

            action(42);

            Assert.Equal(42, result);
        }

        [Fact]
        public void Action_TwoParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int> action = (x, y) => { result = x + y; };

            action(10, 20);

            Assert.Equal(30, result);
        }

        [Fact]
        public void Action_ThreeParameters_ExecutesWithParameters()
        {
            int result = 0;
            Action<int, int, int> action = (x, y, z) => { result = x + y + z; };

            action(10, 20, 30);

            Assert.Equal(60, result);
        }

        [Fact]
        public void Func_NoParameters_ReturnsValue()
        {
            Func<int> func = () => 42;

            int result = func();

            Assert.Equal(42, result);
        }

        [Fact]
        public void Func_OneParameter_ReturnsTransformedValue()
        {
            Func<int, int> func = (x) => x * 2;

            int result = func(21);

            Assert.Equal(42, result);
        }

        [Fact]
        public void Func_TwoParameters_ReturnsSum()
        {
            Func<int, int, int> func = (x, y) => x + y;

            int result = func(20, 22);

            Assert.Equal(42, result);
        }

        [Fact]
        public void Predicate_ValidCondition_ReturnsTrue()
        {
            Predicate<int> predicate = (x) => x > 40;

            bool result = predicate(42);

            Assert.True(result);
        }

        [Fact]
        public void Predicate_InvalidCondition_ReturnsFalse()
        {
            Predicate<int> predicate = (x) => x > 40;

            bool result = predicate(10);

            Assert.False(result);
        }

        [Fact]
        public void Lazy_ValueNotCreatedInitially()
        {
            var lazy = new Lazy<int>(() => 42);

            Assert.False(lazy.IsValueCreated);
        }

        [Fact]
        public void Lazy_ValueCreatedOnFirstAccess()
        {
            var lazy = new Lazy<int>(() => 42);

            int value = lazy.Value;

            Assert.True(lazy.IsValueCreated);
            Assert.Equal(42, value);
        }

        [Fact]
        public void Lazy_ValueCachedAfterFirstAccess()
        {
            int callCount = 0;
            var lazy = new Lazy<int>(() => { callCount++; return 42; });

            int value1 = lazy.Value;
            int value2 = lazy.Value;

            Assert.Equal(1, callCount);
            Assert.Equal(42, value1);
            Assert.Equal(42, value2);
        }

        [Fact]
        public void Lazy_WithDirectValue()
        {
            var lazy = new Lazy<int>(() => 42);

            int value = lazy.Value;

            Assert.True(lazy.IsValueCreated);
            Assert.Equal(42, value);
        }

        [Fact]
        public void Lazy_DefaultConstructor()
        {
            var lazy = new Lazy<int>();

            int value = lazy.Value;

            Assert.True(lazy.IsValueCreated);
            Assert.Equal(0, value);
        }
    }
}
