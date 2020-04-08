namespace CodeMonkeys.DependencyInjection
{
    public interface IDependencyResolver
    {
        TInterfaceToResolve Resolve<TInterfaceToResolve>()
            where TInterfaceToResolve : class;
    }
}