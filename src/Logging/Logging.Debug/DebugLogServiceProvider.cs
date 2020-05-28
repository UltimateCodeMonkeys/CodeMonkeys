using SystemDiagnosticsDebug = System.Diagnostics.Debug;

namespace CodeMonkeys.Logging.Debug
{
    public sealed class DebugLogServiceProvider : LogServiceProvider<DebugLogOptions>
    {
        private readonly LogMessageFormatter _formatter;

        internal DebugLogServiceProvider()
        {
            _formatter = new LogMessageFormatter();
        }

        public override ILogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));

            return new DebugLogService(this, context);
        }

        public override void ProcessMessage(LogMessage message)
        {
            try
            {
                SystemDiagnosticsDebug.WriteLine(
                    _formatter.Format(
                        message,
                        Options.TimeStampFormat));
            }
            catch { }
        }
    }
}
