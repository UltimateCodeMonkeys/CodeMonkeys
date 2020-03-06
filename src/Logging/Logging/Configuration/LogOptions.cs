using CodeMonkeys.Core.Configuration;
using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging.Configuration
{
    public class LogOptions : Options
    {
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// The minimum log level. All messages below this are not processed.
        /// </summary>
        public LogLevel MinLevel { get; set; }

        /// <summary>
        /// The timestamp format used for DateTime in the log message.
        /// </summary>
        public string TimeStampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff zzz";
    }
}
