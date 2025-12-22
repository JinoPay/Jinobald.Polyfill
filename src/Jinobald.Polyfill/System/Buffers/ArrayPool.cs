#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46

using System.Collections.Generic;

namespace System.Buffers;

/// <summary>
/// T[] 인스턴스의 리소스 풀을 제공합니다.
/// </summary>
/// <typeparam name="T">배열의 요소 형식입니다.</typeparam>
//Link: https://learn.microsoft.com/en-us/dotnet/api/system.buffers.arraypool-1
public abstract class ArrayPool<T>
{
    private static ArrayPool<T>? _shared;

    /// <summary>
    /// 공유 ArrayPool 인스턴스를 가져옵니다.
    /// </summary>
    public static ArrayPool<T> Shared => _shared ??= new DefaultArrayPool<T>();

    /// <summary>
    /// 최소 지정된 길이의 배열을 검색합니다.
    /// </summary>
    /// <param name="minimumLength">필요한 배열의 최소 길이입니다.</param>
    /// <returns>최소 minimumLength 길이의 배열입니다.</returns>
    public abstract T[] Rent(int minimumLength);

    /// <summary>
    /// 이전에 Rent 메서드를 사용하여 가져온 배열을 풀에 반환합니다.
    /// </summary>
    /// <param name="array">풀에 반환할 배열입니다.</param>
    /// <param name="clearArray">반환하기 전에 배열의 내용을 지울지 여부입니다.</param>
    public abstract void Return(T[] array, bool clearArray = false);

    /// <summary>
    /// 구성 가능한 ArrayPool 인스턴스를 만듭니다.
    /// </summary>
    public static ArrayPool<T> Create(int maxArrayLength = 1024 * 1024, int maxArraysPerBucket = 50)
    {
        return new DefaultArrayPool<T>(maxArrayLength, maxArraysPerBucket);
    }
}

/// <summary>
/// ArrayPool의 기본 구현입니다.
/// </summary>
internal sealed class DefaultArrayPool<T> : ArrayPool<T>
{
    private readonly int _maxArrayLength;
    private readonly int _maxPoolSize;
    private readonly Stack<T[]> _pool;
    private readonly object _lock = new object();

    public DefaultArrayPool() : this(1024 * 1024, 50)
    {
    }

    public DefaultArrayPool(int maxArrayLength, int maxPoolSize)
    {
        _maxArrayLength = maxArrayLength;
        _maxPoolSize = maxPoolSize;
        _pool = new Stack<T[]>();
    }

    public override T[] Rent(int minimumLength)
    {
        if (minimumLength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumLength));
        }

        if (minimumLength == 0)
        {
            return Array.Empty<T>();
        }

        // 간단한 구현: 풀에서 적절한 크기의 배열 찾기
        lock (_lock)
        {
            if (_pool.Count > 0)
            {
                var array = _pool.Pop();
                if (array.Length >= minimumLength)
                {
                    return array;
                }
                // 크기가 맞지 않으면 다시 넣고 새로 생성
                _pool.Push(array);
            }
        }

        // 2의 제곱수로 반올림
        int size = 16;
        while (size < minimumLength && size < _maxArrayLength)
        {
            size *= 2;
        }

        return new T[Math.Max(size, minimumLength)];
    }

    public override void Return(T[] array, bool clearArray = false)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (array.Length == 0 || array.Length > _maxArrayLength)
        {
            return;
        }

        if (clearArray)
        {
            Array.Clear(array, 0, array.Length);
        }

        lock (_lock)
        {
            if (_pool.Count < _maxPoolSize)
            {
                _pool.Push(array);
            }
        }
    }
}

#endif
