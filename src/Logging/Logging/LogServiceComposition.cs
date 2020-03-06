using CodeMonkeys.Core.Logging;

using System;
using System.Collections.Generic;

namespace CodeMonkeys.Logging
{
    internal class LogServiceComposition : ILogService
    {
        private readonly ContextAwareLogServiceProvider[] _providers;

        public LogServiceComposition(ContextAwareLogServiceProvider[] providers)
        {
            _providers = providers;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            List<Exception> exceptions = null;

            foreach (var provider in _providers)
            {
                var service = provider.LogService;

                try
                {
                    if (!service.IsEnabled(logLevel))
                        continue;

                    return true;
                }
                catch (Exception e)
                {
                    if (exceptions == null)
                        exceptions = new List<Exception>();

                    exceptions.Add(e);
                }
            }

            if (exceptions != null && exceptions.Count > 0)
                throw new AggregateException(
                    "Error(s) occured while querying the associated log services!",
                    exceptions);

            return false;
        }

        public void Log<TState>(
            DateTimeOffset timestamp, 
            LogLevel logLevel, 
            TState state, 
            Exception ex, 
            Func<TState, Exception, string> formatter)
        {
            List<Exception> exceptions = null;

            foreach (var provider in _providers)
            {
                var service = provider.LogService;

                if (!service.IsEnabled(logLevel))
                    continue;

                try
                {
                    service.Log(timestamp, logLevel, state, ex, formatter);
                }
                catch (Exception e)
                {
                    if (exceptions == null)
                        exceptions = new List<Exception>(_providers.Length);

                    exceptions.Add(e);
                }
            }

            if (exceptions != null && exceptions.Count > 0)
                throw new AggregateException(
                    "Error(s) occured while writing to the associated log services!",
                    exceptions);
        }

        public void Log<TState>(
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter) => Log(DateTimeOffset.Now, logLevel, state, ex, formatter);
    }
}
