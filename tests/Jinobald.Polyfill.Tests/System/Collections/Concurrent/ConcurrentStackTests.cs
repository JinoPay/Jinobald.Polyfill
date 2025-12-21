// Copyright (c) 2024 Park Jinho. All rights reserved.

namespace Jinobald.Polyfill.Tests.System.Collections.Concurrent;

using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;
using Xunit;

#if NET35 || NET40
using ConcurrentStack = global::System.Collections.Concurrent.ConcurrentStack<int>;
#else
using ConcurrentStack = global::System.Collections.Concurrent.ConcurrentStack<int>;
#endif

/// <summary>
/// ConcurrentStack에 대한 테스트입니다.
/// </summary>
public class ConcurrentStackTests
{
    #region Basic Operations

    /// <summary>
    /// 빈 스택이 올바르게 초기화되는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Constructor_Default_IsEmpty()
    {
        var stack = new ConcurrentStack();

        Assert.True(stack.IsEmpty);
        Assert.Equal(0, stack.Count);
    }

    /// <summary>
    /// 컬렉션으로 스택을 초기화하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Constructor_WithCollection_ContainsElements()
    {
        var items = new[] { 1, 2, 3, 4, 5 };
        var stack = new ConcurrentStack(items);

        Assert.False(stack.IsEmpty);
        Assert.Equal(5, stack.Count);
    }

    /// <summary>
    /// Push가 요소를 추가하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Push_AddsElement()
    {
        var stack = new ConcurrentStack();

        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        Assert.False(stack.IsEmpty);
        Assert.Equal(3, stack.Count);
    }

    /// <summary>
    /// TryPop이 LIFO 순서로 요소를 제거하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void TryPop_RemovesInLifoOrder()
    {
        var stack = new ConcurrentStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        Assert.True(stack.TryPop(out int result1));
        Assert.Equal(3, result1);

        Assert.True(stack.TryPop(out int result2));
        Assert.Equal(2, result2);

        Assert.True(stack.TryPop(out int result3));
        Assert.Equal(1, result3);

        Assert.True(stack.IsEmpty);
    }

    /// <summary>
    /// 빈 스택에서 TryPop이 false를 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void TryPop_EmptyStack_ReturnsFalse()
    {
        var stack = new ConcurrentStack();

        Assert.False(stack.TryPop(out int result));
        Assert.Equal(0, result);
    }

    /// <summary>
    /// TryPeek가 요소를 제거하지 않고 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void TryPeek_DoesNotRemoveElement()
    {
        var stack = new ConcurrentStack();
        stack.Push(42);

        Assert.True(stack.TryPeek(out int result));
        Assert.Equal(42, result);
        Assert.Equal(1, stack.Count);

        Assert.True(stack.TryPeek(out result));
        Assert.Equal(42, result);
        Assert.Equal(1, stack.Count);
    }

    /// <summary>
    /// 빈 스택에서 TryPeek가 false를 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void TryPeek_EmptyStack_ReturnsFalse()
    {
        var stack = new ConcurrentStack();

        Assert.False(stack.TryPeek(out int result));
        Assert.Equal(0, result);
    }

    /// <summary>
    /// Clear가 모든 요소를 제거하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Clear_RemovesAllElements()
    {
        var stack = new ConcurrentStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        stack.Clear();

        Assert.True(stack.IsEmpty);
        Assert.Equal(0, stack.Count);
    }

    #endregion

    #region PushRange / TryPopRange

    /// <summary>
    /// PushRange가 여러 요소를 추가하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void PushRange_AddsMultipleElements()
    {
        var stack = new ConcurrentStack();
        var items = new[] { 1, 2, 3, 4, 5 };

        stack.PushRange(items);

        Assert.Equal(5, stack.Count);
    }

    /// <summary>
    /// PushRange(배열, 시작, 개수)가 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void PushRange_WithStartAndCount_AddsElements()
    {
        var stack = new ConcurrentStack();
        var items = new[] { 1, 2, 3, 4, 5 };

        stack.PushRange(items, 1, 3);

        Assert.Equal(3, stack.Count);

        Assert.True(stack.TryPop(out int result1));
        Assert.Equal(4, result1);

        Assert.True(stack.TryPop(out int result2));
        Assert.Equal(3, result2);

        Assert.True(stack.TryPop(out int result3));
        Assert.Equal(2, result3);
    }

    /// <summary>
    /// TryPopRange가 여러 요소를 제거하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void TryPopRange_RemovesMultipleElements()
    {
        var stack = new ConcurrentStack();
        stack.PushRange(new[] { 1, 2, 3, 4, 5 });

        var results = new int[3];
        int count = stack.TryPopRange(results);

        Assert.Equal(3, count);
        Assert.Equal(new[] { 5, 4, 3 }, results);
        Assert.Equal(2, stack.Count);
    }

    /// <summary>
    /// TryPopRange가 빈 스택에서 0을 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void TryPopRange_EmptyStack_ReturnsZero()
    {
        var stack = new ConcurrentStack();
        var results = new int[5];

        int count = stack.TryPopRange(results);

        Assert.Equal(0, count);
    }

    /// <summary>
    /// TryPopRange가 스택보다 많은 요소를 요청하면 가능한만큼 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void TryPopRange_RequestMoreThanAvailable_ReturnsAvailable()
    {
        var stack = new ConcurrentStack();
        stack.PushRange(new[] { 1, 2, 3 });

        var results = new int[10];
        int count = stack.TryPopRange(results);

        Assert.Equal(3, count);
        Assert.True(stack.IsEmpty);
    }

    #endregion

    #region ToArray and Enumeration

    /// <summary>
    /// ToArray가 스택의 요소를 올바른 순서로 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void ToArray_ReturnsElementsInOrder()
    {
        var stack = new ConcurrentStack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        var result = stack.ToArray();

        Assert.Equal(new[] { 3, 2, 1 }, result);
    }

    /// <summary>
    /// GetEnumerator가 스택의 요소를 열거하는지 테스트합니다.
    /// </summary>
    [Fact]
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

        Assert.Equal(new[] { 3, 2, 1 }, list);
    }

    #endregion

    #region Concurrent Operations

    /// <summary>
    /// 여러 스레드에서 동시에 Push를 수행하는지 테스트합니다.
    /// </summary>
    [Fact]
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

        Assert.Equal(itemsPerThread * threadCount, stack.Count);
    }

    /// <summary>
    /// 여러 스레드에서 동시에 TryPop을 수행하는지 테스트합니다.
    /// </summary>
    [Fact]
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

        Assert.Equal(totalItems, popped.Count);
        Assert.True(stack.IsEmpty);
    }

    /// <summary>
    /// 여러 스레드에서 동시에 PushRange를 수행하는지 테스트합니다.
    /// </summary>
    [Fact]
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

        Assert.Equal(itemsPerThread * threadCount, stack.Count);
    }

    #endregion

    #region Large Scale Tests

    /// <summary>
    /// 대량의 요소를 처리하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void LargeScale_PushAndPop()
    {
        var stack = new ConcurrentStack();
        const int itemCount = 100000;

        for (int i = 0; i < itemCount; i++)
        {
            stack.Push(i);
        }

        Assert.Equal(itemCount, stack.Count);

        for (int i = itemCount - 1; i >= 0; i--)
        {
            Assert.True(stack.TryPop(out int result));
            Assert.Equal(i, result);
        }

        Assert.True(stack.IsEmpty);
    }

    #endregion
}
