using System;

namespace CodeMonkeys.Core.DependencyInjection
{
    public interface IDependencyRegister
    {
        void RegisterType<TImplementation>();

        void RegisterType(
            Type typeToRegister);


        void RegisterType<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface;

        void RegisterType(
            Type interfaceType,
            Type implementationType);



        void RegisterSingleton<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface;

        void RegisterSingleton(
            Type interfaceType,
            Type implementationType);

        void RegisterSingleton<TImplementation>();

        void RegisterSingleton(
            Type typeToRegister);



        void RegisterInstance<TInstance>(
            TInstance instance)
            where TInstance : class;

        void RegisterInstance<TInstance>(
            Func<TInstance> factoryFunc)
            where TInstance : class;
    }
}