using CodeMonkeys.Logging.Configuration;

namespace CodeMonkeys.Logging.Console
{
    public class ConsoleLogOptions : LogOptions
    {
        /// <summary>
        /// Flag which indicates if the console output should be colorized depending on the log level.
        /// <para>Default value: <see langword="true"/>.</para>
        /// </summary>
        public bool ColorizeOutput
        {
            get => GetValue(true);
            set => SetValue(value);
        }
    }
}
