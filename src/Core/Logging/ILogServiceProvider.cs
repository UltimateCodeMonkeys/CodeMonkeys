using System;

namespace CodeMonkeys.Core.Logging
{
    public interface ILogServiceProvider : IDisposable
    {
        bool IsEnabledFor(LogLevel logLevel);
        ILogService Create(string context);
    }
}
