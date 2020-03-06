﻿using System;
using System.Runtime.CompilerServices;
using System.Threading;

using CodeMonkeys.Core;
using CodeMonkeys.Core.Interfaces.DependencyInjection;
using CodeMonkeys.Core.Interfaces.Logging;
using CodeMonkeys.DependencyInjection.Core.Exceptions;

[assembly: InternalsVisibleTo("CodeMonkeys.DependencyInjection.Ninject")]
namespace CodeMonkeys.DependencyInjection.Core
{
    internal static class DependencyContainerFactoryBase
    {
        private static IDependencyContainer _container;

        private static readonly SemaphoreSlim _semaphore =
            new SemaphoreSlim(1, 1);

        internal static IDependencyContainer CreateInstance<TDependencyContainer>(object containerImplementation)
            where TDependencyContainer : class, IDependencyContainer
        {
            Argument.NotNull(containerImplementation,
                nameof(containerImplementation));

            if (_container == null)
            {
                _semaphore.Wait();

                if (_container == null)
                {
                    _container = Activator.CreateInstance<TDependencyContainer>();

                    if (!(_container is DependencyContainerBase containerBase))
                        throw new ContainerBaseNotImplementedException(containerImplementation.GetType());

                    containerBase.SetContainerImplementation(containerImplementation);

                    _container.RegisterInstance<IDependencyRegister>(_container);
                    _container.RegisterInstance<IDependencyResolver>(_container);
                }

                _semaphore.Release();
            }

            return _container;
        }

        internal static IDependencyContainer CreateInstance<TDependencyContainer>(
            object containerImplementation,
            ILogService logService)
           where TDependencyContainer : class, IDependencyContainer
        {
            Argument.NotNull(containerImplementation,
                nameof(containerImplementation));

            if (!(_container is DependencyContainerBase containerBase))
                throw new ContainerBaseNotImplementedException(containerImplementation.GetType());

            if (_container == null ||
                !containerBase.IsLogServiceInstanceSet)
            {
                _semaphore.Wait();

                if (_container == null ||
                    !containerBase.IsLogServiceInstanceSet)
                {
                    logService?.Info("Creating DI container instance");

                    _container = containerBase as IDependencyContainer;
                    containerBase.Log = logService;
                    containerBase.SetContainerImplementation(containerImplementation);

                    _container.RegisterInstance<IDependencyRegister>(_container);
                    _container.RegisterInstance<IDependencyResolver>(_container);
                }

                _semaphore.Release();
            }

            return _container;
        }
    }
}