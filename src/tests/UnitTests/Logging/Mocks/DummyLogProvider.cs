using CodeMonkeys.Logging;

namespace CodeMonkeys.UnitTests.Logging.Mocks
{
    public class DummyLogProvider : LogServiceProvider<DummyLogOptions>
    {
        public DummyLogProvider(DummyLogOptions options) 
            : base(options)
        {
        }

        public override ILogService Create(string context)
        {
            return null;
        }

        public override void ProcessMessage(LogMessage message)
        {
            return;
        }
    }
}
