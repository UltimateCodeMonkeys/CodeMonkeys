using CodeMonkeys.Core;
using CodeMonkeys.Core.Configuration;
using CodeMonkeys.Core.Logging;

using System.Text;

namespace CodeMonkeys.Logging.Console
{
    public class ConsoleLogServiceProvider : 
        OptionsConsumer<ConsoleLogOptions>,
        ILogServiceProvider
    {
        public ConsoleLogServiceProvider(ConsoleLogOptions options)
            : base(options)
        {
        }        

        public ILogService Create(string context)
        {
            Argument.NotEmptyOrWhitespace(
                context,
                nameof(context));

            return new ConsoleLogService(this, context);
        }

        protected override void OnOptionsHasChanged(ConsoleLogOptions options)
        {

        }

        internal bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        internal void ProcessMessage(LogMessage message)
        {
            var builder = new StringBuilder();
            builder.Append(message.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));
            builder.Append(" [");
            builder.Append(message.LogLevel.ToString());
            builder.Append("] ");
            builder.Append(message.Category);
            builder.Append(": ");

            builder.AppendLine(message.FormattedMessage);

            if (message.Exception != null)
            {
                builder.AppendLine(message.Exception.ToString());
            }

            System.Console.WriteLine(builder.ToString());
        }
    }
}
