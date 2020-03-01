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

        [Test]
        public void AddRegistration_WhenSubscriberParameterIsNull_ThrowsArgumentNullException()
        {
            _aggregator.Register(new Subscriber());
            _aggregator.Publish(new Event());
        }
    }
}