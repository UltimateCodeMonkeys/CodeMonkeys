namespace CodeMonkeys.Logging
{
    public interface ILogServiceFactory
    {
        ILogService Create(string context);

        void AddProvider<TProvider>(TProvider provider)
            where TProvider : class, ILogServiceProvider;

        void AddProvider<TProvider>()
            where TProvider : class, ILogServiceProvider, new();



        bool HasProvider<TProvider>()
            where TProvider : class, ILogServiceProvider;

        bool HasProviders();
    }
}
