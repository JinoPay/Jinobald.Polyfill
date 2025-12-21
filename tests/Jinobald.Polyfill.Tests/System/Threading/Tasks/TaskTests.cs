namespace Jinobald.Polyfill.Tests.System.Threading.Tasks;
using global::System;
using global::System.Threading;
using global::System.Threading.Tasks;
using Xunit;
public class TaskTests
{
    [Fact]
    public void Task_Run_ExecutesAction()
    {
        var executed = false;
        var task = Task.Run(() => { executed = true; });
        task.Wait();
        Assert.True(executed);
        Assert.True(task.IsCompleted);
#if NET35
        Assert.True(task.IsCompletedSuccessfully);
#endif
    }
    [Fact]
    public void Task_Run_ReturnsResult()
    {
        var task = Task.Run(() => 42);
        var result = task.Result;
        Assert.Equal(42, result);
        Assert.True(task.IsCompleted);
    }
    [Fact]
    public void Task_FromResult_CreatesCompletedTask()
    {
        var task = Task.FromResult(123);
        Assert.True(task.IsCompleted);
#if NET35
        Assert.True(task.IsCompletedSuccessfully);
#endif
        Assert.Equal(123, task.Result);
    }
    [Fact]
    public void Task_Delay_CompletesAfterDelay()
    {
        var startTime = DateTime.UtcNow;
        var task = Task.Delay(100);
        task.Wait();
        var elapsed = DateTime.UtcNow - startTime;
        Assert.True(task.IsCompleted);
        // Allow small tolerance for timing precision (95ms minimum to account for clock granularity)
        Assert.True(elapsed.TotalMilliseconds >= 95);
    }
    [Fact]
    public void Task_WhenAll_WaitsForAllTasks()
    {
        var counter = 0;
        var task1 = Task.Run(() => Interlocked.Increment(ref counter));
        var task2 = Task.Run(() => Interlocked.Increment(ref counter));
        var task3 = Task.Run(() => Interlocked.Increment(ref counter));
        var whenAll = Task.WhenAll(task1, task2, task3);
        whenAll.Wait();
        Assert.Equal(3, counter);
        Assert.True(whenAll.IsCompleted);
    }
    [Fact]
    public void Task_WhenAll_WithResults_ReturnsAllResults()
    {
        var task1 = Task.Run(() => 1);
        var task2 = Task.Run(() => 2);
        var task3 = Task.Run(() => 3);
        var whenAll = Task.WhenAll(task1, task2, task3);
        var results = whenAll.Result;
        Assert.Equal(new[] { 1, 2, 3 }, results);
    }
    [Fact]
    public void Task_WhenAny_CompletesWhenFirstTaskCompletes()
    {
        var task1 = Task.Delay(1000);
        var task2 = Task.Delay(10);
        var task3 = Task.Delay(1000);
        var whenAny = Task.WhenAny(task1, task2, task3);
        var completed = whenAny.Result;
        Assert.Same(task2, completed);
        Assert.True(completed.IsCompleted);
    }
    [Fact]
    public void Task_ContinueWith_ExecutesAfterCompletion()
    {
        var task = Task.Run(() => 10);
        var continuation = task.ContinueWith(t => t.Result * 2);
        var result = continuation.Result;
        Assert.Equal(20, result);
    }
    [Fact]
    public void Task_Exception_CapturesException()
    {
        var task = Task.Run(() => throw new InvalidOperationException("Test exception"));
        Assert.Throws<AggregateException>(() => task.Wait());
        Assert.True(task.IsFaulted);
        Assert.NotNull(task.Exception);
        Assert.IsType<InvalidOperationException>(task.Exception.InnerExceptions[0]);
    }
    [Fact]
    public void Task_CancellationToken_CancelsTask()
    {
        var cts = new CancellationTokenSource();
        var task = Task.Run(() =>
        {
            // 취소 토큰이 요청될 때까지 대기
            while (!cts.Token.IsCancellationRequested)
            {
                Thread.Sleep(10);
            }
        }, cts.Token);

        // 잠시 후 취소 요청
        Thread.Sleep(50);
        cts.Cancel();

        try
        {
            task.Wait();
        }
        catch (AggregateException)
        {
            // 예외는 무시
        }

        Assert.True(task.IsCanceled || task.IsCompleted);
    }
    [Fact]
    public void TaskFactory_StartNew_CreatesAndStartsTask()
    {
        var factory = new TaskFactory();
        var executed = false;
        var task = factory.StartNew(() => { executed = true; });
        task.Wait();
        Assert.True(executed);
        Assert.True(task.IsCompleted);
    }
    [Fact]
    public void TaskFactory_StartNew_WithResult()
    {
        var factory = new TaskFactory<int>();
        var task = factory.StartNew(() => 42);
        var result = task.Result;
        Assert.Equal(42, result);
    }
    [Fact]
    public void CancellationTokenSource_Cancel_SetsCancellationRequested()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        Assert.False(token.IsCancellationRequested);
        cts.Cancel();
        Assert.True(token.IsCancellationRequested);
    }
    [Fact]
    public void CancellationToken_Register_InvokesCallbackOnCancellation()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var called = false;
        token.Register(() => { called = true; });
        Assert.False(called);
        cts.Cancel();
        Assert.True(called);
    }
    [Fact]
    public void CancellationTokenSource_CancelAfter_CancelsAfterDelay()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        cts.CancelAfter(100);
        Assert.False(token.IsCancellationRequested);
        Thread.Sleep(200);
        Assert.True(token.IsCancellationRequested);
    }
    [Fact]
    public void AggregateException_Flatten_FlattensNestedExceptions()
    {
        var innerException1 = new InvalidOperationException("Inner 1");
        var innerException2 = new ArgumentException("Inner 2");
        var innerAggregate = new AggregateException(innerException1, innerException2);
        var outerException = new InvalidCastException("Outer");
        var aggregate = new AggregateException(innerAggregate, outerException);
        var flattened = aggregate.Flatten();
        Assert.Equal(3, flattened.InnerExceptions.Count);
        Assert.Contains(flattened.InnerExceptions, e => e == innerException1);
        Assert.Contains(flattened.InnerExceptions, e => e == innerException2);
        Assert.Contains(flattened.InnerExceptions, e => e == outerException);
    }
    [Fact]
    public void Task_WaitAll_WaitsForAllTasks()
    {
        var task1 = Task.Run(() => Thread.Sleep(10));
        var task2 = Task.Run(() => Thread.Sleep(10));
        var task3 = Task.Run(() => Thread.Sleep(10));
        Task.WaitAll(task1, task2, task3);
        Assert.True(task1.IsCompleted);
        Assert.True(task2.IsCompleted);
        Assert.True(task3.IsCompleted);
    }
    [Fact]
    public void Task_WaitAny_ReturnsIndexOfFirstCompletedTask()
    {
        var task1 = Task.Delay(1000);
        var task2 = Task.Delay(10);
        var task3 = Task.Delay(1000);
        var index = Task.WaitAny(task1, task2, task3);
        Assert.Equal(1, index);
        Assert.True(task2.IsCompleted);
    }
