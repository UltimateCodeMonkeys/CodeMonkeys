using CodeMonkeys.Core;
using CodeMonkeys.Logging.Configuration;

using System;

namespace CodeMonkeys.Logging.Batching
{
    public class BatchingLogOptions : LogOptions
    {
        private TimeSpan _flushPeriod;
        private int? _batchSize;
        private int? _queueSize;

        /// <summary>
        /// The period after which log messages will be flushed to the store.
        /// <para>Defaults to <c>5</c> seconds.</para>
        /// </summary>
        public TimeSpan FlushPeriod
        {
            get => _flushPeriod;
            set
            {
                Property.NotDefault(value);
                SetValue(ref _flushPeriod, value);
            }
        }

        /// <summary>
        /// The maximum number of items to include in a single batch or null for no limit.
        /// <para>Defaults to <c>50</c>.</para>
        /// </summary>        
        public int? BatchSize
        {
            get => _batchSize;
            set
            {
                Property.Min(value, 1);
                SetValue(ref _batchSize, value);
            }
        }

        /// <summary>
        /// The maximum number of items in the background queue or null for no limit.
        /// <para>Defaults to <c>1000</c>.</para>
        /// </summary>
        public int? QueueSize
        {
            get => _queueSize;
            set
            {
                Property.Min(value, 1);
                SetValue(ref _queueSize, value);
            }
        }

        public BatchingLogOptions()
        {
            FlushPeriod = TimeSpan.FromSeconds(5);
            BatchSize = 50;
            QueueSize = 1000;
        }
    }
}
