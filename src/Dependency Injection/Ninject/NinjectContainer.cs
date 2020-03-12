using System;
using System.Collections.Generic;

using CodeMonkeys.Core.Interfaces.DependencyInjection;
using CodeMonkeys.DependencyInjection.Core;
using CodeMonkeys.Logging;

using Ninject;

namespace CodeMonkeys.DependencyInjection.Ninject
{
    internal class NinjectDependencyContainer :
        DependencyContainerBase,
        IDependencyContainer
    {
        private static StandardKernel container;

        internal override void SetContainerImplementation(object innerContainer)
        {
            container = innerContainer as StandardKernel;
        }

        public TResolve Resolve<TResolve>()
            where TResolve : class
        {
            Log?.Info($"Trying to resolve type '{typeof(TResolve).Name}'...");

            if (!container.CanResolve<TResolve>())
            {
                throw new KeyNotFoundException(
                    $"There is no registration for the type '{typeof(TResolve).Name}'. Check your bootstrap!");
            }

            return container.TryGetAndThrowOnInvalidBinding<TResolve>();
        }

        public void RegisterType<TImplementation>()
        {
            Log?.Debug($"Registering type {typeof(TImplementation).Name} (no abstraction provided).");

            container
                .Bind<TImplementation>()
                .ToSelf();
        }

        public void RegisterType(
            Type typeToRegister)
        {
            Log?.Debug($"Registering type {typeToRegister.Name} (no abstraction provided).");

            container
                .Bind(typeToRegister)
                .ToSelf();
        }

        public void RegisterType<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            Log?.Debug($"Registering type {typeof(TInterface).Name}/{typeof(TImplementation).Name}.");

            container
                .Bind<TInterface>()
                .To<TImplementation>();
        }

        public void RegisterType(
            Type interfaceType,
            Type implementationType)
        {
            Log?.Debug($"Registering types {interfaceType.Name}/{implementationType.Name}.");

            container
                .Bind(interfaceType)
                .To(implementationType);
        }


        public void RegisterSingleton<TImplementation>()
        {
            Log?.Debug($"Registering type {typeof(TImplementation).Name} (no abstraction provided).");

            container
                .Bind<TImplementation>()
                .ToSelf()
                .InSingletonScope();
        }

        public void RegisterSingleton(
            Type typeToRegister)
        {
            Log?.Debug($"Registering type {typeToRegister.Name} (no abstraction provided).");

            container
                .Bind(typeToRegister)
                .ToSelf()
                .InSingletonScope();
        }

        public void RegisterSingleton<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            Log?.Debug($"Registering type {typeof(TInterface).Name}/{typeof(TImplementation).Name} as singleton.");

            container
                .Bind<TInterface>()
                .To<TImplementation>()
                .InSingletonScope();
        }

        public void RegisterSingleton(
            Type interfaceType,
            Type implementationType)
        {
            Log?.Debug($"Registering type {interfaceType.Name}/{implementationType.Name} as singleton.");

            container
                .Bind(interfaceType)
                .To(implementationType)
                .InSingletonScope();
        }


        public void RegisterInstance<TInstance>(
            TInstance instance)
            where TInstance : class
        {
            Log?.Debug($"Registering instance for type {typeof(TInstance).Name}.");

            container
                .Bind<TInstance>()
                .ToConstant<TInstance>(instance);
        }

        public void RegisterInstance<TInstance>(
            Func<TInstance> factoryFunc)
            where TInstance : class
        {
            container
                .Bind<TInstance>()
                .ToMethod((context) => factoryFunc.Invoke());
        }
    }
}