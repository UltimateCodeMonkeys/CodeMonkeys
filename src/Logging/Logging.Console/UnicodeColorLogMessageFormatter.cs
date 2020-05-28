using System.Collections.Generic;

namespace CodeMonkeys.Logging.Console
{
    internal class LogMessageColorizer : LogMessageFormatter
    {
        private static readonly Dictionary<LogLevel, UnicodeColor> _levelColorMapping = new Dictionary<LogLevel, UnicodeColor>
        {
            { LogLevel.Trace, UnicodeColor.Magenta },
            { LogLevel.Debug, UnicodeColor.Cyan },
            { LogLevel.Info, UnicodeColor.Green },
            { LogLevel.Warning, UnicodeColor.Yellow },
            { LogLevel.Error, UnicodeColor.BrightRed },
            { LogLevel.Critical, UnicodeColor.Red }
        };

        protected override string FormatLogLevel(LogLevel logLevel)
        {
            var level = base.FormatLogLevel(logLevel);

            if (!ConsoleLogServiceProvider.Options.ColorizeOutput)
            {
                return level;
            }

            var color = _levelColorMapping[logLevel];
            return $"{color.Value}{level}\u001b[0m";
        }
    }
}
