using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging;
using CodeMonkeys.Logging.Console;
using CodeMonkeys.Logging.Console.Extensions;

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
            _factory.AddConsole(new ConsoleOptions());
            _factory.AddConsole(() => new ConsoleOptions());
            _factory.AddConsole(LogLevel.Info);

            //_factory.Create<ConsoleLogServiceProvider>("");
        }

        [Test]
        public void Test()
        {
            var instance = _factory.Create("test");
            
            instance.Log(
                LogLevel.Critical,
                "bla bla state",
                new Exception(),
                (s, ex) => $"{s}: {ex}");
        }
    }
}
