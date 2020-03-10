using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging.Console
{
    public class ConsoleLogServiceProvider : LogServiceProvider<ConsoleLogOptions>
    {
        private readonly ConsoleLogMessageFormatter _formatter;

        public ConsoleLogServiceProvider(ConsoleLogOptions options)
            : base(options)
        {
            _formatter = new ConsoleLogMessageFormatter(
                options.UseColoredOutput);
        }        

        public override ILogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));
            
            return new ConsoleLogService(this, context);
        }

        internal void ProcessMessage(LogMessage message)
        {
            ConsoleWriteLine(_formatter
                .Format(
                    message, 
                    TimeStampFormat));
        }

        private void ConsoleWriteLine(string value)
        {
            System.Console.WriteLine(value);
        }
    }
}
