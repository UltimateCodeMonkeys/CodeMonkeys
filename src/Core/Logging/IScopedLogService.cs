namespace CodeMonkeys.Logging
{
    internal interface IScopedLogService : ILogService
    {
        void EnableProvider();
        void DisableProvider();
    }
}
