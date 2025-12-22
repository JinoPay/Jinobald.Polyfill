using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System.Threading.Tasks;

/// <summary>
///     ValueTask 클래스의 테스트입니다.
/// </summary>
public class ValueTaskTests
{
    [Test]
    public void ValueTask_결과값으로_생성_IsCompletedSuccessfully()
    {
        var valueTask = new ValueTask<int>(42);

        Assert.IsTrue(valueTask.IsCompleted);
        Assert.IsTrue(valueTask.IsCompletedSuccessfully);
        Assert.IsFalse(valueTask.IsFaulted);
        Assert.IsFalse(valueTask.IsCanceled);
        Assert.AreEqual(42, valueTask.Result);
    }

    [Test]
    public void ValueTask_Task로_생성()
    {
        var task = Task.FromResult("Hello");
        var valueTask = new ValueTask<string>(task);

        Assert.IsTrue(valueTask.IsCompleted);
        Assert.AreEqual("Hello", valueTask.Result);
    }

    [Test]
    public void ValueTask_AsTask_결과값인_경우_새_Task_생성()
    {
        var valueTask = new ValueTask<int>(100);

        var task = valueTask.AsTask();

        Assert.IsTrue(task.IsCompleted);
        Assert.AreEqual(100, task.Result);
    }

    [Test]
    public void ValueTask_AsTask_Task로_생성된_경우_원본_반환()
    {
        var originalTask = Task.FromResult(50);
        var valueTask = new ValueTask<int>(originalTask);

        var task = valueTask.AsTask();

        Assert.AreSame(originalTask, task);
    }

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46
    [Test]
    public void ValueTask_비제네릭_기본_생성자()
    {
        var valueTask = new ValueTask();

        Assert.IsTrue(valueTask.IsCompleted);
        Assert.IsTrue(valueTask.IsCompletedSuccessfully);
    }

    [Test]
    public void ValueTask_비제네릭_Task로_생성()
    {
        var task = Task.Run(() => { });
        task.Wait();
        var valueTask = new ValueTask(task);

        Assert.IsTrue(valueTask.IsCompleted);
    }

    [Test]
    public void ValueTask_FromResult_정적_메서드()
    {
        var valueTask = ValueTask.FromResult(99);

        Assert.IsTrue(valueTask.IsCompleted);
        Assert.AreEqual(99, valueTask.Result);
    }

    [Test]
    public void ValueTask_FromException_Faulted_상태()
    {
        var exception = new InvalidOperationException("테스트 예외");
        var valueTask = ValueTask.FromException<int>(exception);

        Assert.IsTrue(valueTask.IsCompleted);
        Assert.IsTrue(valueTask.IsFaulted);
    }

    [Test]
    public void ValueTask_비제네릭_FromException()
    {
        var exception = new InvalidOperationException("테스트 예외");
        var valueTask = ValueTask.FromException(exception);

        Assert.IsTrue(valueTask.IsCompleted);
        Assert.IsTrue(valueTask.IsFaulted);
    }

    [Test]
    public void ValueTask_FromCanceled_Canceled_상태()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();
        var valueTask = ValueTask.FromCanceled<int>(cts.Token);

        Assert.IsTrue(valueTask.IsCompleted);
        Assert.IsTrue(valueTask.IsCanceled);
    }

    [Test]
    public void ValueTask_비제네릭_FromCanceled()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();
        var valueTask = ValueTask.FromCanceled(cts.Token);

        Assert.IsTrue(valueTask.IsCompleted);
        Assert.IsTrue(valueTask.IsCanceled);
    }

    [Test]
    public void ValueTask_CompletedTask()
    {
        var completedTask = ValueTask.CompletedTask;

        Assert.IsTrue(completedTask.IsCompleted);
        Assert.IsTrue(completedTask.IsCompletedSuccessfully);
    }
