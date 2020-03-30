using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging.Debug
{
    public sealed class DebugLogServiceProvider : LogServiceProvider<DebugLogOptions>
    {
        private LogMessageFormatter _formatter;

        internal DebugLogServiceProvider(DebugLogOptions options)
            : base(options)
        {
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
            _formatter ??= new LogMessageFormatter();

            try
            {
                DebugWriteLine(
                    _formatter.Format(
                        message,
                        TimeStampFormat));
            }
            catch { }
        }

        private void DebugWriteLine(string value)
        {
            System.Diagnostics.Debug.WriteLine(value);
        }
    }
}
