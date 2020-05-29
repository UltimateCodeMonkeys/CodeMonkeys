namespace CodeMonkeys.Logging
{
    public interface ILogServiceProvider
    {
        ILogService Create(string context);
    }
}
