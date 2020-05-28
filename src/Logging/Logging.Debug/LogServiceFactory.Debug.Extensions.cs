using CodeMonkeys.Logging.Debug;

namespace CodeMonkeys.Logging
{
    public static partial class LogServiceFactoryExtensions
    {
        public static void AddDebug(this ILogServiceFactory _this)
        {
            var provider = new DebugLogServiceProvider();
            _this.AddProvider(provider);
        }
    }
}
