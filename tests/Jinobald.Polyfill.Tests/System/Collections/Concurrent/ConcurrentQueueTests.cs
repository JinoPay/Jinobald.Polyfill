// Copyright (c) 2024 Park Jinho. All rights reserved.

namespace Jinobald.Polyfill.Tests.System.Collections.Concurrent;

using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;
using Xunit;

#if NET35 || NET40
using ConcurrentQueue = global::System.Collections.Concurrent.ConcurrentQueue<int>;
#else
using ConcurrentQueue = global::System.Collections.Concurrent.ConcurrentQueue<int>;
#endif

/// <summary>
/// ConcurrentQueue에 대한 테스트입니다.
/// </summary>
public class ConcurrentQueueTests
{
    #region Basic Operations

    /// <summary>
    /// 빈 큐가 올바르게 초기화되는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Constructor_Default_IsEmpty()
    {
        var queue = new ConcurrentQueue();

        Assert.True(queue.IsEmpty);
        Assert.Equal(0, queue.Count);
    }

    /// <summary>
    /// 컬렉션으로 큐를 초기화하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Constructor_WithCollection_ContainsElements()
    {
        var items = new[] { 1, 2, 3, 4, 5 };
        var queue = new ConcurrentQueue(items);

        Assert.False(queue.IsEmpty);
        Assert.Equal(5, queue.Count);

        var result = queue.ToArray();
        Assert.Equal(items, result);
    }

    /// <summary>
    /// Enqueue가 요소를 추가하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void Enqueue_AddsElement()
    {
        var queue = new ConcurrentQueue();

        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        Assert.False(queue.IsEmpty);
        Assert.Equal(3, queue.Count);
    }

    /// <summary>
    /// TryDequeue가 FIFO 순서로 요소를 제거하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void TryDequeue_RemovesInFifoOrder()
    {
        var queue = new ConcurrentQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        Assert.True(queue.TryDequeue(out int result1));
        Assert.Equal(1, result1);

        Assert.True(queue.TryDequeue(out int result2));
        Assert.Equal(2, result2);

        Assert.True(queue.TryDequeue(out int result3));
        Assert.Equal(3, result3);

        Assert.True(queue.IsEmpty);
    }

    /// <summary>
    /// 빈 큐에서 TryDequeue가 false를 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void TryDequeue_EmptyQueue_ReturnsFalse()
    {
        var queue = new ConcurrentQueue();

        Assert.False(queue.TryDequeue(out int result));
        Assert.Equal(0, result);
    }

    /// <summary>
    /// TryPeek가 요소를 제거하지 않고 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void TryPeek_DoesNotRemoveElement()
    {
        var queue = new ConcurrentQueue();
        queue.Enqueue(42);

        Assert.True(queue.TryPeek(out int result));
        Assert.Equal(42, result);
        Assert.Equal(1, queue.Count);

        Assert.True(queue.TryPeek(out result));
        Assert.Equal(42, result);
        Assert.Equal(1, queue.Count);
    }

    /// <summary>
    /// 빈 큐에서 TryPeek가 false를 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void TryPeek_EmptyQueue_ReturnsFalse()
    {
        var queue = new ConcurrentQueue();

        Assert.False(queue.TryPeek(out int result));
        Assert.Equal(0, result);
    }

    #endregion

    #region ToArray and Enumeration

    /// <summary>
    /// ToArray가 큐의 요소를 올바른 순서로 반환하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void ToArray_ReturnsElementsInOrder()
    {
        var queue = new ConcurrentQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        var result = queue.ToArray();

        Assert.Equal(new[] { 1, 2, 3 }, result);
    }

    /// <summary>
    /// GetEnumerator가 큐의 요소를 열거하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void GetEnumerator_EnumeratesElements()
    {
        var queue = new ConcurrentQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        var list = new List<int>();
        foreach (int item in queue)
        {
            list.Add(item);
        }

        Assert.Equal(new[] { 1, 2, 3 }, list);
    }

    #endregion

    #region Concurrent Operations

    /// <summary>
    /// 여러 스레드에서 동시에 Enqueue를 수행하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void ConcurrentEnqueue_MultipleThreads()
    {
        var queue = new ConcurrentQueue();
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
                    queue.Enqueue(threadId * itemsPerThread + i);
                }
            });
        }

        Task.WaitAll(tasks);

        Assert.Equal(itemsPerThread * threadCount, queue.Count);
    }

    /// <summary>
    /// 여러 스레드에서 동시에 TryDequeue를 수행하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void ConcurrentDequeue_MultipleThreads()
    {
        var queue = new ConcurrentQueue();
        const int totalItems = 10000;

        for (int i = 0; i < totalItems; i++)
        {
            queue.Enqueue(i);
        }

        var dequeued = new global::System.Collections.Concurrent.ConcurrentBag<int>();
        const int threadCount = 4;

        var tasks = new Task[threadCount];
        for (int t = 0; t < threadCount; t++)
        {
            tasks[t] = Task.Run(() =>
            {
                while (queue.TryDequeue(out int item))
                {
                    dequeued.Add(item);
                }
            });
        }

        Task.WaitAll(tasks);

        Assert.Equal(totalItems, dequeued.Count);
        Assert.True(queue.IsEmpty);
    }

    /// <summary>
    /// 여러 스레드에서 동시에 Enqueue와 TryDequeue를 수행하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void ConcurrentEnqueueAndDequeue_MultipleThreads()
    {
        var queue = new ConcurrentQueue();
        const int operationsPerThread = 1000;
        const int threadCount = 4;

        var enqueueTask = Task.Run(() =>
        {
            for (int i = 0; i < operationsPerThread * threadCount; i++)
            {
                queue.Enqueue(i);
                Thread.Sleep(0);
            }
        });

        var dequeueTasks = new Task[threadCount];
        var dequeued = new global::System.Collections.Concurrent.ConcurrentBag<int>();

        for (int t = 0; t < threadCount; t++)
        {
            dequeueTasks[t] = Task.Run(() =>
            {
                for (int i = 0; i < operationsPerThread; i++)
                {
                    while (!queue.TryDequeue(out int item))
                    {
                        Thread.Sleep(0);
                    }
                    dequeued.Add(item);
                }
            });
        }

        Task.WaitAll(dequeueTasks);
        enqueueTask.Wait();

        Assert.Equal(operationsPerThread * threadCount, dequeued.Count);
    }

    #endregion

    #region Large Scale Tests

    /// <summary>
    /// 대량의 요소를 처리하는지 테스트합니다.
    /// </summary>
    [Fact]
    public void LargeScale_EnqueueAndDequeue()
    {
        var queue = new ConcurrentQueue();
        const int itemCount = 100000;

        for (int i = 0; i < itemCount; i++)
        {
            queue.Enqueue(i);
        }

        Assert.Equal(itemCount, queue.Count);

        for (int i = 0; i < itemCount; i++)
        {
            Assert.True(queue.TryDequeue(out int result));
            Assert.Equal(i, result);
        }

        Assert.True(queue.IsEmpty);
    }

    #endregion
}
