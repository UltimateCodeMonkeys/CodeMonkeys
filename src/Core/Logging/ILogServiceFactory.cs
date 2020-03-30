namespace CodeMonkeys.Core.Logging
{
    public interface ILogServiceFactory
    {
        ILogService Create(string context);

        void AddProvider<TProvider>(TProvider provider)
            where TProvider : class, ILogServiceProvider;
    }
}
