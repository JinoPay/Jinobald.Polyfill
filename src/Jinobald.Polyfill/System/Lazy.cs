#if NET35
namespace System
{
    public class Lazy<T>
    {
        private readonly Func<T> valueFactory;
        private readonly object lockObject = new();
        private bool isValueCreated;
        private T value;

        public Lazy()
        {
            valueFactory = () => Activator.CreateInstance<T>();
            isValueCreated = false;
        }

        public Lazy(T value)
        {
            this.value = value;
            isValueCreated = true;
        }

        public Lazy(Func<T> valueFactory)
        {
            if (valueFactory == null)
            {
                throw new ArgumentNullException(nameof(valueFactory));
            }

            this.valueFactory = valueFactory;
            isValueCreated = false;
        }

        public bool IsValueCreated
        {
            get
            {
                lock (lockObject)
                {
                    return isValueCreated;
                }
            }
        }

        public T Value
        {
            get
            {
                lock (lockObject)
                {
                    if (!isValueCreated)
                    {
                        if (valueFactory != null)
                        {
                            value = valueFactory();
                        }

                        isValueCreated = true;
                    }

                    return value;
                }
            }
        }

        public override string ToString()
        {
            if (!IsValueCreated)
            {
                return "값이 생성되지 않았습니다";
            }

            T val = Value;
            return val == null ? "null" : val.ToString();
        }
    }
}
#endif
