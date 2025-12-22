#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    ///     Represents a range with start and end indexes.
    /// </summary>
    /// <remarks>
    ///     Range is used by the C# compiler to support the range syntax (.. operator).
    ///     This is a polyfill implementation for .NET Framework 3.5+.
    /// </remarks>
    public readonly struct Range : IEquatable<Range>
    {
        /// <summary>
        ///     Gets the start index of the range.
        /// </summary>
        public Index Start { get; }

        /// <summary>
        ///     Gets the end index of the range.
        /// </summary>
        public Index End { get; }

        /// <summary>
        ///     Initializes a new Range with the specified start and end indexes.
        /// </summary>
        /// <param name="start">The start index of the range.</param>
        /// <param name="end">The end index of the range.</param>
        [MethodImpl((MethodImplOptions)256)]
        public Range(Index start, Index end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        ///     Creates a Range starting at the specified start index to the end.
        /// </summary>
        /// <param name="start">The start index.</param>
        [MethodImpl((MethodImplOptions)256)]
        public static Range StartAt(Index start)
        {
            return new Range(start, Index.End);
        }

        /// <summary>
        ///     Creates a Range from the beginning to the specified end index.
        /// </summary>
        /// <param name="end">The end index.</param>
        [MethodImpl((MethodImplOptions)256)]
        public static Range EndAt(Index end)
        {
            return new Range(Index.Start, end);
        }

        /// <summary>
        ///     Creates a Range that encompasses all elements.
        /// </summary>
        [MethodImpl((MethodImplOptions)256)]
        public static Range All()
        {
            return new Range(Index.Start, Index.End);
        }

        /// <summary>
        ///     Calculates the start offset and length of the range given a collection length.
        /// </summary>
        /// <param name="length">The length of the collection.</param>
        /// <returns>A tuple containing the offset and length.</returns>
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
        ///     Indicates whether the current Range is equal to another Range.
        /// </summary>
        public bool Equals(Range other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        /// <summary>
        ///     Indicates whether the current Range is equal to the specified object.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Range other && Start.Equals(other.Start) && End.Equals(other.End);
        }

        /// <summary>
        ///     Returns the hash code for this Range.
        /// </summary>
        public override int GetHashCode()
        {
#if NET35
            // For .NET 3.5, use simple hash combination
            int hash = 17;
            hash = (hash * 31) + Start.GetHashCode();
            hash = (hash * 31) + End.GetHashCode();
            return hash;
#else
            return Start.GetHashCode() * 31 + End.GetHashCode();
#endif
        }

        /// <summary>
        ///     Returns a string representation of this Range.
        /// </summary>
        public override string ToString()
        {
            return Start.ToString() + ".." + End.ToString();
        }

        /// <summary>
        ///     Determines whether two Range values are equal.
        /// </summary>
        public static bool operator ==(Range left, Range right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Determines whether two Range values are not equal.
        /// </summary>
        public static bool operator !=(Range left, Range right)
        {
            return !left.Equals(right);
        }
    }
}

#endif
