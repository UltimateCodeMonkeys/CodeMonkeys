using CodeMonkeys.Logging;

using DryIoc;

namespace CodeMonkeys.DependencyInjection.DryIoC
{
    public static class DryFactory
    {
        public static IDependencyContainer CreateInstance()
        {
            var instance = DependencyContainerFactory.CreateInstance<DryContainer>(
                new Container());

            return instance;
        }

        public static IDependencyContainer CreateInstance(
            ILogService logService)
        {
            var instance = DependencyContainerFactory.CreateInstance<DryContainer>(
                new Container(), logService);

            return instance;
        }
    }
}