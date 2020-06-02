using System;

namespace CodeMonkeys.Logging
{
    public interface ILogServiceBase
    {    
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
