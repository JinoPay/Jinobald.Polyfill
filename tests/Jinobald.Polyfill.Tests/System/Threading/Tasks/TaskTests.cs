using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System.Threading.Tasks;

public class TaskTests
{
    [Test]
    public void Task_Run_ExecutesAction()
    {
        bool executed = false;
        var task = Task.Run(() => { executed = true; });
        task.Wait();
        Assert.IsTrue(executed);
        Assert.IsTrue(task.IsCompleted);
#if NET35
        Assert.IsTrue(task.IsCompletedSuccessfully);
#endif
    }

    [Test]
    public void Task_Run_ReturnsResult()
    {
        var task = Task.Run(() => 42);
        int result = task.Result;
        Assert.AreEqual(42, result);
        Assert.IsTrue(task.IsCompleted);
    }

    [Test]
    public void Task_FromResult_CreatesCompletedTask()
    {
        var task = Task.FromResult(123);
        Assert.IsTrue(task.IsCompleted);
#if NET35
        Assert.IsTrue(task.IsCompletedSuccessfully);
#endif
        Assert.AreEqual(123, task.Result);
    }

    [Test]
    public void Task_Delay_CompletesAfterDelay()
    {
        DateTime startTime = DateTime.UtcNow;
        var task = Task.Delay(100);
        task.Wait();
        TimeSpan elapsed = DateTime.UtcNow - startTime;
        Assert.IsTrue(task.IsCompleted);
        // Allow small tolerance for timing precision (95ms minimum to account for clock granularity)
        Assert.IsTrue(elapsed.TotalMilliseconds >= 95);
    }

    [Test]
    public void Task_WhenAll_WaitsForAllTasks()
    {
        int counter = 0;
        var task1 = Task.Run(() => Interlocked.Increment(ref counter));
        var task2 = Task.Run(() => Interlocked.Increment(ref counter));
        var task3 = Task.Run(() => Interlocked.Increment(ref counter));
        var whenAll = Task.WhenAll(task1, task2, task3);
        whenAll.Wait();
        Assert.AreEqual(3, counter);
        Assert.IsTrue(whenAll.IsCompleted);
    }

    [Test]
    public void Task_WhenAll_WithResults_ReturnsAllResults()
    {
        var task1 = Task.Run(() => 1);
        var task2 = Task.Run(() => 2);
        var task3 = Task.Run(() => 3);
        var whenAll = Task.WhenAll(task1, task2, task3);
        int[]? results = whenAll.Result;
        Assert.AreEqual(new[] { 1, 2, 3 }, results);
    }

    [Test]
    public void Task_WhenAny_CompletesWhenFirstTaskCompletes()
    {
        var task1 = Task.Delay(1000);
        var task2 = Task.Delay(10);
        var task3 = Task.Delay(1000);
        var whenAny = Task.WhenAny(task1, task2, task3);
        Task? completed = whenAny.Result;
        Assert.AreSame(task2, completed);
        Assert.IsTrue(completed.IsCompleted);
    }

    [Test]
    public void Task_ContinueWith_ExecutesAfterCompletion()
    {
        var task = Task.Run(() => 10);
        Task<int> continuation = task.ContinueWith(t => t.Result * 2);
        int result = continuation.Result;
        Assert.AreEqual(20, result);
    }

    [Test]
    public void Task_Exception_CapturesException()
    {
        var task = Task.Run(() => throw new InvalidOperationException("Test exception"));
        Assert.Throws<AggregateException>(() => task.Wait());
        Assert.IsTrue(task.IsFaulted);
        Assert.IsNotNull(task.Exception);
        Assert.IsInstanceOf<InvalidOperationException>(task.Exception!.InnerExceptions[0]);
    }

    [Test]
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

        Assert.IsTrue(task.IsCanceled || task.IsCompleted);
    }

    [Test]
    public void TaskFactory_StartNew_CreatesAndStartsTask()
    {
        var factory = new TaskFactory();
        bool executed = false;
        Task task = factory.StartNew(() => { executed = true; });
        task.Wait();
        Assert.IsTrue(executed);
        Assert.IsTrue(task.IsCompleted);
    }

    [Test]
    public void TaskFactory_StartNew_WithResult()
    {
        var factory = new TaskFactory<int>();
        Task<int> task = factory.StartNew(() => 42);
        int result = task.Result;
        Assert.AreEqual(42, result);
    }

    [Test]
    public void CancellationTokenSource_Cancel_SetsCancellationRequested()
    {
        var cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        Assert.IsFalse(token.IsCancellationRequested);
        cts.Cancel();
        Assert.IsTrue(token.IsCancellationRequested);
    }

    [Test]
    public void CancellationToken_Register_InvokesCallbackOnCancellation()
    {
        var cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        bool called = false;
        token.Register(() => { called = true; });
        Assert.IsFalse(called);
        cts.Cancel();
        Assert.IsTrue(called);
    }

    [Test]
    public void CancellationTokenSource_CancelAfter_CancelsAfterDelay()
    {
        var cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        cts.CancelAfter(100);
        Assert.IsFalse(token.IsCancellationRequested);
        Thread.Sleep(300);
        Assert.IsTrue(token.IsCancellationRequested);
    }

    [Test]
    public void AggregateException_Flatten_FlattensNestedExceptions()
    {
        var innerException1 = new InvalidOperationException("Inner 1");
        var innerException2 = new ArgumentException("Inner 2");
        var innerAggregate = new AggregateException(innerException1, innerException2);
        var outerException = new InvalidCastException("Outer");
        var aggregate = new AggregateException(innerAggregate, outerException);
        AggregateException flattened = aggregate.Flatten();
        Assert.AreEqual(3, flattened.InnerExceptions.Count);
        Assert.IsTrue(flattened.InnerExceptions.Any(e => e == innerException1));
        Assert.IsTrue(flattened.InnerExceptions.Any(e => e == innerException2));
        Assert.IsTrue(flattened.InnerExceptions.Any(e => e == outerException));
    }

    [Test]
    public void Task_WaitAll_WaitsForAllTasks()
    {
        var task1 = Task.Run(() => Thread.Sleep(10));
        var task2 = Task.Run(() => Thread.Sleep(10));
        var task3 = Task.Run(() => Thread.Sleep(10));
        Task.WaitAll(task1, task2, task3);
        Assert.IsTrue(task1.IsCompleted);
        Assert.IsTrue(task2.IsCompleted);
        Assert.IsTrue(task3.IsCompleted);
    }

    [Test]
    public void Task_WaitAny_ReturnsIndexOfFirstCompletedTask()
    {
        var task1 = Task.Delay(1000);
        var task2 = Task.Delay(10);
        var task3 = Task.Delay(1000);
        int index = Task.WaitAny(task1, task2, task3);
        Assert.AreEqual(1, index);
        Assert.IsTrue(task2.IsCompleted);
    }
