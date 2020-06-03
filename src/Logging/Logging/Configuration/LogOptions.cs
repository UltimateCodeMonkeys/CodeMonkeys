using CodeMonkeys.Configuration;

namespace CodeMonkeys.Logging.Configuration
{
    public abstract class LogOptions : Options
    {
        /// <summary>
        /// Flag which indicates if the service or provider accepts and queues writes.
        /// <para>Defaults to <see langword="true"/>.</para>
        /// </summary>
        public bool IsEnabled
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

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
            IsEnabled = true;
            TimeStampFormat = "yyyy-MM-dd HH:mm:ss.fff zzz";
        }
    }
}
