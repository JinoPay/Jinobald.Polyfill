// Copyright (c) 2024 Park Jinho. All rights reserved.

namespace Jinobald.Polyfill.Tests.System.Collections.Concurrent;

using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;
using NUnit.Framework;

#if NET35 || NET40
using ConcurrentBag = global::System.Collections.Concurrent.ConcurrentBag<int>;
#else
using ConcurrentBag = global::System.Collections.Concurrent.ConcurrentBag<int>;
#endif

/// <summary>
/// ConcurrentBag에 대한 테스트입니다.
/// </summary>
public class ConcurrentBagTests
{
    #region Basic Operations

    /// <summary>
    /// 빈 백이 올바르게 초기화되는지 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_Default_IsEmpty()
    {
        var bag = new ConcurrentBag();

        Assert.IsTrue(bag.IsEmpty);
        Assert.AreEqual(0, bag.Count);
    }

    /// <summary>
    /// 컬렉션으로 백을 초기화하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_WithCollection_ContainsElements()
    {
        var items = new[] { 1, 2, 3, 4, 5 };
        var bag = new ConcurrentBag(items);

        Assert.IsFalse(bag.IsEmpty);
        Assert.AreEqual(5, bag.Count);
    }

    /// <summary>
    /// Add가 요소를 추가하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Add_AddsElement()
    {
        var bag = new ConcurrentBag();

        bag.Add(1);
        bag.Add(2);
        bag.Add(3);

        Assert.IsFalse(bag.IsEmpty);
        Assert.AreEqual(3, bag.Count);
    }

    /// <summary>
    /// TryTake가 요소를 제거하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryTake_RemovesElement()
    {
        var bag = new ConcurrentBag();
        bag.Add(1);
        bag.Add(2);
        bag.Add(3);

        Assert.IsTrue(bag.TryTake(out int result1));
        Assert.AreEqual(2, bag.Count);

        Assert.IsTrue(bag.TryTake(out int result2));
        Assert.AreEqual(1, bag.Count);

        Assert.IsTrue(bag.TryTake(out int result3));
        Assert.IsTrue(bag.IsEmpty);
    }

    /// <summary>
    /// 빈 백에서 TryTake가 false를 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryTake_EmptyBag_ReturnsFalse()
    {
        var bag = new ConcurrentBag();

        Assert.IsFalse(bag.TryTake(out int result));
        Assert.AreEqual(0, result);
    }

    /// <summary>
    /// TryPeek가 요소를 제거하지 않고 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryPeek_DoesNotRemoveElement()
    {
        var bag = new ConcurrentBag();
        bag.Add(42);

        Assert.IsTrue(bag.TryPeek(out int result));
        Assert.AreEqual(1, bag.Count);

        Assert.IsTrue(bag.TryPeek(out result));
        Assert.AreEqual(1, bag.Count);
    }

    /// <summary>
    /// 빈 백에서 TryPeek가 false를 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void TryPeek_EmptyBag_ReturnsFalse()
    {
        var bag = new ConcurrentBag();

        Assert.IsFalse(bag.TryPeek(out int result));
        Assert.AreEqual(0, result);
    }

    #endregion

    #region Same Thread Operations

    /// <summary>
    /// 같은 스레드에서 Add와 TryTake가 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public void SameThread_AddAndTake()
    {
        var bag = new ConcurrentBag();

        bag.Add(1);
        bag.Add(2);
        bag.Add(3);

        // 같은 스레드에서 추가한 항목은 LIFO 순서로 제거됨
        Assert.IsTrue(bag.TryTake(out int result1));
        Assert.AreEqual(3, result1);

        Assert.IsTrue(bag.TryTake(out int result2));
        Assert.AreEqual(2, result2);

        Assert.IsTrue(bag.TryTake(out int result3));
        Assert.AreEqual(1, result3);
    }

    #endregion

    #region ToArray and Enumeration

    /// <summary>
    /// ToArray가 백의 모든 요소를 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ToArray_ReturnsAllElements()
    {
        var bag = new ConcurrentBag();
        bag.Add(1);
        bag.Add(2);
        bag.Add(3);

        var result = bag.ToArray();

        Assert.AreEqual(3, result.Length);
        Assert.IsTrue(result.Contains(1));
        Assert.IsTrue(result.Contains(2));
        Assert.IsTrue(result.Contains(3));
    }

