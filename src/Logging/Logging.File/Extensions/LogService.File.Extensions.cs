using CodeMonkeys.Logging.File;

namespace CodeMonkeys.Logging
{
    public static partial class LogServiceExtensions
    {
        /// <summary>
        /// If a <see cref="FileLogService"/> is attached to the <see cref="LogService"/> this method enables
        /// logging with it.
        /// </summary>
        public static void EnableFileLogging(this ILogService @this) =>
            @this.EnableScopedService<FileLogService>();

        /// <summary>
        /// If a <see cref="FileLogService"/> is attached to the <see cref="LogService"/> this method disables
        /// logging with it.
        /// </summary>
        public static void DisableileLogging(this ILogService @this) =>
            @this.DisableScopedService<FileLogService>();
    }
}
