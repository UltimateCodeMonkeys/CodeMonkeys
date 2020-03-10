using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging.Console;

using static CodeMonkeys.Core.Argument;

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
            NotNull(
                options,
                nameof(options));
            
            var provider = new ConsoleLogServiceProvider(options);

            _this.AddProvider(provider);
        }

        public static void AddConsole(
            this ILogServiceFactory _this,
            Func<ConsoleLogOptions> optionsFactory)
        {
            NotNull(
                optionsFactory,
                nameof(optionsFactory));

            _this.AddConsole(optionsFactory());
        }
    }
}
