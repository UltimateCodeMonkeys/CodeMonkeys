using CodeMonkeys.DependencyInjection;
using CodeMonkeys.Logging;
using CodeMonkeys.Navigation;

using System;
using System.Threading.Tasks;

namespace CodeMonkeys.MVVM.Factories
{
    /// <summary>
    /// Factory to create and initialize ViewModel instances using the DI container
    /// </summary>
    public static class ViewModelFactory
    {
        private static ILogService log;
        private static IDependencyResolver iocContainer;

        private static INavigationService navigationServiceInstance;

        /// <summary>
        /// Sets up the factory for further usage by passing the DI container instance and an optional logger
        /// </summary>
        /// <param name="typeResolver">DI container</param>
        /// <param name="logService">LogService to use for internal logging (optional)</param>
        public static void Configure(
            IDependencyResolver typeResolver,
            ILogService logService = null)
        {
            iocContainer = typeResolver;
            log = logService;
        }


        internal static INavigationService TryResolveNavigationServiceInstance()
        {
            try
            {
                return navigationServiceInstance ??
                       (navigationServiceInstance = iocContainer.Resolve<INavigationService>());
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
        /// Creates a new ViewModel instance and optionally invokes the <see cref="IViewModel.InitializeAsync"/> method.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the ViewModel to create</typeparam>
        /// <param name="initialize">Specifies if <see cref="IViewModel.InitializeAsync"/> should get called after instance creation.</param>
        /// <returns>A ViewModel instance of the given type</returns>
        public static TViewModel Resolve<TViewModel>(
            bool initialize = true)
            where TViewModel : class, IViewModel
        {
            try
            {
                var instance = iocContainer.Resolve<TViewModel>();

                if (initialize)
                    TaskHelper.RunSync(instance.InitializeAsync);

                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = $"ViewModel of type {typeof(TViewModel).Name} cannot be resolved --- is it registered?";

                log?.Critical(
                    errorMessage,
                    innerException);

                throw new TypeLoadException(
                    errorMessage,
                    innerException);
            }
        }

        /// <summary>
        /// Creates a new ViewModel instance and optionally invokes the <see cref="IViewModel.InitializeAsync"/> method.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the ViewModel to create</typeparam>
        /// <param name="initialize">Specifies if <see cref="IViewModel.InitializeAsync"/> should get called after instance creation.</param>
        /// <returns>A ViewModel instance of the given type</returns>
        public static async Task<TViewModel> ResolveAsync<TViewModel>(
            bool initialize = true)
            where TViewModel : class, IViewModel
        {
            try
            {
                var instance = iocContainer.Resolve<TViewModel>();

                if (initialize)
                    await instance.InitializeAsync();

                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = $"ViewModel of type {typeof(TViewModel).Name} cannot be resolved --- is it registered?";

                log?.Critical(
                    errorMessage,
                    innerException);

                throw new TypeLoadException(
                    errorMessage,
                    innerException);
            }
        }


        /// <summary>
        /// Creates a new ViewModel instance and invokes the <see cref="IViewModel{TModel}.InitializeAsync(TModel)"/> method.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the ViewModel to create</typeparam>
        /// <typeparam name="TModel">Type of the parameter that will be passed in the <see cref="IViewModel{TModel}.InitializeAsync(TModel)"/> method</typeparam>
        /// <returns>A ViewModel instance of the given type</returns>
        public static async Task<TViewModel> ResolveAsync<TViewModel, TModel>(
            TModel model)
            where TViewModel : class, IViewModel<TModel>
        {
            try
            {
                var instance = iocContainer.Resolve<TViewModel>();

                await instance.InitializeAsync(
                    model);

                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = $"ViewModel of type {typeof(TViewModel).Name} cannot be resolved --- is it registered?";

                log?.Critical(
                    errorMessage,
                    innerException);

                throw new TypeLoadException(
                    errorMessage,
                    innerException);
            }
        }


        /// <summary>
        /// Creates a new ViewModel instance and optionally invokes the <see cref="IViewModel.InitializeAsync"/> method.
        /// </summary>
        /// <param name="viewModelType">Type of the ViewModel to create</param>
        /// <param name="initialize">Specifies if <see cref="IViewModel.InitializeAsync"/> should get called after instance creation.</param>
        /// <returns>A ViewModel instance of the given type.</returns>
        public static IViewModel Resolve(
            Type viewModelType,
            bool initialize = true)
        {
            var interfaceType = typeof(IViewModel);

            try
            {
                if (!interfaceType.IsAssignableFrom(viewModelType))
                {
                    throw new InvalidOperationException(
                        $"The type {viewModelType.Name} must implement interface type {interfaceType.Name}");
                }

                var instance = iocContainer.Resolve(viewModelType) as IViewModel;

                if (initialize)
                    TaskHelper.RunSync(instance.InitializeAsync);

                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = $"ViewModel of type {interfaceType.Name} cannot be resolved --- is it registered?";

                log?.Critical(
                    errorMessage,
                    innerException);

                throw new TypeLoadException(
                    errorMessage,
                    innerException);
            }
        }   
        

        /// <summary>
        /// Creates a new ViewModel instance and optionally invokes the <see cref="IViewModel.InitializeAsync"/> method.
        /// </summary>
        /// <param name="viewModelType">Type of the ViewModel to create</param>
        /// <param name="initialize">Specifies if <see cref="IViewModel.InitializeAsync"/> should get called after instance creation.</param>
        /// <returns>A ViewModel instance of the given type</returns>
        public static async Task<IViewModel> ResolveAsync(
            Type viewModelType,
            bool initialize = true)
        {
            var interfaceType = typeof(IViewModel);

            try
            {
                if (!interfaceType.IsAssignableFrom(viewModelType))
                {
                    throw new InvalidOperationException(
                        $"The type {viewModelType.Name} must implement interface type {interfaceType.Name}");
                }

                var instance = iocContainer.Resolve(viewModelType) as IViewModel;

                if (initialize)
                    await instance.InitializeAsync();

                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = $"ViewModel of type {interfaceType.Name} cannot be resolved --- is it registered?";

                log?.Critical(
                    errorMessage,
                    innerException);

                throw new TypeLoadException(
                    errorMessage,
                    innerException);
            }
        }

        /// <summary>
        /// Creates a new ViewModel instance and invokes the <see cref="IViewModel{TModel}.InitializeAsync(TModel)"/> method.
        /// </summary>
        /// <typeparam name="TModel">Type of the parameter that will be passed in the <see cref="IViewModel{TModel}.InitializeAsync(TModel)"/> method</typeparam>
        /// <param name="viewModelType"></param>
        /// <param name="model">Parameter that will be passed in the <see cref="IViewModel{TModel}.InitializeAsync(TModel)"/> method</param>
        /// <returns>A ViewModel instance of the given type</returns>
        public static async Task<IViewModel<TModel>> ResolveAsync<TModel>(
            Type viewModelType,
            TModel model)
        {
            var interfaceType = typeof(IViewModel<TModel>);

            try
            {
                if (!interfaceType.IsAssignableFrom(viewModelType))
                {
                    throw new InvalidOperationException(
                        $"The type {viewModelType.Name} must implement interface type {interfaceType.Name}");
                }

                var instance = iocContainer.Resolve(viewModelType) as IViewModel<TModel>;

                await instance.InitializeAsync(
                    model);

                return instance;
            }
            catch (Exception innerException)
            {
                string errorMessage = $"ViewModel of type {interfaceType.Name} cannot be resolved --- is it registered?";

                log?.Critical(
                    errorMessage,
                    innerException);

                throw new TypeLoadException(
                    errorMessage,
                    innerException);
            }
        }
    }
}