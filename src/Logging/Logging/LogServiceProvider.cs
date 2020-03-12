using CodeMonkeys.Core.Configuration;
using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging.Configuration;

namespace CodeMonkeys.Logging
{
    public abstract class LogServiceProvider<TOptions> :
        OptionsConsumer<TOptions>,
        ILogServiceProvider

        where TOptions : LogOptions
    {
        protected bool IsEnabled { get; set; }
        protected LogLevel MinLevel { get; set; }
        protected string TimeStampFormat { get; set; }

        protected LogServiceProvider(TOptions options) 
            : base(options)
        {
            IsEnabled = options.IsEnabled;
            MinLevel = options.MinLevel;
        }

        public abstract ILogService Create(string context);

        public virtual bool IsEnabledFor(LogLevel logLevel)
        {
            if (IsEnabled)
                return logLevel >= MinLevel;

            return false;
        }

        public abstract void ProcessMessage(LogMessage message);

        protected override void OnOptionsChanged(TOptions options)
        {
            IsEnabled = options.IsEnabled;
            MinLevel = options.MinLevel;
            TimeStampFormat = options.TimeStampFormat;
        }

        public virtual void Dispose()
        {
            OptionsChangeToken.Dispose();
        }
    }
}
