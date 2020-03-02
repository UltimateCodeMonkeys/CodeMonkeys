using CodeMonkeys.Core.Messaging;
using CodeMonkeys.Messaging;
using CodeMonkeys.Messaging.Caching;
using CodeMonkeys.Messaging.Configuration;

using Moq;
using NUnit.Framework;

using System;

namespace CodeMonkeys.UnitTests.Messaging
{
    public class EventAggregatorTests
    {
        private IEventAggregator _aggregator;
        private Mock<IEventTypeCache> _eventTypeCache;
        private Mock<ISubscriptionManager> _subscriptionManagerStub;

        [SetUp]
        public void Setup()
        {
            _aggregator = new EventAggregator();
            _eventTypeCache = new Mock<IEventTypeCache>();
            _subscriptionManagerStub = new Mock<ISubscriptionManager>();
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

            _subscriptionManagerStub.Verify(
                sms => sms.GetSubscribersOf<Event>(), Times.Never);
        }

        [Test]
        public void PublishAsync_WhenEventParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _aggregator.PublishAsync<Event>(null);
            });

            _subscriptionManagerStub.Verify(
                sms => sms.GetSubscribersOf<Event>(), Times.Never);
        }

        [Test]
        public void RegisterTo_WhenSubscriberParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _aggregator.RegisterTo<Event>(null);
            });

            _subscriptionManagerStub.Verify(
                sms => sms.Add(It.IsAny<Type>(), It.IsAny<ISubscriber>()), Times.Never);
        }

        [Test]
        public void Register_WhenSubscriberParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _aggregator.Register(null);
            });

            _subscriptionManagerStub.Verify(
                sms => sms.Add(It.IsAny<Type>(), It.IsAny<ISubscriber>()), Times.Never);
        }

        [Test]
        public void DeregisterFrom_WhenSubscriberParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _aggregator.DeregisterFrom<Event>(null);
            });

            _subscriptionManagerStub.Verify(
                sms => sms.Remove(It.IsAny<Type>(), It.IsAny<ISubscriber>()), Times.Never);
        }

        [Test]
        public void Deregister_WhenSubscriberParameterIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _aggregator.Deregister(null);
            });

            _subscriptionManagerStub.Verify(
                sms => sms.Remove(It.IsAny<Type>(), It.IsAny<ISubscriber>()), Times.Never);
        }
    }
}