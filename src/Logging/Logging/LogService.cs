using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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

        public void EnableScopedService<TScopedService>()
            where TScopedService : class, IScopedLogService
        {
            if (TryGetScopedService<TScopedService>(out var service))
            {
                service.EnableLogging();
            }
        }       

        public void DisableScopedService<TScopedService>() 
            where TScopedService : class, IScopedLogService
        {
            if (TryGetScopedService<TScopedService>(out var service))
            {
                service.DisableLogging();
            }
        }

        public void Log<TState>(
            DateTimeOffset timestamp,
            LogLevel logLevel,
            TState state,
            [CallerMemberName] string methodName = "")
        {
            Log(timestamp, logLevel, state, null, null, methodName);
        }

        public void Log<TState>(
            LogLevel logLevel,
            TState state,
            [CallerMemberName] string methodName = "")
        {
            Log(DateTimeOffset.Now, logLevel, state, null, null, methodName);
        }

        public void Log<TState>(
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter,
            [CallerMemberName] string methodName = "")
        {
            Log(DateTimeOffset.Now, logLevel, state, ex, formatter, methodName);
        }

        public void Log<TState>(
            DateTimeOffset timestamp,
            LogLevel logLevel, 
            TState state, 
            Exception ex, 
            Func<TState, Exception, string> formatter,
            [CallerMemberName] string methodName = "")
        {
            if (!IsEnabled)
            {
                return;
            }

            List<Exception> exceptions = null;

            formatter = DefaultFormatter(formatter);

            foreach (var service in _services)
            {
                if (!service.IsEnabledFor(logLevel))
                {
                    continue;
                }

                try
                {
                    service.Log(timestamp, logLevel, state, ex, formatter, methodName);
                }
                catch (Exception e)
                {
                    if (exceptions == null)
                        exceptions = new List<Exception>(_services.Length);

                    exceptions.Add(e);
                }
            }

            if (exceptions != null && exceptions.Count > 0)
            {
                throw new AggregateException(
                    "Error(s) occured while writing to the associated log services!",
                    exceptions);
            }
        }

        private bool TryGetScopedService<TScopedService>(out TScopedService service)
            where TScopedService : class, IScopedLogService
        {
            var serviceType = typeof(TScopedService);

            var scopedService = _services.FirstOrDefault(s => s.GetType() == serviceType);
            service = scopedService as TScopedService;

            return service != null;
        }

        private Func<TState, Exception, string> DefaultFormatter<TState>(Func<TState, Exception, string> formatter)
        {
            formatter ??= new Func<TState, Exception, string>((s, e) =>
            {
                if (e == null && s != null)
                {
                    return s.ToString();
                }

                if (s == null && e != null)
                {
                    return e.ToString();
                }

                return $"{s}{Environment.NewLine}{e}";
            });

            return formatter;
        }
    }
}
