using CodeMonkeys.DependencyInjection;

using System;

namespace CodeMonkeys.UnitTests.DependencyInjection
{
    internal class BaselessContainer : IDependencyContainer
    {
        public void RegisterInstance<TInstance>(TInstance instance) where TInstance : class
        {
            throw new NotImplementedException();
        }

        public void RegisterInstance<TInstance>(Func<TInstance> factoryFunc) where TInstance : class
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type interfaceType, Type implementationType)
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton<TImplementation>()
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type typeToRegister)
        {
            throw new NotImplementedException();
        }

        public void RegisterType<TImplementation>()
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type typeToRegister)
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type interfaceType, Type implementationType)
        {
            throw new NotImplementedException();
        }

        public TInterfaceToResolve Resolve<TInterfaceToResolve>() where TInterfaceToResolve : class
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type interfaceType)
        {
            throw new NotImplementedException();
        }

        public TImplementation Resolve<TImplementation>(Type interfaceType) where TImplementation : class
        {
            throw new NotImplementedException();
        }

        void IDependencyRegister.RegisterSingleton<TInterface, TImplementation>()
        {
            throw new NotImplementedException();
        }

        void IDependencyRegister.RegisterType<TInterface, TImplementation>()
        {
            throw new NotImplementedException();
        }
    }
}
