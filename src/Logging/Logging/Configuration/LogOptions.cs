using CodeMonkeys.Core;
using CodeMonkeys.Core.Configuration;
using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging.Configuration
{
    public abstract class LogOptions : Options
    {
        private bool _isEnabled;
        private LogLevel _minLevel;
        private string _timeStampFormat;

        /// <summary>
        /// Flag which indicates if the service or provider accepts and queues writes.
        /// <para>Defaults to <see langword="true"/>.</para>
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetValue(ref _isEnabled, value);
        }

        /// <summary>
        /// All messages below this level are NOT accepted and queued.
        /// </summary>
        public LogLevel MinLevel
        {
            get => _minLevel;
            set => SetValue(ref _minLevel, value);
        }

        /// <summary>
        /// Format used to 
        /// </summary>
        public string TimeStampFormat
        {
            get => _timeStampFormat;
            set
            {
                Property.NotEmptyOrWhiteSpace(value);
                SetValue(ref _timeStampFormat, value);
            }
        }

        protected LogOptions()
        {
            IsEnabled = true;
            TimeStampFormat = "yyyy-MM-dd HH:mm:ss.fff zzz";
        }
    }
}
