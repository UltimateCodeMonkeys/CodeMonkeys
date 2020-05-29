using CodeMonkeys.Configuration;

namespace CodeMonkeys.Logging
{
    public abstract class LogOptions : Options
    {
        /// <summary>
        /// All messages below this level are NOT accepted and queued.
        /// </summary>
        public LogLevel MinLevel
        {
            get => GetValue<LogLevel>();
            set => SetValue(value);
        }

        /// <summary>
        /// The timestamp format used in the written log messages.
        /// </summary>
        public string TimeStampFormat
        {
            get => GetValue<string>();
            set
            {
                Property.NotEmptyOrWhiteSpace(value);
                SetValue(value);
            }
        }

        protected LogOptions()
        {
            TimeStampFormat = "yyyy-MM-dd HH:mm:ss.fff zzz";
        }
    }
}
