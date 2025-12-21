#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

namespace System;

/// <summary>
///     임의의 메모리의 연속 영역을 나타냅니다.
/// </summary>
/// <typeparam name="T">Memory에 저장된 항목의 형식입니다.</typeparam>
public readonly struct Memory<T>
{
    private readonly T[] _array;
    private readonly int _start;

    /// <summary>
    ///     현재 Memory의 길이를 가져옵니다.
    /// </summary>
    public int Length { get; }

    /// <summary>
    ///     현재 Memory가 비어 있는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsEmpty => Length == 0;

    /// <summary>
    ///     현재 Memory에 대한 Span을 반환합니다.
    /// </summary>
    public Span<T> Span => new(_array, _start, Length);

    /// <summary>
    ///     지정된 배열에서 Memory를 생성합니다.
    /// </summary>
    public Memory(T[] array)
    {
        if (array == null)
        {
            _array = null!;
            _start = 0;
            Length = 0;
            return;
        }

        _array = array;
        _start = 0;
        Length = array.Length;
    }

    /// <summary>
    ///     지정된 배열의 세그먼트에서 Memory를 생성합니다.
    /// </summary>
    public Memory(T[] array, int start, int length)
    {
        if (array == null)
        {
            if (start != 0 || length != 0)
            {
                throw new ArgumentException();
            }

            _array = null!;
            _start = 0;
            Length = 0;
            return;
        }

        if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start))
        {
            throw new ArgumentOutOfRangeException();
        }

        _array = array;
        _start = start;
        Length = length;
    }

    /// <summary>
    ///     지정된 오프셋에서 시작하는 이 Memory의 새 슬라이스를 형성합니다.
    /// </summary>
    public Memory<T> Slice(int start)
    {
        if ((uint)start > (uint)Length)
        {
            throw new ArgumentOutOfRangeException(nameof(start));
        }

        return new Memory<T>(_array, _start + start, Length - start);
    }

    /// <summary>
    ///     지정된 오프셋에서 시작하여 지정된 길이를 가지는 이 Memory의 새 슬라이스를 형성합니다.
    /// </summary>
    public Memory<T> Slice(int start, int length)
    {
        if ((uint)start > (uint)Length || (uint)length > (uint)(Length - start))
        {
            throw new ArgumentOutOfRangeException();
        }

        return new Memory<T>(_array, _start + start, length);
    }

    /// <summary>
    ///     이 Memory의 내용을 새 배열에 복사합니다.
    /// </summary>
    public T[] ToArray()
    {
        return Span.ToArray();
    }

    /// <summary>
    ///     배열을 Memory로 암시적 변환합니다.
    /// </summary>
    public static implicit operator Memory<T>(T[] array)
    {
        return new Memory<T>(array);
    }

    /// <summary>
    ///     Memory를 ReadOnlyMemory로 암시적 변환합니다.
    /// </summary>
    public static implicit operator ReadOnlyMemory<T>(Memory<T> memory)
    {
        return new ReadOnlyMemory<T>(memory._array, memory._start, memory.Length);
    }

    /// <summary>
    ///     빈 Memory를 반환합니다.
    /// </summary>
    public static Memory<T> Empty => default;

    /// <summary>
    ///     두 Memory 인스턴스가 같은지 비교합니다.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is Memory<T> other)
        {
            return _array == other._array && _start == other._start && Length == other.Length;
        }

        return false;
    }

    /// <summary>
    ///     이 인스턴스에 대한 해시 코드를 반환합니다.
    /// </summary>
    public override int GetHashCode()
    {
        return _array != null ? _array.GetHashCode() ^ _start ^ Length : 0;
    }
}

#endif
