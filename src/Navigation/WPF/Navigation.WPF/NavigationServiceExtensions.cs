using CodeMonkeys.MVVM;

using System;
using System.Windows;

using NavigationService = CodeMonkeys.Navigation.WPF.NavigationService;
using RegistrationInfo = CodeMonkeys.Navigation.WPF.RegistrationInfo;

namespace CodeMonkeys.Navigation
{
    public static class NavigationServiceExtensions
    {
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
        /// <typeparam name="TViewModel">ViewModel type to remove from registrations</typeparam>
        /// <param name="navigationService"></param>
        /// <returns>Navigation service instance</returns>
        public static INavigationService Unregister<TViewModel>(
            this INavigationService navigationService)

            where TViewModel : class, IViewModel
        {
            navigationService.Unregister<TViewModel>();
            return navigationService;
        }

        public static INavigationService Unregister<TViewModel, TView>(
            this INavigationService navigationService)

            where TViewModel : class, IViewModel
            where TView : FrameworkElement
        {
            navigationService.Unregister<TViewModel, TView>();
            return navigationService;
        }


        /// <summary>
        /// Adds the given condition to the registration.
        /// Every time a associated ViewModel is requested, the condition is evaluated before showing it.
        /// </summary>
        /// <param name="registration"></param>
        /// <param name="condition"><see cref="Func{TResult}" />Condition to evaluate before showing</param>
        /// <returns>Registration info</returns>
        public static RegistrationInfo WithCondition(
            this RegistrationInfo registration,
            Func<bool> condition)
        {
            registration.Condition = condition;

            return registration;
        }

        /// <summary>
        /// Indicates that the view from this registration should be opened in a new <see cref="System.Windows.Window" />
        /// </summary>
        /// <param name="registrationInfo"></param>
        /// <returns></returns>
        public static RegistrationInfo OpenInNewWindow(
            this RegistrationInfo registrationInfo)
        {
            throw new NotSupportedException(
                $"This functionality is not yet supported! We will provide it in a future release.");
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