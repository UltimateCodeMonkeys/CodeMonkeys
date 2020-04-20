using System;
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
            builder.Append(FormatTimestamp(message.Timestamp, timestampFormat));
            builder.Append(" [");
            builder.Append(FormatLogLevel(message.LogLevel));
            builder.Append("] - ");
            builder.Append(message.Context);
            builder.Append(" - ");

            builder.AppendLine(message.FormattedMessage);

            return builder.ToString();
        }

        protected virtual string FormatLogLevel(
            LogLevel logLevel) => logLevel.ToString();

        protected virtual string FormatTimestamp(
            DateTimeOffset timestamp, 
            string timestampFormat) => timestamp.ToString(timestampFormat);
    }
}
