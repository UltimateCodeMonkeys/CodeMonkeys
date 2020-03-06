using CodeMonkeys.Core;
using CodeMonkeys.Core.Configuration;
using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging.Console
{
    public class ConsoleLogServiceProvider : 
        OptionsConsumer<ConsoleLogOptions>,
        ILogServiceProvider
    {
        private LogLevel _minLevel;

        private readonly ConsoleOutputBuilder _consoleOutputBuilder;

        public ConsoleLogServiceProvider(ConsoleLogOptions options)
            : base(options)
        {
            _consoleOutputBuilder = new ConsoleOutputBuilder
            {
                UseColors = options.UseColors,
                TimeStampFormat = options.TimeStampFormat
            };

            _minLevel = options.MinLevel;
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
            _minLevel = options.MinLevel;
            _consoleOutputBuilder.UseColors = options.UseColors;
            _consoleOutputBuilder.TimeStampFormat = options.TimeStampFormat;
        }

        internal bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minLevel;
        }

        internal void ProcessMessage(LogMessage message)
        {
            var output = _consoleOutputBuilder.BuildMessage(message);
            System.Console.WriteLine(output);
        }
    }
}
