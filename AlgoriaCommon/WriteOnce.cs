using System;

//https://stackoverflow.com/questions/839788/is-there-a-way-of-setting-a-property-once-only-in-c-sharp

namespace AlgoriaCommon
{
    public sealed class WriteOnce<T>
    {
        private bool hasValue;

        public override string ToString()
        {
            return hasValue ? Convert.ToString(ValueOrDefault) : "";
        }

        public T Value
        {
            get
            {
                return ValueOrDefault;
            }
            set
            {
                if (hasValue) throw new AccessViolationException();
                ValueOrDefault = value;
                hasValue = true;
            }
        }
        public T ValueOrDefault { get; private set; }

        public static implicit operator T(WriteOnce<T> value) { return value.Value; }
    }
}
