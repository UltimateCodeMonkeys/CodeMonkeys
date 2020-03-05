using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging.Configuration;

namespace CodeMonkeys.Logging.Console
{
    public class ConsoleOptions : LogOptions
    {
        /// <summary>
        /// Enables colored output when <see langword="true"/>.
        /// </summary>
        public bool UseColors { get; set; }

        public string TimeStampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff zzz";

        public ConsoleOptions()
        {
        }

        public ConsoleOptions(LogLevel minLevel)
            : base(minLevel)
        {
        }
    }
}
