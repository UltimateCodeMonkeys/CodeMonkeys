using CodeMonkeys.Logging.Configuration;

namespace CodeMonkeys.Logging.Console
{
    public class ConsoleLogOptions : LogOptions
    {
        /// <summary>
        /// Flag which indicates if the output should be highlighted with colors.
        /// <para>Defaults to <see langword="true"/>.</para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
        /// </summary>
        public bool UseColoredOutput
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ConsoleLogOptions()
        {
            UseColoredOutput = true;
        }
    }
}
