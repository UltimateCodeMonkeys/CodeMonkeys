using CodeMonkeys.Logging.Configuration;

using System;

namespace CodeMonkeys.Logging.Batching
{
    public class BatchingLogOptions : LogOptions
    {
        private TimeSpan _flushPeriod;
        private int _batchSize = 50;
        private int _queueSize = 1000;

        public TimeSpan FlushPeriod
        {
            get => _flushPeriod;
            set => SetValue(ref _flushPeriod, value);
        }

        public int BatchSize
        {
            get => _batchSize;
            set => SetValue(ref _batchSize, value);
        }

        public int QueueSize
        {
            get => _queueSize;
            set => SetValue(ref _queueSize, value);
        }
    }
}
