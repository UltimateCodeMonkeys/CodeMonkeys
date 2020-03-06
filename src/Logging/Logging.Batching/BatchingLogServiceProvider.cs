namespace CodeMonkeys.Logging.Batching
{
    public abstract class BatchingLogServiceProvider<TOptions> : LogServiceProvider<TOptions>
        where TOptions : BatchingLogOptions
    {
        protected BatchingLogServiceProvider(TOptions options) 
            : base(options)
        {
        }
    }
}
