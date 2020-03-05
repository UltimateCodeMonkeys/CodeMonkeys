using CodeMonkeys.Core.Logging;

using System;

namespace CodeMonkeys.Logging.Debug
{
    internal class DebugLogService : ILogService
    {
        public void Log<TState>(DateTimeOffset timestamp, LogLevel logLevel, TState state, Exception ex, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, TState state, Exception ex, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}
