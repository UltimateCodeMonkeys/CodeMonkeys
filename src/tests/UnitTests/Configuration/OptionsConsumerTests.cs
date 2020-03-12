using System;

using Moq;
using NUnit.Framework;

namespace CodeMonkeys.UnitTests.Configuration
{
    [TestFixture]
    public class OptionsConsumerTests
    {
        private MockOptions _options;
        private Mock<IOptionsConsumerForwarder> _consumerStub;     

        [SetUp]
        public void Setup()
        {
            _options = new MockOptions();
            _consumerStub = new Mock<IOptionsConsumerForwarder>();

            new MockOptionsConsumer(
                _consumerStub.Object,
                _options);
        }

        [Test]
        public void ObservedPropertyChanged_ConsumerDoesReceiveNotification()
        {
            _options.Prop1 = !_options.Prop1;

            _consumerStub.Verify(
                consumer => consumer.OnOptionsChanged(It.IsAny<MockOptions>()), Times.Once);
        }

        [Test]
        public void ReadOnlyPropertyChanged_ConsumerDoesNotReceiveNotification()
        {
            _options.Prop2 = new Random().Next();

            _consumerStub.Verify(
                consumer => consumer.OnOptionsChanged(It.IsAny<MockOptions>()), Times.Never);
        }
    }
}
