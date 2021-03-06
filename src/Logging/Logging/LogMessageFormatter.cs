﻿using System;
using System.Text;

namespace CodeMonkeys.Logging
{
    public class LogMessageFormatter
    {
        public virtual string Format(
            LogMessage message, 
            string timestampFormat)
        {
            var builder = new StringBuilder();

            var timestamp = FormatTimestamp(
                message.Timestamp,
                timestampFormat);

            var level = FormatLogLevel(
                message.LogLevel);

            builder.AppendLine($"{timestamp} [{ level}] - {message.Context} -> {message.MethodName}:");
            builder.AppendLine(message.FormattedMessage);

            return builder.ToString();
        }

        protected virtual string FormatLogLevel(LogLevel logLevel)
        {
            return logLevel.ToString().ToUpperInvariant();
        }

        protected virtual string FormatTimestamp(
            DateTimeOffset timestamp,
            string timestampFormat)
        {
            return timestamp.ToString(timestampFormat);
        }
    }
}
