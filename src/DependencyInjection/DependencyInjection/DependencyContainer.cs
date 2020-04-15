using CodeMonkeys.Logging;

namespace CodeMonkeys.DependencyInjection
{
    internal abstract class DependencyContainer
    {
        internal ILogService Log { get; set; }

        /// <summary>
        /// This method sets the internal container implementation.
        /// </summary>
        /// <param name="instance"></param>
        internal abstract void SetContainer(
            object instance);
    }
}