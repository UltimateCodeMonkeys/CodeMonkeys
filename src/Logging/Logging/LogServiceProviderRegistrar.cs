using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMonkeys.Logging
{
    public class LogServiceProviderRegistrar : ILogServiceProviderRegistrar
    {
        public LogServiceProviderRegistrar(IEnumerable<ILogServiceProvider> providers)
        {
        }

        //private void MakeKnown(IEnumerable<ILogServiceProvider> providers)
        //{
        //    foreach (var provider in providers)
        //        LogServiceFactory.Instance.AddProvider(provider);
        //}
    }
}
