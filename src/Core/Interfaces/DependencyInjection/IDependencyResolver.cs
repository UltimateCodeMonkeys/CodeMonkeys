namespace CodeMonkeys.Core.Interfaces.DependencyInjection
{
    public interface IDependencyResolver
    {
        TInterfaceToResolve Resolve<TInterfaceToResolve>()
            where TInterfaceToResolve : class;
    }
}