using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging
{
    internal readonly struct ContextAwareLogServiceProvider
    {
        internal string Context { get; }
        internal ILogService LogService { get; }

        internal ContextAwareLogServiceProvider(
            string context,
            ILogServiceProvider provider)
        {
            Context = context;
            LogService = provider.Create(Context);
        }
    }
}
