namespace CodeMonkeys.Logging
{
    public interface ILogServiceProvider
    {
        IScopedLogService Create(string context);
    }
}
