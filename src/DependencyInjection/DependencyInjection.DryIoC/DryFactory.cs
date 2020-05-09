using CodeMonkeys.Logging;

using DryIoc;

namespace CodeMonkeys.DependencyInjection.DryIoC
{
    public static class DryFactory
    {
        public static IDependencyContainer CreateInstance(
            bool allowReflectionEmit = false)
        {
            var instance = DependencyContainerFactory.CreateInstance<DryContainer>(
                CreateImplementation(allowReflectionEmit));

            return instance;
        }

        public static IDependencyContainer CreateInstance(
            ILogService logService,
            bool allowReflectionEmit = false)
        {
            var instance = DependencyContainerFactory.CreateInstance<DryContainer>(
                CreateImplementation(allowReflectionEmit), 
                logService);

            return instance;
        }

        private static Container CreateImplementation(
            bool allowReflectionEmit)
        {
            return allowReflectionEmit ?
                new Container() :
                new Container(r => r.WithoutFastExpressionCompiler());
        }
    }
}