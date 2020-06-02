namespace CodeMonkeys.Logging.Debug
{
    public sealed class DebugLogService : ScopedLogService<DebugLogOptions>
    {
        internal DebugLogService(string context)
            : base(context)
        {
        }

        protected override void PublishMessage(LogMessage message)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(
                        MessageFormatter.Format(
                            message,
                            Options.TimeStampFormat));
            }
            catch { }
        }
    }
}
