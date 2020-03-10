using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging.Console;

using System;

namespace CodeMonkeys.Logging.Extensions
{
    public static partial class LogServiceFactoryExtensions
    {
        public static void AddConsole(this ILogServiceFactory _this)
        {
            _this.AddConsole(new ConsoleLogOptions());
        }

        public static void AddConsole(
            this ILogServiceFactory _this,
            ConsoleLogOptions options)
        {
            Argument.NotNull(
                options,
                nameof(options));

            var provider = new ConsoleLogServiceProvider(options);

            _this.AddProvider(provider);
        }

        public static void AddConsole(
            this ILogServiceFactory _this,
            Func<ConsoleLogOptions> optionsFactory)
        {
            Argument.NotNull(
                optionsFactory,
                nameof(optionsFactory));

            _this.AddConsole(optionsFactory());
        }
    }
}
