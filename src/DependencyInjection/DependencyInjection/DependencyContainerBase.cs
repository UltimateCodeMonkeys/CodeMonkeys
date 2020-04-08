using CodeMonkeys.Logging;

namespace CodeMonkeys.DependencyInjection
{
    internal abstract class DependencyContainerBase
    {
        internal ILogService Log { get; set; }

        internal bool IsLogServiceInstanceSet => Log != null;


        internal abstract void SetContainerImplementation(object instance);
    }
}