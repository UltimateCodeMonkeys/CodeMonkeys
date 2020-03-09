using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

using System.Text;

namespace CodeMonkeys.Logging.Debug
{
    public class DebugLogServiceProvider : LogServiceProvider<DebugLogOptions>
    {
        public DebugLogServiceProvider(DebugLogOptions options)
            : base(options)
        {
        }

        public override ILogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));

            return new DebugLogService(this, context);
        }

        internal void ProcessMessage(LogMessage message)
        {
            var builder = new StringBuilder();
            builder.Append(message.Timestamp.ToString(TimeStampFormat));
            builder.Append(" [");
            builder.Append(message.LogLevel);
            builder.Append("] - ");
            builder.Append(message.Context);
            builder.Append(" - ");

            builder.AppendLine(message.FormattedMessage);

            DebugWriteLine(builder.ToString());
        }

        private void DebugWriteLine(string value)
        {
            System.Diagnostics.Debug.WriteLine(value);
        }
    }
}
