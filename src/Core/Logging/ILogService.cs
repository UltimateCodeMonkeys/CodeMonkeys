namespace CodeMonkeys.Logging
{
    public interface ILogService : ILogServiceBase
    {
        /// <summary>
        /// Flag which indicates if the service accepts and queues writes.
        /// <para>Defaults to <see langword="true"/>.</para>
        /// </summary>
        bool IsEnabled { get; set; }

        void EnableLogging<TService>()
            where TService : IScopedLogService;

        void DisableLogging<TService>()
            where TService : IScopedLogService;
    }
}
