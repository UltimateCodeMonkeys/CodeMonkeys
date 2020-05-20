using CodeMonkeys.MVVM;

using System;
using System.Threading.Tasks;
using System.Windows;

using NavigationService = CodeMonkeys.Navigation.WPF.NavigationService;
using RegistrationInfo = CodeMonkeys.Navigation.WPF.RegistrationInfo;

namespace CodeMonkeys.Navigation
{
    public static class NavigationServiceExtensions
    {
        public static async Task SetRootWindow<TViewModel>(
            this INavigationService service)

            where TViewModel : class, IViewModel
        {
            ThrowIfNavigationServiceIsOfWrongType(
                service,
                out var navigationService);


            var viewModel = await navigationService.InitializeViewModelInternal<TViewModel>();

            var window = navigationService.CreateContentInternal<TViewModel, Window>(
                viewModel);


            Application.Current.MainWindow = window;
            Application.Current.MainWindow.Show();
        }

        public static async Task SetRootWindow<TRootViewModel, TInitialViewModel>(
            this INavigationService service)

            where TRootViewModel : class, IViewModel
            where TInitialViewModel : class, IViewModel
        {
            ThrowIfNavigationServiceIsOfWrongType(
                service,
                out var navigationService);


            await SetRootWindow<TRootViewModel>(
                navigationService);

            await navigationService.SetRoot<TInitialViewModel>();

            navigationService.ClearStacks();
        }


        public static IViewModel GetCurrentViewModel(
            this INavigationService service)
        {
            ThrowIfNavigationServiceIsOfWrongType(
                service,
                out var navigationService);


            return navigationService.CurrentViewModel;
        }



        /// <summary>
        /// States wether it is possible to navigate backwards and <see cref="CodeMonkeys.Navigation.WPF.NavigationService.TryGoBack"/> can be successfully executed
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static bool CanGoBack(
            this INavigationService service)
        {
            ThrowIfNavigationServiceIsOfWrongType(
                service,
                out var navigationService);


            return navigationService.CanGoBack();
        }

        /// <summary>
        /// Tries to go back on navigation stack
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static bool TryGoBack(
            this INavigationService service)
        {
            ThrowIfNavigationServiceIsOfWrongType(
                service,
                out var navigationService);


            return navigationService.TryGoBack();
        }


        /// <summary>
        /// States wether it is possible to navigate backwards and <see cref="CodeMonkeys.Navigation.WPF.NavigationService.TryGoForward"/> can be successfully executed
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static bool CanGoForward(
            this INavigationService service)
        {
            ThrowIfNavigationServiceIsOfWrongType(
                service,
                out var navigationService);


            return navigationService.CanGoForward();
        }

        /// <summary>
        /// Tries to go forward on navigation stack
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static bool TryGoForward(
            this INavigationService service)
        {
            ThrowIfNavigationServiceIsOfWrongType(
                service,
                out var navigationService);


            return navigationService.TryGoForward();
        }


        /// <summary>
        /// Clears the stacks for backwards and forward navigation
        /// </summary>
        /// <param name="service"></param>
        public static void ClearStacks(
            this INavigationService service)
        {
            ThrowIfNavigationServiceIsOfWrongType(
                service,
                out var navigationService);


            navigationService.ClearStacks();
        }

        /// <summary>
        /// Clears the stack for backwards navigation
        /// </summary>
        /// <param name="service"></param>
        public static void ClearBackStack(
            this INavigationService service)
        {
            ThrowIfNavigationServiceIsOfWrongType(
                service,
                out var navigationService);


            navigationService.ClearBackStack();
        }

        /// <summary>
        /// Clears the stack for forward navigation
        /// </summary>
        /// <param name="service"></param>
        public static void ClearForwardStack(
            this INavigationService service)
        {
            ThrowIfNavigationServiceIsOfWrongType(
                service,
                out var navigationService);


            navigationService.ClearForwardStack();
        }



        /// <summary>
        /// Register a ViewModel interface to a specific view
        /// </summary>
        /// <typeparam name="TViewModel">Type of the ViewModel interface</typeparam>
        /// <typeparam name="TView"></typeparam>
        /// <param name="navigationService">Type of the associated view</param>
        /// <param name="preCreateInstance">Indicates wether an instance of the view should be created and cached before it is displayed</param>
        /// <returns><see cref="CodeMonkeys.Navigation.WPF.RegistrationInfo"/></returns>
        public static RegistrationInfo Register<TViewModel, TView>(
            this INavigationService navigationService,
            bool preCreateInstance = false)

            where TViewModel : class, IViewModel
            where TView : FrameworkElement
        {
            var registrationInfo = new RegistrationInfo
            {
                ViewModelType = typeof(TViewModel),
                ViewType = typeof(TView),
                PreCreateInstance = preCreateInstance
            };

            navigationService.Register(registrationInfo);

            return registrationInfo;
        }

        /// <summary>
        /// Register a ViewModel interface to a specific view
        /// </summary>
        /// <typeparam name="TViewModelInterface">Type of the ViewModel interface</typeparam>
        /// <param name="navigationService">Navigation service instance</param>
        /// <param name="typeOfView">Type of the associated view</param>
        /// <param name="preCreateInstance">Indicates wether an instance of the view should be created and cached before it is displayed</param>
        /// <returns><see cref="CodeMonkeys.Navigation.WPF.RegistrationInfo"/></returns>
        public static RegistrationInfo Register<TViewModelInterface>(
            this INavigationService navigationService,
            Type typeOfView,
            bool preCreateInstance = false)

            where TViewModelInterface : class, IViewModel
        {
            var navigationRegistration = new RegistrationInfo
            {
                ViewModelType = typeof(TViewModelInterface),
                ViewType = typeOfView,
                PreCreateInstance = preCreateInstance
            };

            navigationService.Register(navigationRegistration);

            return navigationRegistration;
        }

        /// <summary>
        /// Removes the registration associated with the given ViewModel interface type
        /// </summary>
        /// <typeparam name="TViewModelInterface">ViewModel type to remove from registrations</typeparam>
        /// <param name="navigationService"></param>
        /// <returns>Navigation service instance</returns>
        public static INavigationService Unregister<TViewModelInterface>(
            this INavigationService navigationService)

            where TViewModelInterface : class, IViewModel
        {
            navigationService.Unregister<TViewModelInterface>();
            return navigationService;
        }


        /// <summary>
        /// Indicates that the view from this registration should be opened in a new <see cref="System.Windows.Window" />
        /// </summary>
        /// <param name="registrationInfo"></param>
        /// <returns></returns>
        public static RegistrationInfo OpenInNewWindow(
            this RegistrationInfo registrationInfo)
        {
            registrationInfo.OpenInNewWindow = true;

            return registrationInfo;
        }

        /// <summary>
        /// Indicates that the view from this registration should be build and cached before it is actually shown
        /// Displaying those views will be a lot faster, however this consumes more memory
        /// </summary>
        /// <param name="registrationInfo"></param>
        /// <returns></returns>
        public static RegistrationInfo Prebuild(
            this RegistrationInfo registrationInfo)
        {
            registrationInfo.PreCreateInstance = true;

            return registrationInfo;
        }



        private static void ThrowIfNavigationServiceIsOfWrongType(
            INavigationService service,
            out NavigationService navigationService)
        {
            if (!(service is NavigationService wpfService))
            {
                throw new InvalidOperationException(
                    $"This extension method can only be used with {nameof(NavigationService)} of type {typeof(NavigationService).FullName}!");
            }

            navigationService = wpfService;
        }
    }
}