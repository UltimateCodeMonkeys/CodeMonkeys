using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CodeMonkeys.Logging
{
    public class LogServiceFactory : ILogServiceFactory
    {
        private readonly List<ILogServiceProvider> _providers;
        private readonly ConcurrentDictionary<string, LogServiceComposition> _services;

        private static readonly Lazy<ILogServiceFactory> lazy = new Lazy<ILogServiceFactory>(
            () => new LogServiceFactory());

        public static ILogServiceFactory Instance => lazy.Value;

        private LogServiceFactory()
        {
            _providers = new List<ILogServiceProvider>();
            _services = new ConcurrentDictionary<string, LogServiceComposition>();
        }

        public ILogServiceComposition Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));

            if (_services.TryGetValue(context, out var service))
                return service;

            service = new LogServiceComposition(
                context,
                CreateScopedLogServices(context));

            _services.TryAdd(context, service);

            return service;
        }

        public void AddProvider<TProvider>(TProvider provider)
            where TProvider : class, ILogServiceProvider
        {
            Argument.NotNull(
                provider,
                nameof(provider));

            _providers.Add(provider);
        }

        private IScopedLogService[] CreateScopedLogServices(string context)
        {
            var services = new IScopedLogService[_providers.Count];

            for (int i = 0; i < _providers.Count; i++)
            {
                services[i] = _providers[i].Create(
                    context);
            }

            return services;
        }        
    }
}
