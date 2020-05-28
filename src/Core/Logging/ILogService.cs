using System;

namespace CodeMonkeys.Logging
{
    public interface ILogService1 : ILogService
    {
        void EnableProvider<TProvider>()
            where TProvider : ILogServiceProvider;

        void DisableProvider<TProvider>()
            where TProvider : ILogServiceProvider;
    }

    public interface ILogService
    {
        bool IsEnabledFor(LogLevel logLevel);

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
