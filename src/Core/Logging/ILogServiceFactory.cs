namespace CodeMonkeys.Core.Logging
{
    public interface ILogServiceFactory
    {
        void AddProvider<TProvider>(TProvider provider)
            where TProvider : class, ILogServiceProvider;
    }
}
