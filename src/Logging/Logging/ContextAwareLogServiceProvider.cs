using CodeMonkeys.Core.Logging;

using System;

namespace CodeMonkeys.Logging
{
    internal readonly struct ContextAwareLogServiceProvider
    {
        internal string Context { get; }
        internal Type ProviderType { get; }
        internal ILogService LogService { get; }

        internal ContextAwareLogServiceProvider(
            string context,
            ILogServiceProvider provider)
        {
            Context = context;
            ProviderType = provider.GetType();
            LogService = provider.Create(Context);
        }
    }
}
