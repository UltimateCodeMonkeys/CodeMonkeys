using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMonkeys.Logging.Batching
{
    public abstract class BatchLogService<TOptions> : ScopedLogService<TOptions>
        where TOptions : BatchLogOptions, new()
    {
        private ConcurrentQueue<LogMessage> _queue;

        public override bool IsEnabled
        {
            get => base.IsEnabled;
            protected set
            {
                if (value)
                {
                    StartProcessingLoop();
                }

                base.IsEnabled = value;
            }
        }

        protected BatchLogService(string context)
            : base(context) => StartProcessingLoop();

        protected abstract Task PublishMessageBatch(IEnumerable<LogMessage> messageBatch);

        protected override void PublishMessage(LogMessage message)
        {
            _queue.Enqueue(message);
        }

        private void StartProcessingLoop()
        {
            _queue = new ConcurrentQueue<LogMessage>();

            Task.Run(ProcessingLoop);
        }

        private async Task ProcessingLoop()
        {
            var currentBatch = new List<LogMessage>(Options.BatchCapacity ?? int.MaxValue);

            while (IsEnabled)
            {
                TakeBatch(currentBatch);

                if (currentBatch.Count > 0)
                {
                    await PublishMessageBatch(currentBatch)
                        .ConfigureAwait(false);
                }

                currentBatch.Clear();

                await Task.Delay(Options.FlushPeriod)
                    .ConfigureAwait(false);
            }
        }

        private void TakeBatch(List<LogMessage> currentBatch)
        {
            var counter = Options.BatchCapacity;

            while (counter > 0 && _queue.TryDequeue(out var item))
            {
                currentBatch.Add(item);
                counter--;
            }
        }
    }
}
