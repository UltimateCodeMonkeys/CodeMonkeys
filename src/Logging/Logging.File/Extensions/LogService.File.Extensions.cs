using CodeMonkeys.Logging.File;

namespace CodeMonkeys.Logging
{
    public static partial class LogServiceExtensions
    {
        /// <summary>
        /// If a <see cref="FileLogService"/> is attached to the <see cref="LogService"/> this method enables
        /// logging with it.
        /// </summary>
        public static void EnableFileLogging(this ILogService @this)
        {
            Argument.NotNull(
                @this,
                nameof(@this));

            @this.EnableScopedService<FileLogService>();
        }

        /// <summary>
        /// If a <see cref="FileLogService"/> is attached to the <see cref="LogService"/> this method disables
        /// logging with it.
        /// </summary>
        public static void DisableFileLogging(this ILogService @this)
        {
            Argument.NotNull(
                @this,
                nameof(@this));

            @this.DisableScopedService<FileLogService>();
        }
    }
}
