using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging.Console.Extensions
{
    public static class LogServiceFactoryExtensions
    {
        //public static ILogService CreateConsole(
        //    this ILogServiceFactory _this)
        //{
        //    throw new I
        //}

        public static void RegisterConsole(
            this ILogServiceFactory _this,
            ConsoleLogOptions options)
        {
            Argument.NotNull(
                options,
                nameof(options));

            var provider = new ConsoleLogServiceProvider(options);

            _this.AddProvider(provider);
        }
    }
}
