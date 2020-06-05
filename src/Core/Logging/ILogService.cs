namespace CodeMonkeys.Logging
{
    public interface ILogService : ILogServiceBase
    {
        /// <summary>
        /// Flag which indicates if the service accepts and queues writes.
        /// <para>Defaults to <see langword="true"/>.</para>
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// If the specified <see cref="IScopedLogService"/> is attached to the <see cref="LogService"/> this method enables
        /// logging with it.
        /// </summary>
        void EnableScopedService<TScopedService>()
            where TScopedService : class, IScopedLogService;

        /// <summary>
        /// If the specified <see cref="IScopedLogService"/> is attached to the <see cref="LogService"/> this method disables
        /// logging with it.
        /// </summary>
        void DisableScopedService<TScopedService>()
            where TScopedService : class, IScopedLogService;
    }
}
