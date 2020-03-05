using CodeMonkeys.Core.Logging;

using System;

namespace CodeMonkeys.Logging
{
    internal class LogServiceComposition : ILogService
    {
        private readonly ContextAwareLogServiceProvider[] _serviceBuilders;

        public LogServiceComposition(ContextAwareLogServiceProvider[] serviceBuilders)
        {
            _serviceBuilders = serviceBuilders;
        }

        public void Log<TState>(DateTimeOffset timestamp, LogLevel logLevel, TState state, Exception ex, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, TState state, Exception ex, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}
