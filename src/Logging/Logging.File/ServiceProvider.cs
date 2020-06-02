namespace CodeMonkeys.Logging.File
{
    public sealed class ServiceProvider : ILogServiceProvider
    {
        public IScopedLogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));

            return new FileLogService(context);
        }        
    }
}
