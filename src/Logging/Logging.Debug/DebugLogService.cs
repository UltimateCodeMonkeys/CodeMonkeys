using System;
using SystemDiagnosticsDebug = System.Diagnostics.Debug;

namespace CodeMonkeys.Logging.Debug
{
    internal sealed class DebugLogService : ScopedLogService<DebugServiceProvider, DebugLogOptions>, IScopedLogService
    {
        private readonly LogMessageFormatter _formatter;

        internal DebugLogService(DebugServiceProvider provider, string context)
            : base(provider, context)
        {
            _formatter = new LogMessageFormatter();
        }

        public void Log<TState>(
            DateTimeOffset timestamp, 
            LogLevel logLevel, 
            TState state, 
            Exception ex, 
            Func<TState, Exception, string> formatter)
        {
            Argument.NotNull(formatter, nameof(formatter));

            var message = CreateMessage(timestamp,
                logLevel,
                state,
                ex,
                formatter);

            //SystemDiagnosticsDebug.WriteLine(
            //    _formatter.Format(
            //        message,
            //        ))

            //SystemDiagnosticsDebug.WriteLine(
            //        formatter.Format(
            //            message,
            //            Options.TimeStampFormat));

            //Provider.ProcessMessage(new LogMessage(
            //    timestamp,
            //    logLevel,
            //    formatter(state, ex),
            //    Context,
            //    ex));
        }

        public void Log<TState>(
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter) => Log(DateTimeOffset.Now, logLevel, state, ex, formatter);
    }
}
