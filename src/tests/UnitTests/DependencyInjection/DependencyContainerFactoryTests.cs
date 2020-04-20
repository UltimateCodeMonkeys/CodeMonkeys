using CodeMonkeys.DependencyInjection;
using CodeMonkeys.Logging;

using NUnit.Framework;

using System;

namespace CodeMonkeys.UnitTests.DependencyInjection
{
    [TestFixture]
    public class DependencyContainerFactoryTests
    {
        [Test]
        public void CreateInstance_InnerContainerParamIsPresent()
        {
            var instance = DependencyContainerFactory.CreateInstance<Container>(
                 new Container());

            Assert.NotNull(instance);
        }

        [Test]
        public void CreateInstance_InnerContainerParamIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                DependencyContainerFactory.CreateInstance<Container>(
                    null);
            });
        }

        [Test]
        public void CreateInstance_ContainerDoesNotImplementBaseClass_ThrowsNonDependencyContainerException()
        {
            Assert.Throws<NonDependencyContainerException>(() =>
            {
                DependencyContainerFactory.CreateInstance<BaselessContainer>(
                    new BaselessContainer());
            });
        }

        [Test]
        public void CreateInstance_InnerContainerAndLogServiceParamIsPresent()
        {
            LogServiceFactory.Instance.AddConsole();

            var instance = DependencyContainerFactory.CreateInstance<Container>(
                new Container(),
                LogServiceFactory.Instance.Create("unit test"));

            Assert.NotNull(instance);
            Assert.NotNull((instance as Container).Log);
        }
    }
}
