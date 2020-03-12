using CodeMonkeys.Core.Interfaces.DependencyInjection;
using CodeMonkeys.Core.Logging;
using CodeMonkeys.DependencyInjection.Core;

using Ninject;

namespace CodeMonkeys.DependencyInjection.Ninject
{
    public static class NinjectFactory
    {
        public static IDependencyContainer CreateInstance()
        {
            var instance = DependencyContainerFactoryBase.CreateInstance<NinjectDependencyContainer>(
                new StandardKernel());

            return instance;
        }

        public static IDependencyContainer CreateInstance(
            ILogService logService)
        {
            var instance = DependencyContainerFactoryBase.CreateInstance<NinjectDependencyContainer>(
                new StandardKernel(), logService);

            return instance;
        }
    }
}