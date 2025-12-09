using System.Diagnostics.CodeAnalysis;

#if !NET

#nullable enable

namespace System.Threading.Tasks;

/// <summary>
///     Represents the producer side of a <see cref="Task" /> unbound to a
///     delegate, providing access to the consumer side through the <see cref="Task" /> property.
/// </summary>
[ExcludeFromCodeCoverage]
[System.Diagnostics.DebuggerNonUserCode]
//Link: https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskcompletionsource?view=net-10.0
public class TaskCompletionSource
{
    private readonly TaskCompletionSource<object?> inner;

    /// <summary>Creates a <see cref="TaskCompletionSource" />.</summary>
    public TaskCompletionSource()
    {
        inner = new TaskCompletionSource<object?>();
    }

    /// <summary>Creates a <see cref="TaskCompletionSource" /> with the specified options.</summary>
    public TaskCompletionSource(TaskCreationOptions creationOptions) :
        this(null, creationOptions)
    {
    }

    /// <summary>Creates a <see cref="TaskCompletionSource" /> with the specified state.</summary>
    public TaskCompletionSource(object? state) :
        this(state, TaskCreationOptions.None)
    {
    }

    /// <summary>Creates a <see cref="TaskCompletionSource" /> with the specified state and options.</summary>
    public TaskCompletionSource(object? state, TaskCreationOptions creationOptions)
    {
        inner = new TaskCompletionSource<object?>(state, creationOptions);
    }

    /// <summary>
    ///     Gets the <see cref="Task" /> created
    ///     by this <see cref="TaskCompletionSource" />.
    /// </summary>
    public Task Task => inner.Task;

    /// <summary>
    ///     Attempts to transition the underlying <see cref="Task" /> into the <see cref="TaskStatus.Canceled" /> state.
    /// </summary>
    public bool TrySetCanceled()
    {
        return TrySetCanceled(default);
    }

    /// <summary>
    ///     Attempts to transition the underlying <see cref="Task" /> into the <see cref="TaskStatus.Canceled" /> state.
    /// </summary>
    public bool TrySetCanceled(CancellationToken cancellationToken)
    {
        return inner.TrySetCanceled(default);
    }

    /// <summary>
    ///     Attempts to transition the underlying <see cref="Task" /> into the <see cref="TaskStatus.Faulted" /> state.
    /// </summary>
    public bool TrySetException(Exception exception)
    {
        return inner.TrySetException(exception);
    }

    /// <summary>
    ///     Attempts to transition the underlying <see cref="Task" /> into the <see cref="TaskStatus.Faulted" /> state.
    /// </summary>
    public bool TrySetException(IEnumerable<Exception> exceptions)
    {
        return inner.TrySetException(exceptions);
    }

    /// <summary>
    ///     Attempts to transition the underlying <see cref="Task" /> into the <see cref="TaskStatus.RanToCompletion" />
    ///     state.
    /// </summary>
    public bool TrySetResult()
    {
        return inner.TrySetResult(null);
    }

    /// <summary>
    ///     Transitions the underlying <see cref="Task" /> into the <see cref="TaskStatus.Canceled" /> state.
    /// </summary>
    public void SetCanceled()
    {
        SetCanceled(default);
    }

    /// <summary>
    ///     Transitions the underlying <see cref="Task" /> into the <see cref="TaskStatus.Canceled" /> state
    ///     using the specified token.
    /// </summary>
    public void SetCanceled(CancellationToken cancellationToken)
    {
        inner.SetCanceled(cancellationToken);
    }

    /// <summary>Transitions the underlying <see cref="Task" /> into the <see cref="TaskStatus.Faulted" /> state.</summary>
    public void SetException(Exception exception)
    {
        inner.SetException(exception);
    }

    /// <summary>Transitions the underlying <see cref="Task" /> into the <see cref="TaskStatus.Faulted" /> state.</summary>
    public void SetException(IEnumerable<Exception> exceptions)
    {
        inner.SetException(exceptions);
    }

    /// <summary>
    ///     Transitions the underlying <see cref="Task" /> into the <see cref="TaskStatus.RanToCompletion" /> state.
    /// </summary>
    public void SetResult()
    {
        inner.SetResult(null);
    }
}
#endif
