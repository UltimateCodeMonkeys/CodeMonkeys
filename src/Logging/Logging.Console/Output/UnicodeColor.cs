namespace CodeMonkeys.Logging.Console
{
    internal readonly struct UnicodeColor
    {
        internal string Value { get; }

        private UnicodeColor(string value)
        {
            Value = value;
        }

        internal static UnicodeColor White =>
            new UnicodeColor("\u001b[37m");

        internal static UnicodeColor Yellow =>
            new UnicodeColor("\u001b[33m");

        internal static UnicodeColor BrightRed =>
            new UnicodeColor("\u001b[31;1m");

        internal static UnicodeColor Red =>
            new UnicodeColor("\u001b[31m");

        internal static UnicodeColor Green =>
            new UnicodeColor("\u001b[32m");

        internal static UnicodeColor Magenta =>
            new UnicodeColor("\u001b[35m");

        internal static UnicodeColor Cyan =>
            new UnicodeColor("\u001b[36m");
    }
}