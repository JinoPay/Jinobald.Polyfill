#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

using System.Runtime.InteropServices;

namespace System.Buffers;

/// <summary>
/// 메모리 핸들을 제공합니다.
/// </summary>
// Link: https://learn.microsoft.com/en-us/dotnet/api/system.buffers.memoryhandle
public unsafe struct MemoryHandle : IDisposable
{
    private void* _pointer;
    private GCHandle _handle;
    private IPinnable? _pinnable;

    /// <summary>
    /// 지정된 매개 변수를 사용하여 MemoryHandle의 새 인스턴스를 만듭니다.
    /// </summary>
    /// <param name="pointer">고정된 메모리에 대한 포인터입니다.</param>
    /// <param name="handle">메모리를 고정하는 데 사용되는 GCHandle입니다.</param>
    /// <param name="pinnable">고정할 수 있는 객체입니다.</param>
    public MemoryHandle(void* pointer, GCHandle handle = default, IPinnable? pinnable = null)
    {
        _pointer = pointer;
        _handle = handle;
        _pinnable = pinnable;
    }

    /// <summary>
    /// 고정된 메모리에 대한 포인터를 반환합니다.
    /// </summary>
    public void* Pointer => _pointer;

    /// <summary>
    /// 핸들에서 사용하는 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        if (_handle.IsAllocated)
        {
            _handle.Free();
        }

        if (_pinnable != null)
        {
            _pinnable.Unpin();
            _pinnable = null;
        }

        _pointer = null;
    }
}

/// <summary>
/// 고정할 수 있는 객체에 대한 메커니즘을 제공합니다.
/// </summary>
// Link: https://learn.microsoft.com/en-us/dotnet/api/system.buffers.ipinnable
public interface IPinnable
{
    /// <summary>
    /// 객체를 고정하고 고정된 요소에 대한 핸들을 반환합니다.
    /// </summary>
    /// <param name="elementIndex">고정할 요소의 오프셋입니다.</param>
    /// <returns>고정된 요소에 대한 핸들입니다.</returns>
    MemoryHandle Pin(int elementIndex);

    /// <summary>
    /// 고정된 객체의 고정을 해제합니다.
    /// </summary>
    void Unpin();
}

#endif
