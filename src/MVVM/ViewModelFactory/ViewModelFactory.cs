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

        private const string ARGUMENT_NULL_EXCEPTION_MESSAGE = "ViewModel cannot be resolved --- is it registered?";

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
        /// Creates a new ViewModel instance, invokes the InitializeAsync method and returns the initialized instance
        /// </summary>
        /// <typeparam name="TInterface">Type of the ViewModel to create</typeparam>
        /// <returns>ViewModel instance of the given type</returns>
        public static TInterface Resolve<TInterface>()
            where TInterface : class, IViewModel => 
                Resolve(typeof(TInterface)) as TInterface;

        /// <summary>
        /// Creates a new ViewModel instance, invokes the InitializeAsync method and returns the initialized instance
        /// </summary>
        /// <param name="viewModelType">Type of the ViewModel to create</param>
        /// <returns>ViewModel instance of the given type</returns>
        public static IViewModel Resolve(
            Type viewModelType)
        {
            try
            {
                ValidateNonGenericParameters<IViewModel>(viewModelType);

                var instance = container.Resolve<IViewModel>(viewModelType);

                var registration = _registrations
                    .FirstOrDefault(registration => registration.Interface == viewModelType ||
                                    registration.ViewModel == viewModelType);

                if (registration != null &&
                    registration.Initialize)
                {
                    TaskHelper.RunSync(instance.InitializeAsync);
                }

                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = viewModelType == null ?
                    ARGUMENT_NULL_EXCEPTION_MESSAGE :
                    $"ViewModel of type {viewModelType.Name} cannot be resolved --- is it registered?";

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
        /// <returns>ViewModel instance of the given type</returns>
        public static async Task<TInterface> ResolveAsync<TInterface>()
            where TInterface : class, IViewModel
        {
            return await ResolveAsync(typeof(TInterface))
                .ConfigureAwait(false) as TInterface;
        }

        /// <summary>
        /// Creates a new ViewModel instance, invokes the InitializeAsync method and returns the initialized instance
        /// </summary>
        /// <param name="viewModelType">Type of the ViewModel to create</param>
        /// <returns>ViewModel instance of the given type</returns>
        public static async Task<IViewModel> ResolveAsync(
            Type viewModelType)
        {
            try
            {
                ValidateNonGenericParameters<IViewModel>(viewModelType);

                var instance = container.Resolve<IViewModel>(viewModelType);

                var registration = _registrations
                    .FirstOrDefault(registration => registration.Interface == viewModelType ||
                                    registration.ViewModel == viewModelType);

                if (registration != null &&
                    registration.Initialize)
                {
                    await instance.InitializeAsync();
                }

                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = viewModelType == null ?
                    ARGUMENT_NULL_EXCEPTION_MESSAGE :
                    $"ViewModel of type {viewModelType.Name} cannot be resolved --- is it registered?";

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
        /// <param name="model">Instance of the initialization parameter</param>
        /// <returns>ViewModel instance of the given type</returns>
        public static TInterface Resolve<TInterface, TModel>(TModel model)
            where TInterface : class, IViewModel<TModel> =>
                Resolve(typeof(TInterface), model) as TInterface;

        /// <summary>
        /// Creates a new ViewModel instance, invokes the InitializeAsync method using the parameter and returns the initialized instance
        /// </summary>
        /// <typeparam name="TModel">Type of the parameter that will be used for initialization</typeparam>
        /// <param name="model">Instance of the initialization parameter</param>
        /// <returns>ViewModel instance of the given type</returns>
        public static IViewModel<TModel> Resolve<TModel>(
            Type viewModelType,
            TModel model)
        {
            try
            {
                ValidateNonGenericParameters<IViewModel<TModel>>(viewModelType);

                var instance = container.Resolve<IViewModel<TModel>>(viewModelType);

                var registration = _registrations
                    .FirstOrDefault(registration => registration.Interface == viewModelType ||
                                    registration.ViewModel == viewModelType);

                if (registration != null &&
                    registration.Initialize)
                {
                    TaskHelper.RunSync(() => instance.InitializeAsync(model));
                }

                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = viewModelType == null ?
                    ARGUMENT_NULL_EXCEPTION_MESSAGE :
                    $"ViewModel of type {viewModelType.Name} cannot be resolved --- is it registered?";

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
        /// <param name="model">Instance of the initialization parameter</param>
        /// <returns>ViewModel instance of the given type</returns>
        public static async Task<TInterface> ResolveAsync<TInterface, TModel>(TModel model)
            where TInterface : class, IViewModel<TModel>
        {
            return await ResolveAsync(typeof(TInterface), model)
                .ConfigureAwait(false) as TInterface;
        }

        /// <summary>
        /// Creates a new ViewModel instance, invokes the InitializeAsync method using the parameter and returns the initialized instance
        /// </summary>
        /// <typeparam name="TModel">Type of the parameter that will be used for initialization</typeparam>
        /// <param name="viewModelType">Type of the ViewModel to create</param>
        /// <param name="model">Instance of the initialization parameter</param>
        /// <returns>ViewModel instance of the given type</returns>
        public static async Task<IViewModel<TModel>> ResolveAsync<TModel>(
            Type viewModelType,
            TModel model)
        {
            try
            {
                ValidateNonGenericParameters<IViewModel<TModel>>(viewModelType);

                var instance = container.Resolve<IViewModel<TModel>>(viewModelType);

                var registration = _registrations
                    .FirstOrDefault(registration => registration.Interface == viewModelType ||
                                    registration.ViewModel == viewModelType);

                if (registration != null &&
                    registration.Initialize)
                {
                    await instance.InitializeAsync(
                        model);
                }                

                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = viewModelType == null ?
                    ARGUMENT_NULL_EXCEPTION_MESSAGE :
                    $"ViewModel of type {viewModelType.Name} cannot be resolved --- is it registered?";

                log?.Critical(
                    errorMessage,
                    innerException);

                throw new TypeLoadException(
                    errorMessage,
                    innerException);
            }
        }

        public static async Task<TViewModel> ResolveFromModelAsync<TViewModel, TModel>(
            TModel model)

            where TViewModel : class, IViewModel<TModel>
        {
            var registration = _registrations?.FirstOrDefault(
                registration => registration.Model == typeof(TModel));

            if (registration == null)
            {
                throw new InvalidOperationException(
                    $"No registration found for model type {typeof(TModel).Name}!");
            }


            var instance = await ResolveAsync(
                registration.Interface);

            if (instance == null)
            {
                instance = await ResolveAsync(
                    registration.ViewModel);
            }

            if (!(instance is TViewModel viewModel))
            {
                throw new InvalidOperationException(
                    $"Registered ViewModel is of type {instance.GetType().Name}, not {typeof(TViewModel).Name}!");
            }


            await viewModel.InitializeAsync(
                model);


            return viewModel;
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


            if (!(instance is IViewModel<TModel> viewModel))
            {
                throw new InvalidOperationException(
                    $"'{instance.GetType().Name}' does not implement '{typeof(IViewModel<TModel>).Name}'!");
            }


            await viewModel.InitializeAsync(
                model);


            return viewModel;
        }

        private static void ValidateNonGenericParameters<TTargetViewModel>(Type viewModelType)
            where TTargetViewModel : IViewModel
        {
            Argument.NotNull(
                    viewModelType,
                    nameof(viewModelType),
                    $"To resolve a viewmodel the '{nameof(viewModelType)} parameter can't be null.");

            var interfaceType = typeof(TTargetViewModel);

            if (!interfaceType.IsAssignableFrom(viewModelType))
            {
                throw new InvalidOperationException(
                    $"The type {viewModelType.Name} must implement interface type {interfaceType.Name}");
            }
        }
    }
}