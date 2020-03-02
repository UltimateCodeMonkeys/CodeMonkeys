using CodeMonkeys.Core.Messaging;
using CodeMonkeys.Messaging;
using CodeMonkeys.Messaging.Configuration;

using NUnit.Framework;

using System;

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
        public void Constructor_WhenSubscribersParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new EventAggregator(null, new SubscriptionManagerOptions());
            });
        }

        [Test]
        public void Publish_WhenEventParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _aggregator.Publish<Event>(null);
            });
        }

        [Test]
        public void PublishAsync_WhenEventParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _aggregator.PublishAsync<Event>(null);
            });
        }

        [Test]
        public void RegisterTo_WhenSubscriberParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _aggregator.RegisterTo<Event>(null);
            });
        }

        [Test]
        public void Register_WhenSubscriberParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _aggregator.Register(null);
            });
        }

        [Test]
        public void DeregisterFrom_WhenSubscriberParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _aggregator.DeregisterFrom<Event>(null);
            });
        }

        [Test]
        public void Deregister_WhenSubscriberParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _aggregator.Deregister(null);
            });
        }
    }
}