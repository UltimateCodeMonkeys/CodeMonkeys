using CodeMonkeys.Logging;

namespace CodeMonkeys.UnitTests.Logging.Mocks
{
    public class DummyLogProvider :
        ILogServiceProvider
    {
        public DummyLogProvider()
        {
        }

        public IScopedLogService Create(
            string context)
        {
            return new DummyLogService(
                context);
        }
    }
}
