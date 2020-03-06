using CodeMonkeys.Logging.Configuration;

namespace CodeMonkeys.Logging.Console
{
    public class ConsoleLogOptions : LogOptions
    {
        /// <summary>
        /// Enables colored output when <see langword="true"/>.
        /// </summary>
        public bool UseColors { get; set; }
    }
}
