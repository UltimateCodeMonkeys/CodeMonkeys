using CodeMonkeys.Logging.AppCenter;

namespace CodeMonkeys.Logging
{
    public static partial class LogServiceExtensions
    {
        /// <summary>
        /// If a <see cref="AppCenterLogService"/> is attached to the <see cref="LogService"/> this method enables
        /// logging with it.
        /// </summary>
        public static void EnableAppCenterLogging(this ILogService @this)
        {
            Argument.NotNull(
                @this,
                nameof(@this));

            @this.EnableScopedService<AppCenterLogService>();
        }

        /// <summary>
        /// If a <see cref="AppCenterLogService"/> is attached to the <see cref="LogService"/> this method disables
        /// logging with it.
        /// </summary>
        public static void DisableAppCenterLogging(this ILogService @this)
        {
            Argument.NotNull(
                @this,
                nameof(@this));

            @this.DisableScopedService<AppCenterLogService>();
        }
    }
}
