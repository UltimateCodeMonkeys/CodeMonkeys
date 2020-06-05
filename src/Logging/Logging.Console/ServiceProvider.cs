namespace CodeMonkeys.Logging.Console
{
    internal sealed class ServiceProvider : ILogServiceProvider
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
