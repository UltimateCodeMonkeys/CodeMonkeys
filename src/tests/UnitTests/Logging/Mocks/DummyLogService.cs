using CodeMonkeys.Logging;

using System;

namespace CodeMonkeys.UnitTests.Logging.Mocks
{
    internal class DummyLogService :
        ScopedLogService<DummyLogOptions>
    {
        public DummyLogService(
            string context)

            : base (context)
        {

        }


        protected override void PublishMessage(
            LogMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
