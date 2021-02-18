using System;

namespace CodeMonkeys.Logging.Batching
{
    public class BatchLogOptions : LogOptions
    {
        private const int DEFAULT_FLUSHPERIOD = 5;
        private const int DEFAULT_BATCHCAPACITY = 50;

        /// <summary>
        /// The period after which log messages will be flushed to the store.
        /// <para>Default value: <c>5</c> seconds.</para>
        /// </summary>
        public TimeSpan FlushPeriod
        {
            get => GetValue(TimeSpan.FromSeconds(DEFAULT_FLUSHPERIOD));
            set => SetValue(value);
        }

        /// <summary>
        /// The maximum number of items to include in a single batch or <see langword="null"/> for no limit.
        /// <para>Default value: <c>50</c>.</para>
        /// </summary>        
        public int? BatchCapacity
        {
            get => GetValue<int?>(DEFAULT_BATCHCAPACITY);
            set
            {
                if (value != null)
                    Property.GreaterThan(value.Value, 0);

                SetValue(value);
            }
        }
    }
}
