using Xunit;

namespace Jinobald.Polyfill.Tests.System;

public class PredicateTests
{
    [Fact]
    public void Predicate_ReturnsTrue_WhenConditionMet()
    {
        Predicate<int> isEven = x => x % 2 == 0;
        Assert.True(isEven(4));
    }

    [Fact]
    public void Predicate_ReturnsFalse_WhenConditionNotMet()
    {
        Predicate<int> isEven = x => x % 2 == 0;
        Assert.False(isEven(3));
    }

    [Fact]
    public void Predicate_WithString_ChecksLength()
    {
        Predicate<string> isLongString = s => s.Length > 5;
        Assert.True(isLongString("Hello World"));
        Assert.False(isLongString("Hi"));
    }

    [Fact]
    public void Predicate_WithNull_CanHandleNullCheck()
    {
        Predicate<string> isNotNull = s => s != null;
        Assert.True(isNotNull("test"));
        Assert.False(isNotNull(null));
    }

    [Fact]
    public void Predicate_WithComplexType_Works()
    {
        Predicate<Person> isAdult = p => p.Age >= 18;

        var adult = new Person { Age = 25 };
        var child = new Person { Age = 15 };

        Assert.True(isAdult(adult));
        Assert.False(isAdult(child));
    }

    [Fact]
    public void Predicate_CombinedWithAnd_Works()
    {
        Predicate<int> isPositive = x => x > 0;
        Predicate<int> isEven = x => x % 2 == 0;
        Predicate<int> isPositiveAndEven = x => isPositive(x) && isEven(x);

        Assert.True(isPositiveAndEven(4));
        Assert.False(isPositiveAndEven(3));
        Assert.False(isPositiveAndEven(-4));
    }

    [Fact]
    public void Predicate_UsedWithArrayFind_Works()
    {
        int[] numbers = { 1, 2, 3, 4, 5, 6 };
        Predicate<int> isGreaterThanThree = x => x > 3;

        int result = Array.Find(numbers, isGreaterThanThree);
        Assert.Equal(4, result);
    }

    private class Person
    {
        public int Age { get; set; }
    }
}