#if NET35
    [Test]
    public void Task_CompletedTask_IsAlreadyCompleted()
    {
        var task = Task.CompletedTask;
        Assert.IsTrue(task.IsCompleted);
        Assert.IsTrue(task.IsCompletedSuccessfully);
        Assert.IsFalse(task.IsFaulted);
        Assert.IsFalse(task.IsCanceled);
    }
    [Test]
    public void Task_Status_TransitionsCorrectly()
    {
        var task = new Task();
        Assert.AreEqual(TaskStatus.Created, task.Status);
        // Task가 시작되면 상태가 변경됨
        task.SetResult();
        Assert.AreEqual(TaskStatus.RanToCompletion, task.Status);
    }
#endif
    [Test]
    public void Task_Wait_WithTimeout_ReturnsTrue()
    {
        var task = Task.Run(() => Thread.Sleep(50));
        bool completed = task.Wait(2000);
        Assert.IsTrue(completed);
        Assert.IsTrue(task.IsCompleted);
    }

    [Test]
    public void Task_Wait_WithTimeout_ReturnsFalse()
    {
        var task = Task.Delay(5000);
        bool completed = task.Wait(100);
        Assert.IsFalse(completed);
    }

    [Test]
    public void Task_ContinueWith_Action_ExecutesAfterCompletion()
    {
        bool executed = false;
        var task = Task.Run(() => 42);
        Task continuation = task.ContinueWith(t => { executed = true; });
        continuation.Wait();
        Assert.IsTrue(executed);
        Assert.IsTrue(continuation.IsCompleted);
    }

    [Test]
    public void Task_MultipleContinuations_AllExecute()
    {
        int counter = 0;
        var task = Task.Run(() => 10);
        Task<int> cont1 = task.ContinueWith(t => Interlocked.Increment(ref counter));
        Task<int> cont2 = task.ContinueWith(t => Interlocked.Increment(ref counter));
        Task<int> cont3 = task.ContinueWith(t => Interlocked.Increment(ref counter));
        Task.WaitAll(cont1, cont2, cont3);
        Assert.AreEqual(3, counter);
    }

    [Test]
    public void Task_FromException_CreatesFailedTask()
    {
        var exception = new InvalidOperationException("Test");
        var task = Task.FromException(exception);
        Assert.IsTrue(task.IsCompleted);
        Assert.IsTrue(task.IsFaulted);
        Assert.IsNotNull(task.Exception);
        Assert.IsInstanceOf<InvalidOperationException>(task.Exception!.InnerExceptions[0]);
    }

    [Test]
    public void Task_FromException_Generic_CreatesFailedTask()
    {
        var exception = new InvalidOperationException("Test");
        var task = Task.FromException<int>(exception);
        Assert.IsTrue(task.IsCompleted);
        Assert.IsTrue(task.IsFaulted);
        Assert.Throws<AggregateException>(() =>
        {
            int result = task.Result;
        });
    }

    [Test]
    public void Task_FromCanceled_CreatesCanceledTask()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();
        var task = Task.FromCanceled(cts.Token);
        Assert.IsTrue(task.IsCompleted);
        Assert.IsTrue(task.IsCanceled);
    }

    [Test]
    public void Task_Dispose_ReleasesResources()
    {
        var task = Task.Run(() => 42);
        task.Wait();
        // Dispose 호출 후에도 상태는 유지되어야 함
        task.Dispose();
        Assert.IsTrue(task.IsCompleted);
    }

    [Test]
    public void Task_WhenAll_EmptyArray_CompletesImmediately()
    {
        var task = Task.WhenAll(new Task[0]);
        Assert.IsTrue(task.IsCompleted);
    }

    [Test]
    public void Task_Result_BlocksUntilCompletion()
    {
        bool executed = false;
        var task = Task.Run(() =>
        {
            Thread.Sleep(100);
            executed = true;
            return 42;
        });
        int result = task.Result;
        Assert.IsTrue(executed);
        Assert.AreEqual(42, result);
    }

    [Test]
    public void Task_Exception_PropagatesOnWait()
    {
        var task = Task.Run(() =>
        {
            throw new InvalidOperationException("Test exception");
        });
        AggregateException exception = Assert.Throws<AggregateException>(() => task.Wait())!;
        Assert.IsInstanceOf<InvalidOperationException>(exception.InnerExceptions[0]);
        Assert.AreEqual("Test exception", exception.InnerExceptions[0].Message);
    }
}
