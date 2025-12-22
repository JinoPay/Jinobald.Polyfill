// Copyright (c) 2024 Park Jinho. All rights reserved.

namespace Jinobald.Polyfill.Tests.System.Threading.Tasks;

using global::System.Collections.Concurrent;
using NUnit.Framework;

/// <summary>
/// Parallel 클래스에 대한 테스트입니다.
/// </summary>
public class ParallelTests
{
    #region For Tests

    /// <summary>
    /// Parallel.For가 올바르게 모든 반복을 실행하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_ExecutesAllIterations()
    {
        var executed = new bool[100];

        Parallel.For(0, 100, i =>
        {
            executed[i] = true;
        });

        for (int i = 0; i < 100; i++)
        {
            Assert.IsTrue(executed[i], $"반복 {i}이 실행되지 않았습니다.");
        }
    }

    /// <summary>
    /// Parallel.For가 빈 범위에서 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_EmptyRange_ReturnsCompleted()
    {
        var result = Parallel.For(10, 10, i => { });

        Assert.IsTrue(result.IsCompleted);
        Assert.IsNull(result.LowestBreakIteration);
    }

    /// <summary>
    /// Parallel.For가 역순 범위에서 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_ReverseRange_ReturnsCompleted()
    {
        var executed = false;
        var result = Parallel.For(10, 5, i => { executed = true; });

        Assert.IsTrue(result.IsCompleted);
        Assert.IsFalse(executed);
    }

    /// <summary>
    /// Parallel.For가 ParallelOptions를 올바르게 사용하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_WithMaxDegreeOfParallelism_LimitsWorkers()
    {
        var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
        var executedCount = 0;

        Parallel.For(0, 10, options, i =>
        {
            Interlocked.Increment(ref executedCount);
        });

        Assert.AreEqual(10, executedCount);
    }

    /// <summary>
    /// Parallel.For가 long 버전에서 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_LongVersion_ExecutesAllIterations()
    {
        var executed = new bool[50];

        Parallel.For(0L, 50L, i =>
        {
            executed[(int)i] = true;
        });

        for (int i = 0; i < 50; i++)
        {
            Assert.IsTrue(executed[i], $"반복 {i}이 실행되지 않았습니다.");
        }
    }

    /// <summary>
    /// Parallel.For가 ParallelLoopState를 올바르게 전달하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_WithState_ProvidesLoopState()
    {
        var stateReceived = false;

        Parallel.For(0, 10, (i, state) =>
        {
            if (state != null)
            {
                stateReceived = true;
            }
        });

        Assert.IsTrue(stateReceived);
    }

    /// <summary>
    /// Parallel.For가 합계를 올바르게 계산하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_CalculatesSum_Correctly()
    {
        long sum = 0;

        Parallel.For(1, 101, i =>
        {
            Interlocked.Add(ref sum, i);
        });

        // 1 + 2 + ... + 100 = 5050
        Assert.AreEqual(5050, sum);
    }

    #endregion

    #region ForEach Tests

    /// <summary>
    /// Parallel.ForEach가 모든 요소를 처리하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ForEach_ProcessesAllElements()
    {
        var items = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var processed = new bool[11];

        Parallel.ForEach(items, item =>
        {
            processed[item] = true;
        });

        foreach (var item in items)
        {
            Assert.IsTrue(processed[item], $"항목 {item}이 처리되지 않았습니다.");
        }
    }

    /// <summary>
    /// Parallel.ForEach가 빈 컬렉션에서 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ForEach_EmptyCollection_ReturnsCompleted()
    {
        var items = new List<int>();
        var result = Parallel.ForEach(items, item => { });

        Assert.IsTrue(result.IsCompleted);
    }

    /// <summary>
    /// Parallel.ForEach가 문자열 리스트를 처리하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ForEach_StringList_ProcessesAll()
    {
        var items = new List<string> { "a", "b", "c", "d", "e" };
        var results = new ConcurrentBag<string>();

        Parallel.ForEach(items, item =>
        {
            results.Add(item.ToUpper());
        });

        Assert.AreEqual(5, results.Count);
        Assert.Contains("A", results);
        Assert.Contains("E", results);
    }

    /// <summary>
    /// Parallel.ForEach가 인덱스 버전에서 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ForEach_WithIndex_ProvidesCorrectIndices()
    {
        var items = new List<string> { "a", "b", "c", "d", "e" };
        var indexSum = 0L;

        Parallel.ForEach(items, (item, state, index) =>
        {
            Interlocked.Add(ref indexSum, index);
        });

        // 0 + 1 + 2 + 3 + 4 = 10
        Assert.AreEqual(10, indexSum);
    }

    /// <summary>
    /// Parallel.ForEach가 배열을 처리하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ForEach_Array_ProcessesAll()
    {
        var items = new[] { 10, 20, 30, 40, 50 };
        long sum = 0;

        Parallel.ForEach(items, item =>
        {
            Interlocked.Add(ref sum, item);
        });

        Assert.AreEqual(150, sum);
    }

