namespace CodeMonkeys.Logging.AppCenter
{
    public class ServiceProvider : ILogServiceProvider
    {
        public IScopedLogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));

            return new AppCenterLogService(context);
        }
    }
}