    /// <summary>
    /// GetEnumerator가 백의 요소를 열거하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GetEnumerator_EnumeratesElements()
    {
        var bag = new ConcurrentBag();
        bag.Add(1);
        bag.Add(2);
        bag.Add(3);

        var list = new List<int>();
        foreach (int item in bag)
        {
            list.Add(item);
        }

        Assert.AreEqual(3, list.Count);
        Assert.IsTrue(list.Contains(1));
        Assert.IsTrue(list.Contains(2));
        Assert.IsTrue(list.Contains(3));
    }

    #endregion

    #region Concurrent Operations

    /// <summary>
    /// 여러 스레드에서 동시에 Add를 수행하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ConcurrentAdd_MultipleThreads()
    {
        var bag = new ConcurrentBag();
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
                    bag.Add(threadId * itemsPerThread + i);
                }
            });
        }

        Task.WaitAll(tasks);

        Assert.AreEqual(itemsPerThread * threadCount, bag.Count);
    }

    /// <summary>
    /// 여러 스레드에서 동시에 TryTake를 수행하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ConcurrentTake_MultipleThreads()
    {
        var bag = new ConcurrentBag();
        const int totalItems = 10000;

        for (int i = 0; i < totalItems; i++)
        {
            bag.Add(i);
        }

        var taken = new global::System.Collections.Concurrent.ConcurrentBag<int>();
        const int threadCount = 4;

        var tasks = new Task[threadCount];
        for (int t = 0; t < threadCount; t++)
        {
            tasks[t] = Task.Run(() =>
            {
                while (bag.TryTake(out int item))
                {
                    taken.Add(item);
                }
            });
        }

        Task.WaitAll(tasks);

        Assert.AreEqual(totalItems, taken.Count);
        Assert.IsTrue(bag.IsEmpty);
    }

    /// <summary>
    /// 여러 스레드에서 동시에 Add와 TryTake를 수행하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ConcurrentAddAndTake_MultipleThreads()
    {
        var bag = new ConcurrentBag();
        const int operationsPerThread = 1000;
        const int threadCount = 4;

        var addTask = Task.Run(() =>
        {
            for (int i = 0; i < operationsPerThread * threadCount; i++)
            {
                bag.Add(i);
                Thread.Sleep(0);
            }
        });

        var takeTasks = new Task[threadCount];
        var taken = new global::System.Collections.Concurrent.ConcurrentBag<int>();

        for (int t = 0; t < threadCount; t++)
        {
            takeTasks[t] = Task.Run(() =>
            {
                for (int i = 0; i < operationsPerThread; i++)
                {
                    while (!bag.TryTake(out int item))
                    {
                        Thread.Sleep(0);
                    }
                    taken.Add(item);
                }
            });
        }

        Task.WaitAll(takeTasks);
        addTask.Wait();

        Assert.AreEqual(operationsPerThread * threadCount, taken.Count);
    }

    /// <summary>
    /// Work-stealing이 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public void WorkStealing_DifferentThreads()
    {
        var bag = new ConcurrentBag();
        const int itemsPerThread = 100;

        // 한 스레드에서 항목 추가
        Task.Run(() =>
        {
            for (int i = 0; i < itemsPerThread; i++)
            {
                bag.Add(i);
            }
        }).Wait();

        // 다른 스레드에서 항목 제거 (work-stealing)
        var taken = new List<int>();
        Task.Run(() =>
        {
            while (bag.TryTake(out int item))
            {
                taken.Add(item);
            }
        }).Wait();

        Assert.AreEqual(itemsPerThread, taken.Count);
        Assert.IsTrue(bag.IsEmpty);
    }

    #endregion

    #region Large Scale Tests

    /// <summary>
    /// 대량의 요소를 처리하는지 테스트합니다.
    /// </summary>
    [Test]
    public void LargeScale_AddAndTake()
    {
        var bag = new ConcurrentBag();
        const int itemCount = 100000;

        for (int i = 0; i < itemCount; i++)
        {
            bag.Add(i);
        }

        Assert.AreEqual(itemCount, bag.Count);

        int count = 0;
        while (bag.TryTake(out int _))
        {
            count++;
        }

        Assert.AreEqual(itemCount, count);
        Assert.IsTrue(bag.IsEmpty);
    }

    #endregion
}
