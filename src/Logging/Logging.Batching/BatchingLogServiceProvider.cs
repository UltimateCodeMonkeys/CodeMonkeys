using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMonkeys.Logging.Batching
{
    public abstract class BatchingLogServiceProvider<TOptions> : LogServiceProvider<TOptions>
        where TOptions : BatchingLogOptions
    {
        private BlockingCollection<LogMessage> _queue;
        private CancellationTokenSource _cts;

        private TimeSpan _flushPeriod;
        private int _queueSize;
        private int _batchSize;

        protected BatchingLogServiceProvider(TOptions options) 
            : base(options)
        {
            OnOptionsHasChanged(options);

            _queueSize = options.QueueSize;

            Run();
        }

        protected void EnqueueMessage(LogMessage message)
        {
            if (_queue.IsAddingCompleted)
                return;

            try
            {
                _queue.Add(message, _cts.Token);
            }
            catch { }
        }

        protected override void OnOptionsHasChanged(TOptions options)
        {
            _flushPeriod = options.FlushPeriod;
            _batchSize = options.BatchSize;            

            var previousIsEnabled = IsEnabled;
            base.OnOptionsHasChanged(options);

            if (previousIsEnabled != IsEnabled)
            {
                if (IsEnabled)
                    Run();
                else
                    Stop();
            }
        }

        protected abstract Task ProcessBatch(IEnumerable<LogMessage> batch, CancellationToken token);

        private void Run()
        {
            _queue = new BlockingCollection<LogMessage>(
                new ConcurrentQueue<LogMessage>(),
                _queueSize);

            _cts = new CancellationTokenSource();

            Task.Run(ProcessingLoop);
        }

        private void Stop()
        {
            _cts.Cancel();
            _queue.CompleteAdding();
        }

        private async Task ProcessingLoop()
        {
            var currentBatch = new List<LogMessage>(_batchSize);

            while (!_cts.Token.IsCancellationRequested)
            {
                TakeBatch(currentBatch);

                if (currentBatch.Count > 0)
                    await ProcessBatch(currentBatch, _cts.Token);

                currentBatch.Clear();

                await Task.Delay(_flushPeriod);
            }
        }

        private void TakeBatch(List<LogMessage> currentBatch)
        {
            var batchCounter = _batchSize;

            while (batchCounter > 0 && _queue.TryTake(out var item))
            {
                currentBatch.Add(item);
                batchCounter--;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            _cts?.Cancel();
        }
    }
}
