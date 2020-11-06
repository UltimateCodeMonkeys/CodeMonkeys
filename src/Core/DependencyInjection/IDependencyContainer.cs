namespace CodeMonkeys.DependencyInjection
{
    public interface IDependencyContainer :
        IDependencyRegister,
        IDependencyContainer
    {
    }
}