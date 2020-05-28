namespace CodeMonkeys.Logging
{
    public interface ILogServiceProvider
    {
        bool IsEnabledFor(LogLevel logLevel);
        ILogService Create(string context);
    }
}
