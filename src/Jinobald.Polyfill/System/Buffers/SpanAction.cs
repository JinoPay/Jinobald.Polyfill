#if FeatureMemory

namespace System.Buffers;

/// <summary>
/// Encapsulates a method that receives a span of objects of type T and a state object of type TArg.
/// </summary>
/// <typeparam name="T">The type of the objects in the span.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <param name="span">A span of objects of type T.</param>
/// <param name="arg">A state object of type TArg.</param>
public delegate void SpanAction<T, in TArg>(Span<T> span, TArg arg);

#endif
