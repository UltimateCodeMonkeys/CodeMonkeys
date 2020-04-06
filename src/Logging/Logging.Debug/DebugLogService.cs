﻿using static CodeMonkeys.Core.Argument;

using System;

namespace CodeMonkeys.Logging.Debug
{
    internal sealed class DebugLogService : ILogService
    {
        private readonly DebugLogServiceProvider _provider;
        private readonly string _context;

        internal DebugLogService(
            DebugLogServiceProvider provider, 
            string context)
        {
            _provider = provider;
            _context = context;
        }

        public bool IsEnabledFor(LogLevel logLevel) => _provider.IsEnabledFor(logLevel);

        public void Log<TState>(
            DateTimeOffset timestamp, 
            LogLevel logLevel, 
            TState state, 
            Exception ex, 
            Func<TState, Exception, string> formatter)
        {
            NotNull(formatter, nameof(formatter));

            _provider.ProcessMessage(new LogMessage(
                timestamp,
                logLevel,
                formatter(state, ex),
                _context,
                ex));
        }

        public void Log<TState>(
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter) => Log(DateTimeOffset.Now, logLevel, state, ex, formatter);
    }
}
