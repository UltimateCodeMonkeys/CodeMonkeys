using System;

namespace CodeMonkeys.Core.Logging
{
    public interface ILogService
    {
        bool IsEnabled(LogLevel logLevel);

        void Log<TState>(
            DateTimeOffset timestamp,
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter);

        void Log<TState>(
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter);
    }
}
