using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging.Debug;
using CodeMonkeys.Logging.Extensions;

using Moq;
using NUnit.Framework;

using System;

namespace CodeMonkeys.UnitTests.Logging.Extensions
{
    [TestFixture]
    public class LogServiceFactoryDebugExtensionsTests
    {
        private Mock<ILogServiceFactory> _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new Mock<ILogServiceFactory>();
        }

        [Test]
        public void AddDebug_WithOptions_InvokesFactoryAddProvider()
        {
            _factory.Object.AddDebug(new DebugLogOptions());
            _factory.Verify(f => f.AddProvider(It.IsAny<DebugLogServiceProvider>()), Times.Once);
        }

        [Test]
        public void AddDebug_WithOptionsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _factory.Object.AddDebug(null as DebugLogOptions);
            });

            _factory.Verify(f => f.AddProvider(It.IsAny<DebugLogServiceProvider>()), Times.Never);
        }

        [Test]
        public void AddDebug_WithOptionsFactory_InvokesFactoryAddProvider()
        {
            _factory.Object.AddDebug(() => new DebugLogOptions());
            _factory.Verify(f => f.AddProvider(It.IsAny<DebugLogServiceProvider>()), Times.Once);
        }

        [Test]
        public void AddDebug_WithOptionsFactoryNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _factory.Object.AddDebug(null as Func<DebugLogOptions>);
            });

            _factory.Verify(f => f.AddProvider(It.IsAny<DebugLogServiceProvider>()), Times.Never);
        }

        [Test]
        public void AddDebug_WithOptionsFactoryWhichProducesNull_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _factory.Object.AddDebug(() => null);
            });

            _factory.Verify(f => f.AddProvider(It.IsAny<DebugLogServiceProvider>()), Times.Never);
        }
    }
}
