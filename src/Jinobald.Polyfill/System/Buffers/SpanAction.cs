#if NETFRAMEWORK || NETSTANDARD2_0 || NETCOREAPP2X

namespace System.Buffers;

/// <summary>
/// Span 형식의 객체와 TArg 형식의 상태 객체를 받는 메서드를 캡슐화합니다.
/// </summary>
/// <typeparam name="T">span의 객체 형식입니다.</typeparam>
/// <typeparam name="TArg">상태를 나타내는 객체의 형식입니다.</typeparam>
/// <param name="span">T 형식의 객체 span입니다.</param>
/// <param name="arg">TArg 형식의 상태 객체입니다.</param>
public delegate void SpanAction<T, in TArg>(Span<T> span, TArg arg);

#endif
