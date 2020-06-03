using CodeMonkeys.Logging.Debug;

namespace CodeMonkeys.Logging
{
    public static partial class LogServiceExtensions
    {
        /// <summary>
        /// If a <see cref="DebugLogService"/> is attached to the <see cref="LogService"/> this method enables
        /// logging with it.
        /// </summary>
        public static void EnableDebugLogging(this ILogService @this)
        {
            Argument.NotNull(
                @this,
                nameof(@this));

            @this.EnableScopedService<DebugLogService>();
        }

        /// <summary>
        /// If a <see cref="DebugLogService"/> is attached to the <see cref="LogService"/> this method disables
        /// logging with it.
        /// </summary>
        public static void DisableDebugLogging(this ILogService @this)
        {
            Argument.NotNull(
                @this,
                nameof(@this));

            @this.DisableScopedService<DebugLogService>();
        }
    }
}
