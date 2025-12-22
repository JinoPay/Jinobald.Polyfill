#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    ///     시작 및 끝 인덱스를 갖는 범위를 나타냅니다.
    /// </summary>
    /// <remarks>
    ///     Range는 C# 컴파일러가 범위 구문(.. 연산자)을 지원하는 데 사용됩니다.
    ///     이것은 .NET Framework 3.5+ 용 폴리필 구현입니다.
    /// </remarks>
    public readonly struct Range : IEquatable<Range>
    {
        /// <summary>
        ///     범위의 시작 인덱스를 가져옵니다.
        /// </summary>
        public Index Start { get; }

        /// <summary>
        ///     범위의 끝 인덱스를 가져옵니다.
        /// </summary>
        public Index End { get; }

        /// <summary>
        ///     지정된 시작 및 끝 인덱스로 새 Range를 초기화합니다.
        /// </summary>
        /// <param name="start">범위의 시작 인덱스입니다.</param>
        /// <param name="end">범위의 끝 인덱스입니다.</param>
        [MethodImpl((MethodImplOptions)256)]
        public Range(Index start, Index end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        ///     지정된 시작 인덱스부터 끝까지의 Range를 만듭니다.
        /// </summary>
        /// <param name="start">시작 인덱스입니다.</param>
        [MethodImpl((MethodImplOptions)256)]
        public static Range StartAt(Index start)
        {
            return new Range(start, Index.End);
        }

        /// <summary>
        ///     처음부터 지정된 끝 인덱스까지의 Range를 만듭니다.
        /// </summary>
        /// <param name="end">끝 인덱스입니다.</param>
        [MethodImpl((MethodImplOptions)256)]
        public static Range EndAt(Index end)
        {
            return new Range(Index.Start, end);
        }

        /// <summary>
        ///     모든 요소를 포함하는 Range를 만듭니다.
        /// </summary>
        [MethodImpl((MethodImplOptions)256)]
        public static Range All()
        {
            return new Range(Index.Start, Index.End);
        }

        /// <summary>
        ///     컬렉션 길이가 주어지면 범위의 시작 오프셋과 길이를 계산합니다.
        /// </summary>
        /// <param name="length">컬렉션의 길이입니다.</param>
        /// <returns>오프셋과 길이를 포함하는 튜플입니다.</returns>
        [MethodImpl((MethodImplOptions)256)]
        public (int Offset, int Length) GetOffsetAndLength(int length)
        {
            int startIndex;
            if (Start.IsFromEnd)
            {
                startIndex = length - Start.Value;
            }
            else
            {
                startIndex = Start.Value;
            }

            int endIndex;
            if (End.IsFromEnd)
            {
                endIndex = length - End.Value;
            }
            else
            {
                endIndex = End.Value;
            }

            if ((uint)endIndex > (uint)length || (uint)startIndex > (uint)endIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            return (startIndex, endIndex - startIndex);
        }

        /// <summary>
        ///     현재 Range가 다른 Range와 같은지 여부를 나타냅니다.
        /// </summary>
        public bool Equals(Range other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        /// <summary>
        ///     현재 Range가 지정된 개체와 같은지 여부를 나타냅니다.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Range other && Start.Equals(other.Start) && End.Equals(other.End);
        }

        /// <summary>
        ///     이 Range의 해시 코드를 반환합니다.
        /// </summary>
        public override int GetHashCode()
        {
#if NET35
            // .NET 3.5의 경우 단순 해시 조합 사용
            int hash = 17;
            hash = (hash * 31) + Start.GetHashCode();
            hash = (hash * 31) + End.GetHashCode();
            return hash;
#else
            return Start.GetHashCode() * 31 + End.GetHashCode();
#endif
        }

        /// <summary>
        ///     이 Range의 문자열 표현을 반환합니다.
        /// </summary>
        public override string ToString()
        {
            return Start.ToString() + ".." + End.ToString();
        }

        /// <summary>
        ///     두 Range 값이 같은지 여부를 확인합니다.
        /// </summary>
        public static bool operator ==(Range left, Range right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     두 Range 값이 다른지 여부를 확인합니다.
        /// </summary>
        public static bool operator !=(Range left, Range right)
        {
            return !left.Equals(right);
        }
    }
}

#endif
