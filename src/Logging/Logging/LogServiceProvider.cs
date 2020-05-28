using CodeMonkeys.Configuration;
using CodeMonkeys.Logging.Configuration;

namespace CodeMonkeys.Logging
{
    public abstract class LogServiceProvider<TOptions> :
        OptionsConsumer<TOptions>,
        ILogServiceProvider

        where TOptions : LogOptions, new()
    {
        /// <summary>
        /// Flag which indicates if the provider accepts and queues writes.
        /// <para>Defaults to <see langword="true"/>.</para>
        /// </summary>
        public virtual bool IsEnabled { get; set; } = true;

        public abstract ILogService Create(string context);

        public virtual bool IsEnabledFor(LogLevel logLevel)
        {
            if (IsEnabled)
                return logLevel >= Options.MinLevel;

            return false;
        }

        public abstract void ProcessMessage(LogMessage message);
    }
}
