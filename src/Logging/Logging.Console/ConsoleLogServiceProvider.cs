using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging.Console
{
    public class ConsoleLogServiceProvider : LogServiceProvider<ConsoleLogOptions>
    {
        private readonly ConsoleOutputBuilder _consoleOutputBuilder;

        public ConsoleLogServiceProvider(ConsoleLogOptions options)
            : base(options)
        {
            _consoleOutputBuilder = new ConsoleOutputBuilder
            {
                UseColors = options.UseColors
            };
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
            ConsoleWriteLine(_consoleOutputBuilder
                .BuildMessage(
                    message, 
                    TimeStampFormat));
        }

        protected override void OnOptionsHasChanged(ConsoleLogOptions options)
        {
            base.OnOptionsHasChanged(options);

            _consoleOutputBuilder.UseColors = options.UseColors;
        }

        private void ConsoleWriteLine(string value)
        {
            System.Console.WriteLine(value);
        }
    }
}
