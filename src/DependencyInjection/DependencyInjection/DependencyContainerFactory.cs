using CodeMonkeys.Logging;

namespace CodeMonkeys.DependencyInjection
{
    internal static class DependencyContainerFactory
    {
        private static IDependencyContainer _container;
        private static readonly object _sync = new object();

        internal static IDependencyContainer CreateInstance<TDependencyContainer>(
            object innerContainer,
            ILogService logService = null,
            params object[] args)

            where TDependencyContainer : class, IDependencyContainer
        {
            Argument.NotNull(innerContainer,
                nameof(innerContainer));

            if (_container == null)
            {
                var container = Activator.CreateInstance<TDependencyContainer>(args);

                if (!(container is DependencyContainer containerBase))
                    throw new NonDependencyContainerException(container.GetType());

                containerBase.Log = logService;
                containerBase.SetContainer(innerContainer);

                lock (_sync)
                {
                    _container = container;
                }

                container.RegisterInstance<IDependencyContainer>(_container);
                container.RegisterInstance<IDependencyRegister>(_container);
                container.RegisterInstance<IDependencyContainer>(_container);
            }

            return _container;
        }
    }
}