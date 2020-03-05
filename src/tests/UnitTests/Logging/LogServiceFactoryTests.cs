﻿using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging;
using CodeMonkeys.Logging.Console;

using NUnit.Framework;

using System;

namespace CodeMonkeys.UnitTests.Logging
{
    [TestFixture]
    public class LogServiceFactoryTests
    {
        private ILogServiceFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new LogServiceFactory();
        }

        [Test]
        public void Create_WhenContextIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _factory.Create(null);
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        public void Create_WhenContextIsEmptyOrWhitespace_ThrowsArgumentException(string value)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _factory.Create(value);
            });
        }

        [Test]
        public void AddProvider_WhenProviderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _factory.AddProvider<ConsoleLogServiceProvider>(null);
            });
        }        
    }
}