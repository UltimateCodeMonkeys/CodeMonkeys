using CodeMonkeys.Logging.File;

namespace CodeMonkeys.Logging
{
    public static partial class LogServiceFactoryExtensions
    {
        public static void AddFile(this ILogServiceFactory _this)
        {
            var provider = new FileLogServiceProvider();
            _this.AddProvider(provider);
        }
    }
}
