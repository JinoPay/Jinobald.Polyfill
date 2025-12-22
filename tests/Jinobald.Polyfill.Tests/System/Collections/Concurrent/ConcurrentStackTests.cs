// Copyright (c) 2024 Park Jinho. All rights reserved.

namespace Jinobald.Polyfill.Tests.System.Collections.Concurrent;

using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;
using NUnit.Framework;

using ConcurrentStack = global::System.Collections.Concurrent.ConcurrentStack<int>;

/// <summary>
/// ConcurrentStack에 대한 테스트입니다.
/// </summary>
public class ConcurrentStackTests
{
    #region Basic Operations

    /// <summary>
    /// 빈 스택이 올바르게 초기화되는지 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_Default_IsEmpty()
    {
        var stack = new ConcurrentStack();

        Assert.IsTrue(stack.IsEmpty);
        Assert.AreEqual(0, stack.Count);
    }

    /// <summary>
    /// 컬렉션으로 스택을 초기화하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_WithCollection_ContainsElements()
    {
        var items = new[] { 1, 2, 3, 4, 5 };
        var stack = new ConcurrentStack(items);

        Assert.IsFalse(stack.IsEmpty);
        Assert.AreEqual(5, stack.Count);
    }

    /// <summary>
    /// Push가 요소를 추가하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Push_AddsElement()
    {
        var stack = new ConcurrentStack();

        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        Assert.IsFalse(stack.IsEmpty);
        Assert.AreEqual(3, stack.Count);
    }

    /// <summary>
    /// TryPop이 LIFO 순서로 요소를 제거하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryPop_RemovesInLifoOrder()
    {
        var stack = new ConcurrentStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        Assert.IsTrue(stack.TryPop(out int result1));
        Assert.AreEqual(3, result1);

        Assert.IsTrue(stack.TryPop(out int result2));
        Assert.AreEqual(2, result2);

        Assert.IsTrue(stack.TryPop(out int result3));
        Assert.AreEqual(1, result3);

        Assert.IsTrue(stack.IsEmpty);
    }

    /// <summary>
    /// 빈 스택에서 TryPop이 false를 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryPop_EmptyStack_ReturnsFalse()
    {
        var stack = new ConcurrentStack();

        Assert.IsFalse(stack.TryPop(out int result));
        Assert.AreEqual(0, result);
    }

    /// <summary>
    /// TryPeek가 요소를 제거하지 않고 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryPeek_DoesNotRemoveElement()
    {
        var stack = new ConcurrentStack();
        stack.Push(42);

        Assert.IsTrue(stack.TryPeek(out int result));
        Assert.AreEqual(42, result);
        Assert.AreEqual(1, stack.Count);

        Assert.IsTrue(stack.TryPeek(out result));
        Assert.AreEqual(42, result);
        Assert.AreEqual(1, stack.Count);
    }

    /// <summary>
    /// 빈 스택에서 TryPeek가 false를 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryPeek_EmptyStack_ReturnsFalse()
    {
        var stack = new ConcurrentStack();

        Assert.IsFalse(stack.TryPeek(out int result));
        Assert.AreEqual(0, result);
    }

    /// <summary>
    /// Clear가 모든 요소를 제거하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Clear_RemovesAllElements()
    {
        var stack = new ConcurrentStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        stack.Clear();

        Assert.IsTrue(stack.IsEmpty);
        Assert.AreEqual(0, stack.Count);
    }

    #endregion

    #region PushRange / TryPopRange

    /// <summary>
    /// PushRange가 여러 요소를 추가하는지 테스트합니다.
    /// </summary>
    [Test]
    public void PushRange_AddsMultipleElements()
    {
        var stack = new ConcurrentStack();
        var items = new[] { 1, 2, 3, 4, 5 };

        stack.PushRange(items);

        Assert.AreEqual(5, stack.Count);
    }

    /// <summary>
    /// PushRange(배열, 시작, 개수)가 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public void PushRange_WithStartAndCount_AddsElements()
    {
        var stack = new ConcurrentStack();
        var items = new[] { 1, 2, 3, 4, 5 };

        stack.PushRange(items, 1, 3);

        Assert.AreEqual(3, stack.Count);

        Assert.IsTrue(stack.TryPop(out int result1));
        Assert.AreEqual(4, result1);

        Assert.IsTrue(stack.TryPop(out int result2));
        Assert.AreEqual(3, result2);

        Assert.IsTrue(stack.TryPop(out int result3));
        Assert.AreEqual(2, result3);
    }

