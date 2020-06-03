using CodeMonkeys.Logging.Console;

namespace CodeMonkeys.Logging
{
    public static partial class LogServiceFactoryExtensions
    {
        /// <summary>
        /// Method for adding the <see cref="ServiceProvider"/> to the <see cref="LogService"/>.
        /// </summary>
        public static void AddConsole(this ILogServiceFactory @this)
        {
            Argument.NotNull(
                @this,
                nameof(@this));

            var provider = new ServiceProvider();
            @this.AddProvider(provider);
        }
    }
}
