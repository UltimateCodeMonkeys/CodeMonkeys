using CodeMonkeys.Logging;
using CodeMonkeys.Logging.Console;

using Moq;
using NUnit.Framework;

using System;

namespace CodeMonkeys.UnitTests.Logging.Extensions
{
    [TestFixture]
    public class LogServiceFactoryConsoleExtensionsTests
    {
        private Mock<ILogServiceFactory> _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new Mock<ILogServiceFactory>();
        }

        [Test]
        public void AddConsole_WithOptions_InvokesFactoryAddProvider()
        {
            _factory.Object.AddConsole(new ConsoleLogOptions());
            _factory.Verify(f => f.AddProvider(It.IsAny<ConsoleLogServiceProvider>()), Times.Once);
        }

        [Test]
        public void AddConsole_WithOptionsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _factory.Object.AddConsole(null as ConsoleLogOptions);
            });

            _factory.Verify(f => f.AddProvider(It.IsAny<ConsoleLogServiceProvider>()), Times.Never);
        }

        [Test]
        public void AddConsole_WithOptionsFactory_InvokesFactoryAddProvider()
        {
            _factory.Object.AddConsole(() => new ConsoleLogOptions());
            _factory.Verify(f => f.AddProvider(It.IsAny<ConsoleLogServiceProvider>()), Times.Once);
        }

        [Test]
        public void AddConsole_WithOptionsFactoryNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _factory.Object.AddConsole(null as Func<ConsoleLogOptions>);
            });

            _factory.Verify(f => f.AddProvider(It.IsAny<ConsoleLogServiceProvider>()), Times.Never);
        }

        [Test]
        public void AddConsole_WithOptionsFactoryWhichProducesNull_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _factory.Object.AddConsole(() => null);
            });

            _factory.Verify(f => f.AddProvider(It.IsAny<ConsoleLogServiceProvider>()), Times.Never);
        }
    }
}