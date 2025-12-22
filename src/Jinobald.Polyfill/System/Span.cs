#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

namespace System;

/// <summary>
///     임의의 메모리의 연속 영역에 대한 형식 안전 및 메모리 안전 표현을 제공합니다.
/// </summary>
/// <typeparam name="T">Span에 저장된 항목의 형식입니다.</typeparam>
public readonly ref struct Span<T>
{
    private readonly T[] _array;
    private readonly int _start;

    /// <summary>
    ///     현재 Span의 길이를 가져옵니다.
    /// </summary>
    public int Length { get; }

    /// <summary>
    ///     현재 Span이 비어 있는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsEmpty => Length == 0;

    /// <summary>
    ///     지정된 배열에서 Span을 생성합니다.
    /// </summary>
    public Span(T[]? array)
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
    ///     지정된 배열의 세그먼트에서 Span을 생성합니다.
    /// </summary>
    public Span(T[] array, int start, int length)
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
    ///     현재 Span에서 지정된 인덱스의 요소를 가져오거나 설정합니다.
    /// </summary>
    public ref T this[int index]
    {
        get
        {
            if ((uint)index >= (uint)Length)
            {
                throw new IndexOutOfRangeException();
            }

            return ref _array[_start + index];
        }
    }

    /// <summary>
    ///     현재 Span에서 모든 요소를 지정된 값으로 설정합니다.
    /// </summary>
    public void Fill(T value)
    {
        for (int i = 0; i < Length; i++)
        {
            _array[_start + i] = value;
        }
    }

    /// <summary>
    ///     현재 Span의 내용을 기본값으로 지웁니다.
    /// </summary>
    public void Clear()
    {
        if (_array != null && Length > 0)
        {
            Array.Clear(_array, _start, Length);
        }
    }

    /// <summary>
    ///     현재 Span의 내용을 지정된 대상 Span에 복사합니다.
    /// </summary>
    public void CopyTo(Span<T> destination)
    {
        if (Length > destination.Length)
        {
            throw new ArgumentException("대상이 너무 짧습니다.");
        }

        for (int i = 0; i < Length; i++)
        {
            destination._array[destination._start + i] = _array[_start + i];
        }
    }

    /// <summary>
    ///     현재 Span의 내용을 지정된 대상 Span에 복사를 시도합니다.
    /// </summary>
    public bool TryCopyTo(Span<T> destination)
    {
        if (Length > destination.Length)
        {
            return false;
        }

        CopyTo(destination);
        return true;
    }

    /// <summary>
    ///     지정된 오프셋에서 시작하는 이 Span의 새 슬라이스를 형성합니다.
    /// </summary>
    public Span<T> Slice(int start)
    {
        if ((uint)start > (uint)Length)
        {
            throw new ArgumentOutOfRangeException(nameof(start));
        }

        return new Span<T>(_array, _start + start, Length - start);
    }

    /// <summary>
    ///     지정된 오프셋에서 시작하여 지정된 길이를 가지는 이 Span의 새 슬라이스를 형성합니다.
    /// </summary>
    public Span<T> Slice(int start, int length)
    {
        if ((uint)start > (uint)Length || (uint)length > (uint)(Length - start))
        {
            throw new ArgumentOutOfRangeException();
        }

        return new Span<T>(_array, _start + start, length);
    }

    /// <summary>
    ///     Span의 문자열 표현을 반환합니다.
    ///     char 형식의 경우 실제 문자열을 반환하고, 그 외의 경우 형식 정보를 반환합니다.
    /// </summary>
    public override string ToString()
    {
        if (typeof(T) == typeof(char))
        {
            if (_array == null || Length == 0)
            {
                return string.Empty;
            }
            return new string((char[])(object)_array, _start, Length);
        }
        return string.Format("System.Span<{0}>[{1}]", typeof(T).Name, Length);
    }

    /// <summary>
    ///     이 Span의 내용을 새 배열에 복사합니다.
    /// </summary>
    public T[] ToArray()
    {
        if (Length == 0)
        {
            return new T[0];
        }

        var result = new T[Length];
        for (int i = 0; i < Length; i++)
        {
            result[i] = _array[_start + i];
        }

        return result;
    }

    /// <summary>
    ///     현재 Span에 대한 열거자를 반환합니다.
    /// </summary>
    public Enumerator GetEnumerator()
    {
        return new Enumerator(this);
    }

    /// <summary>
    ///     Span에 대한 열거자입니다.
    /// </summary>
    public ref struct Enumerator
    {
        private readonly Span<T> _span;
        private int _index;

        internal Enumerator(Span<T> span)
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

        public ref T Current => ref _span[_index];
    }

    /// <summary>
    ///     배열을 Span으로 암시적 변환합니다.
    /// </summary>
    public static implicit operator Span<T>(T[] array)
    {
        return new Span<T>(array);
    }

    /// <summary>
    ///     Span을 ReadOnlySpan으로 암시적 변환합니다.
    /// </summary>
    public static implicit operator ReadOnlySpan<T>(Span<T> span)
    {
        return new ReadOnlySpan<T>(span._array, span._start, span.Length);
    }

    /// <summary>
    ///     빈 Span을 반환합니다.
    /// </summary>
    public static Span<T> Empty => default;
}

#endif
