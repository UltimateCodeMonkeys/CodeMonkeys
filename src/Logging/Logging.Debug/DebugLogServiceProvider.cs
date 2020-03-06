using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging.Debug
{
    public class DebugLogServiceProvider : LogServiceProvider<DebugLogOptions>
    {
        public DebugLogServiceProvider(DebugLogOptions options)
            : base(options)
        {
        }

        public override ILogService Create(string context)
        {
            Argument.NotEmptyOrWhitespace(
                context,
                nameof(context));

            return new DebugLogService(this, context);
        }
    }
}
