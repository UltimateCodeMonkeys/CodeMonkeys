using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

using System;

namespace CodeMonkeys.Logging.File
{
    public static partial class LogServiceFactoryExtensions
    {
        public static void AddFile(this ILogServiceFactory _this)
        {
            _this.AddFile(new FileLogOptions());
        }

        public static void AddFile(
            this ILogServiceFactory _this,
            FileLogOptions options)
        {
            Argument.NotNull(
                options,
                nameof(options));

            var provider = new FileLogServiceProvider(options);

            _this.AddProvider(provider);
        }

        public static void AddFile(
            this ILogServiceFactory _this,
            Func<FileLogOptions> optionsFactory)
        {
            Argument.NotNull(
                optionsFactory,
                nameof(optionsFactory));

            _this.AddFile(optionsFactory());
        }
    }
}