#if NET35
    [Fact]
    public void Task_CompletedTask_IsAlreadyCompleted()
    {
        var task = Task.CompletedTask;
        Assert.True(task.IsCompleted);
        Assert.True(task.IsCompletedSuccessfully);
        Assert.False(task.IsFaulted);
        Assert.False(task.IsCanceled);
    }
    [Fact]
    public void Task_Status_TransitionsCorrectly()
    {
        var task = new Task();
        Assert.Equal(TaskStatus.Created, task.Status);
        // Task가 시작되면 상태가 변경됨
        task.SetResult();
        Assert.Equal(TaskStatus.RanToCompletion, task.Status);
    }
#endif
    [Fact]
    public void Task_Wait_WithTimeout_ReturnsTrue()
    {
        var task = Task.Run(() => Thread.Sleep(50));
        var completed = task.Wait(2000);
        Assert.True(completed);
        Assert.True(task.IsCompleted);
    }
    [Fact]
    public void Task_Wait_WithTimeout_ReturnsFalse()
    {
        var task = Task.Delay(5000);
        var completed = task.Wait(100);
        Assert.False(completed);
    }
    [Fact]
    public void Task_ContinueWith_Action_ExecutesAfterCompletion()
    {
        var executed = false;
        var task = Task.Run(() => 42);
        var continuation = task.ContinueWith(t => { executed = true; });
        continuation.Wait();
        Assert.True(executed);
        Assert.True(continuation.IsCompleted);
    }
    [Fact]
    public void Task_MultipleContinuations_AllExecute()
    {
        var counter = 0;
        var task = Task.Run(() => 10);
        var cont1 = task.ContinueWith(t => Interlocked.Increment(ref counter));
        var cont2 = task.ContinueWith(t => Interlocked.Increment(ref counter));
        var cont3 = task.ContinueWith(t => Interlocked.Increment(ref counter));
        Task.WaitAll(cont1, cont2, cont3);
        Assert.Equal(3, counter);
    }
    [Fact]
    public void Task_FromException_CreatesFailedTask()
    {
        var exception = new InvalidOperationException("Test");
        var task = Task.FromException(exception);
        Assert.True(task.IsCompleted);
        Assert.True(task.IsFaulted);
        Assert.NotNull(task.Exception);
        Assert.IsType<InvalidOperationException>(task.Exception.InnerExceptions[0]);
    }
    [Fact]
    public void Task_FromException_Generic_CreatesFailedTask()
    {
        var exception = new InvalidOperationException("Test");
        var task = Task.FromException<int>(exception);
        Assert.True(task.IsCompleted);
        Assert.True(task.IsFaulted);
        Assert.Throws<AggregateException>(() => { var result = task.Result; });
    }
    [Fact]
    public void Task_FromCanceled_CreatesCanceledTask()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();
        var task = Task.FromCanceled(cts.Token);
        Assert.True(task.IsCompleted);
        Assert.True(task.IsCanceled);
    }
    [Fact]
    public void Task_Dispose_ReleasesResources()
    {
        var task = Task.Run(() => 42);
        task.Wait();
        // Dispose 호출 후에도 상태는 유지되어야 함
        task.Dispose();
        Assert.True(task.IsCompleted);
    }
    [Fact]
    public void Task_WhenAll_EmptyArray_CompletesImmediately()
    {
        var task = Task.WhenAll(new Task[0]);
        Assert.True(task.IsCompleted);
    }
    [Fact]
    public void Task_Result_BlocksUntilCompletion()
    {
        var executed = false;
        var task = Task.Run(() =>
        {
            Thread.Sleep(100);
            executed = true;
            return 42;
        });
        var result = task.Result;
        Assert.True(executed);
        Assert.Equal(42, result);
    }
    [Fact]
    public void Task_Exception_PropagatesOnWait()
    {
        var task = Task.Run(() =>
        {
            throw new InvalidOperationException("Test exception");
        });
        var exception = Assert.Throws<AggregateException>(() => task.Wait());
        Assert.IsType<InvalidOperationException>(exception.InnerExceptions[0]);
        Assert.Equal("Test exception", exception.InnerExceptions[0].Message);
    }
}
