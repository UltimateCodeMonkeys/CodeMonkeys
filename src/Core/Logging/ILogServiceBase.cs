using System;
using System.Runtime.CompilerServices;

namespace CodeMonkeys.Logging
{
    public interface ILogServiceBase
    {
        void Log<TState>(
            DateTimeOffset timestamp,
            LogLevel logLevel,
            TState state,
            [CallerMemberName] string methodName = "");

        void Log<TState>(
            LogLevel logLevel,
            TState state,
            [CallerMemberName] string methodName = "");        

        void Log<TState>(
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter,
            [CallerMemberName] string methodName = "");

        void Log<TState>(
            DateTimeOffset timestamp,
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter,
            [CallerMemberName] string methodName = "");
    }
}
