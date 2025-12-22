#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

namespace System.Buffers;

/// <summary>
///     메모리 블록 풀을 나타냅니다.
/// </summary>
/// <typeparam name="T">풀의 메모리 블록에 저장할 항목의 형식입니다.</typeparam>
public abstract class MemoryPool<T> : IDisposable
{
    private static readonly ArrayMemoryPool<T> s_shared = new();

    /// <summary>
    ///     공유 메모리 풀 인스턴스를 가져옵니다.
    /// </summary>
    public static MemoryPool<T> Shared => s_shared;

    /// <summary>
    ///     이 풀에서 빌릴 수 있는 최대 버퍼 크기를 가져옵니다.
    /// </summary>
    public abstract int MaxBufferSize { get; }

    /// <summary>
    ///     지정된 최소 길이를 가진 메모리 블록을 빌립니다.
    /// </summary>
    /// <param name="minBufferSize">요청된 최소 버퍼 크기입니다.</param>
    /// <returns>메모리 소유자입니다.</returns>
    public abstract IMemoryOwner<T> Rent(int minBufferSize = -1);

    /// <summary>
    ///     관리되는 리소스와 관리되지 않는 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     관리되는 리소스와 관리되지 않는 리소스를 해제합니다.
    /// </summary>
    /// <param name="disposing">관리되는 리소스를 해제해야 하면 true, 그렇지 않으면 false입니다.</param>
    protected abstract void Dispose(bool disposing);
}

/// <summary>
///     ArrayPool을 사용하는 MemoryPool 구현입니다.
/// </summary>
/// <typeparam name="T">풀의 메모리 블록에 저장할 항목의 형식입니다.</typeparam>
internal sealed class ArrayMemoryPool<T> : MemoryPool<T>
{
    private const int DefaultMaxBufferSize = int.MaxValue;

    /// <summary>
    ///     이 풀에서 빌릴 수 있는 최대 버퍼 크기를 가져옵니다.
    /// </summary>
    public override int MaxBufferSize => DefaultMaxBufferSize;

    /// <summary>
    ///     지정된 최소 길이를 가진 메모리 블록을 빌립니다.
    /// </summary>
    /// <param name="minBufferSize">요청된 최소 버퍼 크기입니다.</param>
    /// <returns>메모리 소유자입니다.</returns>
    public override IMemoryOwner<T> Rent(int minBufferSize = -1)
    {
        if (minBufferSize == -1)
        {
            minBufferSize = 1 + (4095 / Runtime.CompilerServices.Unsafe.SizeOf<T>());
        }
        else if ((uint)minBufferSize > DefaultMaxBufferSize)
        {
            throw new ArgumentOutOfRangeException(nameof(minBufferSize));
        }

        return new ArrayMemoryPoolBuffer(minBufferSize);
    }

    /// <summary>
    ///     관리되는 리소스와 관리되지 않는 리소스를 해제합니다.
    /// </summary>
    /// <param name="disposing">관리되는 리소스를 해제해야 하면 true, 그렇지 않으면 false입니다.</param>
    protected override void Dispose(bool disposing)
    {
        // 공유 풀은 dispose되지 않음
    }

    /// <summary>
    ///     ArrayPool을 사용하는 메모리 버퍼입니다.
    /// </summary>
    private sealed class ArrayMemoryPoolBuffer : IMemoryOwner<T>
    {
        private T[]? _array;

        /// <summary>
        ///     지정된 크기로 메모리 버퍼를 만듭니다.
        /// </summary>
        /// <param name="size">버퍼 크기입니다.</param>
        public ArrayMemoryPoolBuffer(int size)
        {
            _array = ArrayPool<T>.Shared.Rent(size);
        }

        /// <summary>
        ///     이 소유자가 보유한 메모리를 가져옵니다.
        /// </summary>
        public Memory<T> Memory
        {
            get
            {
                var array = _array;
                if (array == null)
                {
                    throw new ObjectDisposedException(nameof(ArrayMemoryPoolBuffer));
                }

                return new Memory<T>(array);
            }
        }

        /// <summary>
        ///     리소스를 해제하고 배열을 풀에 반환합니다.
        /// </summary>
        public void Dispose()
        {
            var array = _array;
            if (array != null)
            {
                _array = null;
                ArrayPool<T>.Shared.Return(array);
            }
        }
    }
}

#endif
