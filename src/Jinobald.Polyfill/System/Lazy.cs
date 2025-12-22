#if NET35
namespace System
{
    /// <summary>
    /// 지연 초기화를 지원합니다.
    /// </summary>
    /// <typeparam name="T">지연 초기화되는 개체의 형식입니다.</typeparam>
    public class Lazy<T>
    {
        private readonly Func<T>? _valueFactory;
        private readonly object _lockObject = new();
        private bool _isValueCreated;
        private T _value = default!;

        /// <summary>
        /// Lazy 클래스의 새 인스턴스를 초기화합니다.
        /// 지연 초기화가 발생하면 대상 형식의 기본 생성자가 사용됩니다.
        /// </summary>
        public Lazy()
        {
            _valueFactory = () => Activator.CreateInstance<T>();
        }

        /// <summary>
        /// 지정된 값 팩터리를 사용하여 Lazy 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="valueFactory">지연 초기화된 값이 필요할 때 호출되는 대리자입니다.</param>
        /// <exception cref="ArgumentNullException">valueFactory가 null인 경우</exception>
        public Lazy(Func<T> valueFactory)
        {
            if (valueFactory == null)
            {
                throw new ArgumentNullException(nameof(valueFactory));
            }

            _valueFactory = valueFactory;
        }

        /// <summary>
        /// 이 Lazy 인스턴스에 대한 값이 생성되었는지 여부를 나타내는 값을 가져옵니다.
        /// </summary>
        public bool IsValueCreated
        {
            get
            {
                lock (_lockObject)
                {
                    return _isValueCreated;
                }
            }
        }

        /// <summary>
        /// 현재 Lazy 인스턴스의 지연 초기화된 값을 가져옵니다.
        /// </summary>
        public T Value
        {
            get
            {
                lock (_lockObject)
                {
                    if (!_isValueCreated)
                    {
                        _value = _valueFactory!();
                        _isValueCreated = true;
                    }

                    return _value;
                }
            }
        }

        /// <summary>
        /// 이 인스턴스의 문자열 표현을 반환합니다.
        /// </summary>
        /// <returns>값이 생성되었으면 값의 ToString() 결과, 그렇지 않으면 상태 메시지입니다.</returns>
        public override string? ToString()
        {
            if (!IsValueCreated)
            {
                return "값이 생성되지 않았습니다";
            }

            T val = Value;
            return val?.ToString();
        }
    }
}
#endif
