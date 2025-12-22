#if NET35
namespace System.Threading.Tasks;

/// <summary>
///     병렬 루프 및 영역에 대한 지원을 제공합니다.
/// </summary>
public static class Parallel
{
    #region For

    /// <summary>
    ///     반복이 병렬로 실행될 수 있는 for 루프를 실행합니다.
    /// </summary>
    /// <param name="fromInclusive">시작 인덱스(포함)</param>
    /// <param name="toExclusive">끝 인덱스(제외)</param>
    /// <param name="body">각 반복에서 호출되는 대리자</param>
    /// <returns>루프의 완료 상태에 대한 정보가 포함된 구조체</returns>
    public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int> body)
    {
        return For(fromInclusive, toExclusive, new ParallelOptions(), body);
    }

    /// <summary>
    ///     반복이 병렬로 실행될 수 있는 for 루프를 실행합니다.
    /// </summary>
    /// <param name="fromInclusive">시작 인덱스(포함)</param>
    /// <param name="toExclusive">끝 인덱스(제외)</param>
    /// <param name="parallelOptions">루프의 동작을 구성하는 개체</param>
    /// <param name="body">각 반복에서 호출되는 대리자</param>
    /// <returns>루프의 완료 상태에 대한 정보가 포함된 구조체</returns>
    public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions,
        Action<int> body)
    {
        if (parallelOptions == null)
        {
            throw new ArgumentNullException(nameof(parallelOptions));
        }

        if (body == null)
        {
            throw new ArgumentNullException(nameof(body));
        }

        return ForInternal(fromInclusive, toExclusive, parallelOptions, (i, state) => body(i));
    }

    /// <summary>
    ///     반복이 병렬로 실행될 수 있는 for 루프를 실행합니다. 루프 상태를 모니터링하고 조작할 수 있습니다.
    /// </summary>
    /// <param name="fromInclusive">시작 인덱스(포함)</param>
    /// <param name="toExclusive">끝 인덱스(제외)</param>
    /// <param name="body">각 반복에서 호출되는 대리자</param>
    /// <returns>루프의 완료 상태에 대한 정보가 포함된 구조체</returns>
    public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int, ParallelLoopState> body)
    {
        return For(fromInclusive, toExclusive, new ParallelOptions(), body);
    }

    /// <summary>
    ///     반복이 병렬로 실행될 수 있는 for 루프를 실행합니다. 루프 상태를 모니터링하고 조작할 수 있습니다.
    /// </summary>
    /// <param name="fromInclusive">시작 인덱스(포함)</param>
    /// <param name="toExclusive">끝 인덱스(제외)</param>
    /// <param name="parallelOptions">루프의 동작을 구성하는 개체</param>
    /// <param name="body">각 반복에서 호출되는 대리자</param>
    /// <returns>루프의 완료 상태에 대한 정보가 포함된 구조체</returns>
    public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions,
        Action<int, ParallelLoopState> body)
    {
        if (parallelOptions == null)
        {
            throw new ArgumentNullException(nameof(parallelOptions));
        }

        if (body == null)
        {
            throw new ArgumentNullException(nameof(body));
        }

        return ForInternal(fromInclusive, toExclusive, parallelOptions, body);
    }

    /// <summary>
    ///     64비트 인덱스를 사용하여 반복이 병렬로 실행될 수 있는 for 루프를 실행합니다.
    /// </summary>
    public static ParallelLoopResult For(long fromInclusive, long toExclusive, Action<long> body)
    {
        return For(fromInclusive, toExclusive, new ParallelOptions(), body);
    }

    /// <summary>
    ///     64비트 인덱스를 사용하여 반복이 병렬로 실행될 수 있는 for 루프를 실행합니다.
    /// </summary>
    public static ParallelLoopResult For(long fromInclusive, long toExclusive, ParallelOptions parallelOptions,
        Action<long> body)
    {
        if (parallelOptions == null)
        {
            throw new ArgumentNullException(nameof(parallelOptions));
        }

        if (body == null)
        {
            throw new ArgumentNullException(nameof(body));
        }

        return ForInternal64(fromInclusive, toExclusive, parallelOptions, (i, state) => body(i));
    }

    /// <summary>
    ///     64비트 인덱스를 사용하여 반복이 병렬로 실행될 수 있는 for 루프를 실행합니다. 루프 상태를 모니터링하고 조작할 수 있습니다.
    /// </summary>
    public static ParallelLoopResult For(long fromInclusive, long toExclusive, Action<long, ParallelLoopState> body)
    {
        return For(fromInclusive, toExclusive, new ParallelOptions(), body);
    }

    /// <summary>
    ///     64비트 인덱스를 사용하여 반복이 병렬로 실행될 수 있는 for 루프를 실행합니다. 루프 상태를 모니터링하고 조작할 수 있습니다.
    /// </summary>
    public static ParallelLoopResult For(long fromInclusive, long toExclusive, ParallelOptions parallelOptions,
        Action<long, ParallelLoopState> body)
    {
        if (parallelOptions == null)
        {
            throw new ArgumentNullException(nameof(parallelOptions));
        }

        if (body == null)
        {
            throw new ArgumentNullException(nameof(body));
        }

        return ForInternal64(fromInclusive, toExclusive, parallelOptions, body);
    }

    private static ParallelLoopResult ForInternal(int fromInclusive, int toExclusive, ParallelOptions parallelOptions,
        Action<int, ParallelLoopState> body)
    {
        if (fromInclusive >= toExclusive)
        {
            return new ParallelLoopResult(true, null);
        }

        parallelOptions.CancellationToken.ThrowIfCancellationRequested();

        var state = new ParallelLoopState();
        var exceptions = new List<Exception>();
        object exceptionsLock = new();
        int currentIndex = fromInclusive - 1;
        int degreeOfParallelism = parallelOptions.EffectiveMaxDegreeOfParallelism;
        int completedCount = 0;
        var completionEvent = new ManualResetEvent(false);

        for (int workerIndex = 0; workerIndex < degreeOfParallelism; workerIndex++)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    while (true)
                    {
                        if (state.ShouldExitCurrentIteration ||
                            parallelOptions.CancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        int index = Interlocked.Increment(ref currentIndex);
                        if (index >= toExclusive)
                        {
                            break;
                        }

                        // Break 상태 확인: LowestBreakIteration보다 큰 인덱스는 건너뜀
                        long? lowestBreak = state.LowestBreakIteration;
                        if (lowestBreak.HasValue && index > lowestBreak.Value)
                        {
                            break;
                        }

                        try
                        {
                            body(index, state);
                        }
                        catch (Exception ex)
                        {
                            lock (exceptionsLock)
                            {
                                exceptions.Add(ex);
                            }

                            state.SetExceptionThrown();
                        }

                        if (state.IsBreakRequested)
                        {
                            state.SetLowestBreakIteration(index);
                        }
                    }
                }
                finally
                {
                    if (Interlocked.Increment(ref completedCount) == degreeOfParallelism)
                    {
                        completionEvent.Set();
                    }
                }
            });
        }

        completionEvent.WaitOne();
        completionEvent.Close();

        if (parallelOptions.CancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("작업이 취소되었습니다.");
        }

        if (exceptions.Count > 0)
        {
            throw new AggregateException(exceptions);
        }

        bool isCompleted = !state.IsBreakRequested && !state.IsStopRequested;
        return new ParallelLoopResult(isCompleted, state.LowestBreakIteration);
    }

    private static ParallelLoopResult ForInternal64(long fromInclusive, long toExclusive,
        ParallelOptions parallelOptions, Action<long, ParallelLoopState> body)
    {
        if (fromInclusive >= toExclusive)
        {
            return new ParallelLoopResult(true, null);
        }

        parallelOptions.CancellationToken.ThrowIfCancellationRequested();

        var state = new ParallelLoopState();
        var exceptions = new List<Exception>();
        object exceptionsLock = new();
        long currentIndex = fromInclusive - 1;
        int degreeOfParallelism = parallelOptions.EffectiveMaxDegreeOfParallelism;
        int completedCount = 0;
        var completionEvent = new ManualResetEvent(false);

        for (int workerIndex = 0; workerIndex < degreeOfParallelism; workerIndex++)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    while (true)
                    {
                        if (state.ShouldExitCurrentIteration ||
                            parallelOptions.CancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        long index = Interlocked.Increment(ref currentIndex);
                        if (index >= toExclusive)
                        {
                            break;
                        }

                        long? lowestBreak = state.LowestBreakIteration;
                        if (lowestBreak.HasValue && index > lowestBreak.Value)
                        {
                            break;
                        }

                        try
                        {
                            body(index, state);
                        }
                        catch (Exception ex)
                        {
                            lock (exceptionsLock)
                            {
                                exceptions.Add(ex);
                            }

                            state.SetExceptionThrown();
                        }

                        if (state.IsBreakRequested)
                        {
                            state.SetLowestBreakIteration(index);
                        }
                    }
                }
                finally
                {
                    if (Interlocked.Increment(ref completedCount) == degreeOfParallelism)
                    {
                        completionEvent.Set();
                    }
                }
            });
        }

        completionEvent.WaitOne();
        completionEvent.Close();

        if (parallelOptions.CancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("작업이 취소되었습니다.");
        }

        if (exceptions.Count > 0)
        {
            throw new AggregateException(exceptions);
        }

        bool isCompleted = !state.IsBreakRequested && !state.IsStopRequested;
        return new ParallelLoopResult(isCompleted, state.LowestBreakIteration);
    }

    #endregion

    #region ForEach

    /// <summary>
    ///     IEnumerable에서 foreach 작업을 실행합니다. 반복이 병렬로 실행될 수 있습니다.
    /// </summary>
    /// <typeparam name="TSource">소스의 데이터 형식</typeparam>
    /// <param name="source">열거 가능한 데이터 소스</param>
    /// <param name="body">각 반복에서 호출되는 대리자</param>
    /// <returns>루프의 완료 상태에 대한 정보가 포함된 구조체</returns>
    public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> body)
    {
        return ForEach(source, new ParallelOptions(), body);
    }

    /// <summary>
    ///     IEnumerable에서 foreach 작업을 실행합니다. 반복이 병렬로 실행될 수 있습니다.
    /// </summary>
    /// <typeparam name="TSource">소스의 데이터 형식</typeparam>
    /// <param name="source">열거 가능한 데이터 소스</param>
    /// <param name="parallelOptions">루프의 동작을 구성하는 개체</param>
    /// <param name="body">각 반복에서 호출되는 대리자</param>
    /// <returns>루프의 완료 상태에 대한 정보가 포함된 구조체</returns>
    public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions,
        Action<TSource> body)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (parallelOptions == null)
        {
            throw new ArgumentNullException(nameof(parallelOptions));
        }

        if (body == null)
        {
            throw new ArgumentNullException(nameof(body));
        }

        return ForEachInternal(source, parallelOptions, (item, state) => body(item));
    }

    /// <summary>
    ///     IEnumerable에서 foreach 작업을 실행합니다. 반복이 병렬로 실행될 수 있고 루프 상태를 모니터링하고 조작할 수 있습니다.
    /// </summary>
    /// <typeparam name="TSource">소스의 데이터 형식</typeparam>
    /// <param name="source">열거 가능한 데이터 소스</param>
    /// <param name="body">각 반복에서 호출되는 대리자</param>
    /// <returns>루프의 완료 상태에 대한 정보가 포함된 구조체</returns>
    public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source,
        Action<TSource, ParallelLoopState> body)
    {
        return ForEach(source, new ParallelOptions(), body);
    }

    /// <summary>
    ///     IEnumerable에서 foreach 작업을 실행합니다. 반복이 병렬로 실행될 수 있고 루프 상태를 모니터링하고 조작할 수 있습니다.
    /// </summary>
    /// <typeparam name="TSource">소스의 데이터 형식</typeparam>
    /// <param name="source">열거 가능한 데이터 소스</param>
    /// <param name="parallelOptions">루프의 동작을 구성하는 개체</param>
    /// <param name="body">각 반복에서 호출되는 대리자</param>
    /// <returns>루프의 완료 상태에 대한 정보가 포함된 구조체</returns>
    public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions,
        Action<TSource, ParallelLoopState> body)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (parallelOptions == null)
        {
            throw new ArgumentNullException(nameof(parallelOptions));
        }

        if (body == null)
        {
            throw new ArgumentNullException(nameof(body));
        }

        return ForEachInternal(source, parallelOptions, body);
    }

    /// <summary>
    ///     IEnumerable에서 foreach 작업을 실행합니다. 반복이 병렬로 실행될 수 있고 인덱스와 루프 상태를 사용할 수 있습니다.
    /// </summary>
    /// <typeparam name="TSource">소스의 데이터 형식</typeparam>
    /// <param name="source">열거 가능한 데이터 소스</param>
    /// <param name="body">각 반복에서 호출되는 대리자</param>
    /// <returns>루프의 완료 상태에 대한 정보가 포함된 구조체</returns>
    public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source,
        Action<TSource, ParallelLoopState, long> body)
    {
        return ForEach(source, new ParallelOptions(), body);
    }

    /// <summary>
    ///     IEnumerable에서 foreach 작업을 실행합니다. 반복이 병렬로 실행될 수 있고 인덱스와 루프 상태를 사용할 수 있습니다.
    /// </summary>
    /// <typeparam name="TSource">소스의 데이터 형식</typeparam>
    /// <param name="source">열거 가능한 데이터 소스</param>
    /// <param name="parallelOptions">루프의 동작을 구성하는 개체</param>
    /// <param name="body">각 반복에서 호출되는 대리자</param>
    /// <returns>루프의 완료 상태에 대한 정보가 포함된 구조체</returns>
    public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions,
        Action<TSource, ParallelLoopState, long> body)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (parallelOptions == null)
        {
            throw new ArgumentNullException(nameof(parallelOptions));
        }

        if (body == null)
        {
            throw new ArgumentNullException(nameof(body));
        }

        return ForEachWithIndexInternal(source, parallelOptions, body);
    }

    private static ParallelLoopResult ForEachInternal<TSource>(IEnumerable<TSource> source,
        ParallelOptions parallelOptions, Action<TSource, ParallelLoopState> body)
    {
        parallelOptions.CancellationToken.ThrowIfCancellationRequested();

        var state = new ParallelLoopState();
        var exceptions = new List<Exception>();
        object exceptionsLock = new();
        object enumeratorLock = new();
        int degreeOfParallelism = parallelOptions.EffectiveMaxDegreeOfParallelism;
        int completedCount = 0;
        var completionEvent = new ManualResetEvent(false);
        IEnumerator<TSource> enumerator = source.GetEnumerator();
        bool enumeratorComplete = false;

        for (int workerIndex = 0; workerIndex < degreeOfParallelism; workerIndex++)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    while (true)
                    {
                        if (state.ShouldExitCurrentIteration ||
                            parallelOptions.CancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        TSource item;
                        lock (enumeratorLock)
                        {
                            if (enumeratorComplete || !enumerator.MoveNext())
                            {
                                enumeratorComplete = true;
                                break;
                            }

                            item = enumerator.Current;
                        }

                        try
                        {
                            body(item, state);
                        }
                        catch (Exception ex)
                        {
                            lock (exceptionsLock)
                            {
                                exceptions.Add(ex);
                            }

                            state.SetExceptionThrown();
                        }
                    }
                }
                finally
                {
                    if (Interlocked.Increment(ref completedCount) == degreeOfParallelism)
                    {
                        completionEvent.Set();
                    }
                }
            });
        }

        completionEvent.WaitOne();
        completionEvent.Close();

        var disposable = enumerator as IDisposable;
        if (disposable != null)
        {
            disposable.Dispose();
        }

        if (parallelOptions.CancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("작업이 취소되었습니다.");
        }

        if (exceptions.Count > 0)
        {
            throw new AggregateException(exceptions);
        }

        bool isCompleted = !state.IsBreakRequested && !state.IsStopRequested;
        return new ParallelLoopResult(isCompleted, state.LowestBreakIteration);
    }

    private static ParallelLoopResult ForEachWithIndexInternal<TSource>(IEnumerable<TSource> source,
        ParallelOptions parallelOptions, Action<TSource, ParallelLoopState, long> body)
    {
        parallelOptions.CancellationToken.ThrowIfCancellationRequested();

        var state = new ParallelLoopState();
        var exceptions = new List<Exception>();
        object exceptionsLock = new();
        object enumeratorLock = new();
        int degreeOfParallelism = parallelOptions.EffectiveMaxDegreeOfParallelism;
        int completedCount = 0;
        var completionEvent = new ManualResetEvent(false);
        IEnumerator<TSource> enumerator = source.GetEnumerator();
        bool enumeratorComplete = false;
        long currentIndex = -1;

        for (int workerIndex = 0; workerIndex < degreeOfParallelism; workerIndex++)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    while (true)
                    {
                        if (state.ShouldExitCurrentIteration ||
                            parallelOptions.CancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        TSource item;
                        long index;
                        lock (enumeratorLock)
                        {
                            if (enumeratorComplete || !enumerator.MoveNext())
                            {
                                enumeratorComplete = true;
                                break;
                            }

                            item = enumerator.Current;
                            index = ++currentIndex;
                        }

                        long? lowestBreak = state.LowestBreakIteration;
                        if (lowestBreak.HasValue && index > lowestBreak.Value)
                        {
                            continue;
                        }

                        try
                        {
                            body(item, state, index);
                        }
                        catch (Exception ex)
                        {
                            lock (exceptionsLock)
                            {
                                exceptions.Add(ex);
                            }

                            state.SetExceptionThrown();
                        }

                        if (state.IsBreakRequested)
                        {
                            state.SetLowestBreakIteration(index);
                        }
                    }
                }
                finally
                {
                    if (Interlocked.Increment(ref completedCount) == degreeOfParallelism)
                    {
                        completionEvent.Set();
                    }
                }
            });
        }

        completionEvent.WaitOne();
        completionEvent.Close();

        var disposable = enumerator as IDisposable;
        if (disposable != null)
        {
            disposable.Dispose();
        }

        if (parallelOptions.CancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("작업이 취소되었습니다.");
        }

        if (exceptions.Count > 0)
        {
            throw new AggregateException(exceptions);
        }

        bool isCompleted = !state.IsBreakRequested && !state.IsStopRequested;
        return new ParallelLoopResult(isCompleted, state.LowestBreakIteration);
    }

    #endregion

    #region Invoke

    /// <summary>
    ///     제공된 각 작업을 실행하며, 병렬로 실행될 수 있습니다.
    /// </summary>
    /// <param name="actions">실행할 작업의 배열</param>
    /// <exception cref="ArgumentNullException">actions가 null인 경우</exception>
    /// <exception cref="ArgumentException">actions 배열에 null 요소가 포함된 경우</exception>
    /// <exception cref="AggregateException">작업 중 하나라도 예외를 throw한 경우</exception>
    public static void Invoke(params Action[] actions)
    {
        Invoke(new ParallelOptions(), actions);
    }

    /// <summary>
    ///     제공된 각 작업을 실행하며, 병렬로 실행될 수 있습니다.
    /// </summary>
    /// <param name="parallelOptions">루프의 동작을 구성하는 개체</param>
    /// <param name="actions">실행할 작업의 배열</param>
    /// <exception cref="ArgumentNullException">parallelOptions 또는 actions가 null인 경우</exception>
    /// <exception cref="ArgumentException">actions 배열에 null 요소가 포함된 경우</exception>
    /// <exception cref="AggregateException">작업 중 하나라도 예외를 throw한 경우</exception>
    public static void Invoke(ParallelOptions parallelOptions, params Action[] actions)
    {
        if (parallelOptions == null)
        {
            throw new ArgumentNullException(nameof(parallelOptions));
        }

        if (actions == null)
        {
            throw new ArgumentNullException(nameof(actions));
        }

        for (int i = 0; i < actions.Length; i++)
        {
            if (actions[i] == null)
            {
                throw new ArgumentException("Action 배열에 null 요소가 포함되어 있습니다.", nameof(actions));
            }
        }

        if (actions.Length == 0)
        {
            return;
        }

        parallelOptions.CancellationToken.ThrowIfCancellationRequested();

        if (actions.Length == 1)
        {
            actions[0]();
            return;
        }

        var exceptions = new List<Exception>();
        object exceptionsLock = new();
        int completedCount = 0;
        var completionEvent = new ManualResetEvent(false);
        int totalActions = actions.Length;

        for (int i = 0; i < actions.Length; i++)
        {
            Action action = actions[i];
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    if (!parallelOptions.CancellationToken.IsCancellationRequested)
                    {
                        action();
                    }
                }
                catch (Exception ex)
                {
                    lock (exceptionsLock)
                    {
                        exceptions.Add(ex);
                    }
                }
                finally
                {
                    if (Interlocked.Increment(ref completedCount) == totalActions)
                    {
                        completionEvent.Set();
                    }
                }
            });
        }

        completionEvent.WaitOne();
        completionEvent.Close();

        if (parallelOptions.CancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("작업이 취소되었습니다.");
        }

        if (exceptions.Count > 0)
        {
            throw new AggregateException(exceptions);
        }
    }

    #endregion
}

#endif
