using CodeMonkeys.Core.Logging;

using System;

namespace CodeMonkeys.Logging.Debug
{
    internal class DebugLogService : ILogService
    {
        private readonly DebugLogServiceProvider _provider;
        private readonly string _context;

        internal DebugLogService(DebugLogServiceProvider provider, string context)
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
            throw new NotImplementedException();
        }

        public void Log<TState>(
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter) => Log(DateTimeOffset.Now, logLevel, state, ex, formatter);
    }
}
