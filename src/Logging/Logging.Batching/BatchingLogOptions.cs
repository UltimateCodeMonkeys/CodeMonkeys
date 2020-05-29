using CodeMonkeys.Logging.Configuration;

using System;

namespace CodeMonkeys.Logging.Batching
{
    public class BatchLogOptions : LogOptions
    {
        /// <summary>
        /// The period after which log messages will be flushed to the store.
        /// <para>Default value: <c>5</c> seconds.</para>
        /// </summary>
        public TimeSpan FlushPeriod
        {
            get => GetValue(TimeSpan.FromSeconds(5));
            set => SetValue(value);
        }

        /// <summary>
        /// The maximum number of items to include in a single batch or <see langword="null"/> for no limit.
        /// <para>Default value: <c>50</c>.</para>
        /// </summary>        
        public int? BatchCapacity
        {
            get => GetValue<int?>(50);
            set
            {
                if (value != null)
                    Property.GreaterThan(value.Value, 0);

                SetValue(value);
            }
        }

        /// <summary>
        /// The maximum number of items in the background queue or <see langword="null"/> for no limit.
        /// <para>Default value: <c>1000</c>.</para>
        /// </summary>
        public int? QueueCapacity
        {
            get => GetValue<int?>(1000);
            set
            {
                if (value != null)
                    Property.GreaterThan(value.Value, 0);

                SetValue(value);
            }
        }
    }
}
