#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// 컬렉션의 시작 또는 끝에서 인덱싱하는 데 사용할 수 있는 형식을 나타냅니다.
    /// </summary>
    /// <remarks>
    /// Index는 C# 컴파일러가 새로운 인덱싱 구문(^ 연산자)을 지원하는 데 사용됩니다.
    /// 이것은 .NET Framework 3.5 이상을 위한 폴리필 구현입니다.
    /// </remarks>
    public readonly struct Index : IEquatable<Index>
    {
        private readonly int _value;

        /// <summary>
        /// 지정된 값으로 새 Index를 초기화하고 시작 또는 끝에서부터인지를 나타냅니다.
        /// </summary>
        /// <param name="value">인덱스 값입니다. 0보다 크거나 같아야 합니다.</param>
        /// <param name="fromEnd">인덱스가 시작(false)에서인지 끝(true)에서인지를 나타냅니다.</param>
        /// <exception cref="ArgumentOutOfRangeException">value가 음수인 경우</exception>
        [MethodImpl((MethodImplOptions)256)]
        public Index(int value, bool fromEnd = false)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "인덱스는 음수가 아니어야 합니다.");
            }

            if (fromEnd)
            {
                _value = ~value;
            }
            else
            {
                _value = value;
            }
        }

        // 원시 값에서 Index를 만들기 위한 전용 생성자 (내부적으로 사용)
        private Index(int value)
        {
            _value = value;
        }

        /// <summary>
        /// 지정된 위치의 시작부터 Index를 만듭니다.
        /// </summary>
        /// <param name="value">시작부터의 인덱스 값입니다.</param>
        /// <returns>시작부터의 Index입니다.</returns>
        /// <exception cref="ArgumentOutOfRangeException">value가 음수인 경우</exception>
        [MethodImpl((MethodImplOptions)256)]
        public static Index FromStart(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "인덱스는 음수가 아니어야 합니다.");
            }

            return new Index(value);
        }

        /// <summary>
        /// 지정된 위치의 끝부터 Index를 만듭니다.
        /// </summary>
        /// <param name="value">끝부터의 인덱스 값입니다.</param>
        /// <returns>끝부터의 Index입니다.</returns>
        /// <exception cref="ArgumentOutOfRangeException">value가 음수인 경우</exception>
        [MethodImpl((MethodImplOptions)256)]
        public static Index FromEnd(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "인덱스는 음수가 아니어야 합니다.");
            }

            return new Index(~value);
        }

        /// <summary>
        /// 인덱스 값을 가져옵니다.
        /// </summary>
        public int Value
        {
            [MethodImpl((MethodImplOptions)256)]
            get
            {
                if (_value < 0)
                {
                    return ~_value;
                }
                else
                {
                    return _value;
                }
            }
        }

        /// <summary>
        /// 인덱스가 끝에서부터인지 여부를 나타내는 값을 가져옵니다.
        /// </summary>
        public bool IsFromEnd
        {
            [MethodImpl((MethodImplOptions)256)] get => _value < 0;
        }

        /// <summary>
        /// 지정된 컬렉션 길이에서 시작 위치로부터의 오프셋을 계산합니다.
        /// </summary>
        /// <param name="length">컬렉션의 길이입니다.</param>
        /// <returns>시작 위치로부터의 오프셋입니다.</returns>
        [MethodImpl((MethodImplOptions)256)]
        public int GetOffset(int length)
        {
            // _value가 음수이면 끝에서부터의 인덱스 (~value로 저장됨)
            // ~value + length + 1 = -(value+1) + length + 1 = length - value
            // 이는 .NET의 공식 구현과 동일: IsFromEnd ? length - Value : Value
            int offset = _value;
            if (IsFromEnd)
            {
                offset += length + 1;
            }

            return offset;
        }

        /// <summary>
        /// 현재 Index가 다른 Index와 같은지 여부를 나타냅니다.
        /// </summary>
        /// <param name="other">비교할 Index입니다.</param>
        /// <returns>같으면 true, 그렇지 않으면 false입니다.</returns>
        public bool Equals(Index other)
        {
            return _value == other._value;
        }

        /// <summary>
        /// 현재 Index가 지정된 개체와 같은지 여부를 나타냅니다.
        /// </summary>
        /// <param name="obj">비교할 개체입니다.</param>
        /// <returns>같으면 true, 그렇지 않으면 false입니다.</returns>
        public override bool Equals(object? obj)
        {
            return obj is Index other && _value == other._value;
        }

        /// <summary>
        /// 이 Index의 해시 코드를 반환합니다.
        /// </summary>
        /// <returns>해시 코드입니다.</returns>
        public override int GetHashCode()
        {
            return _value;
        }

        /// <summary>
        /// 정수를 시작부터의 Index로 변환합니다.
        /// </summary>
        /// <param name="value">변환할 정수 값입니다.</param>
        public static implicit operator Index(int value)
        {
            return FromStart(value);
        }

        /// <summary>
        /// 이 Index의 문자열 표현을 반환합니다.
        /// </summary>
        /// <returns>끝에서부터인 경우 "^값" 형식, 그렇지 않으면 "값" 형식입니다.</returns>
        public override string ToString()
        {
            if (IsFromEnd)
            {
                return "^" + Value.ToString();
            }
            else
            {
                return Value.ToString();
            }
        }

        /// <summary>
        /// 두 Index 값이 같은지 확인합니다.
        /// </summary>
        /// <param name="left">첫 번째 Index입니다.</param>
        /// <param name="right">두 번째 Index입니다.</param>
        /// <returns>같으면 true, 그렇지 않으면 false입니다.</returns>
        public static bool operator ==(Index left, Index right)
        {
            return left._value == right._value;
        }

        /// <summary>
        /// 두 Index 값이 다른지 확인합니다.
        /// </summary>
        /// <param name="left">첫 번째 Index입니다.</param>
        /// <param name="right">두 번째 Index입니다.</param>
        /// <returns>다르면 true, 그렇지 않으면 false입니다.</returns>
        public static bool operator !=(Index left, Index right)
        {
            return left._value != right._value;
        }

        /// <summary>
        /// 첫 번째 요소를 가리키는 Index를 가져옵니다.
        /// </summary>
        public static Index Start => new(0);

        /// <summary>
        /// 마지막 요소 다음을 가리키는 Index를 가져옵니다.
        /// </summary>
        public static Index End => new(~0);
    }
}

#endif
