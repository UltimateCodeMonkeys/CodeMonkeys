using CodeMonkeys.Logging.Console;

using static CodeMonkeys.Core.Argument;

using System;

namespace CodeMonkeys.Logging.Extensions
{
    public static partial class LogServiceFactoryExtensions
    {
        /// <summary>
        /// Makes the <see cref="ConsoleLogServiceProvider"/> known to the factory and uses the default configuration.
        /// </summary>
        public static void AddConsole(this ILogServiceFactory _this)
        {
            _this.AddConsole(new ConsoleLogOptions());
        }

        /// <summary>
        /// Makes the <see cref="ConsoleLogServiceProvider"/> known to the factory and uses the given configuration.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
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

        /// <summary>
        /// Makes the <see cref="ConsoleLogServiceProvider"/> known to the factory and uses the given <paramref name="optionsFactory"/> to configure itself.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="optionsFactory"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when <paramref name="optionsFactory"/> produces a null value.</exception>
        public static void AddConsole(
            this ILogServiceFactory _this,
            Func<ConsoleLogOptions> optionsFactory)
        {
            NotNull(
                optionsFactory,
                nameof(optionsFactory));

            var options = optionsFactory() ??
                throw new InvalidOperationException($"The {nameof(optionsFactory)} produced a null value.");

            _this.AddConsole(options);
        }
    }
}
