using CodeMonkeys.Logging;

using Ninject;

namespace CodeMonkeys.DependencyInjection.Ninject
{
    public static class NinjectFactory
    {
        public static IDependencyContainer CreateInstance(
            bool allowReflectionEmit = false)
        {
            var instance = DependencyContainerFactory.CreateInstance<NinjectDependencyContainer>(
                CreateImplementation(allowReflectionEmit));

            return instance;
        }

        public static IDependencyContainer CreateInstance(
            ILogService logService,
            bool allowReflectionEmit = false)
        {
            var instance = DependencyContainerFactory.CreateInstance<NinjectDependencyContainer>(
                CreateImplementation(allowReflectionEmit),
                logService);

            return instance;
        }

        private static StandardKernel CreateImplementation(
            bool allowReflectionEmit)
        {
            return allowReflectionEmit ?
                new StandardKernel() :
                new StandardKernel(new NinjectSettings { UseReflectionBasedInjection = true });
        }
    }
}