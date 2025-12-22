#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

namespace System.Buffers;

/// <summary>
///     Memory 인스턴스를 만드는 데 사용되는 추상 기본 클래스입니다.
/// </summary>
/// <typeparam name="T">Memory에 저장할 항목의 형식입니다.</typeparam>
public abstract class MemoryManager<T> : IMemoryOwner<T>, IPinnable
{
    /// <summary>
    ///     이 메모리 관리자에서 만들어진 메모리를 가져옵니다.
    /// </summary>
    public virtual Memory<T> Memory => CreateMemory(GetSpan().Length);

    /// <summary>
    ///     지정된 길이로 메모리를 만듭니다.
    /// </summary>
    /// <param name="length">메모리의 길이입니다.</param>
    /// <returns>새로운 Memory 인스턴스입니다.</returns>
    protected Memory<T> CreateMemory(int length)
    {
        return CreateMemory(0, length);
    }

    /// <summary>
    ///     지정된 시작 인덱스와 길이로 메모리를 만듭니다.
    /// </summary>
    /// <param name="start">시작 인덱스입니다.</param>
    /// <param name="length">메모리의 길이입니다.</param>
    /// <returns>새로운 Memory 인스턴스입니다.</returns>
    protected Memory<T> CreateMemory(int start, int length)
    {
        var span = GetSpan();
        if ((uint)start > (uint)span.Length || (uint)length > (uint)(span.Length - start))
        {
            throw new ArgumentOutOfRangeException();
        }

        var array = span.Slice(start, length).ToArray();
        return new Memory<T>(array);
    }

    /// <summary>
    ///     기본 메모리에 대한 Span을 가져옵니다.
    /// </summary>
    /// <returns>Span 인스턴스입니다.</returns>
    public abstract Span<T> GetSpan();

    /// <summary>
    ///     메모리를 고정하고 핸들을 반환합니다.
    /// </summary>
    /// <param name="elementIndex">고정할 요소의 인덱스입니다.</param>
    /// <returns>메모리 핸들입니다.</returns>
    public abstract MemoryHandle Pin(int elementIndex = 0);

    /// <summary>
    ///     메모리 고정을 해제합니다.
    /// </summary>
    public abstract void Unpin();

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

#endif
