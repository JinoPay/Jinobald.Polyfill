#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

namespace System;

/// <summary>
///     임의의 메모리의 연속 영역에 대한 읽기 전용 표현을 나타냅니다.
/// </summary>
/// <typeparam name="T">ReadOnlyMemory에 저장된 항목의 형식입니다.</typeparam>
public readonly struct ReadOnlyMemory<T>
{
    private readonly T[] _array;
    private readonly int _start;

    /// <summary>
    ///     현재 ReadOnlyMemory의 길이를 가져옵니다.
    /// </summary>
    public int Length { get; }

    /// <summary>
    ///     현재 ReadOnlyMemory가 비어 있는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsEmpty => Length == 0;

    /// <summary>
    ///     현재 ReadOnlyMemory에 대한 ReadOnlySpan을 반환합니다.
    /// </summary>
    public ReadOnlySpan<T> Span => new(_array, _start, Length);

    /// <summary>
    ///     지정된 배열에서 ReadOnlyMemory를 생성합니다.
    /// </summary>
    public ReadOnlyMemory(T[]? array)
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
    ///     지정된 배열의 세그먼트에서 ReadOnlyMemory를 생성합니다.
    /// </summary>
    public ReadOnlyMemory(T[] array, int start, int length)
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
    ///     지정된 오프셋에서 시작하는 이 ReadOnlyMemory의 새 슬라이스를 형성합니다.
    /// </summary>
    public ReadOnlyMemory<T> Slice(int start)
    {
        if ((uint)start > (uint)Length)
        {
            throw new ArgumentOutOfRangeException(nameof(start));
        }

        return new ReadOnlyMemory<T>(_array, _start + start, Length - start);
    }

    /// <summary>
    ///     지정된 오프셋에서 시작하여 지정된 길이를 가지는 이 ReadOnlyMemory의 새 슬라이스를 형성합니다.
    /// </summary>
    public ReadOnlyMemory<T> Slice(int start, int length)
    {
        if ((uint)start > (uint)Length || (uint)length > (uint)(Length - start))
        {
            throw new ArgumentOutOfRangeException();
        }

        return new ReadOnlyMemory<T>(_array, _start + start, length);
    }

    /// <summary>
    ///     이 ReadOnlyMemory의 내용을 새 배열에 복사합니다.
    /// </summary>
    public T[] ToArray()
    {
        return Span.ToArray();
    }

    /// <summary>
    ///     배열을 ReadOnlyMemory로 암시적 변환합니다.
    /// </summary>
    public static implicit operator ReadOnlyMemory<T>(T[] array)
    {
        return new ReadOnlyMemory<T>(array);
    }

    /// <summary>
    ///     빈 ReadOnlyMemory를 반환합니다.
    /// </summary>
    public static ReadOnlyMemory<T> Empty => default;

    /// <summary>
    ///     두 ReadOnlyMemory 인스턴스가 같은지 비교합니다.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is ReadOnlyMemory<T> other)
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
