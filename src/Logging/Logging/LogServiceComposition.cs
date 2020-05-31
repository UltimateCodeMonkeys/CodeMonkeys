using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeMonkeys.Logging
{
    internal sealed class LogServiceComposition : ILogService
    {
        private readonly IScopedLogService[] _associatedServices;

        internal string Context { get; }

        public LogServiceComposition(
            string context,
            IScopedLogService[] associatedServices)
        {
            Context = context;
            _associatedServices = associatedServices;
        }

        public bool IsEnabledFor(LogLevel logLevel)
        {
            List<Exception> exceptions = null;

            foreach (var service in _associatedServices)
            {
                try
                {
                    if (!service.IsEnabledFor(logLevel))
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

            formatter = DefaultFormatter(formatter);

            foreach (var service in _associatedServices)
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
                        exceptions = new List<Exception>(_associatedServices.Length);

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

        public void Enable<TService>() 
            where TService : IScopedLogService
        {
            var serviceType = typeof(TService);
            var service = _associatedServices.FirstOrDefault(
                s => s.GetType() == serviceType);

            if (service != null)
            {

            }
        }
    }
}
