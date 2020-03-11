using CodeMonkeys.Core.Interfaces.Logging;

namespace CodeMonkeys.DependencyInjection.Core
{
    internal abstract class DependencyContainerBase
    {
        internal ILogService Log { get; set; }

        internal bool IsLogServiceInstanceSet => Log != null;


        internal abstract void SetContainerImplementation(object instance);
    }
}