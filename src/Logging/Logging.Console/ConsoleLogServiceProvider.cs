using SystemConsole = System.Console;

namespace CodeMonkeys.Logging.Console
{
    public sealed class ConsoleLogServiceProvider : LogServiceProvider<ConsoleLogOptions>
    {
        private LogMessageFormatter _formatter;

        internal ConsoleLogServiceProvider()
        {
            _formatter = Options.ColorizeOutput ?
                new LogMessageColorizer() :
                new LogMessageFormatter();
        }

        public override ILogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));
            
            return new ConsoleLogService(this, context);
        }

        public override void ProcessMessage(LogMessage message)
        {
            try
            {
                SystemConsole.WriteLine(_formatter
                    .Format(
                        message,
                        Options.TimeStampFormat));
            }
            catch { }
        }
    }
}
