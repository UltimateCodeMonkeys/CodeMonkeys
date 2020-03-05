using CodeMonkeys.Core.Logging;

using System;

namespace CodeMonkeys.Logging.Debug
{
    public class DebugLogServiceProvider : ILogServiceProvider
    {
        public DebugLogServiceProvider(DebugLogOptions options)
        {

        }

        public ILogService Create(string context)
        {
            throw new NotImplementedException();
        }
    }
}