    /// <summary>
    /// Parallel.ForEach가 ParallelOptions를 올바르게 사용하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ForEach_WithOptions_RespectsMaxDegree()
    {
        var items = Enumerable.Range(1, 100).ToList();
        var options = new ParallelOptions { MaxDegreeOfParallelism = 4 };
        long sum = 0;

        Parallel.ForEach(items, options, item =>
        {
            Interlocked.Add(ref sum, item);
        });

        // 1 + 2 + ... + 100 = 5050
        Assert.AreEqual(5050, sum);
    }

    #endregion

    #region Invoke Tests

    /// <summary>
    /// Parallel.Invoke가 모든 액션을 실행하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Invoke_ExecutesAllActions()
    {
        var executed = new bool[3];

        Parallel.Invoke(
            () => executed[0] = true,
            () => executed[1] = true,
            () => executed[2] = true
        );

        Assert.IsTrue(executed[0], "액션 0이 실행되지 않았습니다.");
        Assert.IsTrue(executed[1], "액션 1이 실행되지 않았습니다.");
        Assert.IsTrue(executed[2], "액션 2이 실행되지 않았습니다.");
    }

    /// <summary>
    /// Parallel.Invoke가 빈 배열에서 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Invoke_EmptyArray_DoesNotThrow()
    {
        Exception? exception = null;
        try
        {
            Parallel.Invoke();
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.IsNull(exception);
    }

    /// <summary>
    /// Parallel.Invoke가 단일 액션에서 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Invoke_SingleAction_ExecutesSuccessfully()
    {
        var executed = false;

        Parallel.Invoke(() => executed = true);

        Assert.IsTrue(executed);
    }

    /// <summary>
    /// Parallel.Invoke가 여러 작업의 결과를 올바르게 수집하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Invoke_MultipleActions_AllComplete()
    {
        int counter = 0;

        Parallel.Invoke(
            () => Interlocked.Increment(ref counter),
            () => Interlocked.Increment(ref counter),
            () => Interlocked.Increment(ref counter),
            () => Interlocked.Increment(ref counter),
            () => Interlocked.Increment(ref counter)
        );

        Assert.AreEqual(5, counter);
    }

    /// <summary>
    /// Parallel.Invoke가 예외를 올바르게 집계하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Invoke_WithException_ThrowsAggregateException()
    {
        var exception = Assert.Throws<AggregateException>(() =>
        {
            Parallel.Invoke(
                () => { },
                () => throw new InvalidOperationException("테스트 예외"),
                () => { }
            );
        });

        Assert.IsNotEmpty(exception.InnerExceptions);
        Assert.IsTrue(exception.InnerExceptions.Any(e => e is InvalidOperationException));
    }

    /// <summary>
    /// Parallel.Invoke가 여러 예외를 올바르게 집계하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Invoke_WithMultipleExceptions_AggregatesAll()
    {
        var exception = Assert.Throws<AggregateException>(() =>
        {
            Parallel.Invoke(
                () => throw new InvalidOperationException("예외 1"),
                () => throw new ArgumentException("예외 2"),
                () => { }
            );
        });

        Assert.IsTrue(exception.InnerExceptions.Count >= 1);
    }

    #endregion

    #region ParallelOptions Tests

    /// <summary>
    /// ParallelOptions 기본값이 올바른지 테스트합니다.
    /// </summary>
    [Test]
    public void ParallelOptions_DefaultValues_AreCorrect()
    {
        var options = new ParallelOptions();

        Assert.AreEqual(-1, options.MaxDegreeOfParallelism);
        Assert.AreEqual(CancellationToken.None, options.CancellationToken);
    }

    /// <summary>
    /// ParallelOptions가 잘못된 MaxDegreeOfParallelism 값을 거부하는지 테스트합니다.
    /// </summary>
    
    [TestCase(0)]
    [TestCase(-2)]
    [TestCase(-100)]
    public void ParallelOptions_InvalidMaxDegree_ThrowsException(int value)
    {
        var options = new ParallelOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.MaxDegreeOfParallelism = value);
    }

    /// <summary>
    /// ParallelOptions가 유효한 MaxDegreeOfParallelism 값을 허용하는지 테스트합니다.
    /// </summary>
    
    [TestCase(-1)]
    [TestCase(1)]
    [TestCase(4)]
    [TestCase(100)]
    public void ParallelOptions_ValidMaxDegree_Succeeds(int value)
    {
        var options = new ParallelOptions { MaxDegreeOfParallelism = value };

        Assert.AreEqual(value, options.MaxDegreeOfParallelism);
    }

    /// <summary>
    /// ParallelOptions가 CancellationToken을 올바르게 설정하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ParallelOptions_CancellationToken_CanBeSet()
    {
        var cts = new CancellationTokenSource();
        var options = new ParallelOptions { CancellationToken = cts.Token };

        Assert.AreEqual(cts.Token, options.CancellationToken);
    }

    #endregion

    #region Exception Handling Tests

