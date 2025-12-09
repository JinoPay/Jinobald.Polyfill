using System.Threading.Tasks;
using Xunit;

namespace Jinobald.Polyfill.Tests.System.Threading.Tasks;

public class TaskCompletionSourceTests
{
    [Fact]
    public void Constructor_Default_ShouldCreateInstance()
    {
        // Act
        var tcs = new TaskCompletionSource();

        // Assert
        Assert.NotNull(tcs);
        Assert.NotNull(tcs.Task);
        Assert.False(tcs.Task.IsCompleted);
    }

    [Fact]
    public void Constructor_WithCreationOptions_ShouldCreateInstance()
    {
        // Act
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        // Assert
        Assert.NotNull(tcs);
        Assert.NotNull(tcs.Task);
    }

    [Fact]
    public void Constructor_WithState_ShouldCreateInstance()
    {
        // Arrange
        var state = new object();

        // Act
        var tcs = new TaskCompletionSource(state);

        // Assert
        Assert.NotNull(tcs);
        Assert.Same(state, tcs.Task.AsyncState);
    }

    [Fact]
    public void Constructor_WithStateAndOptions_ShouldCreateInstance()
    {
        // Arrange
        var state = new object();

        // Act
        var tcs = new TaskCompletionSource(state, TaskCreationOptions.RunContinuationsAsynchronously);

        // Assert
        Assert.NotNull(tcs);
        Assert.Same(state, tcs.Task.AsyncState);
    }

    [Fact]
    public void SetResult_ShouldCompleteTask()
    {
        // Arrange
        var tcs = new TaskCompletionSource();

        // Act
        tcs.SetResult();

        // Assert
        Assert.True(tcs.Task.IsCompleted);
        Assert.True(tcs.Task.IsCompletedSuccessfully);
    }

    [Fact]
    public void TrySetResult_ShouldReturnTrueOnFirstCall()
    {
        // Arrange
        var tcs = new TaskCompletionSource();

        // Act
        var result = tcs.TrySetResult();

        // Assert
        Assert.True(result);
        Assert.True(tcs.Task.IsCompleted);
    }

    [Fact]
    public void TrySetResult_ShouldReturnFalseOnSecondCall()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        tcs.TrySetResult();

        // Act
        var result = tcs.TrySetResult();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void SetCanceled_ShouldCancelTask()
    {
        // Arrange
        var tcs = new TaskCompletionSource();

        // Act
        tcs.SetCanceled();

        // Assert
        Assert.True(tcs.Task.IsCompleted);
        Assert.True(tcs.Task.IsCanceled);
    }

    [Fact]
    public void SetCanceled_WithToken_ShouldCancelTask()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        tcs.SetCanceled(cts.Token);

        // Assert
        Assert.True(tcs.Task.IsCanceled);
    }

    [Fact]
    public void TrySetCanceled_ShouldReturnTrueOnFirstCall()
    {
        // Arrange
        var tcs = new TaskCompletionSource();

        // Act
        var result = tcs.TrySetCanceled();

        // Assert
        Assert.True(result);
        Assert.True(tcs.Task.IsCanceled);
    }

    [Fact]
    public void TrySetCanceled_ShouldReturnFalseOnSecondCall()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        tcs.TrySetCanceled();

        // Act
        var result = tcs.TrySetCanceled();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void SetException_ShouldFaultTask()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var exception = new InvalidOperationException("Test exception");

        // Act
        tcs.SetException(exception);

        // Assert
        Assert.True(tcs.Task.IsCompleted);
        Assert.True(tcs.Task.IsFaulted);
        Assert.NotNull(tcs.Task.Exception);
        Assert.Contains(tcs.Task.Exception.InnerExceptions, e => e == exception);
    }

    [Fact]
    public void SetException_WithMultipleExceptions_ShouldFaultTask()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var exceptions = new Exception[]
        {
            new InvalidOperationException("Test 1"),
            new ArgumentException("Test 2")
        };

        // Act
        tcs.SetException(exceptions);

        // Assert
        Assert.True(tcs.Task.IsFaulted);
        Assert.NotNull(tcs.Task.Exception);
        Assert.Equal(2, tcs.Task.Exception.InnerExceptions.Count);
    }

    [Fact]
    public void TrySetException_ShouldReturnTrueOnFirstCall()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var exception = new InvalidOperationException("Test");

        // Act
        var result = tcs.TrySetException(exception);

        // Assert
        Assert.True(result);
        Assert.True(tcs.Task.IsFaulted);
    }

    [Fact]
    public void TrySetException_ShouldReturnFalseOnSecondCall()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        tcs.TrySetException(new InvalidOperationException("Test"));

        // Act
        var result = tcs.TrySetException(new ArgumentException("Another"));

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void SetResult_AfterSetCanceled_ShouldThrow()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        tcs.SetCanceled();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tcs.SetResult());
    }

    [Fact]
    public void SetCanceled_AfterSetResult_ShouldThrow()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        tcs.SetResult();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tcs.SetCanceled());
    }

    [Fact]
    public void SetException_AfterSetResult_ShouldThrow()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        tcs.SetResult();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tcs.SetException(new Exception()));
    }

    [Fact]
    public async Task Task_CanBeAwaited()
    {
        // Arrange
        var tcs = new TaskCompletionSource();

        // Act
        _ = Task.Run(async () =>
        {
            await Task.Delay(10);
            tcs.SetResult();
        });

        await tcs.Task;

        // Assert
        Assert.True(tcs.Task.IsCompleted);
    }
}
