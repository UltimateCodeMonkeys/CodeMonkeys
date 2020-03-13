using CodeMonkeys.Core.DependencyInjection;
using CodeMonkeys.Core.Logging;
using CodeMonkeys.DependencyInjection.Core;

using DryIoc;

namespace CodeMonkeys.DependencyInjection.DryIoC
{
    public static class DryFactory
    {
        public static IDependencyContainer CreateInstance()
        {
            var instance = DependencyContainerFactoryBase.CreateInstance<DryContainer>(
                new Container());

            return instance;
        }

        public static IDependencyContainer CreateInstance(
            ILogService logService)
        {
            var instance = DependencyContainerFactoryBase.CreateInstance<DryContainer>(
                new Container(), logService);

            return instance;
        }
    }
}