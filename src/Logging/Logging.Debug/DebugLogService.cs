namespace CodeMonkeys.Logging.Debug
{
    /// <summary>
    /// <see cref="IScopedLogService"/> which writes to <see cref="System.Diagnostics.Debug"/>.
    /// </summary>
    public sealed class DebugLogService : ScopedLogService<DebugLogOptions>
    {
        internal DebugLogService(string context)
            : base(context)
        {
        }

        protected override void PublishMessage(LogMessage message)
        {
            System.Diagnostics.Debug.WriteLine(
                    MessageFormatter.Format(
                        message,
                        Options.TimeStampFormat));
        }
    }
}
