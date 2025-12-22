#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices;

/// <summary>
///     메모리 및 Span 작업을 위한 메서드를 제공합니다.
/// </summary>
public static class MemoryMarshal
{
    /// <summary>
    ///     Memory의 기본 배열을 가져옵니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="memory">배열을 가져올 Memory입니다.</param>
    /// <param name="segment">기본 배열 세그먼트입니다.</param>
    /// <returns>배열을 가져온 경우 true, 그렇지 않으면 false입니다.</returns>
    public static bool TryGetArray<T>(ReadOnlyMemory<T> memory, out ArraySegment<T> segment)
    {
        if (memory._array == null)
        {
            segment = default;
            return false;
        }

        segment = new ArraySegment<T>(memory._array, memory._start, memory.Length);
        return true;
    }

    /// <summary>
    ///     ReadOnlySpan을 Span으로 변환합니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">변환할 ReadOnlySpan입니다.</param>
    /// <returns>Span입니다.</returns>
    /// <remarks>이 메서드는 위험하며 읽기 전용 데이터를 수정하는 데 사용될 수 있습니다.</remarks>
    public static unsafe Span<T> AsSpan<T>(ReadOnlySpan<T> span)
    {
        return CreateSpan<T>(ref Unsafe.AsRef<T>(in span.GetPinnableReference()), span.Length);
    }

    /// <summary>
    ///     ReadOnlyMemory를 Memory로 변환합니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="memory">변환할 ReadOnlyMemory입니다.</param>
    /// <returns>Memory입니다.</returns>
    /// <remarks>이 메서드는 위험하며 읽기 전용 데이터를 수정하는 데 사용될 수 있습니다.</remarks>
    public static Memory<T> AsMemory<T>(ReadOnlyMemory<T> memory)
    {
        return new Memory<T>(memory._array, memory._start, memory.Length);
    }

    /// <summary>
    ///     참조에서 지정된 길이를 가진 Span을 만듭니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="reference">Span에 대한 참조입니다.</param>
    /// <param name="length">Span의 길이입니다.</param>
    /// <returns>새로운 Span입니다.</returns>
    public static unsafe Span<T> CreateSpan<T>(ref T reference, int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        fixed (T* ptr = &reference)
        {
            return new Span<T>(ptr, length);
        }
    }

    /// <summary>
    ///     참조에서 지정된 길이를 가진 ReadOnlySpan을 만듭니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="reference">ReadOnlySpan에 대한 참조입니다.</param>
    /// <param name="length">ReadOnlySpan의 길이입니다.</param>
    /// <returns>새로운 ReadOnlySpan입니다.</returns>
    public static unsafe ReadOnlySpan<T> CreateReadOnlySpan<T>(ref readonly T reference, int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        fixed (T* ptr = &reference)
        {
            return new ReadOnlySpan<T>(ptr, length);
        }
    }

    /// <summary>
    ///     Span&lt;T&gt;를 Span&lt;byte&gt;로 캐스팅합니다.
    /// </summary>
    /// <typeparam name="T">소스 요소 형식입니다.</typeparam>
    /// <param name="span">캐스팅할 Span입니다.</param>
    /// <returns>바이트 Span입니다.</returns>
    public static unsafe Span<byte> AsBytes<T>(Span<T> span)
        where T : struct
    {
        if (span.Length == 0)
        {
            return default;
        }

        int byteLength = span.Length * Unsafe.SizeOf<T>();
        ref T reference = ref span.GetPinnableReference();
        return CreateSpan<byte>(ref Unsafe.As<T, byte>(ref reference), byteLength);
    }

    /// <summary>
    ///     ReadOnlySpan&lt;T&gt;를 ReadOnlySpan&lt;byte&gt;로 캐스팅합니다.
    /// </summary>
    /// <typeparam name="T">소스 요소 형식입니다.</typeparam>
    /// <param name="span">캐스팅할 ReadOnlySpan입니다.</param>
    /// <returns>바이트 ReadOnlySpan입니다.</returns>
    public static unsafe ReadOnlySpan<byte> AsBytes<T>(ReadOnlySpan<T> span)
        where T : struct
    {
        if (span.Length == 0)
        {
            return default;
        }

        int byteLength = span.Length * Unsafe.SizeOf<T>();
        ref readonly T reference = ref span.GetPinnableReference();
        return CreateReadOnlySpan<byte>(in Unsafe.As<T, byte>(ref Unsafe.AsRef(in reference)), byteLength);
    }

    /// <summary>
    ///     Span을 다른 형식의 Span으로 캐스팅합니다.
    /// </summary>
    /// <typeparam name="TFrom">소스 요소 형식입니다.</typeparam>
    /// <typeparam name="TTo">대상 요소 형식입니다.</typeparam>
    /// <param name="span">캐스팅할 Span입니다.</param>
    /// <returns>캐스팅된 Span입니다.</returns>
    public static unsafe Span<TTo> Cast<TFrom, TTo>(Span<TFrom> span)
        where TFrom : struct
        where TTo : struct
    {
        if (span.Length == 0)
        {
            return default;
        }

        int fromSize = Unsafe.SizeOf<TFrom>();
        int toSize = Unsafe.SizeOf<TTo>();
        int byteLength = span.Length * fromSize;
        int newLength = byteLength / toSize;

        ref TFrom reference = ref span.GetPinnableReference();
        return CreateSpan<TTo>(ref Unsafe.As<TFrom, TTo>(ref reference), newLength);
    }

