using System;
using System.Collections.Concurrent;
using System.Threading;

namespace CodeMonkeys.Logging.Batching
{
    public abstract class BatchingLogServiceProvider<TOptions> : LogServiceProvider<TOptions>
        where TOptions : BatchingLogOptions
    {
        private BlockingCollection<LogMessage> _queue;
        private CancellationTokenSource _cts;

        private TimeSpan _flushPeriod;

        protected BatchingLogServiceProvider(TOptions options) 
            : base(options)
        {
            //_flushPeriod = options.
        }

        protected void EnqueueMessage(LogMessage message)
        {
            _queue.Add(message);
        }

        private void Run()
        {

        }

        private void Stop()
        {

        }

        protected override void OnOptionsHasChanged(TOptions options)
        {
            _flushPeriod = options.FlushPeriod;

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

        public override void Dispose()
        {
            base.Dispose();

            _cts?.Cancel();
        }
    }
}
