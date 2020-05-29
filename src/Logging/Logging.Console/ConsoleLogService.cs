namespace CodeMonkeys.Logging.Console
{
    internal sealed class ConsoleLogService : ScopedLogService<ConsoleLogOptions>
    {
        private readonly LogMessageFormatter _formatter;

        internal ConsoleLogService(string context)
            : base(context)
        {
            _formatter = Options.ColorizeOutput ?
                new LogMessageColorizer() :
                new LogMessageFormatter();
        }

        protected override void PublishMessage(LogMessage message)
        {
            try
            {
                System.Console.WriteLine(
                    _formatter
                        .Format(
                            message,
                            Options.TimeStampFormat));
            }
            catch { }
        }
    }
}
