namespace CodeMonkeys.Logging.Debug
{
    public sealed class DebugServiceProvider : ILogServiceProvider
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
