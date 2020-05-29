using CodeMonkeys.Configuration;
using CodeMonkeys.Logging.Configuration;

using System;

namespace CodeMonkeys.Logging
{
    // todo: internal
    public class ScopedLogService<TProvider, TOptions> : OptionsConsumer<TOptions>
        where TProvider : ILogServiceProvider
        where TOptions : LogOptions, new()
    {
        protected TProvider Provider { get; private set; }
        protected string Context { get; private set; }

        protected ScopedLogService(
            TProvider provider,
            string context)
        {
            Provider = provider;
            Context = context;
        }

        public bool IsEnabledFor(LogLevel logLevel) => Provider.IsEnabledFor(logLevel);

        protected LogMessage CreateMessage<TState>(DateTimeOffset timestamp,
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter)
        {
            var message = new LogMessage(
                timestamp,
                logLevel,
                formatter(state, ex),
                Context,
                ex);

            return message;
        }
    }
}
