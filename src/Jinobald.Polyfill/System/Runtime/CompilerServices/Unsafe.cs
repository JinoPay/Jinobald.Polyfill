#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices;

/// <summary>
/// 관리되는 포인터를 조작하기 위한 일반적인 저수준 기능을 포함합니다.
/// </summary>
// Link: https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafe
public static unsafe class Unsafe
{
    /// <summary>
    /// 지정된 형식의 크기(바이트)를 반환합니다.
    /// </summary>
    /// <typeparam name="T">크기를 계산할 형식입니다.</typeparam>
    /// <returns>지정된 형식 매개 변수의 크기입니다.</returns>
    public static int SizeOf<T>()
    {
        return Marshal.SizeOf(typeof(T));
    }

    /// <summary>
    /// 지정된 관리되는 포인터를 새 형식의 관리되는 포인터로 재해석합니다.
    /// </summary>
    /// <typeparam name="TFrom">변환할 원래 형식입니다.</typeparam>
    /// <typeparam name="TTo">변환될 대상 형식입니다.</typeparam>
    /// <param name="source">재해석할 관리되는 포인터입니다.</param>
    /// <returns>새 형식의 관리되는 포인터입니다.</returns>
    public static ref TTo As<TFrom, TTo>(ref TFrom source)
    {
        // 참조를 포인터로 변환하여 재해석
        // 실제 .NET의 Unsafe.As는 IL 수준에서 동작하지만
        // 폴리필에서는 포인터 변환을 사용
        fixed (TFrom* ptr = &source)
        {
            return ref *(TTo*)ptr;
        }
    }

    /// <summary>
    /// 지정된 개체를 지정된 형식의 개체로 캐스팅합니다.
    /// </summary>
    /// <typeparam name="T">개체를 캐스팅할 형식입니다.</typeparam>
    /// <param name="o">캐스팅할 개체입니다.</param>
    /// <returns>지정된 형식으로 캐스팅된 원래 개체입니다.</returns>
    public static T As<T>(object? o)
        where T : class?
    {
        return (T)o!;
    }

    /// <summary>
    /// 참조에 요소 오프셋을 추가합니다.
    /// </summary>
    /// <typeparam name="T">참조의 형식입니다.</typeparam>
    /// <param name="source">오프셋을 추가할 참조입니다.</param>
    /// <param name="elementOffset">추가할 오프셋입니다.</param>
    /// <returns>새 참조입니다.</returns>
    public static ref T Add<T>(ref T source, int elementOffset)
    {
        fixed (T* ptr = &source)
        {
            return ref *(ptr + elementOffset);
        }
    }

    /// <summary>
    /// 참조에 요소 오프셋을 추가합니다.
    /// </summary>
    /// <typeparam name="T">참조의 형식입니다.</typeparam>
    /// <param name="source">오프셋을 추가할 참조입니다.</param>
    /// <param name="elementOffset">추가할 오프셋입니다.</param>
    /// <returns>새 참조입니다.</returns>
    public static ref T Add<T>(ref T source, IntPtr elementOffset)
    {
        return ref Add(ref source, (int)elementOffset);
    }

    /// <summary>
    /// 참조에서 요소 오프셋을 뺍니다.
    /// </summary>
    /// <typeparam name="T">참조의 형식입니다.</typeparam>
    /// <param name="source">오프셋을 뺄 참조입니다.</param>
    /// <param name="elementOffset">뺄 오프셋입니다.</param>
    /// <returns>새 참조입니다.</returns>
    public static ref T Subtract<T>(ref T source, int elementOffset)
    {
        return ref Add(ref source, -elementOffset);
    }

