using CodeMonkeys.Logging;

using Ninject;

namespace CodeMonkeys.DependencyInjection.Ninject
{
    public static class NinjectFactory
    {
        public static IDependencyContainer CreateInstance()
        {
            var instance = DependencyContainerFactory.CreateInstance<NinjectDependencyContainer>(
                new StandardKernel());

            return instance;
        }

        public static IDependencyContainer CreateInstance(
            ILogService logService)
        {
            var instance = DependencyContainerFactory.CreateInstance<NinjectDependencyContainer>(
                new StandardKernel(), logService);

            return instance;
        }
    }
}