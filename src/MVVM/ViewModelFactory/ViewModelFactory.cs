using System;
using System.Linq;
using System.Threading.Tasks;

using CodeMonkeys.DependencyInjection;
using CodeMonkeys.Logging;
using CodeMonkeys.Navigation;

namespace CodeMonkeys.MVVM
{
    /// <summary>
    /// Factory to create and initialize ViewModel instances using the DI container
    /// </summary>
    public static partial class ViewModelFactory
    {
        private static ILogService log;
        private static IDependencyContainer container;

        private static INavigationService navigationServiceInstance;

        /// <summary>
        /// Sets up the factory for further usage by passing the DI container instance and an optional logger
        /// </summary>
        /// <param name="typeResolver">DI container</param>
        /// <param name="logService">LogService to use for internal logging (optional)</param>
        public static void Configure(
            IDependencyContainer typeResolver,
            ILogService logService = null)
        {
            container = typeResolver;
            log = logService;
        }


        internal static INavigationService TryResolveNavigationServiceInstance()
        {
            try
            {
                return navigationServiceInstance ??= container.Resolve<INavigationService>();
            }
            catch (Exception innerException)
            {
                string errorMessage =
                    $"Unable to resolve instance of type {typeof(INavigationService)} --- did you register an implementation?";

                log?.Error(
                    errorMessage,
                    innerException);

                throw new TypeLoadException(
                    errorMessage,
                    innerException);
            }
        }

        /// <summary>
        /// Creates a new ViewModel instance and returns it
        /// InitializeAsync is invoked in the background
        /// </summary>
        /// <typeparam name="TInterface">Type of the ViewModel to create</typeparam>
        /// <param name="initialize">Should InitializeAsync() get called after getting the instance? <see cref="IViewModel"/></param>
        /// <returns>ViewModel instance of the given type</returns>
        public static TInterface Resolve<TInterface>()

            where TInterface : class, IViewModel
        {
            try
            {
                var instance = container.Resolve<TInterface>();

                var registration = _registrations.FirstOrDefault(
                    registration => registration.Interface == typeof(TInterface) ||
                    registration.ViewModel == typeof(TInterface));

                if (registration != null &&
                    registration.Initialize)
                {
                    TaskHelper.RunSync(instance.InitializeAsync);
                }

                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = $"ViewModel of type {typeof(TInterface).Name} cannot be resolved --- is it registered?";

                log?.Critical(
                    errorMessage,
                    innerException);

                throw new TypeLoadException(
                    errorMessage,
                    innerException);
            }
        }

        /// <summary>
        /// Creates a new ViewModel instance, invokes the InitializeAsync method and returns the initialized instance
        /// </summary>
        /// <typeparam name="TInterface">Type of the ViewModel to create</typeparam>
        /// <param name="initialize">Should InitializeAsync() get called after getting the instance? <see cref="IViewModel"/></param>
        /// <returns>ViewModel instance of the given type</returns>
        public static async Task<TInterface> ResolveAsync<TInterface>()

            where TInterface : class, IViewModel
        {
            try
            {
                var instance = container.Resolve<TInterface>();

                var registration = _registrations.FirstOrDefault(
                    registration => registration.Interface == typeof(TInterface) ||
                    registration.ViewModel == typeof(TInterface));

                if (registration != null &&
                    registration.Initialize)
                {
                    await instance.InitializeAsync();
                }


                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = $"ViewModel of type {typeof(TInterface).Name} cannot be resolved --- is it registered?";

                log?.Critical(
                    errorMessage,
                    innerException);

                throw new TypeLoadException(
                    errorMessage,
                    innerException);
            }
        }


        /// <summary>
        /// Creates a new ViewModel instance, invokes the InitializeAsync method using the parameter and returns the initialized instance
        /// </summary>
        /// <typeparam name="TInterface">Type of the ViewModel to create</typeparam>
        /// <typeparam name="TModel">Type of the parameter that will be used for initialization</typeparam>
        /// <returns>ViewModel instance of the given type</returns>
        public static async Task<TInterface> ResolveAsync<TInterface, TModel>(
            TModel model)

            where TInterface : class, IViewModel<TModel>
        {
            try
            {
                var instance = container.Resolve<TInterface>();

                await instance.InitializeAsync(
                    model);


                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = $"ViewModel of type {typeof(TInterface).Name} cannot be resolved --- is it registered?";

                log?.Critical(
                    errorMessage,
                    innerException);

                throw new TypeLoadException(
                    errorMessage,
                    innerException);
            }
        }

        public static async Task<IViewModel> ResolveAsync(
            Type viewModelType)
        {
            try
            {
                var instance = container.Resolve(viewModelType);

                if (!(instance is IViewModel viewModel))
                {
                    return null;
                }

                var registration = _registrations.FirstOrDefault(
                    registration => registration.Interface ==viewModelType ||
                    registration.ViewModel == viewModelType);

                if (registration != null &&
                    registration.Initialize)
                {
                    await viewModel.InitializeAsync();
                }
                

                return viewModel;
            }
            catch (Exception exception)
            {
                string errorMessage = $"ViewModel of type {viewModelType.Name} cannot be resolved --- is it registered?";

                log?.Critical(
                    errorMessage,
                    exception);

                throw new TypeLoadException(
                    errorMessage,
                    exception);
            }
        }

        public static async Task<IViewModel<TModel>> ResolveFromModelAsync<TModel>(
            TModel model)
        {
            var registration = _registrations?.FirstOrDefault(
                registration => registration.Model == typeof(TModel));
            
            if (registration == null)
            {
                return null;
            }


            var instance = await ResolveAsync(
                registration.Interface); 

            if (instance == null)
            {
                instance = await ResolveAsync(
                    registration.ViewModel);
            }

            if (!(instance is IViewModel<TModel> viewModel))
            {
                return null;
            }


            await viewModel.InitializeAsync(
                model);


            return viewModel;
        }
    }
}