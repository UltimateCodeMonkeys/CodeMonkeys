namespace CodeMonkeys.Logging.Console
{
    internal partial class ConsoleLogMessageFormatter
    {
        private readonly struct ConsoleColor
        {
            internal string Value { get; }

            internal ConsoleColor(string value)
            {
                Value = value;
            }

            internal static ConsoleColor White => new ConsoleColor("\u001b[37m");
            internal static ConsoleColor Yellow => new ConsoleColor("\u001b[33m");
            internal static ConsoleColor BrightRed => new ConsoleColor("\u001b[31;1m");
            internal static ConsoleColor Red => new ConsoleColor("\u001b[31m");
            internal static ConsoleColor Green => new ConsoleColor("\u001b[32m");
            internal static ConsoleColor Magenta => new ConsoleColor("\u001b[35m");
            internal static ConsoleColor Cyan => new ConsoleColor("\u001b[36m");
        }
    }
}
