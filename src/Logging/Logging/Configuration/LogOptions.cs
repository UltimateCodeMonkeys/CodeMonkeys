using CodeMonkeys.Core.Configuration;
using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging.Configuration
{
    public class LogOptions : Options
    {
        private bool _isEnabled = true;
        private LogLevel _minLevel;
        private string _timeStampFormat = "yyyy-MM-dd HH:mm:ss.fff zzz";

        /// <summary>
        /// Flag to enable or disable the log service / provider.
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetValue(ref _isEnabled, value);
        }

        /// <summary>
        /// The minimum log level. All messages below this are not processed.
        /// </summary>
        public LogLevel MinLevel
        {
            get => _minLevel;
            set => SetValue(ref _minLevel, value);
        }

        /// <summary>
        /// The timestamp format used for DateTime in the log message.
        /// </summary>
        public string TimeStampFormat
        {
            get => _timeStampFormat;
            set => SetValue(ref _timeStampFormat, value);
        }
    }
}