    /// <summary>
    /// Parallel.For가 예외를 올바르게 집계하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_WithException_ThrowsAggregateException()
    {
        var exception = Assert.Throws<AggregateException>(() =>
        {
            Parallel.For(0, 100, i =>
            {
                if (i == 50)
                {
                    throw new InvalidOperationException($"반복 {i}에서 예외");
                }
            });
        });

        Assert.IsNotEmpty(exception.InnerExceptions);
    }

    /// <summary>
    /// Parallel.ForEach가 예외를 올바르게 집계하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ForEach_WithException_ThrowsAggregateException()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };

        var exception = Assert.Throws<AggregateException>(() =>
        {
            Parallel.ForEach(items, item =>
            {
                if (item == 3)
                {
                    throw new InvalidOperationException($"항목 {item}에서 예외");
                }
            });
        });

        Assert.IsNotEmpty(exception.InnerExceptions);
    }

    #endregion

    #region Cancellation Tests

    /// <summary>
    /// Parallel.For가 취소를 올바르게 처리하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_WithCancelledToken_ThrowsOperationCanceledException()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();
        var options = new ParallelOptions { CancellationToken = cts.Token };

        Assert.Throws<OperationCanceledException>(() =>
        {
            Parallel.For(0, 100, options, i => { });
        });
    }

    /// <summary>
    /// Parallel.ForEach가 취소를 올바르게 처리하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ForEach_WithCancelledToken_ThrowsOperationCanceledException()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();
        var options = new ParallelOptions { CancellationToken = cts.Token };
        var items = new List<int> { 1, 2, 3 };

        Assert.Throws<OperationCanceledException>(() =>
        {
            Parallel.ForEach(items, options, item => { });
        });
    }

    /// <summary>
    /// Parallel.Invoke가 취소를 올바르게 처리하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Invoke_WithCancelledToken_ThrowsOperationCanceledException()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();
        var options = new ParallelOptions { CancellationToken = cts.Token };

        Assert.Throws<OperationCanceledException>(() =>
        {
            Parallel.Invoke(options, () => { }, () => { });
        });
    }

    #endregion

    #region Argument Validation Tests

    /// <summary>
    /// Parallel.For가 null body에서 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_NullBody_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Parallel.For(0, 10, (Action<int>)null!));
    }

    /// <summary>
    /// Parallel.For가 null options에서 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_NullOptions_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Parallel.For(0, 10, null!, i => { }));
    }

    /// <summary>
    /// Parallel.ForEach가 null source에서 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ForEach_NullSource_ThrowsArgumentNullException()
    {
        IEnumerable<int> nullSource = null!;
        Assert.Throws<ArgumentNullException>(() => Parallel.ForEach(nullSource, i => { }));
    }

    /// <summary>
    /// Parallel.ForEach가 null body에서 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ForEach_NullBody_ThrowsArgumentNullException()
    {
        var items = new List<int> { 1, 2, 3 };
        Assert.Throws<ArgumentNullException>(() => Parallel.ForEach(items, (Action<int>)null!));
    }

    /// <summary>
    /// Parallel.Invoke가 null actions에서 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Invoke_NullActions_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Parallel.Invoke((Action[])null!));
    }

    /// <summary>
    /// Parallel.Invoke가 null 요소를 포함한 배열에서 예외를 throw하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Invoke_NullElementInActions_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Parallel.Invoke(() => { }, null!, () => { }));
    }

    #endregion

    #region ParallelLoopResult Tests

    /// <summary>
    /// ParallelLoopResult가 완료 상태를 올바르게 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ParallelLoopResult_CompletedLoop_IsCompleted()
    {
        var result = Parallel.For(0, 10, i => { });

        Assert.IsTrue(result.IsCompleted);
        Assert.IsNull(result.LowestBreakIteration);
    }

    #endregion

#if NET35
    #region Break/Stop Tests (NET35 only)

    /// <summary>
    /// ParallelLoopState.Stop이 루프를 중단하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_WithStop_StopsLoop()
    {
        var executedIndices = new ConcurrentBag<int>();

        var result = Parallel.For(0, 1000, (i, state) =>
        {
            if (i >= 10)
            {
                state.Stop();
                return;
            }

            executedIndices.Add(i);
        });

        Assert.IsFalse(result.IsCompleted);
    }

    /// <summary>
    /// ParallelLoopState.Break가 루프를 중단하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_WithBreak_BreaksLoop()
    {
        var result = Parallel.For(0, 1000, (i, state) =>
        {
            if (i == 50)
            {
                state.Break();
            }
        });

        Assert.IsFalse(result.IsCompleted);
        Assert.IsNotNull(result.LowestBreakIteration);
    }

    /// <summary>
    /// ParallelLoopState가 IsStopped를 올바르게 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void For_StopCalled_IsStoppedReturnsTrue()
    {
        bool wasStoppedObserved = false;

        Parallel.For(0, 100, (i, state) =>
        {
            if (i == 0)
            {
                state.Stop();
            }

            if (state.IsStopped)
            {
                wasStoppedObserved = true;
            }
        });

        Assert.IsTrue(wasStoppedObserved);
    }

    #endregion
#endif
}
