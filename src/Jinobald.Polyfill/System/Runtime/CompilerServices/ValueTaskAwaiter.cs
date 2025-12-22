#if NET35 || NET40 || NET45 || NET451 || NET452 || NET46
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices;

/// <summary>
///     ValueTask를 await하기 위한 awaiter를 제공합니다.
/// </summary>
public readonly struct ValueTaskAwaiter : ICriticalNotifyCompletion
{
    private readonly ValueTask _valueTask;

    /// <summary>
    ///     ValueTaskAwaiter의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="valueTask">await할 ValueTask입니다.</param>
    internal ValueTaskAwaiter(ValueTask valueTask)
    {
        _valueTask = valueTask;
    }

    /// <summary>
    ///     ValueTask가 완료되었는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsCompleted => _valueTask.IsCompleted;

    /// <summary>
    ///     ValueTask의 결과를 가져옵니다.
    /// </summary>
    public void GetResult()
    {
        _valueTask.AsTask().Wait();
    }

    /// <summary>
    ///     이 ValueTask가 완료될 때 실행할 연속 작업을 예약합니다.
    /// </summary>
    /// <param name="continuation">완료 시 실행할 동작입니다.</param>
    public void OnCompleted(Action continuation)
    {
        _valueTask.AsTask().ContinueWith(_ => continuation());
    }

    /// <summary>
    ///     이 ValueTask가 완료될 때 실행할 연속 작업을 예약합니다.
    /// </summary>
    /// <param name="continuation">완료 시 실행할 동작입니다.</param>
    public void UnsafeOnCompleted(Action continuation)
    {
        OnCompleted(continuation);
    }
}

/// <summary>
///     ValueTask&lt;TResult&gt;를 await하기 위한 awaiter를 제공합니다.
/// </summary>
/// <typeparam name="TResult">결과 형식입니다.</typeparam>
public readonly struct ValueTaskAwaiter<TResult> : ICriticalNotifyCompletion
{
    private readonly ValueTask<TResult> _valueTask;

    /// <summary>
    ///     ValueTaskAwaiter&lt;TResult&gt;의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="valueTask">await할 ValueTask입니다.</param>
    internal ValueTaskAwaiter(ValueTask<TResult> valueTask)
    {
        _valueTask = valueTask;
    }

    /// <summary>
    ///     ValueTask가 완료되었는지 여부를 나타내는 값을 가져옵니다.
    /// </summary>
    public bool IsCompleted => _valueTask.IsCompleted;

    /// <summary>
    ///     ValueTask의 결과를 가져옵니다.
    /// </summary>
    /// <returns>ValueTask의 결과입니다.</returns>
    public TResult GetResult()
    {
        return _valueTask.Result;
    }

    /// <summary>
    ///     이 ValueTask가 완료될 때 실행할 연속 작업을 예약합니다.
    /// </summary>
    /// <param name="continuation">완료 시 실행할 동작입니다.</param>
    public void OnCompleted(Action continuation)
    {
        _valueTask.AsTask().ContinueWith(_ => continuation());
    }

    /// <summary>
    ///     이 ValueTask가 완료될 때 실행할 연속 작업을 예약합니다.
    /// </summary>
    /// <param name="continuation">완료 시 실행할 동작입니다.</param>
    public void UnsafeOnCompleted(Action continuation)
    {
        OnCompleted(continuation);
    }
}

/// <summary>
///     ConfigureAwait를 통해 구성된 ValueTask를 await하기 위한 개체입니다.
/// </summary>
public readonly struct ConfiguredValueTaskAwaitable
{
    private readonly ValueTask _valueTask;
    private readonly bool _continueOnCapturedContext;

    /// <summary>
    ///     ConfiguredValueTaskAwaitable의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="valueTask">await할 ValueTask입니다.</param>
    /// <param name="continueOnCapturedContext">캡처된 컨텍스트에서 연속을 실행할지 여부입니다.</param>
    internal ConfiguredValueTaskAwaitable(ValueTask valueTask, bool continueOnCapturedContext)
    {
        _valueTask = valueTask;
        _continueOnCapturedContext = continueOnCapturedContext;
    }

    /// <summary>
    ///     이 개체의 awaiter를 가져옵니다.
    /// </summary>
    /// <returns>awaiter 인스턴스입니다.</returns>
    public ConfiguredValueTaskAwaiter GetAwaiter()
    {
        return new ConfiguredValueTaskAwaiter(_valueTask, _continueOnCapturedContext);
    }

    /// <summary>
    ///     ConfiguredValueTaskAwaitable을 위한 awaiter입니다.
    /// </summary>
    public readonly struct ConfiguredValueTaskAwaiter : ICriticalNotifyCompletion
    {
        private readonly ValueTask _valueTask;
        private readonly bool _continueOnCapturedContext;

        /// <summary>
        ///     ConfiguredValueTaskAwaiter의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="valueTask">await할 ValueTask입니다.</param>
        /// <param name="continueOnCapturedContext">캡처된 컨텍스트에서 연속을 실행할지 여부입니다.</param>
        internal ConfiguredValueTaskAwaiter(ValueTask valueTask, bool continueOnCapturedContext)
        {
            _valueTask = valueTask;
            _continueOnCapturedContext = continueOnCapturedContext;
        }

        /// <summary>
        ///     ValueTask가 완료되었는지 여부를 나타내는 값을 가져옵니다.
        /// </summary>
        public bool IsCompleted => _valueTask.IsCompleted;

        /// <summary>
        ///     ValueTask의 결과를 가져옵니다.
        /// </summary>
        public void GetResult()
        {
            _valueTask.AsTask().Wait();
        }

        /// <summary>
        ///     이 ValueTask가 완료될 때 실행할 연속 작업을 예약합니다.
        /// </summary>
        /// <param name="continuation">완료 시 실행할 동작입니다.</param>
        public void OnCompleted(Action continuation)
        {
            // NET35/NET40에서는 ConfigureAwait가 없으므로 직접 ContinueWith 사용
            _valueTask.AsTask().ContinueWith(_ => continuation());
        }

        /// <summary>
        ///     이 ValueTask가 완료될 때 실행할 연속 작업을 예약합니다.
        /// </summary>
        /// <param name="continuation">완료 시 실행할 동작입니다.</param>
        public void UnsafeOnCompleted(Action continuation)
        {
            OnCompleted(continuation);
        }
    }
}

