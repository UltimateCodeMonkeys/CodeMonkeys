using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

using System;

namespace CodeMonkeys.Logging.File
{
    internal class FileLogService : ILogService
    {
        private readonly FileLogServiceProvider _provider;
        private readonly string _context;

        internal FileLogService(FileLogServiceProvider provider, string context)
        {
            _provider = provider;
            _context = context;
        }

        public bool IsEnabled(LogLevel logLevel) => _provider.IsEnabledFor(logLevel);

        public void Log<TState>(
            DateTimeOffset timestamp,
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter)
        {
            Argument.NotNull(
                formatter,
                nameof(formatter));

            _provider.EnqueueMessage(new LogMessage(
                timestamp,
                logLevel,
                formatter(state, ex),
                _context,
                ex));
        }

        public void Log<TState>(
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter) => Log(DateTimeOffset.Now, logLevel, state, ex, formatter);
    }
}
