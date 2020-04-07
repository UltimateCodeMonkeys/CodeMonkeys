﻿using CodeMonkeys.Core.Navigation;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.Xamarin.Forms.Models;

using System;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public static class ViewModelNavigationServiceExtensions
    {
        /// <summary>
        /// Register a ViewModel interface to a specific view
        /// </summary>
        /// <typeparam name="TViewModelInterface">Type of the ViewModel interface</typeparam>
        /// <typeparam name="TView"></typeparam>
        /// <param name="navigationService">Type of the associated view</param>
        /// <param name="preCreateInstance">Indicatesd wether an instance of the view should be created and cached before it is displayed</param>
        /// <returns><see cref="CodeMonkeys.Navigation.Xamarin.Forms.Models.NavigationRegistration"/></returns>
        public static NavigationRegistration Register<TViewModelInterface, TView>(
            this IViewModelNavigationService navigationService,
            bool preCreateInstance = true)

            where TViewModelInterface : class, IViewModel
            where TView : Page
        {
            var registrationInfo = new NavigationRegistration<TViewModelInterface, TView>
            {
                PreCreateInstance = preCreateInstance
            };

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
        /// <param name="preCreateInstance">Indicatesd wether an instance of the view should be created and cached before it is displayed</param>
        /// <returns><see cref="CodeMonkeys.Navigation.Xamarin.Forms.Models.NavigationRegistration"/></returns>
        public static NavigationRegistration Register<TViewModelInterface, TPhoneView, TTabletView>(
            this IViewModelNavigationService navigationService,
            bool preCreateInstance = true)

            where TViewModelInterface : class, IViewModel
            where TPhoneView : Page
            where TTabletView : Page
        {
            var registrationInfo = new NavigationRegistration<TViewModelInterface, TPhoneView, TTabletView>
            {
                PreCreateInstance = preCreateInstance
            };

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
        /// <param name="preCreateInstance">Indicatesd wether an instance of the view should be created and cached before it is displayed</param>
        /// <returns><see cref="CodeMonkeys.Navigation.Xamarin.Forms.Models.NavigationRegistration"/></returns>
        public static NavigationRegistration Register<TViewModelInterface, TPhoneView, TTabletView, TDesktopView>(
            this IViewModelNavigationService navigationService,
            bool preCreateInstance = true)

            where TViewModelInterface : class, IViewModel
            where TPhoneView : Page
            where TTabletView : Page
            where TDesktopView : Page
        {
            var registrationInfo = new NavigationRegistration<TViewModelInterface, TPhoneView, TTabletView, TDesktopView>
            {
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
        /// <param name="preCreateInstance">Indicatesd wether an instance of the view should be created and cached before it is displayed</param>
        /// <returns><see cref="CodeMonkeys.Navigation.Xamarin.Forms.Models.NavigationRegistration"/></returns>
        public static NavigationRegistration Register<TViewModelInterface>(
            this IViewModelNavigationService navigationService,
            Type typeOfView,
            bool preCreateInstance = true)

            where TViewModelInterface : class, IViewModel
        {
            var navigationRegistration = new NavigationRegistration
            {
                ViewModelInterfaceType = typeof(TViewModelInterface),
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
        public static IViewModelNavigationService Unregister<TViewModelInterface>(
            this IViewModelNavigationService navigationService)

            where TViewModelInterface : class, IViewModel
        {
            navigationService.Unregister<TViewModelInterface>();
            return navigationService;
        }

        /// <summary>
        /// Sets the root page of the current application
        /// This is the page that is shown on first at startup (after splashscreen)
        /// </summary>
        /// <typeparam name="TViewModelInterface"></typeparam>
        /// <param name="navigationService"></param>
        /// <param name="dataContext">The associated ViewModel instance</param>
        /// <returns></returns>
        public static IViewModelNavigationService SetRoot<TViewModelInterface>(
            this IViewModelNavigationService navigationService,
            TViewModelInterface dataContext)

            where TViewModelInterface : class, IViewModel
        {
            navigationService.SetRoot(dataContext);
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