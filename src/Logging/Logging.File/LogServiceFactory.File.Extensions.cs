using CodeMonkeys.Core.Logging;

using System;

using static CodeMonkeys.Core.Argument;

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
            NotNull(
                options,
                nameof(options));

            NotEmptyOrWhiteSpace(
                options.FileName,
                nameof(options.FileName));

            NotEmptyOrWhiteSpace(
                options.Extension,
                nameof(options.Extension));

            var provider = new FileLogServiceProvider(options);

            _this.AddProvider(provider);
        }

        public static void AddFile(
            this ILogServiceFactory _this,
            Func<FileLogOptions> optionsFactory)
        {
            NotNull(
                optionsFactory,
                nameof(optionsFactory));

            _this.AddFile(optionsFactory());
        }
    }
}