    /// <summary>
    ///     ReadOnlySpan을 다른 형식의 ReadOnlySpan으로 캐스팅합니다.
    /// </summary>
    /// <typeparam name="TFrom">소스 요소 형식입니다.</typeparam>
    /// <typeparam name="TTo">대상 요소 형식입니다.</typeparam>
    /// <param name="span">캐스팅할 ReadOnlySpan입니다.</param>
    /// <returns>캐스팅된 ReadOnlySpan입니다.</returns>
    public static unsafe ReadOnlySpan<TTo> Cast<TFrom, TTo>(ReadOnlySpan<TFrom> span)
        where TFrom : struct
        where TTo : struct
    {
        if (span.Length == 0)
        {
            return default;
        }

        int fromSize = Unsafe.SizeOf<TFrom>();
        int toSize = Unsafe.SizeOf<TTo>();
        int byteLength = span.Length * fromSize;
        int newLength = byteLength / toSize;

        ref readonly TFrom reference = ref span.GetPinnableReference();
        return CreateReadOnlySpan<TTo>(in Unsafe.As<TFrom, TTo>(ref Unsafe.AsRef(in reference)), newLength);
    }

    /// <summary>
    ///     ReadOnlySpan에서 첫 번째 요소에 대한 참조를 가져옵니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">참조를 가져올 ReadOnlySpan입니다.</param>
    /// <returns>첫 번째 요소에 대한 참조입니다.</returns>
    public static ref readonly T GetReference<T>(ReadOnlySpan<T> span)
    {
        return ref span.GetPinnableReference();
    }

    /// <summary>
    ///     Span에서 첫 번째 요소에 대한 참조를 가져옵니다.
    /// </summary>
    /// <typeparam name="T">요소 형식입니다.</typeparam>
    /// <param name="span">참조를 가져올 Span입니다.</param>
    /// <returns>첫 번째 요소에 대한 참조입니다.</returns>
    public static ref T GetReference<T>(Span<T> span)
    {
        return ref span.GetPinnableReference();
    }

    /// <summary>
    ///     바이트 ReadOnlySpan에서 값을 읽습니다.
    /// </summary>
    /// <typeparam name="T">읽을 값의 형식입니다.</typeparam>
    /// <param name="source">읽을 소스입니다.</param>
    /// <returns>읽은 값입니다.</returns>
    public static unsafe T Read<T>(ReadOnlySpan<byte> source)
        where T : struct
    {
        if (Unsafe.SizeOf<T>() > source.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(source));
        }

        ref readonly byte reference = ref source.GetPinnableReference();
        return Unsafe.ReadUnaligned<T>(ref Unsafe.AsRef(in reference));
    }

    /// <summary>
    ///     바이트 ReadOnlySpan에서 값을 읽기 시도합니다.
    /// </summary>
    /// <typeparam name="T">읽을 값의 형식입니다.</typeparam>
    /// <param name="source">읽을 소스입니다.</param>
    /// <param name="value">읽은 값입니다.</param>
    /// <returns>읽기에 성공한 경우 true, 그렇지 않으면 false입니다.</returns>
    public static unsafe bool TryRead<T>(ReadOnlySpan<byte> source, out T value)
        where T : struct
    {
        if (Unsafe.SizeOf<T>() > source.Length)
        {
            value = default;
            return false;
        }

        ref readonly byte reference = ref source.GetPinnableReference();
        value = Unsafe.ReadUnaligned<T>(ref Unsafe.AsRef(in reference));
        return true;
    }

    /// <summary>
    ///     바이트 Span에 값을 씁니다.
    /// </summary>
    /// <typeparam name="T">쓸 값의 형식입니다.</typeparam>
    /// <param name="destination">쓸 대상입니다.</param>
    /// <param name="value">쓸 값입니다.</param>
    public static unsafe void Write<T>(Span<byte> destination, in T value)
        where T : struct
    {
        if (Unsafe.SizeOf<T>() > destination.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(destination));
        }

        ref byte reference = ref destination.GetPinnableReference();
        Unsafe.WriteUnaligned<T>(ref reference, value);
    }

    /// <summary>
    ///     바이트 Span에 값을 쓰기 시도합니다.
    /// </summary>
    /// <typeparam name="T">쓸 값의 형식입니다.</typeparam>
    /// <param name="destination">쓸 대상입니다.</param>
    /// <param name="value">쓸 값입니다.</param>
    /// <returns>쓰기에 성공한 경우 true, 그렇지 않으면 false입니다.</returns>
    public static unsafe bool TryWrite<T>(Span<byte> destination, in T value)
        where T : struct
    {
        if (Unsafe.SizeOf<T>() > destination.Length)
        {
            return false;
        }

        ref byte reference = ref destination.GetPinnableReference();
        Unsafe.WriteUnaligned<T>(ref reference, value);
        return true;
    }
}

#endif
