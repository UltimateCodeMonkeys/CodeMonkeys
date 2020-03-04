namespace CodeMonkeys.Core.Logging
{
    public interface ILogServiceProvider
    {
        ILogService Create(string context);
    }
}
