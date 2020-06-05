namespace CodeMonkeys.Logging.Debug
{
    internal sealed class ServiceProvider : ILogServiceProvider
    {
        public IScopedLogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));

            return new DebugLogService(context);
        }
    }
}
