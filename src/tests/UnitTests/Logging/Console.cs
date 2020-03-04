using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging;
using CodeMonkeys.Logging.Console;

using NUnit.Framework;
using System;

namespace CodeMonkeys.UnitTests.Logging
{
    [TestFixture]
    public class Console
    {
        private LogServiceFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new LogServiceFactory();

            _factory.AddProvider(
                new ConsoleLogServiceProvider(new ConsoleLogOptions()));
        }

        [Test]
        public void Test()
        {
            var instance = _factory.Create<ConsoleLogServiceProvider>("test");
            
            instance.Log(
                LogLevel.Critical,
                "bla bla state",
                new Exception(),
                (s, ex) => $"{s}: {ex}");
        }
    }
}
