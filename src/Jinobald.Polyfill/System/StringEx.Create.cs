#if NETFRAMEWORK

namespace System;

internal static partial class StringEx
{
    extension(string)
    {
        /// <summary>
        /// 지정된 길이로 새 문자열을 만들고 지정된 콜백을 사용하여 만든 후 초기화합니다.
        /// </summary>
        /// <typeparam name="TState">생성 작업에 전달할 상태의 형식입니다.</typeparam>
        /// <param name="length">만들 문자열의 길이입니다.</param>
        /// <param name="state">action에 전달할 요소입니다.</param>
        /// <param name="action">새로 만든 문자열의 내용을 채우는 콜백입니다.</param>
        /// <returns>초기화된 새 문자열입니다.</returns>
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.string.create
        public static string Create<TState>(int length, TState state, Buffers.SpanAction<char, TState> action)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (length == 0)
            {
                return string.Empty;
            }

#if AllowUnsafeBlocks
            // net462+: unsafe 포인터로 직접 문자열 수정
            var str = new string('\0', length);

            unsafe
            {
                fixed (char* strPtr = str)
                {
                    action(new Span<char>(strPtr, length), state);
                }
            }

            return str;
#else
            // net35~net461: ArrayPool 사용
            var pool = Buffers.ArrayPool<char>.Shared;
            var chars = pool.Rent(length);

            try
            {
                var span = chars.AsSpan(0, length);
                span.Clear();
                action(span, state);

                return new string(chars, 0, length);
            }
            finally
            {
                pool.Return(chars);
            }
#endif
        }
    }
}

#endif
