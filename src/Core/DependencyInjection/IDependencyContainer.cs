namespace CodeMonkeys.Core.DependencyInjection
{
    public interface IDependencyContainer :
        IDependencyRegister,
        IDependencyResolver
    {
    }
}