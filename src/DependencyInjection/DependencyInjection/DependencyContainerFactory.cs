using CodeMonkeys.Logging;

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CodeMonkeys.UnitTests")]
[assembly: InternalsVisibleTo("CodeMonkeys.DependencyInjection.Ninject")]
[assembly: InternalsVisibleTo("CodeMonkeys.DependencyInjection.DryIoC")]
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

                container.RegisterInstance<IDependencyRegister>(_container);
                container.RegisterInstance<IDependencyResolver>(_container);

                lock (_sync)
                {
                    _container = container;
                }
            }

            return _container;
        }
    }
}