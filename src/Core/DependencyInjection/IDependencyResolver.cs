using System;

namespace CodeMonkeys.DependencyInjection
{
    public interface IDependencyResolver
    {
        TInterfaceToResolve Resolve<TInterfaceToResolve>()
            where TInterfaceToResolve : class;


        object Resolve(Type interfaceType);

        TImplementation Resolve<TImplementation>(Type interfaceType)
            where TImplementation : class;
    }
}