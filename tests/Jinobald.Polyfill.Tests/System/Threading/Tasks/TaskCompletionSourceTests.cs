using NUnit.Framework;

namespace Jinobald.Polyfill.Tests.System.Threading.Tasks;

/// <summary>
///     TaskCompletionSource 클래스의 테스트입니다.
/// </summary>
public class TaskCompletionSourceTests
{
    [Test]
    public void TaskCompletionSource_SetResult_완료된_Task를_반환()
    {
        var tcs = new TaskCompletionSource<int>();

        tcs.SetResult(42);

        Assert.IsTrue(tcs.Task.IsCompleted);
        Assert.AreEqual(42, tcs.Task.Result);
    }

    [Test]
    public void TaskCompletionSource_TrySetResult_성공시_True_반환()
    {
        var tcs = new TaskCompletionSource<string>();

        bool result = tcs.TrySetResult("Hello");

        Assert.IsTrue(result);
        Assert.IsTrue(tcs.Task.IsCompleted);
        Assert.AreEqual("Hello", tcs.Task.Result);
    }

    [Test]
    public void TaskCompletionSource_TrySetResult_이미_완료된_경우_False_반환()
    {
        var tcs = new TaskCompletionSource<int>();
        tcs.SetResult(1);

        bool result = tcs.TrySetResult(2);

        Assert.IsFalse(result);
        Assert.AreEqual(1, tcs.Task.Result);
    }

    [Test]
    public void TaskCompletionSource_SetException_Faulted_상태_설정()
    {
        var tcs = new TaskCompletionSource<int>();
        var exception = new InvalidOperationException("테스트 예외");

        tcs.SetException(exception);

        Assert.IsTrue(tcs.Task.IsCompleted);
        Assert.IsTrue(tcs.Task.IsFaulted);
        Assert.IsNotNull(tcs.Task.Exception);
        Assert.IsInstanceOf<InvalidOperationException>(tcs.Task.Exception!.InnerException);
    }

    [Test]
    public void TaskCompletionSource_TrySetException_성공시_True_반환()
    {
        var tcs = new TaskCompletionSource<int>();
        var exception = new ArgumentException("테스트");

        bool result = tcs.TrySetException(exception);

        Assert.IsTrue(result);
        Assert.IsTrue(tcs.Task.IsFaulted);
    }

    [Test]
    public void TaskCompletionSource_TrySetException_이미_완료된_경우_False_반환()
    {
        var tcs = new TaskCompletionSource<int>();
        tcs.SetResult(1);

        bool result = tcs.TrySetException(new Exception());

        Assert.IsFalse(result);
        Assert.IsFalse(tcs.Task.IsFaulted);
    }

    [Test]
    public void TaskCompletionSource_SetException_예외_컬렉션()
    {
        var tcs = new TaskCompletionSource<int>();
        var exceptions = new Exception[]
        {
            new InvalidOperationException("예외1"),
            new ArgumentException("예외2"),
        };

        tcs.SetException(exceptions);

        Assert.IsTrue(tcs.Task.IsFaulted);
        Assert.IsNotNull(tcs.Task.Exception);
    }

    [Test]
    public void TaskCompletionSource_SetCanceled_Canceled_상태_설정()
    {
        var tcs = new TaskCompletionSource<int>();

        tcs.SetCanceled();

        Assert.IsTrue(tcs.Task.IsCompleted);
        Assert.IsTrue(tcs.Task.IsCanceled);
    }

    [Test]
    public void TaskCompletionSource_TrySetCanceled_성공시_True_반환()
    {
        var tcs = new TaskCompletionSource<int>();

        bool result = tcs.TrySetCanceled();

        Assert.IsTrue(result);
        Assert.IsTrue(tcs.Task.IsCanceled);
    }

    [Test]
    public void TaskCompletionSource_TrySetCanceled_이미_완료된_경우_False_반환()
    {
        var tcs = new TaskCompletionSource<int>();
        tcs.SetResult(1);

        bool result = tcs.TrySetCanceled();

        Assert.IsFalse(result);
        Assert.IsFalse(tcs.Task.IsCanceled);
    }

    [Test]
    public void TaskCompletionSource_SetResult_중복_호출시_예외()
    {
        var tcs = new TaskCompletionSource<int>();
        tcs.SetResult(1);

        Assert.Throws<InvalidOperationException>(() => tcs.SetResult(2));
    }

    [Test]
    public void TaskCompletionSource_SetException_null_예외시_ArgumentNullException()
    {
        var tcs = new TaskCompletionSource<int>();

        Assert.Throws<ArgumentNullException>(() => tcs.SetException((Exception)null!));
    }

    [Test]
    public void TaskCompletionSource_SetException_빈_컬렉션시_ArgumentException()
    {
        var tcs = new TaskCompletionSource<int>();

        Assert.Throws<ArgumentException>(() => tcs.SetException(Array.Empty<Exception>()));
    }

    [Test]
    public void TaskCompletionSource_Task_Wait_결과_대기()
    {
        var tcs = new TaskCompletionSource<int>();

        ThreadPool.QueueUserWorkItem(_ =>
        {
            Thread.Sleep(50);
            tcs.SetResult(100);
        });

        int result = tcs.Task.Result;
        Assert.AreEqual(100, result);
    }

    [Test]
    public void TaskCompletionSource_여러_생성자_테스트()
    {
        var tcs1 = new TaskCompletionSource<int>();
        var tcs2 = new TaskCompletionSource<int>(new object());
        var tcs3 = new TaskCompletionSource<int>(TaskCreationOptions.None);
        var tcs4 = new TaskCompletionSource<int>(new object(), TaskCreationOptions.None);

        tcs1.SetResult(1);
        tcs2.SetResult(2);
        tcs3.SetResult(3);
        tcs4.SetResult(4);

        Assert.AreEqual(1, tcs1.Task.Result);
        Assert.AreEqual(2, tcs2.Task.Result);
        Assert.AreEqual(3, tcs3.Task.Result);
        Assert.AreEqual(4, tcs4.Task.Result);
    }
}
