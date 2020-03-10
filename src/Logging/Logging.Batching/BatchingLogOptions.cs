using CodeMonkeys.Core;
using CodeMonkeys.Logging.Configuration;

using System;

namespace CodeMonkeys.Logging.Batching
{
    public class BatchingLogOptions : LogOptions
    {
        private TimeSpan _flushPeriod;
        private int _batchSize;
        private int _queueSize;

        public TimeSpan FlushPeriod
        {
            get => _flushPeriod;
            set
            {
                Property.NotDefault(value);
                SetValue(ref _flushPeriod, value);
            }
        }

        public int BatchSize
        {
            get => _batchSize;
            set
            {
                Property.Min(value, 1);
                SetValue(ref _batchSize, value);
            }
        }

        public int QueueSize
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
