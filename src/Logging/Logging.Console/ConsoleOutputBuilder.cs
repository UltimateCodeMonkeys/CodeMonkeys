using CodeMonkeys.Core.Logging;

using System.Collections.Generic;
using System.Text;

namespace CodeMonkeys.Logging.Console
{
    internal class ConsoleOutputBuilder
    {
        private readonly struct Color
        {
            internal string Value { get; }

            internal Color(string value)
            {
                Value = value;
            }

            internal static Color White => new Color("\u001b[37m");
            internal static Color Yellow => new Color("\u001b[33m");
            internal static Color BrightRed => new Color("\u001b[31;1m");
            internal static Color Red => new Color("\u001b[31m");
            internal static Color Green => new Color("\u001b[32m");
            internal static Color Magenta => new Color("\u001b[35m");
            internal static Color Cyan => new Color("\u001b[36m");
        }

        private readonly Dictionary<LogLevel, Color> _levelToColorMap;

        internal bool UseColors { get; set; }

        internal ConsoleOutputBuilder()
        {
            _levelToColorMap = new Dictionary<LogLevel, Color>();
            SeedLevelToColorMap();
        }

        internal string BuildMessage(LogMessage message, string timeStampFormat)
        {
            var builder = new StringBuilder();            
            builder.Append(message.Timestamp.ToString(timeStampFormat));
            builder.Append(" [");
            builder.Append(GetLogLevelString(message.LogLevel));
            builder.Append("] - ");
            builder.Append(message.Context);
            builder.Append(" - ");

            builder.AppendLine(message.FormattedMessage);

            return builder.ToString();
        }

        private string GetLogLevelString(LogLevel logLevel)
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
            _levelToColorMap.Add(LogLevel.Trace, Color.Magenta);
            _levelToColorMap.Add(LogLevel.Debug, Color.Cyan);
            _levelToColorMap.Add(LogLevel.Info, Color.Green);
            _levelToColorMap.Add(LogLevel.Warning, Color.Yellow);
            _levelToColorMap.Add(LogLevel.Error, Color.BrightRed);
            _levelToColorMap.Add(LogLevel.Critical, Color.Red);
        }
    }
}
