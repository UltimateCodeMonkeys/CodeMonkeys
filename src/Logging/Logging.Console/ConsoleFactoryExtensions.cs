using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

using System;

namespace CodeMonkeys.Logging.Console.Extensions
{
    public static class ConsoleFactoryExtensions
    {
        public static void AddConsole(
            this ILogServiceFactory _this,
            ConsoleOptions options)
        {
            Argument.NotNull(
                options,
                nameof(options));

            var provider = new ConsoleServiceProvider(options);

            _this.AddProvider(provider);
        }

        public static void AddConsole(
            this ILogServiceFactory _this,
            Func<ConsoleOptions> optionsFactory)
        {
            Argument.NotNull(
                optionsFactory,
                nameof(optionsFactory));

            _this.AddConsole(optionsFactory());
        }

        public static void AddConsole(
            this ILogServiceFactory _this,
            LogLevel minLevel)
        {
            _this.AddConsole(new ConsoleOptions(minLevel));
        }
    }
}