/// <summary>
///     ConfigureAwait를 통해 구성된 ValueTask&lt;TResult&gt;를 await하기 위한 개체입니다.
/// </summary>
/// <typeparam name="TResult">결과 형식입니다.</typeparam>
public readonly struct ConfiguredValueTaskAwaitable<TResult>
{
    private readonly ValueTask<TResult> _valueTask;
    private readonly bool _continueOnCapturedContext;

    /// <summary>
    ///     ConfiguredValueTaskAwaitable&lt;TResult&gt;의 새 인스턴스를 초기화합니다.
    /// </summary>
    /// <param name="valueTask">await할 ValueTask입니다.</param>
    /// <param name="continueOnCapturedContext">캡처된 컨텍스트에서 연속을 실행할지 여부입니다.</param>
    internal ConfiguredValueTaskAwaitable(ValueTask<TResult> valueTask, bool continueOnCapturedContext)
    {
        _valueTask = valueTask;
        _continueOnCapturedContext = continueOnCapturedContext;
    }

    /// <summary>
    ///     이 개체의 awaiter를 가져옵니다.
    /// </summary>
    /// <returns>awaiter 인스턴스입니다.</returns>
    public ConfiguredValueTaskAwaiter GetAwaiter()
    {
        return new ConfiguredValueTaskAwaiter(_valueTask, _continueOnCapturedContext);
    }

    /// <summary>
    ///     ConfiguredValueTaskAwaitable&lt;TResult&gt;를 위한 awaiter입니다.
    /// </summary>
    public readonly struct ConfiguredValueTaskAwaiter : ICriticalNotifyCompletion
    {
        private readonly ValueTask<TResult> _valueTask;
        private readonly bool _continueOnCapturedContext;

        /// <summary>
        ///     ConfiguredValueTaskAwaiter의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="valueTask">await할 ValueTask입니다.</param>
        /// <param name="continueOnCapturedContext">캡처된 컨텍스트에서 연속을 실행할지 여부입니다.</param>
        internal ConfiguredValueTaskAwaiter(ValueTask<TResult> valueTask, bool continueOnCapturedContext)
        {
            _valueTask = valueTask;
            _continueOnCapturedContext = continueOnCapturedContext;
        }

        /// <summary>
        ///     ValueTask가 완료되었는지 여부를 나타내는 값을 가져옵니다.
        /// </summary>
        public bool IsCompleted => _valueTask.IsCompleted;

        /// <summary>
        ///     ValueTask의 결과를 가져옵니다.
        /// </summary>
        /// <returns>ValueTask의 결과입니다.</returns>
        public TResult GetResult()
        {
            return _valueTask.Result;
        }

        /// <summary>
        ///     이 ValueTask가 완료될 때 실행할 연속 작업을 예약합니다.
        /// </summary>
        /// <param name="continuation">완료 시 실행할 동작입니다.</param>
        public void OnCompleted(Action continuation)
        {
            // NET35/NET40에서는 ConfigureAwait가 없으므로 직접 ContinueWith 사용
            _valueTask.AsTask().ContinueWith(_ => continuation());
        }

        /// <summary>
        ///     이 ValueTask가 완료될 때 실행할 연속 작업을 예약합니다.
        /// </summary>
        /// <param name="continuation">완료 시 실행할 동작입니다.</param>
        public void UnsafeOnCompleted(Action continuation)
        {
            OnCompleted(continuation);
        }
    }
}
#endif
