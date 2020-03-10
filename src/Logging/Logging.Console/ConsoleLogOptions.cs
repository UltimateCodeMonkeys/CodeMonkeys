using CodeMonkeys.Logging.Configuration;

namespace CodeMonkeys.Logging.Console
{
    public class ConsoleLogOptions : LogOptions
    {
        private bool _useColors;

        /// <summary>
        /// Flag which indicates if the output should be highlighted with colors.
        /// <para>Defaults to <see langword="true"/>.</para>
        /// </summary>
        public bool UseColors
        {
            get => _useColors;
            set => SetValue(ref _useColors, value);
        }
    }
}
