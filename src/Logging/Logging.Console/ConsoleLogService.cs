namespace CodeMonkeys.Logging.Console
{
    internal sealed class ConsoleLogService : ScopedLogService<ConsoleLogOptions>
    {
        internal ConsoleLogService(string context)
            : base(context)
        {
            if (Options.ColorizeOutput)
                MessageFormatter = new LogMessageColorizer();
        }

        protected override void PublishMessage(LogMessage message)
        {
            try
            {
                System.Console.WriteLine(
                    MessageFormatter.Format(
                            message,
                            Options.TimeStampFormat));
            }
            catch { }
        }
    }
}
