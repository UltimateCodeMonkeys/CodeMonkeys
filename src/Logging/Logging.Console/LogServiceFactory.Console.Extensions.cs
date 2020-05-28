using CodeMonkeys.Logging.Console;

namespace CodeMonkeys.Logging
{
    public static partial class LogServiceFactoryExtensions
    {
        public static void AddConsole(this ILogServiceFactory _this)
        {            
            var provider = new ConsoleLogServiceProvider();
            _this.AddProvider(provider);
        }
    }
}
