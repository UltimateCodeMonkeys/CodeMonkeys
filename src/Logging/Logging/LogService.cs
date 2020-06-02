using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeMonkeys.Logging
{
    internal sealed class LogService : ILogService
    {
        private readonly IScopedLogService[] _services;

        public bool IsEnabled { get; set; } = true;

        public LogService(IScopedLogService[] services)
        {
            _services = services;
        }

        public void EnableLogging<TService>()
            where TService : IScopedLogService
        {
            var serviceType = typeof(TService);
            var service = _services.FirstOrDefault(s => s.GetType() == serviceType);

            service.DisableLogging();
        }

        public void DisableLogging<TService>() 
            where TService : IScopedLogService
        {
            var serviceType = typeof(TService);
            var service = _services.FirstOrDefault(s => s.GetType() == serviceType);

            service.DisableLogging();
        }

        public void Log<TState>(
            DateTimeOffset timestamp,
            LogLevel logLevel, 
            TState state, 
            Exception ex, 
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled)
                return;

            List<Exception> exceptions = null;

            formatter = DefaultFormatter(formatter);

            foreach (var service in _services)
            {
                if (!service.IsEnabledFor(logLevel))
                    continue;

                try
                {
                    service.Log(timestamp, logLevel, state, ex, formatter);
                }
                catch (Exception e)
                {
                    if (exceptions == null)
                        exceptions = new List<Exception>(_services.Length);

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

        private Func<TState, Exception, string> DefaultFormatter<TState>(Func<TState, Exception, string> formatter)
        {
            formatter ??= new Func<TState, Exception, string>((s, e) =>
            {
                if (e == null && s != null)
                    return s.ToString();

                if (s == null && e != null)
                    return e.ToString();

                return $"{s}:\n{e}";
            });

            return formatter;
        }
    }
}
