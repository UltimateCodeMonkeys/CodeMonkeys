using CodeMonkeys.Messaging;

using Moq;
using NUnit.Framework;

using System;
using System.Threading.Tasks;

namespace CodeMonkeys.UnitTests.Messaging
{
    public class EventAggregatorTests
    {
        private IEventAggregator _aggregator;
        private Mock<ISubscriberOf<Event>> _subscriberStub;

        [SetUp]
        public void Setup()
        {
            _aggregator = new EventAggregator();
            _subscriberStub = new Mock<ISubscriberOf<Event>>();
        }

        [Test]
        public void Publish_WhenEventParameterNotNull_SubscriberIsNotified()
        {
            _aggregator.RegisterTo(_subscriberStub.Object);

            _aggregator.Publish(new Event());

            _subscriberStub.Verify(
                ss => ss.ReceiveEventAsync(It.IsAny<Event>()), Times.Once);
        }

        [Test]
        public void Publish_WhenEventParameterNotNull_AndSubscriberIsDeregistered_SubscriberIsNotNotified()
        {
            _aggregator.RegisterTo(_subscriberStub.Object);
            _aggregator.DeregisterFrom(_subscriberStub.Object);

            _aggregator.Publish(new Event());

            _subscriberStub.Verify(
                ss => ss.ReceiveEventAsync(It.IsAny<Event>()), Times.Never);
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
        public async Task PublishAsync_WhenEventParameterNotNull_SubscriberIsNotified()
        {
            _aggregator.RegisterTo(_subscriberStub.Object);

            await _aggregator.PublishAsync(new Event());

            _subscriberStub.Verify(
                ss => ss.ReceiveEventAsync(It.IsAny<Event>()), Times.Once);
        }

        [Test]
        public void PublishAsync_WhenEventParameterNotNull_AndSubscriberIsDeregistered_SubscriberIsNotNotified()
        {
            _aggregator.RegisterTo(_subscriberStub.Object);
            _aggregator.DeregisterFrom(_subscriberStub.Object);

            _aggregator.PublishAsync(new Event());

            _subscriberStub.Verify(
                ss => ss.ReceiveEventAsync(It.IsAny<Event>()), Times.Never);
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