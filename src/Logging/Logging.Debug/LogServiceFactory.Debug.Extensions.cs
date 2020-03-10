using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging.Debug;

using System;

namespace CodeMonkeys.Logging.Extensions
{
    public static partial class LogServiceFactoryExtensions
    {
        public static void AddDebug(this ILogServiceFactory _this)
        {
            _this.AddDebug(new DebugLogOptions());
        }

        public static void AddDebug(
            this ILogServiceFactory _this,
            DebugLogOptions options)
        {
            Argument.NotNull(
                options,
                nameof(options));

            var provider = new DebugLogServiceProvider(options);

            _this.AddProvider(provider);
        }

        public static void AddDebug(
            this ILogServiceFactory _this,
            Func<DebugLogOptions> optionsFactory)
        {
            Argument.NotNull(
                optionsFactory,
                nameof(optionsFactory));

            _this.AddDebug(optionsFactory());
        }
    }
}
