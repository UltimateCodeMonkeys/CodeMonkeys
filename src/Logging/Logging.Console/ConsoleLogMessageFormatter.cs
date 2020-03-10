using CodeMonkeys.Core.Logging;

using System.Collections.Generic;

namespace CodeMonkeys.Logging.Console
{
    internal partial class ConsoleLogMessageFormatter : LogMessageFormatter
    {
        private readonly Dictionary<LogLevel, ConsoleColor> _levelToColorMap;

        internal bool UseColors { get; set; }

        internal ConsoleLogMessageFormatter(bool useColoredOutput)
        {
            UseColors = useColoredOutput;

            _levelToColorMap = new Dictionary<LogLevel, ConsoleColor>();
            SeedLevelToColorMap();
        }

        protected override string FormatLogLevel(LogLevel logLevel)
        {
            var level = logLevel
                .ToString()
                .ToUpper();

            if (!UseColors)
                return level;

            var color = _levelToColorMap[logLevel];

            return $"{color.Value}{level}\u001b[0m";
        }

        private void SeedLevelToColorMap()
        {
            _levelToColorMap.Add(LogLevel.Trace, ConsoleColor.Magenta);
            _levelToColorMap.Add(LogLevel.Debug, ConsoleColor.Cyan);
            _levelToColorMap.Add(LogLevel.Info, ConsoleColor.Green);
            _levelToColorMap.Add(LogLevel.Warning, ConsoleColor.Yellow);
            _levelToColorMap.Add(LogLevel.Error, ConsoleColor.BrightRed);
            _levelToColorMap.Add(LogLevel.Critical, ConsoleColor.Red);
        }
    }
}
