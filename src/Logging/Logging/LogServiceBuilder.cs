namespace CodeMonkeys.Logging
{
    internal class LogServiceBuilder
    {
        internal string Context { get; }
        internal ILogService LogService { get; }

        internal LogServiceBuilder(
            string context,
            ILogServiceProvider provider)
        {
            Context = context;
            LogService = provider.Create(Context);
        }
    }
}