#endif

    [Test]
    public void ValueTask_Equals_동일한_결과값()
    {
        var vt1 = new ValueTask<int>(42);
        var vt2 = new ValueTask<int>(42);

        Assert.IsTrue(vt1.Equals(vt2));
        Assert.IsTrue(vt1 == vt2);
        Assert.IsFalse(vt1 != vt2);
    }

    [Test]
    public void ValueTask_Equals_다른_결과값()
    {
        var vt1 = new ValueTask<int>(42);
        var vt2 = new ValueTask<int>(99);

        Assert.IsFalse(vt1.Equals(vt2));
        Assert.IsFalse(vt1 == vt2);
        Assert.IsTrue(vt1 != vt2);
    }

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46
    [Test]
    public void ValueTask_비제네릭_Equals()
    {
        var vt1 = new ValueTask();
        var vt2 = new ValueTask();

        Assert.IsTrue(vt1.Equals(vt2));
        Assert.IsTrue(vt1 == vt2);
    }
#endif

    [Test]
    public void ValueTask_GetHashCode_일관성()
    {
        var valueTask = new ValueTask<int>(42);

        int hash1 = valueTask.GetHashCode();
        int hash2 = valueTask.GetHashCode();

        Assert.AreEqual(hash1, hash2);
    }

    [Test]
    public void ValueTask_GetAwaiter_IsCompleted()
    {
        var valueTask = new ValueTask<int>(42);
        var awaiter = valueTask.GetAwaiter();

        Assert.IsTrue(awaiter.IsCompleted);
        Assert.AreEqual(42, awaiter.GetResult());
    }

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46
    [Test]
    public void ValueTask_비제네릭_GetAwaiter()
    {
        var valueTask = new ValueTask();
        var awaiter = valueTask.GetAwaiter();

        Assert.IsTrue(awaiter.IsCompleted);
        awaiter.GetResult(); // void 반환이므로 예외 없이 완료
    }
#endif

    [Test]
    public void ValueTask_ConfigureAwait_반환()
    {
        var valueTask = new ValueTask<int>(42);

        var configuredAwaitable = valueTask.ConfigureAwait(false);
        var awaiter = configuredAwaitable.GetAwaiter();

        Assert.IsTrue(awaiter.IsCompleted);
        Assert.AreEqual(42, awaiter.GetResult());
    }

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46
    [Test]
    public void ValueTask_비제네릭_ConfigureAwait()
    {
        var valueTask = new ValueTask();

        var configuredAwaitable = valueTask.ConfigureAwait(false);
        var awaiter = configuredAwaitable.GetAwaiter();

        Assert.IsTrue(awaiter.IsCompleted);
    }
#endif

    [Test]
    public void ValueTask_Null_Task_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ValueTask<int>(null!));
    }

#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46
    [Test]
    public void ValueTask_비제네릭_Null_Task_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ValueTask(null!));
    }
#endif

    [Test]
    public void ValueTask_Equals_Object_다른_타입()
    {
        var valueTask = new ValueTask<int>(42);

        Assert.IsFalse(valueTask.Equals("string"));
        Assert.IsFalse(valueTask.Equals(42));
        Assert.IsFalse(valueTask.Equals(null));
    }

    [Test]
    public void ValueTask_Equals_Object_동일한_ValueTask()
    {
        var vt1 = new ValueTask<int>(42);
        object vt2 = new ValueTask<int>(42);

        Assert.IsTrue(vt1.Equals(vt2));
    }

    [Test]
    public void ValueTask_비완료_Task_IsCompleted_False()
    {
        var tcs = new TaskCompletionSource<int>();
        var valueTask = new ValueTask<int>(tcs.Task);

        Assert.IsFalse(valueTask.IsCompleted);

        tcs.SetResult(42);

        Assert.IsTrue(valueTask.IsCompleted);
        Assert.AreEqual(42, valueTask.Result);
    }

    [Test]
    public void ValueTask_Awaiter_OnCompleted_연속_작업_실행()
    {
        var tcs = new TaskCompletionSource<int>();
        var valueTask = new ValueTask<int>(tcs.Task);
        var awaiter = valueTask.GetAwaiter();
        bool continuationExecuted = false;

        awaiter.OnCompleted(() => continuationExecuted = true);

        Assert.IsFalse(continuationExecuted);

        tcs.SetResult(42);

        // Wait a bit for the continuation to execute
        Thread.Sleep(100);

        Assert.IsTrue(continuationExecuted);
    }
}
