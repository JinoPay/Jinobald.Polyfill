// Copyright (c) 2024 Park Jinho. All rights reserved.

using System.Collections.Concurrent;

namespace Jinobald.Polyfill.Tests.System.Collections.Concurrent;

using ConcurrentQueue = ConcurrentQueue<int>;

/// <summary>
///     ConcurrentQueue에 대한 테스트입니다.
/// </summary>
public class ConcurrentQueueTests
{
    /// <summary>
    ///     여러 스레드에서 동시에 TryDequeue를 수행하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ConcurrentDequeue_MultipleThreads()
    {
        var queue = new ConcurrentQueue();
        const int totalItems = 10000;

        for (int i = 0; i < totalItems; i++)
        {
            queue.Enqueue(i);
        }

        var dequeued = new ConcurrentBag<int>();
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

        Assert.AreEqual(totalItems, dequeued.Count);
        Assert.IsTrue(queue.IsEmpty);
    }

    /// <summary>
    ///     여러 스레드에서 동시에 Enqueue를 수행하는지 테스트합니다.
    /// </summary>
    [Test]
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
                    queue.Enqueue((threadId * itemsPerThread) + i);
                }
            });
        }

        Task.WaitAll(tasks);

        Assert.AreEqual(itemsPerThread * threadCount, queue.Count);
    }

    /// <summary>
    ///     여러 스레드에서 동시에 Enqueue와 TryDequeue를 수행하는지 테스트합니다.
    /// </summary>
    [Test]
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
        var dequeued = new ConcurrentBag<int>();

        for (int t = 0; t < threadCount; t++)
        {
            dequeueTasks[t] = Task.Run(() =>
            {
                for (int i = 0; i < operationsPerThread; i++)
                {
                    int item;
                    while (!queue.TryDequeue(out item))
                    {
                        Thread.Sleep(0);
                    }

                    dequeued.Add(item);
                }
            });
        }

        Task.WaitAll(dequeueTasks);
        enqueueTask.Wait();

        Assert.AreEqual(operationsPerThread * threadCount, dequeued.Count);
    }

    /// <summary>
    ///     빈 큐가 올바르게 초기화되는지 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_Default_IsEmpty()
    {
        var queue = new ConcurrentQueue();

        Assert.IsTrue(queue.IsEmpty);
        Assert.AreEqual(0, queue.Count);
    }

    /// <summary>
    ///     컬렉션으로 큐를 초기화하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_WithCollection_ContainsElements()
    {
        int[] items = new[] { 1, 2, 3, 4, 5 };
        var queue = new ConcurrentQueue(items);

        Assert.IsFalse(queue.IsEmpty);
        Assert.AreEqual(5, queue.Count);

        int[] result = queue.ToArray();
        Assert.AreEqual(items, result);
    }

    /// <summary>
    ///     Enqueue가 요소를 추가하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Enqueue_AddsElement()
    {
        var queue = new ConcurrentQueue();

        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        Assert.IsFalse(queue.IsEmpty);
        Assert.AreEqual(3, queue.Count);
    }

    /// <summary>
    ///     GetEnumerator가 큐의 요소를 열거하는지 테스트합니다.
    /// </summary>
    [Test]
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

        Assert.AreEqual(new[] { 1, 2, 3 }, list);
    }

    /// <summary>
    ///     대량의 요소를 처리하는지 테스트합니다.
    /// </summary>
    [Test]
    public void LargeScale_EnqueueAndDequeue()
    {
        var queue = new ConcurrentQueue();
        const int itemCount = 100000;

        for (int i = 0; i < itemCount; i++)
        {
            queue.Enqueue(i);
        }

        Assert.AreEqual(itemCount, queue.Count);

        for (int i = 0; i < itemCount; i++)
        {
            Assert.IsTrue(queue.TryDequeue(out int result));
            Assert.AreEqual(i, result);
        }

        Assert.IsTrue(queue.IsEmpty);
    }

    /// <summary>
    ///     ToArray가 큐의 요소를 올바른 순서로 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ToArray_ReturnsElementsInOrder()
    {
        var queue = new ConcurrentQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        int[] result = queue.ToArray();

        Assert.AreEqual(new[] { 1, 2, 3 }, result);
    }

    /// <summary>
    ///     빈 큐에서 TryDequeue가 false를 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryDequeue_EmptyQueue_ReturnsFalse()
    {
        var queue = new ConcurrentQueue();

        Assert.IsFalse(queue.TryDequeue(out int result));
        Assert.AreEqual(0, result);
    }

    /// <summary>
    ///     TryDequeue가 FIFO 순서로 요소를 제거하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryDequeue_RemovesInFifoOrder()
    {
        var queue = new ConcurrentQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        Assert.IsTrue(queue.TryDequeue(out int result1));
        Assert.AreEqual(1, result1);

        Assert.IsTrue(queue.TryDequeue(out int result2));
        Assert.AreEqual(2, result2);

        Assert.IsTrue(queue.TryDequeue(out int result3));
        Assert.AreEqual(3, result3);

        Assert.IsTrue(queue.IsEmpty);
    }

    /// <summary>
    ///     TryPeek가 요소를 제거하지 않고 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryPeek_DoesNotRemoveElement()
    {
        var queue = new ConcurrentQueue();
        queue.Enqueue(42);

        Assert.IsTrue(queue.TryPeek(out int result));
        Assert.AreEqual(42, result);
        Assert.AreEqual(1, queue.Count);

        Assert.IsTrue(queue.TryPeek(out result));
        Assert.AreEqual(42, result);
        Assert.AreEqual(1, queue.Count);
    }

    /// <summary>
    ///     빈 큐에서 TryPeek가 false를 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryPeek_EmptyQueue_ReturnsFalse()
    {
        var queue = new ConcurrentQueue();

        Assert.IsFalse(queue.TryPeek(out int result));
        Assert.AreEqual(0, result);
    }
}
