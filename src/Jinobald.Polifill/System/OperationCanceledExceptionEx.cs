#pragma warning disable S3376 // Make this class name end with 'Exception'

using System.Runtime.Serialization;

namespace System;

[Serializable]
public partial class OperationCanceledExceptionEx : OperationCanceledException
{
    public OperationCanceledExceptionEx()
    {
        // Empty
    }

    public OperationCanceledExceptionEx(string message)
        : base(message)
    {
        // Empty
    }

    public OperationCanceledExceptionEx(string message, Exception innerException)
        : base(message, innerException)
    {
        // Empty
    }

    protected OperationCanceledExceptionEx(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        _ = info;
        _ = context;
    }
}

public partial class OperationCanceledExceptionEx
{
#if NET35
    [NonSerialized] private readonly CancellationToken? _token;

    public OperationCanceledExceptionEx(CancellationToken token)
    {
        _token = token;
    }

    public OperationCanceledExceptionEx(string message, CancellationToken token)
        : base(message)
    {
        _token = token;
    }

    public OperationCanceledExceptionEx(string message, Exception innerException, CancellationToken token)
        : base(message, innerException)
    {
        _token = token;
    }

    public CancellationToken CancellationToken => _token ?? CancellationToken.None;

#else
    public OperationCanceledExceptionEx(CancellationToken token)
        : base(token)
    {
        // Empty
    }

    public OperationCanceledExceptionEx(string message, CancellationToken token)
        : base(message, token)
    {
        // Empty
    }

    public OperationCanceledExceptionEx(string message, Exception innerException, CancellationToken token)
        : base(message, innerException, token)
    {
        // Empty
    }

#endif

    public void Deconstruct(out CancellationToken token)
    {
        token = CancellationToken;
    }
}