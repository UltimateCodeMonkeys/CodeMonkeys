namespace CodeMonkeys.Logging.Debug
{
    public sealed class DebugServiceProvider : ILogServiceProvider
    {
        public ILogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));

            return new DebugLogService(context);
        }
    }
}
