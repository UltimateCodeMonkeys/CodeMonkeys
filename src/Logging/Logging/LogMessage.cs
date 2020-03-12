using CodeMonkeys.Core.Logging;

using System;

namespace CodeMonkeys.Logging
{
    public readonly struct LogMessage
    {
        public DateTimeOffset Timestamp { get; }
        public LogLevel LogLevel { get; }
        public string FormattedMessage { get; }
        public string Context { get; }
        public Exception Exception { get; }

        public LogMessage(
            DateTimeOffset timestamp,
            LogLevel logLevel,
            string message,
            string context,
            Exception exception)
        {
            Timestamp = timestamp;
            LogLevel = logLevel;
            FormattedMessage = message;
            Context = context;
            Exception = exception;
        }
    }
}
