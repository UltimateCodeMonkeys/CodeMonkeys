using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

using System.Collections.Generic;
using System.Linq;

namespace CodeMonkeys.Logging
{
    public class LogServiceFactory : ILogServiceFactory
    {
        private readonly HashSet<ILogServiceProvider> _serviceProviders;

        public LogServiceFactory()
        {
            _serviceProviders = new HashSet<ILogServiceProvider>();
        }

        public ILogService Create<TProvider>(string context)
            where TProvider : class, ILogServiceProvider
        {
            Argument.NotEmptyOrWhitespace(
                context,
                nameof(context));

            return GetProvider<TProvider>()?.Create(context);
        }

        public void AddProvider<TProvider>(TProvider provider) 
            where TProvider : class, ILogServiceProvider
        {
            Argument.NotNull(
                provider,
                nameof(provider));

            _serviceProviders.Add(provider);
        }

        private ILogServiceProvider GetProvider<TProvider>()
            where TProvider : class, ILogServiceProvider
        {
            return _serviceProviders
                .FirstOrDefault(sp => sp
                    .GetType()
                    .Equals(typeof(TProvider)));
        }
    }
}
