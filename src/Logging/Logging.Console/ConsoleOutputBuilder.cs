using CodeMonkeys.Core.Logging;

using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMonkeys.Logging.Console
{
    internal class ConsoleOutputBuilder
    {
        private readonly struct Color
        {
            internal ConsoleColor Foreground { get; }
            internal string ANSIValue { get; }

            internal Color(ConsoleColor color, string ansiValue)
            {
                Foreground = color;
                ANSIValue = ansiValue;
            }

            internal static Color White => new Color(ConsoleColor.White, "\u001b[37m");
            internal static Color Yellow => new Color(ConsoleColor.Yellow, "\u001b[33m");
            internal static Color Red => new Color(ConsoleColor.Red, "\u001b[31m");
        }

        private readonly Dictionary<LogLevel, Color> _levelToColorMap;

        internal bool UseColors { get; set; }

        internal string TimeStampFormat { get; set; }

        internal ConsoleOutputBuilder()
        {
            _levelToColorMap = new Dictionary<LogLevel, Color>();
            SeedLevelToColorMap();
        }

        internal string BuildMessage(LogMessage message)
        {
            var builder = new StringBuilder();
            builder.Append(message.Timestamp.ToString(TimeStampFormat));
            builder.Append(" [");
            builder.Append(GetLogLevelString(message.LogLevel));
            builder.Append("] - ");
            builder.Append(message.Context);
            builder.Append(" - ");

            builder.AppendLine(message.FormattedMessage);

            if (message.Exception != null)
                builder.AppendLine(message.Exception.ToString());

            return builder.ToString();
        }

        private string GetLogLevelString(LogLevel logLevel)
        {
            if (!UseColors)
                return logLevel.ToString();

            var color = _levelToColorMap[logLevel];

            return $"{color.ANSIValue}{logLevel}\u001b[0m";
        }

        private void SeedLevelToColorMap()
        {
            _levelToColorMap.Add(LogLevel.Debug, Color.White);
            _levelToColorMap.Add(LogLevel.Info, Color.White);
            _levelToColorMap.Add(LogLevel.Warning, Color.Yellow);
            _levelToColorMap.Add(LogLevel.Error, Color.Red);
            _levelToColorMap.Add(LogLevel.Critical, Color.Red);
        }
    }
}
