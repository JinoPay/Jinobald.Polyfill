#if NET35 || NET40
namespace System
{
    using System.Threading;

    public class Lazy<T>
    {
        private readonly object lockObject = new object();
        private Func<T> valueFactory;
        private T value;
        private bool isValueCreated;

        public Lazy()
        {
            this.valueFactory = () => Activator.CreateInstance<T>();
            this.isValueCreated = false;
        }

        public Lazy(T value)
        {
            this.value = value;
            this.isValueCreated = true;
        }

        public Lazy(Func<T> valueFactory)
        {
            if (valueFactory == null)
            {
                throw new ArgumentNullException(nameof(valueFactory));
            }

            this.valueFactory = valueFactory;
            this.isValueCreated = false;
        }

        public bool IsValueCreated
        {
            get
            {
                lock (this.lockObject)
                {
                    return this.isValueCreated;
                }
            }
        }

        public T Value
        {
            get
            {
                lock (this.lockObject)
                {
                    if (!this.isValueCreated)
                    {
                        if (this.valueFactory != null)
                        {
                            this.value = this.valueFactory();
                        }

                        this.isValueCreated = true;
                    }

                    return this.value;
                }
            }
        }

        public override string ToString()
        {
            if (!this.IsValueCreated)
            {
                return "값이 생성되지 않았습니다";
            }

            T val = this.Value;
            return val == null ? "null" : val.ToString();
        }
    }
}
#endif
