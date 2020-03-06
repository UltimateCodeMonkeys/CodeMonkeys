using System.Collections.Concurrent;

namespace CodeMonkeys.Logging.Batching
{
    public abstract class BatchingLogServiceProvider<TOptions> : LogServiceProvider<TOptions>
        where TOptions : BatchingLogOptions
    {
        private readonly BlockingCollection<LogMessage> _queue;

        protected BatchingLogServiceProvider(TOptions options) 
            : base(options)
        {
            _queue = new BlockingCollection<LogMessage>(options.QueueSize);
        }
    }
}
