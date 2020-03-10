using CodeMonkeys.Logging;
using CodeMonkeys.Logging.Console;
using CodeMonkeys.Logging.Extensions;
using CodeMonkeys.Logging.File;

using NUnit.Framework;
using System;

namespace CodeMonkeys.UnitTests.Logging
{
    [TestFixture]
    public class File
    {
        [Test]
        public void Test()
        {
            var factory = new LogServiceFactory();
            factory.AddFile(new FileLogOptions
            {
                Directory = Environment.CurrentDirectory,
                FlushPeriod = TimeSpan.FromSeconds(2)
            });

            var log = factory.Create("test");

            for (int i = 0; i < 5000; i++)
                log.Critical("test" + i, new Exception());
        }
    }
}
