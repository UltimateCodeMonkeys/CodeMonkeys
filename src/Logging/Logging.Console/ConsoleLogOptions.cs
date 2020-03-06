using CodeMonkeys.Logging.Configuration;

namespace CodeMonkeys.Logging.Console
{
    public class ConsoleLogOptions : LogOptions
    {
        private bool _useColors;

        /// <summary>
        /// Enables colored output when <see langword="true"/>.
        /// </summary>
        public bool UseColors
        {
            get => _useColors;
            set => SetValue(ref _useColors, value);
        }
    }
}
