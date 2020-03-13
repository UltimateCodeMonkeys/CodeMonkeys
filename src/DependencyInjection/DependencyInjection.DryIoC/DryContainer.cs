using CodeMonkeys.Core.DependencyInjection;
using CodeMonkeys.DependencyInjection.Core;

using System;

using DryIoc;

namespace CodeMonkeys.DependencyInjection.DryIoC
{
    internal class DryContainer :
        DependencyContainerBase,

        IDependencyContainer
    {
        private IContainer container;

        internal override void SetContainerImplementation(object instance)
        {
            if (instance is IContainer dryContainer)
            {
                container = dryContainer;
            }
            else throw new InvalidCastException($"Cannot use type {instance.GetType()} with {nameof(DryContainer)}!");
        }


        public void RegisterInstance<TInstance>(TInstance instance) where TInstance : class
        {
            container.RegisterInstance(instance);
        }

        public void RegisterInstance<TInstance>(Func<TInstance> factoryFunc) where TInstance : class
        {
            container.RegisterInstance(factoryFunc.Invoke());
        }


        public void RegisterSingleton<TImplementation>()
        {
            container.Register<TImplementation>(Reuse.Singleton);
        }

        public void RegisterSingleton(Type typeToRegister)
        {
            container.Register(typeToRegister, Reuse.Singleton);
        }

        public void RegisterSingleton<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            container.Register<TInterface, TImplementation>(Reuse.Singleton);
        }

        public void RegisterSingleton(Type interfaceType, Type implementationType)
        {
            container.Register(interfaceType, implementationType, Reuse.Singleton);
        }


        public void RegisterType<TImplementation>()
        {
            container.Register<TImplementation>();
        }

        public void RegisterType(Type typeToRegister)
        {
            container.Register(typeToRegister);
        }

        public void RegisterType(Type interfaceType, Type implementationType)
        {
            container.Register(interfaceType, implementationType);
        }

        public void RegisterType<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            container.Register<TInterface, TImplementation>();
        }



        public TInterfaceToResolve Resolve<TInterfaceToResolve>()
            where TInterfaceToResolve : class
        {
            return container.Resolve<TInterfaceToResolve>();
        }
    }
}