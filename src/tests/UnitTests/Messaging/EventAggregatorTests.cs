using CodeMonkeys.Core.Messaging;
using CodeMonkeys.Messaging;

using NUnit.Framework;

namespace CodeMonkeys.UnitTests.Messaging
{
    public class EventAggregatorTests
    {
        private IEventAggregator _aggregator;

        [SetUp]
        public void Setup()
        {
            _aggregator = new EventAggregator();
        }

        [Test]
        public void Constructor_()
        {
            Assert.Pass();
        }
    }
}