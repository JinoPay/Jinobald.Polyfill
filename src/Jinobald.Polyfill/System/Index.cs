#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Represents a type that can be used to index a collection either from the start or the end.
    /// </summary>
    /// <remarks>
    /// Index is used by the C# compiler to support the new indexing syntax (^operator).
    /// This is a polyfill implementation for .NET Framework 3.5+.
    /// </remarks>
    public readonly struct Index : IEquatable<Index>
    {
        private readonly int _value;

        /// <summary>
        /// Initializes a new Index with a given value and indicates whether it's from the start or end.
        /// </summary>
        /// <param name="value">The index value. Must be greater than or equal to 0.</param>
        /// <param name="fromEnd">Indicates whether the index is from the start (false) or from the end (true).</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Index(int value, bool fromEnd = false)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Index must be non-negative.");
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

        // Private constructor for creating Index from raw value (used internally)
        private Index(int value)
        {
            _value = value;
        }

        /// <summary>
        /// Creates an Index from the start at the specified position.
        /// </summary>
        /// <param name="value">The index value from the start.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index FromStart(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Index must be non-negative.");
            }

            return new Index(value);
        }

        /// <summary>
        /// Creates an Index from the end at the specified position.
        /// </summary>
        /// <param name="value">The index value from the end.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index FromEnd(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Index must be non-negative.");
            }

            return new Index(~value);
        }

        /// <summary>
        /// Gets the index value.
        /// </summary>
        public int Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        /// Gets a value indicating whether the index is from the end.
        /// </summary>
        public bool IsFromEnd
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _value < 0; }
        }

        /// <summary>
        /// Calculates the offset from the start given a collection length.
        /// </summary>
        /// <param name="length">The length of the collection.</param>
        /// <returns>The offset from the start.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetOffset(int length)
        {
            int offset = _value;
            if (IsFromEnd)
            {
                // Offset is negative, so add to length
                offset += length + 1;
            }

            return offset;
        }

        /// <summary>
        /// Indicates whether the current Index is equal to another Index.
        /// </summary>
        public bool Equals(Index other)
        {
            return _value == other._value;
        }

        /// <summary>
        /// Indicates whether the current Index is equal to the specified object.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Index other && _value == other._value;
        }

        /// <summary>
        /// Returns the hash code for this Index.
        /// </summary>
        public override int GetHashCode()
        {
            return _value;
        }

        /// <summary>
        /// Converts an integer to an Index from the start.
        /// </summary>
        public static implicit operator Index(int value)
        {
            return FromStart(value);
        }

        /// <summary>
        /// Returns a string representation of this Index.
        /// </summary>
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
        /// Determines whether two Index values are equal.
        /// </summary>
        public static bool operator ==(Index left, Index right)
        {
            return left._value == right._value;
        }

        /// <summary>
        /// Determines whether two Index values are not equal.
        /// </summary>
        public static bool operator !=(Index left, Index right)
        {
            return left._value != right._value;
        }

        /// <summary>
        /// Gets an Index that points to the first element.
        /// </summary>
        public static Index Start => new Index(0);

        /// <summary>
        /// Gets an Index that points to one past the last element.
        /// </summary>
        public static Index End => new Index(~0);
    }
}

#endif
