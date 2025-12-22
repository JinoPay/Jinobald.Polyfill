using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System;

public class PredicateTests
{
    [Test]
    public void Predicate_ReturnsTrue_WhenConditionMet()
    {
        Predicate<int> isEven = x => x % 2 == 0;
        Assert.IsTrue(isEven(4));
    }

    [Test]
    public void Predicate_ReturnsFalse_WhenConditionNotMet()
    {
        Predicate<int> isEven = x => x % 2 == 0;
        Assert.IsFalse(isEven(3));
    }

    [Test]
    public void Predicate_WithString_ChecksLength()
    {
        Predicate<string> isLongString = s => s.Length > 5;
        Assert.IsTrue(isLongString("Hello World"));
        Assert.IsFalse(isLongString("Hi"));
    }

    [Test]
    public void Predicate_WithNull_CanHandleNullCheck()
    {
        Predicate<string?> isNotNull = s => s != null;
        Assert.IsTrue(isNotNull("test"));
        Assert.IsFalse(isNotNull(null));
    }

    [Test]
    public void Predicate_WithComplexType_Works()
    {
        Predicate<Person> isAdult = p => p.Age >= 18;

        var adult = new Person { Age = 25 };
        var child = new Person { Age = 15 };

        Assert.IsTrue(isAdult(adult));
        Assert.IsFalse(isAdult(child));
    }

    [Test]
    public void Predicate_CombinedWithAnd_Works()
    {
        Predicate<int> isPositive = x => x > 0;
        Predicate<int> isEven = x => x % 2 == 0;
        Predicate<int> isPositiveAndEven = x => isPositive(x) && isEven(x);

        Assert.IsTrue(isPositiveAndEven(4));
        Assert.IsFalse(isPositiveAndEven(3));
        Assert.IsFalse(isPositiveAndEven(-4));
    }

    [Test]
    public void Predicate_UsedWithArrayFind_Works()
    {
        int[] numbers = { 1, 2, 3, 4, 5, 6 };
        Predicate<int> isGreaterThanThree = x => x > 3;

        int result = Array.Find(numbers, isGreaterThanThree);
        Assert.AreEqual(4, result);
    }

    private class Person
    {
        public int Age { get; set; }
    }
}