    /// <summary>
    /// TryPopRange가 여러 요소를 제거하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryPopRange_RemovesMultipleElements()
    {
        var stack = new ConcurrentStack();
        stack.PushRange(new[] { 1, 2, 3, 4, 5 });

        var results = new int[3];
        int count = stack.TryPopRange(results);

        Assert.AreEqual(3, count);
        Assert.AreEqual(new[] { 5, 4, 3 }, results);
        Assert.AreEqual(2, stack.Count);
    }

    /// <summary>
    /// TryPopRange가 빈 스택에서 0을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryPopRange_EmptyStack_ReturnsZero()
    {
        var stack = new ConcurrentStack();
        var results = new int[5];

        int count = stack.TryPopRange(results);

        Assert.AreEqual(0, count);
    }

    /// <summary>
    /// TryPopRange가 스택보다 많은 요소를 요청하면 가능한만큼 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryPopRange_RequestMoreThanAvailable_ReturnsAvailable()
    {
        var stack = new ConcurrentStack();
        stack.PushRange(new[] { 1, 2, 3 });

        var results = new int[10];
        int count = stack.TryPopRange(results);

        Assert.AreEqual(3, count);
        Assert.IsTrue(stack.IsEmpty);
    }

    #endregion

    #region ToArray and Enumeration

    /// <summary>
    /// ToArray가 스택의 요소를 올바른 순서로 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ToArray_ReturnsElementsInOrder()
    {
        var stack = new ConcurrentStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        var result = stack.ToArray();

        Assert.AreEqual(new[] { 3, 2, 1 }, result);
    }

    /// <summary>
    /// GetEnumerator가 스택의 요소를 열거하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GetEnumerator_EnumeratesElements()
    {
        var stack = new ConcurrentStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        var list = new List<int>();
        foreach (int item in stack)
        {
            list.Add(item);
        }

        Assert.AreEqual(new[] { 3, 2, 1 }, list);
    }

    #endregion

    #region Concurrent Operations

    /// <summary>
    /// 여러 스레드에서 동시에 Push를 수행하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ConcurrentPush_MultipleThreads()
    {
        var stack = new ConcurrentStack();
        const int itemsPerThread = 1000;
        const int threadCount = 4;

        var tasks = new Task[threadCount];
        for (int t = 0; t < threadCount; t++)
        {
            int threadId = t;
            tasks[t] = Task.Run(() =>
            {
                for (int i = 0; i < itemsPerThread; i++)
                {
                    stack.Push(threadId * itemsPerThread + i);
                }
            });
        }

        Task.WaitAll(tasks);

        Assert.AreEqual(itemsPerThread * threadCount, stack.Count);
    }

    /// <summary>
    /// 여러 스레드에서 동시에 TryPop을 수행하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ConcurrentPop_MultipleThreads()
    {
        var stack = new ConcurrentStack();
        const int totalItems = 10000;

        for (int i = 0; i < totalItems; i++)
        {
            stack.Push(i);
        }

        var popped = new global::System.Collections.Concurrent.ConcurrentBag<int>();
        const int threadCount = 4;

        var tasks = new Task[threadCount];
        for (int t = 0; t < threadCount; t++)
        {
            tasks[t] = Task.Run(() =>
            {
                while (stack.TryPop(out int item))
                {
                    popped.Add(item);
                }
            });
        }

        Task.WaitAll(tasks);

        Assert.AreEqual(totalItems, popped.Count);
        Assert.IsTrue(stack.IsEmpty);
    }

    /// <summary>
    /// 여러 스레드에서 동시에 PushRange를 수행하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ConcurrentPushRange_MultipleThreads()
    {
        var stack = new ConcurrentStack();
        const int itemsPerThread = 100;
        const int threadCount = 4;

        var tasks = new Task[threadCount];
        for (int t = 0; t < threadCount; t++)
        {
            int threadId = t;
            tasks[t] = Task.Run(() =>
            {
                var items = new int[itemsPerThread];
                for (int i = 0; i < itemsPerThread; i++)
                {
                    items[i] = threadId * itemsPerThread + i;
                }
                stack.PushRange(items);
            });
        }

        Task.WaitAll(tasks);

        Assert.AreEqual(itemsPerThread * threadCount, stack.Count);
    }

    #endregion

    #region Large Scale Tests

    /// <summary>
    /// 대량의 요소를 처리하는지 테스트합니다.
    /// </summary>
    [Test]
    public void LargeScale_PushAndPop()
    {
        var stack = new ConcurrentStack();
        const int itemCount = 100000;

        for (int i = 0; i < itemCount; i++)
        {
            stack.Push(i);
        }

        Assert.AreEqual(itemCount, stack.Count);

        for (int i = itemCount - 1; i >= 0; i--)
        {
            Assert.IsTrue(stack.TryPop(out int result));
            Assert.AreEqual(i, result);
        }

        Assert.IsTrue(stack.IsEmpty);
    }

    #endregion
}
