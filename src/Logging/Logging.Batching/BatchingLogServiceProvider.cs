using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMonkeys.Logging.Batching
{
    public abstract class BatchingLogServiceProvider<TOptions> : LogServiceProvider<TOptions>
        where TOptions : BatchingLogOptions, new()
    {
        public override bool IsEnabled 
        { 
            get => base.IsEnabled; 
            set
            {
                if (value)
                    Run();

                base.IsEnabled = value;
            }
        }

        private BlockingCollection<LogMessage> _queue;

        protected BatchingLogServiceProvider()
        {
            Run();
        }

        public override void ProcessMessage(LogMessage message)
        {
            if (_queue.IsAddingCompleted)
                return;

            try
            {
                _queue.Add(message);
            }
            catch { }
        }

        protected abstract Task ProcessBatch(IEnumerable<LogMessage> batch);

        private void Run()
        {
            _queue = Options.QueueCapacity == null?
                new BlockingCollection<LogMessage>(new ConcurrentQueue<LogMessage>()) :
                new BlockingCollection<LogMessage>(
                    new ConcurrentQueue<LogMessage>(),
                    Options.QueueCapacity.Value);

            Task.Run(ProcessingLoop);
        }

        private async Task ProcessingLoop()
        {
            var currentBatch = new List<LogMessage>(Options.BatchCapacity ?? int.MaxValue);

            while (IsEnabled)
            {
                TakeBatch(currentBatch);

                if (currentBatch.Count > 0)
                    await ProcessBatch(currentBatch);

                currentBatch.Clear();

                await Task.Delay(Options.FlushPeriod);
            }
        }

        private void TakeBatch(List<LogMessage> currentBatch)
        {
            var counter = Options.BatchCapacity;

            while (counter > 0 && _queue.TryTake(out var item))
            {
                currentBatch.Add(item);
                counter--;
            }
        }
    }
}
