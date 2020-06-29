using System;

namespace CodeMonkeys.Logging
{
    public readonly struct LogMessage
    {
        public DateTimeOffset Timestamp { get; }
        public LogLevel LogLevel { get; }
        public string FormattedMessage { get; }
        public string Context { get; }
        public string MethodName { get; }
        public Exception Exception { get; }

        public LogMessage(
            DateTimeOffset timestamp,
            LogLevel logLevel,
            string message,
            string context,
            string methodName,
            Exception exception)
        {
            Timestamp = timestamp;
            LogLevel = logLevel;
            FormattedMessage = message;
            Context = context;
            MethodName = methodName;
            Exception = exception;
        }
    }
}
