namespace CodeMonkeys.Logging.Console
{
    public static partial class LogServiceExtensions
    {
        /// <summary>
        /// If a <see cref="ConsoleLogService"/> is attached to the <see cref="LogService"/> this method enables
        /// logging with it.
        /// </summary>
        public static void EnableConsoleLogging(this ILogService @this)
        {
            Argument.NotNull(
                @this,
                nameof(@this));

            @this.EnableScopedService<ConsoleLogService>();
        }


        /// <summary>
        /// If a <see cref="ConsoleLogService"/> is attached to the <see cref="LogService"/> this method disables
        /// logging with it.
        /// </summary>
        public static void DisableConsoleLogging(this ILogService @this)
        {
            Argument.NotNull(
                @this,
                nameof(@this));

            @this.DisableScopedService<ConsoleLogService>();
        }
    }
}
