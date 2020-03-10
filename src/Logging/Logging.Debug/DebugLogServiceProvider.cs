using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging.Debug
{
    internal sealed class DebugLogServiceProvider : LogServiceProvider<DebugLogOptions>
    {
        private DebugLogMessageFormatter _formatter;

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
            _formatter ??= new DebugLogMessageFormatter();

            DebugWriteLine(
                _formatter.Format(
                    message, 
                    TimeStampFormat));
        }

        private void DebugWriteLine(string value)
        {
            System.Diagnostics.Debug.WriteLine(value);
        }
    }
}
