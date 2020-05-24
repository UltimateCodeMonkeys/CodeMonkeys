using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.Xamarin.Forms.Pages;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public static class NavigationServiceExtensions
    {
        public static async Task SetRoot<TMasterViewModel, TDetailViewModel>(
            this INavigationService navigationService)

            where TMasterViewModel : class, IViewModel
            where TDetailViewModel : class, IViewModel
        {
            if (!(navigationService is NavigationService service))
                throw new InvalidOperationException($"Extension can only be used with service type '{typeof(NavigationService).FullName}'");


            NavigationService.ThrowIfNotRegistered<TMasterViewModel>();
            NavigationService.ThrowIfNotRegistered<TDetailViewModel>();


            var masterViewModel = await service.InitializeViewModelInternal<TMasterViewModel>();
            var detailViewModel = await service.InitializeViewModelInternal<TDetailViewModel>();


            var masterPage = service.CreateViewInternal<TMasterViewModel, MasterDetailPage>(
                masterViewModel);

            var detailPage = service.CreateViewInternal<TDetailViewModel, DetailPage>(
                detailViewModel);


            masterPage.Detail = new NavigationPage(detailPage);
            service.SetRootInternal(masterPage);
        }


        /// <summary>
        /// Closes all open pages and goes back to the application's root page
        /// </summary>
        public static async Task CloseAllAsync(
            this INavigationService navigationService)
        {
            if (!(navigationService is NavigationService service))
            {
                throw new InvalidOperationException(
                    $"This extension method can only be used with {nameof(NavigationService)} of type {typeof(NavigationService).FullName}!");
            }


            await service.CloseAllAsync();
        }



        /// <summary>
        /// Register a ViewModel interface to a specific view
        /// </summary>
        /// <typeparam name="TViewModelInterface">Type of the ViewModel interface</typeparam>
        /// <typeparam name="TView"></typeparam>
        /// <param name="navigationService">Type of the associated view</param>
        /// <param name="preCreateInstance">Indicates wether an instance of the view should be created and cached before it is displayed</param>
        /// <returns><see cref="CodeMonkeys.Navigation.Xamarin.Forms.Models.NavigationRegistration"/></returns>
        public static NavigationRegistration Register<TViewModelInterface, TView>(
            this INavigationService navigationService)

            where TViewModelInterface : class, IViewModel
            where TView : Page
        {
            var registrationInfo = new NavigationRegistration<TViewModelInterface, TView>();

            navigationService.Register(registrationInfo);

            return registrationInfo;
        }

        /// <summary>
        /// Register a ViewModel interface to a specific view separated by phone and tablet
        /// </summary>
        /// <typeparam name="TViewModelInterface">Type of the ViewModel interface</typeparam>
        /// <typeparam name="TPhoneView">View type to use on phones</typeparam>
        /// <typeparam name="TTabletView">View type to use on tables</typeparam>
        /// <param name="navigationService">Type of the associated view</param>
        /// <param name="preCreateInstance">Indicates wether an instance of the view should be created and cached before it is displayed</param>
        /// <returns><see cref="CodeMonkeys.Navigation.Xamarin.Forms.Models.NavigationRegistration"/></returns>
        public static NavigationRegistration Register<TViewModelInterface, TPhoneView, TTabletView>(
            this INavigationService navigationService)

            where TViewModelInterface : class, IViewModel
            where TPhoneView : Page
            where TTabletView : Page
        {
            var registrationInfo = new NavigationRegistration<TViewModelInterface, TPhoneView, TTabletView>();

            navigationService.Register(registrationInfo);

            return registrationInfo;
        }

        /// <summary>
        /// Register a ViewModel interface to a specific view separated by phone and tablet
        /// </summary>
        /// <typeparam name="TViewModelInterface">Type of the ViewModel interface</typeparam>
        /// <typeparam name="TPhoneView">View type to use on phones</typeparam>
        /// <typeparam name="TTabletView">View type to use on tables</typeparam>
        /// /// <typeparam name="TDesktopView">View type to use on desktops</typeparam>
        /// <param name="navigationService">Type of the associated view</param>
        /// <param name="preCreateInstance">Indicates wether an instance of the view should be created and cached before it is displayed</param>
        /// <returns><see cref="CodeMonkeys.Navigation.Xamarin.Forms.Models.NavigationRegistration"/></returns>
        public static NavigationRegistration Register<TViewModelInterface, TPhoneView, TTabletView, TDesktopView>(
            this INavigationService navigationService)

            where TViewModelInterface : class, IViewModel
            where TPhoneView : Page
            where TTabletView : Page
            where TDesktopView : Page
        {
            var registrationInfo = new NavigationRegistration<TViewModelInterface, TPhoneView, TTabletView, TDesktopView>();

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
        /// <returns><see cref="CodeMonkeys.Navigation.Xamarin.Forms.Models.NavigationRegistration"/></returns>
        public static NavigationRegistration Register<TViewModelInterface>(
            this INavigationService navigationService,
            Type typeOfView)

            where TViewModelInterface : class, IViewModel
        {
            var navigationRegistration = new NavigationRegistration
            {
                ViewModelType = typeof(TViewModelInterface),
                ViewType = typeOfView
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
        /// States that the specific registration is only relevant for the given platform(s)
        /// </summary>
        /// <param name="registrationInfo"></param>
        /// <param name="platform">Xamarin.Forms platform to register</param>
        /// <returns>Registration info</returns>
        public static NavigationRegistration OnPlatform(
            this NavigationRegistration registrationInfo,
            string platform)
        {
            var devicePlatform = platform.ToDevicePlatform();

            registrationInfo.Platform = devicePlatform;

            return registrationInfo;
        }

        /// <summary>
        /// States that the specific registration is only relevant for the given platform(s)
        /// </summary>
        /// <param name="registrationInfo"></param>
        /// <param name="platform"><see cref="CodeMonkeys.Navigation.Xamarin.Forms.DevicePlatforms"/> to register</param>
        /// <returns>Registration info</returns>
        public static NavigationRegistration OnPlatform(
            this NavigationRegistration registrationInfo,
            DevicePlatforms platform)
        {
            registrationInfo.Platform = platform;

            return registrationInfo;
        }

        /// <summary>
        /// Indicates that the view from this registration should be build and cached before it is actually shown
        /// Displaying those views will be a lot faster, however this consumes more memory
        /// </summary>
        /// <param name="registrationInfo"></param>
        /// <returns></returns>
        public static NavigationRegistration Prebuild(
            this NavigationRegistration registrationInfo)
        {
            registrationInfo.PreCreateInstance = true;

            return registrationInfo;
        }
    }
}