using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging.Configuration;

namespace CodeMonkeys.Logging.Console
{
    public class ConsoleLogOptions : LogOptions
    {
        /// <summary>
        /// Enables colored output when <see langword="true"/>.
        /// </summary>
        public bool UseColors { get; set; }

        public string TimeStampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff zzz";

        public ConsoleLogOptions()
        {
        }

        public ConsoleLogOptions(LogLevel minLevel)
            : base(minLevel)
        {
        }
    }
}
