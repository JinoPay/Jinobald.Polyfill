#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

namespace System;

/// <summary>
/// 임의의 메모리의 연속 영역에 대한 형식 안전 및 메모리 안전 읽기 전용 표현을 제공합니다.
/// </summary>
/// <typeparam name="T">ReadOnlySpan에 저장된 항목의 형식입니다.</typeparam>
public readonly ref struct ReadOnlySpan<T>
{
    private readonly T[] _array;
    private readonly int _start;
    private readonly int _length;

    /// <summary>
    /// 현재 ReadOnlySpan의 길이를 가져옵니다.
    /// </summary>
    public int Length => _length;

    /// <summary>
    /// 현재 ReadOnlySpan이 비어 있는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsEmpty => _length == 0;

    /// <summary>
    /// 지정된 배열에서 ReadOnlySpan을 생성합니다.
    /// </summary>
    public ReadOnlySpan(T[] array)
    {
        if (array == null)
        {
            _array = null!;
            _start = 0;
            _length = 0;
            return;
        }

        _array = array;
        _start = 0;
        _length = array.Length;
    }

    /// <summary>
    /// 지정된 배열의 세그먼트에서 ReadOnlySpan을 생성합니다.
    /// </summary>
    public ReadOnlySpan(T[] array, int start, int length)
    {
        if (array == null)
        {
            if (start != 0 || length != 0)
                throw new ArgumentException();
            _array = null!;
            _start = 0;
            _length = 0;
            return;
        }

        if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start))
            throw new ArgumentOutOfRangeException();

        _array = array;
        _start = start;
        _length = length;
    }

    /// <summary>
    /// 현재 ReadOnlySpan에서 지정된 인덱스의 요소를 가져옵니다.
    /// </summary>
    public ref readonly T this[int index]
    {
        get
        {
            if ((uint)index >= (uint)_length)
                throw new IndexOutOfRangeException();
            return ref _array[_start + index];
        }
    }

    /// <summary>
    /// 현재 ReadOnlySpan의 내용을 지정된 대상 Span에 복사합니다.
    /// </summary>
    public void CopyTo(Span<T> destination)
    {
        if (_length > destination.Length)
            throw new ArgumentException("Destination too short.");

        for (int i = 0; i < _length; i++)
        {
            destination[i] = _array[_start + i];
        }
    }

    /// <summary>
    /// 현재 ReadOnlySpan의 내용을 지정된 대상 Span에 복사를 시도합니다.
    /// </summary>
    public bool TryCopyTo(Span<T> destination)
    {
        if (_length > destination.Length)
            return false;

        CopyTo(destination);
        return true;
    }

    /// <summary>
    /// 지정된 오프셋에서 시작하는 이 ReadOnlySpan의 새 슬라이스를 형성합니다.
    /// </summary>
    public ReadOnlySpan<T> Slice(int start)
    {
        if ((uint)start > (uint)_length)
            throw new ArgumentOutOfRangeException(nameof(start));

        return new ReadOnlySpan<T>(_array, _start + start, _length - start);
    }

    /// <summary>
    /// 지정된 오프셋에서 시작하여 지정된 길이를 가지는 이 ReadOnlySpan의 새 슬라이스를 형성합니다.
    /// </summary>
    public ReadOnlySpan<T> Slice(int start, int length)
    {
        if ((uint)start > (uint)_length || (uint)length > (uint)(_length - start))
            throw new ArgumentOutOfRangeException();

        return new ReadOnlySpan<T>(_array, _start + start, length);
    }

    /// <summary>
    /// 이 ReadOnlySpan의 내용을 새 배열에 복사합니다.
    /// </summary>
    public T[] ToArray()
    {
        if (_length == 0)
            return new T[0];

        var result = new T[_length];
        for (int i = 0; i < _length; i++)
        {
            result[i] = _array[_start + i];
        }
        return result;
    }

    /// <summary>
    /// 현재 ReadOnlySpan에 대한 열거자를 반환합니다.
    /// </summary>
    public Enumerator GetEnumerator() => new Enumerator(this);

    /// <summary>
    /// ReadOnlySpan에 대한 열거자입니다.
    /// </summary>
    public ref struct Enumerator
    {
        private readonly ReadOnlySpan<T> _span;
        private int _index;

        internal Enumerator(ReadOnlySpan<T> span)
        {
            _span = span;
            _index = -1;
        }

        public bool MoveNext()
        {
            int index = _index + 1;
            if (index < _span.Length)
            {
                _index = index;
                return true;
            }
            return false;
        }

        public ref readonly T Current => ref _span[_index];
    }

    /// <summary>
    /// 배열을 ReadOnlySpan으로 암시적 변환합니다.
    /// </summary>
    public static implicit operator ReadOnlySpan<T>(T[] array) => new ReadOnlySpan<T>(array);

    /// <summary>
    /// 빈 ReadOnlySpan을 반환합니다.
    /// </summary>
    public static ReadOnlySpan<T> Empty => default;
}

#endif
