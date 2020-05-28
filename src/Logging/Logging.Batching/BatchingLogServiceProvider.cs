using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMonkeys.Logging.Batching
{
    public abstract class BatchingLogServiceProvider<TOptions> : LogServiceProvider<TOptions>
        where TOptions : BatchingLogOptions, new()
    {
        public override bool IsEnabled 
        { 
            get => base.IsEnabled; 
            set => base.IsEnabled = value; 
        }

        private BlockingCollection<LogMessage> _queue;
        private CancellationTokenSource _cts;
        private Task _task;

        protected BatchingLogServiceProvider() => Run();

        public override void ProcessMessage(LogMessage message)
        {
            if (_queue.IsAddingCompleted)
                return;

            try
            {
                _queue.Add(message, _cts.Token);
            }
            catch { }
        }

        protected abstract Task ProcessBatch(IEnumerable<LogMessage> batch, CancellationToken token);

        private void Run()
        {
            _queue = Options.QueueCapacity == null?
                new BlockingCollection<LogMessage>(new ConcurrentQueue<LogMessage>()) :
                new BlockingCollection<LogMessage>(
                    new ConcurrentQueue<LogMessage>(),
                    Options.QueueCapacity.Value);

            _cts = new CancellationTokenSource();

            _task = Task.Run(ProcessingLoop);
        }

        private void Stop()
        {
            _cts.Cancel();
            _queue.CompleteAdding();

            try
            {
                _task.Wait(Options.FlushPeriod);
            }
            catch (TaskCanceledException) 
            { 
                // task did not finish in given time frame
            }
            catch (AggregateException ex) when (ex.InnerExceptions.FirstOrDefault() is TaskCanceledException) 
            {
                // task did not finish in given time frame
            }
        }

        private async Task ProcessingLoop()
        {
            var currentBatch = new List<LogMessage>(Options.BatchCapacity ?? int.MaxValue);

            while (!_cts.Token.IsCancellationRequested)
            {
                TakeBatch(currentBatch);

                if (currentBatch.Count > 0)
                    await ProcessBatch(currentBatch, _cts.Token);

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
