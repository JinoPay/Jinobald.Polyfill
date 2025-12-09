using System.Threading.Tasks;
using Xunit;

namespace Jinobald.Polyfill.Tests.System.Threading.Tasks;

public class TaskCompletionSourceExTests
{
    [Fact]
    public void SetCanceled_WithCancellationToken_ShouldCancelTask()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        tcs.SetCanceled(cts.Token);

        // Assert
        Assert.True(tcs.Task.IsCanceled);
    }

    [Fact]
    public void SetCanceled_WithDefaultToken_ShouldCancelTask()
    {
        // Arrange
        var tcs = new TaskCompletionSource<string>();

        // Act
        tcs.SetCanceled(default);

        // Assert
        Assert.True(tcs.Task.IsCanceled);
    }

    [Fact]
    public void SetCanceled_WhenAlreadyCompleted_ShouldThrow()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        tcs.SetResult(42);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tcs.SetCanceled(default));
    }

    [Fact]
    public void SetCanceled_WhenAlreadyCanceled_ShouldThrow()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        tcs.SetCanceled(default);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tcs.SetCanceled(default));
    }

    [Fact]
    public void SetCanceled_WhenAlreadyFaulted_ShouldThrow()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        tcs.SetException(new InvalidOperationException());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tcs.SetCanceled(default));
    }

    [Fact]
    public async Task SetCanceled_TaskShouldThrowOnAwait()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        tcs.SetCanceled(cts.Token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await tcs.Task);
    }
}
