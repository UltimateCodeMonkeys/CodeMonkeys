using CodeMonkeys.Configuration;
using CodeMonkeys.Logging.Configuration;

namespace CodeMonkeys.Logging
{
    public abstract class LogServiceProvider<TOptions> :
        OptionsConsumer<TOptions>,
        ILogServiceProvider

        where TOptions : LogOptions, new()
    {
        public abstract ILogService Create(string context);

        public virtual bool IsEnabledFor(LogLevel logLevel)
        {
            if (Options.IsEnabled)
                return logLevel >= Options.MinLevel;

            return false;
        }

        public abstract void ProcessMessage(LogMessage message);
    }
}