    /// <summary>
    /// 참조에 바이트 오프셋을 추가합니다.
    /// </summary>
    /// <typeparam name="T">참조의 형식입니다.</typeparam>
    /// <param name="source">오프셋을 추가할 참조입니다.</param>
    /// <param name="byteOffset">추가할 바이트 오프셋입니다.</param>
    /// <returns>새 참조입니다.</returns>
    public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset)
    {
        fixed (T* ptr = &source)
        {
            return ref *(T*)((byte*)ptr + (int)byteOffset);
        }
    }

    /// <summary>
    /// 두 관리되는 포인터 사이의 요소 오프셋을 계산합니다.
    /// </summary>
    /// <typeparam name="T">참조의 형식입니다.</typeparam>
    /// <param name="origin">첫 번째 참조입니다.</param>
    /// <param name="target">두 번째 참조입니다.</param>
    /// <returns>두 포인터 사이의 요소 오프셋입니다.</returns>
    public static IntPtr ByteOffset<T>(ref T origin, ref T target)
    {
        fixed (T* pOrigin = &origin)
        fixed (T* pTarget = &target)
        {
            return (IntPtr)((byte*)pTarget - (byte*)pOrigin);
        }
    }

    /// <summary>
    /// 지정된 위치에서 형식 T의 값을 읽습니다.
    /// </summary>
    /// <typeparam name="T">읽을 값의 형식입니다.</typeparam>
    /// <param name="source">읽을 위치입니다.</param>
    /// <returns>읽은 값입니다.</returns>
    public static T Read<T>(void* source)
    {
        return *(T*)source;
    }

    /// <summary>
    /// 지정된 위치에서 형식 T의 값을 읽습니다 (정렬되지 않음).
    /// </summary>
    /// <typeparam name="T">읽을 값의 형식입니다.</typeparam>
    /// <param name="source">읽을 위치입니다.</param>
    /// <returns>읽은 값입니다.</returns>
    public static T ReadUnaligned<T>(void* source)
    {
        // 바이트 단위로 복사하여 정렬 문제 방지
        T result = default!;
        byte* dest = (byte*)&result;
        byte* src = (byte*)source;
        int size = SizeOf<T>();

        for (int i = 0; i < size; i++)
        {
            dest[i] = src[i];
        }

        return result;
    }

    /// <summary>
    /// 지정된 참조에서 형식 T의 값을 읽습니다 (정렬되지 않음).
    /// </summary>
    /// <typeparam name="T">읽을 값의 형식입니다.</typeparam>
    /// <param name="source">읽을 참조입니다.</param>
    /// <returns>읽은 값입니다.</returns>
    public static T ReadUnaligned<T>(ref byte source)
    {
        fixed (byte* ptr = &source)
        {
            return ReadUnaligned<T>(ptr);
        }
    }

    /// <summary>
    /// 지정된 위치에 값을 씁니다.
    /// </summary>
    /// <typeparam name="T">쓸 값의 형식입니다.</typeparam>
    /// <param name="destination">쓸 위치입니다.</param>
    /// <param name="value">쓸 값입니다.</param>
    public static void Write<T>(void* destination, T value)
    {
        *(T*)destination = value;
    }

    /// <summary>
    /// 지정된 위치에 값을 씁니다 (정렬되지 않음).
    /// </summary>
    /// <typeparam name="T">쓸 값의 형식입니다.</typeparam>
    /// <param name="destination">쓸 위치입니다.</param>
    /// <param name="value">쓸 값입니다.</param>
    public static void WriteUnaligned<T>(void* destination, T value)
    {
        // 바이트 단위로 복사하여 정렬 문제 방지
        byte* dest = (byte*)destination;
        byte* src = (byte*)&value;
        int size = SizeOf<T>();

        for (int i = 0; i < size; i++)
        {
            dest[i] = src[i];
        }
    }

    /// <summary>
    /// 지정된 참조에 값을 씁니다 (정렬되지 않음).
    /// </summary>
    /// <typeparam name="T">쓸 값의 형식입니다.</typeparam>
    /// <param name="destination">쓸 참조입니다.</param>
    /// <param name="value">쓸 값입니다.</param>
    public static void WriteUnaligned<T>(ref byte destination, T value)
    {
        fixed (byte* ptr = &destination)
        {
            WriteUnaligned(ptr, value);
        }
    }

    /// <summary>
    /// 지정된 관리되지 않는 포인터의 값을 복사합니다.
    /// </summary>
    /// <typeparam name="T">복사할 형식입니다.</typeparam>
    /// <param name="destination">대상 위치입니다.</param>
    /// <param name="source">원본 위치입니다.</param>
    public static void Copy<T>(void* destination, ref T source)
    {
        *(T*)destination = source;
    }

    /// <summary>
    /// 지정된 관리되지 않는 포인터에서 값을 복사합니다.
    /// </summary>
    /// <typeparam name="T">복사할 형식입니다.</typeparam>
    /// <param name="destination">대상 참조입니다.</param>
    /// <param name="source">원본 위치입니다.</param>
    public static void Copy<T>(ref T destination, void* source)
    {
        destination = *(T*)source;
    }

    /// <summary>
    /// 관리되지 않는 포인터를 관리되는 포인터로 변환합니다.
    /// </summary>
    /// <typeparam name="T">참조의 형식입니다.</typeparam>
    /// <param name="source">변환할 관리되지 않는 포인터입니다.</param>
    /// <returns>관리되는 포인터입니다.</returns>
    public static ref T AsRef<T>(void* source)
    {
        return ref *(T*)source;
    }

    /// <summary>
    /// 읽기 전용 참조를 변경 가능한 참조로 재해석합니다.
    /// </summary>
    /// <typeparam name="T">참조의 형식입니다.</typeparam>
    /// <param name="source">변환할 읽기 전용 참조입니다.</param>
    /// <returns>변경 가능한 참조입니다.</returns>
    public static ref T AsRef<T>(in T source)
    {
        fixed (T* ptr = &source)
        {
            return ref *ptr;
        }
    }

    /// <summary>
    /// 두 관리되는 포인터가 같은지 확인합니다.
    /// </summary>
    /// <typeparam name="T">참조의 형식입니다.</typeparam>
    /// <param name="left">첫 번째 참조입니다.</param>
    /// <param name="right">두 번째 참조입니다.</param>
    /// <returns>두 포인터가 같으면 true이고, 그렇지 않으면 false입니다.</returns>
    public static bool AreSame<T>(ref T left, ref T right)
    {
        fixed (T* pLeft = &left)
        fixed (T* pRight = &right)
        {
            return pLeft == pRight;
        }
    }

    /// <summary>
    /// 지정된 참조가 지정된 다른 참조보다 큰 주소에 있는지 확인합니다.
    /// </summary>
    /// <typeparam name="T">참조의 형식입니다.</typeparam>
    /// <param name="left">첫 번째 참조입니다.</param>
    /// <param name="right">두 번째 참조입니다.</param>
    /// <returns>left가 right보다 큰 주소에 있으면 true이고, 그렇지 않으면 false입니다.</returns>
    public static bool IsAddressGreaterThan<T>(ref T left, ref T right)
    {
        fixed (T* pLeft = &left)
        fixed (T* pRight = &right)
        {
            return pLeft > pRight;
        }
    }

    /// <summary>
    /// 지정된 참조가 지정된 다른 참조보다 작은 주소에 있는지 확인합니다.
    /// </summary>
    /// <typeparam name="T">참조의 형식입니다.</typeparam>
    /// <param name="left">첫 번째 참조입니다.</param>
    /// <param name="right">두 번째 참조입니다.</param>
    /// <returns>left가 right보다 작은 주소에 있으면 true이고, 그렇지 않으면 false입니다.</returns>
    public static bool IsAddressLessThan<T>(ref T left, ref T right)
    {
        fixed (T* pLeft = &left)
        fixed (T* pRight = &right)
        {
            return pLeft < pRight;
        }
    }

    /// <summary>
    /// 메모리 블록을 초기화합니다.
    /// </summary>
    /// <param name="startAddress">초기화할 메모리 블록의 시작 주소입니다.</param>
    /// <param name="value">각 바이트를 초기화할 값입니다.</param>
    /// <param name="byteCount">초기화할 바이트 수입니다.</param>
    public static void InitBlock(void* startAddress, byte value, uint byteCount)
    {
        byte* ptr = (byte*)startAddress;
        for (uint i = 0; i < byteCount; i++)
        {
            ptr[i] = value;
        }
    }

    /// <summary>
    /// 메모리 블록을 초기화합니다.
    /// </summary>
    /// <param name="startAddress">초기화할 메모리 블록의 시작 참조입니다.</param>
    /// <param name="value">각 바이트를 초기화할 값입니다.</param>
    /// <param name="byteCount">초기화할 바이트 수입니다.</param>
    public static void InitBlockUnaligned(ref byte startAddress, byte value, uint byteCount)
    {
        fixed (byte* ptr = &startAddress)
        {
            InitBlock(ptr, value, byteCount);
        }
    }

    /// <summary>
    /// 메모리 블록을 복사합니다.
    /// </summary>
    /// <param name="destination">복사 대상 주소입니다.</param>
    /// <param name="source">복사 원본 주소입니다.</param>
    /// <param name="byteCount">복사할 바이트 수입니다.</param>
    public static void CopyBlock(void* destination, void* source, uint byteCount)
    {
        byte* dest = (byte*)destination;
        byte* src = (byte*)source;
        for (uint i = 0; i < byteCount; i++)
        {
            dest[i] = src[i];
        }
    }

    /// <summary>
    /// 메모리 블록을 복사합니다.
    /// </summary>
    /// <param name="destination">복사 대상 참조입니다.</param>
    /// <param name="source">복사 원본 참조입니다.</param>
    /// <param name="byteCount">복사할 바이트 수입니다.</param>
    public static void CopyBlockUnaligned(ref byte destination, ref byte source, uint byteCount)
    {
        fixed (byte* dest = &destination)
        fixed (byte* src = &source)
        {
            CopyBlock(dest, src, byteCount);
        }
    }

    /// <summary>
    /// 지정된 형식이 참조 형식인지 또는 참조를 포함하는지 확인합니다.
    /// </summary>
    /// <typeparam name="T">확인할 형식입니다.</typeparam>
    /// <returns>형식이 참조를 포함하면 true이고, 그렇지 않으면 false입니다.</returns>
    public static bool IsNullRef<T>(ref T source)
    {
        fixed (T* ptr = &source)
        {
            return ptr == null;
        }
    }

    /// <summary>
    /// null 참조를 반환합니다.
    /// </summary>
    /// <typeparam name="T">참조의 형식입니다.</typeparam>
    /// <returns>null 참조입니다.</returns>
    public static ref T NullRef<T>()
    {
        return ref AsRef<T>(null);
    }

    /// <summary>
    /// 건너뛰기 초기화를 수행합니다.
    /// </summary>
    /// <typeparam name="T">건너뛸 형식입니다.</typeparam>
    /// <param name="value">초기화되지 않은 값입니다.</param>
    public static void SkipInit<T>(out T value)
    {
        // 이 메서드는 out 매개변수를 초기화하지 않고 반환합니다.
        // C#에서는 out 매개변수를 반드시 초기화해야 하므로 기본값을 할당합니다.
        value = default!;
    }
}

#endif
