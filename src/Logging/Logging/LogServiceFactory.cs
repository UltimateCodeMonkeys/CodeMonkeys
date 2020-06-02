using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CodeMonkeys.Logging
{
    public class LogServiceFactory : ILogServiceFactory
    {
        #region Singleton

        private static readonly Lazy<ILogServiceFactory> lazy = new Lazy<ILogServiceFactory>(
            () => new LogServiceFactory());

        public static ILogServiceFactory Instance => lazy.Value;

        #endregion

        private readonly List<ILogServiceProvider> _providers;
        private readonly HashSet<Type> _providerTypes;

        private readonly ConcurrentDictionary<string, LogService> _services;

        private LogServiceFactory()
        {
            _providers = new List<ILogServiceProvider>();
            _providerTypes = new HashSet<Type>();

            _services = new ConcurrentDictionary<string, LogService>();
        }

        public ILogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));

            if (_services.TryGetValue(context, out var service))
                return service;

            service = new LogService(CreateScopedLogServices(context));
            _services.TryAdd(context, service);

            return service;
        }

        /// <summary>
        /// Method for adding a <see cref="ILogServiceProvider"/> to the <see cref="LogService"/>.
        /// </summary>
        /// <param name="_this"></param>
        public void AddProvider<TProvider>(TProvider provider)
            where TProvider : class, ILogServiceProvider
        {
            Argument.NotNull(
                provider,
                nameof(provider));

            var providerType = typeof(TProvider);
            
            if (!_providerTypes.Add(providerType))
                throw new InvalidOperationException($"The type '{providerType}' is already registered!");

            _providers.Add(provider);
        }

        public void AddProvider<TProvider>()
            where TProvider : class, ILogServiceProvider, new() => AddProvider(new TProvider());

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
