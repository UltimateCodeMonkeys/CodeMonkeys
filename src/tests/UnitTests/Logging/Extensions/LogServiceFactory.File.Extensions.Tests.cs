using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging.File;

using Moq;
using NUnit.Framework;

using System;

namespace CodeMonkeys.UnitTests.Logging.Extensions
{
    [TestFixture]
    public class LogServiceFactoryFileExtensionsTests
    {
        private Mock<ILogServiceFactory> _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new Mock<ILogServiceFactory>();
        }

        [Test]
        public void AddFile_WithOptions_InvokesFactoryAddProvider()
        {
            _factory.Object.AddFile(new FileLogOptions());
            _factory.Verify(f => f.AddProvider(It.IsAny<FileLogServiceProvider>()), Times.Once);
        }

        [Test]
        public void AddFile_WithOptionsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _factory.Object.AddFile(null as FileLogOptions);
            });

            _factory.Verify(f => f.AddProvider(It.IsAny<FileLogServiceProvider>()), Times.Never);
        }

        [Test]
        public void AddFile_WithOptionsFactory_InvokesFactoryAddProvider()
        {
            _factory.Object.AddFile(() => new FileLogOptions());
            _factory.Verify(f => f.AddProvider(It.IsAny<FileLogServiceProvider>()), Times.Once);
        }

        [Test]
        public void AddFile_WithOptionsFactoryNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _factory.Object.AddFile(null as Func<FileLogOptions>);
            });

            _factory.Verify(f => f.AddProvider(It.IsAny<FileLogServiceProvider>()), Times.Never);
        }

        [Test]
        public void AddFile_WithOptionsFactoryWhichProducesNull_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _factory.Object.AddFile(() => null);
            });

            _factory.Verify(f => f.AddProvider(It.IsAny<FileLogServiceProvider>()), Times.Never);
        }
    }
}
