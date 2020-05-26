using CodeMonkeys.Logging.Configuration;

using System;

namespace CodeMonkeys.Logging.Batching
{
    public class BatchingLogOptions : LogOptions
    {
        /// <summary>
        /// The period after which log messages will be flushed to the store.
        /// <para>Defaults to <c>5</c> seconds.</para>
        /// <para>Value changes of this property are monitored and applied dynamically.</para>
        /// </summary>
        public TimeSpan FlushPeriod
        {
            get => GetValue<TimeSpan>();
            set => SetValue(value);
        }

        /// <summary>
        /// The maximum number of items to include in a single batch or <see langword="null"/> for no limit.
        /// <para>Defaults to <c>50</c>.</para>
        /// <para>Value changes of this property are monitored and applied dynamically.</para>
        /// </summary>        
        public int? BatchCapacity
        {
            get => GetValue<int?>();
            set
            {
                if (value != null)
                    Property.GreaterThan(value.Value, 0);

                SetValue(value);
            }
        }

        /// <summary>
        /// The maximum number of items in the background queue or <see langword="null"/> for no limit.
        /// <para>Defaults to <c>1000</c>.</para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
        /// </summary>
        public int? QueueCapacity
        {
            get => GetValue<int?>();
            set
            {
                if (value != null)
                    Property.GreaterThan(value.Value, 0);

                SetValue(value);
            }
        }

        public BatchingLogOptions()
        {
            FlushPeriod = TimeSpan.FromSeconds(5);
            BatchCapacity = 50;
            QueueCapacity = 1000;
        }
    }
}
