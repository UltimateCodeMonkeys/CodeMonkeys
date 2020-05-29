namespace CodeMonkeys.Logging
{
    public interface ILogServiceFactory
    {
        ILogServiceComposition Create(string context);

        void AddProvider<TProvider>(TProvider provider)
            where TProvider : class, ILogServiceProvider;
    }
}
