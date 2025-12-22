#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

namespace System.Buffers;

/// <summary>
/// Memory 블록의 소유자를 식별하고 해당 수명을 제어합니다.
/// </summary>
/// <typeparam name="T">메모리의 요소 형식입니다.</typeparam>
// Link: https://learn.microsoft.com/en-us/dotnet/api/system.buffers.imemoryowner-1
public interface IMemoryOwner<T> : IDisposable
{
    /// <summary>
    /// 이 소유자가 소유한 메모리를 가져옵니다.
    /// </summary>
    Memory<T> Memory { get; }
}

#endif
