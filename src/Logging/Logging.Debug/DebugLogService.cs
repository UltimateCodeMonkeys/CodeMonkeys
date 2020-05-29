namespace CodeMonkeys.Logging.Debug
{
    internal sealed class DebugLogService : ScopedLogService<DebugLogOptions>
    {
        private readonly LogMessageFormatter _formatter;

        internal DebugLogService(string context)
            : base(context)
        {
            _formatter = new LogMessageFormatter();
        }

        protected override void PublishMessage(LogMessage message)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(
                        _formatter.Format(
                            message,
                            Options.TimeStampFormat));
            }
            catch { }
        }
    }
}
