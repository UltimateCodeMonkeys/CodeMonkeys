using CodeMonkeys.Logging;

using DryIoc;

namespace CodeMonkeys.DependencyInjection.DryIoC
{
    public static class DryFactory
    {
        public static IDependencyContainer CreateInstance(
            bool denyEmitUsage = false)
        {
            var instance = DependencyContainerFactory.CreateInstance<DryContainer>(
                CreateImplementationInstance(denyEmitUsage));

            return instance;
        }

        public static IDependencyContainer CreateInstance(
            ILogService logService,
            bool denyEmitUsage = false)
        {
            var instance = DependencyContainerFactory.CreateInstance<DryContainer>(
                CreateImplementationInstance(denyEmitUsage), 
                logService);

            return instance;
        }

        private static Container CreateImplementationInstance(
            bool denyEmitUsage)
        {
            return denyEmitUsage ?
                new Container() :
                new Container(r => r.WithoutFastExpressionCompiler());
        }
    }
}