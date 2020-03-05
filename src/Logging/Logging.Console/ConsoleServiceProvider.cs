using CodeMonkeys.Core;
using CodeMonkeys.Core.Configuration;
using CodeMonkeys.Core.Logging;

using System.Text;

namespace CodeMonkeys.Logging.Console
{
    public class ConsoleServiceProvider : 
        OptionsConsumer<ConsoleOptions>,
        ILogServiceProvider
    {
        private LogLevel _minLevel;
        private bool _useColors;
        private string _timeStampFormat;

        public ConsoleServiceProvider(ConsoleOptions options)
            : base(options)
        {
            _minLevel = options.MinLevel;
            _useColors = options.UseColors;
            _timeStampFormat = options.TimeStampFormat;
        }        

        public ILogService Create(string context)
        {
            Argument.NotEmptyOrWhitespace(
                context,
                nameof(context));

            return new ConsoleService(this, context);
        }

        protected override void OnOptionsHasChanged(ConsoleOptions options)
        {
            _minLevel = options.MinLevel;
            _useColors = options.UseColors;
            _timeStampFormat = options.TimeStampFormat;
        }

        internal bool IsEnabled(LogLevel logLevel)
        {
            return logLevel > _minLevel;
        }

        internal void ProcessMessage(LogMessage message)
        {
            var builder = new StringBuilder();
            builder.Append(message.Timestamp.ToString(_timeStampFormat));
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
