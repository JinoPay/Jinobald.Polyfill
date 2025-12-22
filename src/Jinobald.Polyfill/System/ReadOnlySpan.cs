#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

namespace System;

/// <summary>
///     임의의 메모리의 연속 영역에 대한 형식 안전 및 메모리 안전 읽기 전용 표현을 제공합니다.
/// </summary>
/// <typeparam name="T">ReadOnlySpan에 저장된 항목의 형식입니다.</typeparam>
public readonly ref struct ReadOnlySpan<T>
{
    private readonly T[] _array;
    private readonly int _start;
    private readonly IntPtr _pointer;

    /// <summary>
    ///     현재 ReadOnlySpan의 길이를 가져옵니다.
    /// </summary>
    public int Length { get; }

    /// <summary>
    ///     현재 ReadOnlySpan이 비어 있는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsEmpty => Length == 0;

    /// <summary>
    ///     지정된 배열에서 ReadOnlySpan을 생성합니다.
    /// </summary>
    public ReadOnlySpan(T[]? array)
    {
        if (array == null)
        {
            _array = null!;
            _start = 0;
            _pointer = IntPtr.Zero;
            Length = 0;
            return;
        }

        _array = array;
        _start = 0;
        _pointer = IntPtr.Zero;
        Length = array.Length;
    }

    /// <summary>
    ///     지정된 배열의 세그먼트에서 ReadOnlySpan을 생성합니다.
    /// </summary>
    public ReadOnlySpan(T[] array, int start, int length)
    {
        if (array == null)
        {
            if (start != 0 || length != 0)
            {
                throw new ArgumentException();
            }

            _array = null!;
            _start = 0;
            _pointer = IntPtr.Zero;
            Length = 0;
            return;
        }

        if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start))
        {
            throw new ArgumentOutOfRangeException();
        }

        _array = array;
        _start = start;
        _pointer = IntPtr.Zero;
        Length = length;
    }

    /// <summary>
    ///     지정된 포인터와 길이로 ReadOnlySpan을 생성합니다.
    /// </summary>
    /// <param name="pointer">관리되지 않는 메모리에 대한 포인터입니다.</param>
    /// <param name="length">ReadOnlySpan의 요소 수입니다.</param>
    public unsafe ReadOnlySpan(void* pointer, int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        _array = null!;
        _start = 0;
        _pointer = (IntPtr)pointer;
        Length = length;
    }

    /// <summary>
    ///     현재 ReadOnlySpan에서 지정된 인덱스의 요소를 가져옵니다.
    /// </summary>
    public unsafe ref readonly T this[int index]
    {
        get
        {
            if ((uint)index >= (uint)Length)
            {
                throw new IndexOutOfRangeException();
            }

            if (_pointer != IntPtr.Zero)
            {
                return ref ((T*)(void*)_pointer)[index];
            }

            return ref _array[_start + index];
        }
    }

    /// <summary>
    ///     지정된 범위의 요소로 구성된 슬라이스를 반환합니다.
    /// </summary>
    /// <param name="range">ReadOnlySpan의 시작 및 끝 인덱스를 나타내는 범위입니다.</param>
    /// <returns>지정된 범위의 요소를 포함하는 ReadOnlySpan입니다.</returns>
    public ReadOnlySpan<T> this[Range range]
    {
        get
        {
            var (offset, length) = range.GetOffsetAndLength(Length);
            return Slice(offset, length);
        }
    }

    /// <summary>
    ///     현재 ReadOnlySpan의 내용을 지정된 대상 Span에 복사합니다.
    /// </summary>
    public void CopyTo(Span<T> destination)
    {
        if (Length > destination.Length)
        {
            throw new ArgumentException("대상이 너무 짧습니다.");
        }

        for (int i = 0; i < Length; i++)
        {
            destination[i] = this[i];
        }
    }

    /// <summary>
    ///     현재 ReadOnlySpan의 내용을 지정된 대상 Span에 복사를 시도합니다.
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
    ///     지정된 오프셋에서 시작하는 이 ReadOnlySpan의 새 슬라이스를 형성합니다.
    /// </summary>
    public unsafe ReadOnlySpan<T> Slice(int start)
    {
        if ((uint)start > (uint)Length)
        {
            throw new ArgumentOutOfRangeException(nameof(start));
        }

        if (_pointer != IntPtr.Zero)
        {
            return new ReadOnlySpan<T>((T*)(void*)_pointer + start, Length - start);
        }

        return new ReadOnlySpan<T>(_array, _start + start, Length - start);
    }

    /// <summary>
    ///     지정된 오프셋에서 시작하여 지정된 길이를 가지는 이 ReadOnlySpan의 새 슬라이스를 형성합니다.
    /// </summary>
    public unsafe ReadOnlySpan<T> Slice(int start, int length)
    {
        if ((uint)start > (uint)Length || (uint)length > (uint)(Length - start))
        {
            throw new ArgumentOutOfRangeException();
        }

        if (_pointer != IntPtr.Zero)
        {
            return new ReadOnlySpan<T>((T*)(void*)_pointer + start, length);
        }

        return new ReadOnlySpan<T>(_array, _start + start, length);
    }

    /// <summary>
    ///     ReadOnlySpan의 문자열 표현을 반환합니다.
    ///     char 형식의 경우 실제 문자열을 반환하고, 그 외의 경우 형식 정보를 반환합니다.
    /// </summary>
    public unsafe override string ToString()
    {
        if (typeof(T) == typeof(char))
        {
            if (Length == 0)
            {
                return string.Empty;
            }

            if (_pointer != IntPtr.Zero)
            {
                return new string((char*)(void*)_pointer, 0, Length);
            }

            return new string((char[])(object)_array, _start, Length);
        }

        return string.Format("System.ReadOnlySpan<{0}>[{1}]", typeof(T).Name, Length);
    }

    /// <summary>
    ///     이 ReadOnlySpan의 내용을 새 배열에 복사합니다.
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
            result[i] = this[i];
        }

        return result;
    }

    /// <summary>
    ///     현재 ReadOnlySpan에 대한 열거자를 반환합니다.
    /// </summary>
    public Enumerator GetEnumerator()
    {
        return new Enumerator(this);
    }

    /// <summary>
    ///     고정 문에서 사용하기 위해 ReadOnlySpan의 첫 번째 요소에 대한 참조를 반환합니다.
    /// </summary>
    /// <returns>첫 번째 요소에 대한 참조, 또는 ReadOnlySpan이 비어 있으면 null 참조입니다.</returns>
    public unsafe ref readonly T GetPinnableReference()
    {
        if (Length == 0)
        {
            return ref Runtime.CompilerServices.Unsafe.NullRef<T>();
        }

        if (_pointer != IntPtr.Zero)
        {
            return ref ((T*)(void*)_pointer)[0];
        }

        return ref _array[_start];
    }

    /// <summary>
    ///     ReadOnlySpan에 대한 열거자입니다.
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
    ///     배열을 ReadOnlySpan으로 암시적 변환합니다.
    /// </summary>
    public static implicit operator ReadOnlySpan<T>(T[] array)
    {
        return new ReadOnlySpan<T>(array);
    }

    /// <summary>
    ///     빈 ReadOnlySpan을 반환합니다.
    /// </summary>
    public static ReadOnlySpan<T> Empty => default;
}

#endif
