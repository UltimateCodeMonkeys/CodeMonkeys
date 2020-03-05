using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

using System;

namespace CodeMonkeys.Logging.Console
{
    internal class ConsoleLogService : ILogService
    {
        private readonly ConsoleLogServiceProvider _provider;
        private readonly string _context;

        internal ConsoleLogService(ConsoleLogServiceProvider provider, string context)
        {
            _provider = provider;
            _context = context;
        }

        public void Log<TState>(
            DateTimeOffset timestamp, 
            LogLevel logLevel, 
            TState state, 
            Exception ex, 
            Func<TState, Exception, string> formatter)
        {
            if (!_provider.IsEnabled(logLevel))
                return;

            Argument.NotNull(
                formatter,
                nameof(formatter));

            _provider.ProcessMessage(new LogMessage(
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
