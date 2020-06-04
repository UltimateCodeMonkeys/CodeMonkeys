using CodeMonkeys.Configuration;

namespace CodeMonkeys.Logging
{
    public abstract class LogOptions : Options
    {
        private const LogLevel DEFAULT_LOGLEVEL = LogLevel.Info;
        private const string DEFAULT_TIMESTAMP_FORMAT = "yyyy-MM-dd HH:mm:ss.fff zzz";

        /// <summary>
        /// All messages below this level are NOT accepted and queued.
        /// </summary>
        public LogLevel MinLevel
        {
            get => GetValue<LogLevel>(DEFAULT_LOGLEVEL);
            set => SetValue(value);
        }

        /// <summary>
        /// The timestamp format used in the written log messages.
        /// </summary>
        public string TimeStampFormat
        {
            get => GetValue<string>(DEFAULT_TIMESTAMP_FORMAT);
            set
            {
                Property.NotEmptyOrWhiteSpace(value);
                SetValue(value);
            }
        }
    }
}
