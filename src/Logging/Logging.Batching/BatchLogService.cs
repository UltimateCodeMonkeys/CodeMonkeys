using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMonkeys.Logging.Batching
{
    public abstract class BatchLogService<TOptions> : ScopedLogService<TOptions>
        where TOptions : BatchLogOptions, new()
    {
        private BlockingCollection<LogMessage> _queue;

        public override bool IsEnabled
        {
            get => base.IsEnabled;
            protected set
            {
                if (value)
                    Run();

                base.IsEnabled = value;
            }
        }

        protected BatchLogService(string context) 
            : base(context)
        {
            Run();
        }

        protected abstract Task PublishMessageBatch(IEnumerable<LogMessage> messageBatch);

        protected override void PublishMessage(LogMessage message)
        {
            if (_queue.IsAddingCompleted)
                return;

            try
            {
                _queue.Add(message);
            }
            catch { }
        }

        private void Run()
        {
            _queue = Options.QueueCapacity == null ?
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
                    await PublishMessageBatch(currentBatch);

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
