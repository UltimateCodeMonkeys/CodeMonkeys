using DryIoc;

using System;

namespace CodeMonkeys.DependencyInjection.DryIoC
{
    internal class DryContainer :
        DependencyContainer,

        IDependencyContainer
    {
        private IContainer container;

        internal override void SetContainer(object instance)
        {
            if (instance is IContainer dryContainer)
            {
                container = dryContainer;
                return;
            }

            throw new InvalidCastException(
                $"Cannot use type {instance.GetType()} with {nameof(DryContainer)}!");
        }

        public TInterfaceToResolve Resolve<TInterfaceToResolve>()

            where TInterfaceToResolve : class
        {
            return container.Resolve<TInterfaceToResolve>();
        }

        public TImplementation Resolve<TImplementation>(
            Type interfaceType)

            where TImplementation : class
        {
            var instance = container.Resolve(interfaceType);


            if (!(instance is TImplementation implementation))
                return null;


            return implementation;
        }

        public object Resolve(
            Type interfaceType)
        {
            return container.Resolve(interfaceType);
        }


        public void RegisterInstance<TInstance>(TInstance instance)
            
            where TInstance : class
        {
            container.RegisterInstance(instance);
        }

        public void RegisterInstance<TInstance>(Func<TInstance> factoryFunc)
            
            where TInstance : class
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
    }
}