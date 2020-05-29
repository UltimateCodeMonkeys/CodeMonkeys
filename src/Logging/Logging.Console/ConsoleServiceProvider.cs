namespace CodeMonkeys.Logging.Console
{
    public sealed class ConsoleServiceProvider : ILogServiceProvider
    {
        public IScopedLogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));
            
            return new ConsoleLogService(context);
        }
    }
}
