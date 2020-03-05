using CodeMonkeys.Core.Configuration;
using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging.Configuration
{
    public class LogOptions : Options
    {
        public LogLevel MinLevel { get; set; }

        public LogOptions()
        {
        }

        public LogOptions(LogLevel minLevel)
        {
            MinLevel = minLevel;
        }
    }
}
