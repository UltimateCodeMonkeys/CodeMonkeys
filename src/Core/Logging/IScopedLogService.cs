namespace CodeMonkeys.Logging
{
    public interface IScopedLogService : ILogServiceBase
    {
        bool IsEnabledFor(LogLevel logLevel);
        void EnableLogging();
        void DisableLogging();
    }
}
