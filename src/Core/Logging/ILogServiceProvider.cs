using System;

namespace CodeMonkeys.Logging
{
    public interface ILogServiceProvider : IDisposable
    {
        bool IsEnabledFor(LogLevel logLevel);
        ILogService Create(string context);
    }
}
