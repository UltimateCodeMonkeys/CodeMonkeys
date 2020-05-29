namespace CodeMonkeys.Logging.Console
{
    public sealed class ConsoleServiceProvider : ILogServiceProvider
    {
        public ILogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));
            
            return new ConsoleLogService(context);
        }
    }
}
