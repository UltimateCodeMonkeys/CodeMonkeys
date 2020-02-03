using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

using CodeMonkeys.Core.Helpers;
using CodeMonkeys.Core.Interfaces.MVVM;
using CodeMonkeys.Core.Interfaces.Navigation;
using CodeMonkeys.Navigation.Xamarin.Forms.Models;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class ViewModelNavigationService :
        IViewModelNavigationService
    {
        protected static readonly IList<INavigationRegistration> NavigationRegistrations =
            new List<INavigationRegistration>();


        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.Register(INavigationRegistration)" />
        public void Register(INavigationRegistration registrationInfo)
        {
            if (registrationInfo is NavigationRegistration xamarinRegistration)
            {

                if (xamarinRegistration.Platform != DevicePlatforms.All &&
                    Device.RuntimePlatform.ToDevicePlatform() != xamarinRegistration.Platform)
                {
                    return;
                }
            }

            RegisterInternal(registrationInfo);

            if (registrationInfo.PreCreateInstance)
            {
                Task.Run(() => CreateCachedPage(registrationInfo.ViewType));
            }

            LogService?.Info($"Registered ViewModel of type {registrationInfo.ViewModelInterfaceType.Name} to page {registrationInfo.ViewType.Name}.");
        }

        public INavigationRegistration Register<TViewModel, TView>()
            where TViewModel : IViewModel
            where TView : class
        {
            var registrationInfo = new NavigationRegistration
            {
                ViewModelInterfaceType = typeof(TViewModel),
                ViewType = typeof(TView),
                Platform = Device.RuntimePlatform.ToDevicePlatform(),
                PreCreateInstance = true
            };

            RegisterInternal(registrationInfo);

            if (registrationInfo.PreCreateInstance)
            {
                Task.Run(() => CreateCachedPage(registrationInfo.ViewType));
            }

            LogService?.Info($"Registered ViewModel of type {registrationInfo.ViewModelInterfaceType.Name} to page {registrationInfo.ViewType.Name}.");

            return registrationInfo;
        }


        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.Unregister{TViewModelInterface}" />
        public void Unregister<TViewModel>()

            where TViewModel : class, IViewModel
        {
            if (!IsRegistered(typeof(TViewModel)))
            {
                return;
            }

            var registrationInfo = NavigationRegistrations
                .First(registration => registration.ViewModelInterfaceType == typeof(TViewModel));

            var cachedPage = PageCache.FirstOrDefault(
                cache => cache.Type == registrationInfo.ViewType);

            if (cachedPage != null)
            {
                PageCache.Remove(cachedPage);
            }

            NavigationRegistrations.Remove(registrationInfo);

            LogService?.Info($"Unregistered views from ViewModel of type {typeof(TViewModel).Name}.");
        }


        // todo: do we need this functionality?
        internal static TViewModel RegisterView<TViewModel>(
            Page page)

            where TViewModel : class, IViewModel
        {
            var viewType = page.GetType();

            var registrationInfo = new NavigationRegistration
            {
                ViewModelInterfaceType = typeof(TViewModel),
                ViewType = viewType
            };

            RegisterInternal(registrationInfo);

            var cachedPage = new CachedPage(page);
            PageCache.Add(cachedPage);

            var viewModel = dependencyResolver.Resolve<TViewModel>();

            if (viewModel != null)
            {
                TaskHelper.RunSync(
                    viewModel.InitializeAsync);
            }

            return viewModel;
        }

        private static void RegisterInternal(INavigationRegistration registration)
        {
            _semaphore.Wait();

            try
            {
                if (TryGetRegistration(
                    registration.ViewModelInterfaceType,
                    registration.ViewType,
                    out var registrationInfo))
                {
                    NavigationRegistrations.Remove(registrationInfo);
                }

                NavigationRegistrations.Add(registration);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private static bool TryGetRegistration(
            Type viewModelInterfaceType,
            out INavigationRegistration registrationInfo)
        {
            if (!IsRegistered(viewModelInterfaceType))
            {
                registrationInfo = null;
                return false;
            }

            registrationInfo = NavigationRegistrations.OfType<NavigationRegistration>()
                .FirstOrDefault(registration =>
                    registration.ViewModelInterfaceType == viewModelInterfaceType &&
                    registration.Platform.ToXamarinPlatform() == Device.RuntimePlatform);

            return registrationInfo != null;
        }

        private static bool TryGetRegistration(
            Type viewModelInterfaceType,
            DevicePlatforms platform,
            out INavigationRegistration registrationInfo)
        {
            if (!IsRegistered(viewModelInterfaceType))
            {
                registrationInfo = null;
                return false;
            }

            registrationInfo = NavigationRegistrations.OfType<NavigationRegistration>()
                .FirstOrDefault(registration =>
                    registration.ViewModelInterfaceType == viewModelInterfaceType &&
                    registration.Platform == platform);

            return registrationInfo != null;
        }

        private static bool TryGetRegistration(
            Type viewModelInterfaceType,
            Type viewType,
            out INavigationRegistration registrationInfo)
        {
            if (!IsRegistered(viewModelInterfaceType, viewType))
            {
                registrationInfo = null;
                return false;
            }

            registrationInfo = NavigationRegistrations.FirstOrDefault(registration =>
                registration.ViewModelInterfaceType == viewModelInterfaceType &&
                registration.ViewType == viewType);

            return registrationInfo != null;
        }

        protected void ThrowIfNotRegistered<TViewModelInterface>(
            Type typeofView = null)
        {
            if (IsRegistered(typeof(TViewModelInterface), typeofView))
            {
                return;
            }

            // todo: which exception type fits best?
            var notRegisteredException = new InvalidOperationException(
                $"There is no reference from viewmodel type {typeof(TViewModelInterface).Name} to a page.");

            LogService?.Error(notRegisteredException);

            throw notRegisteredException;
        }


        private static bool IsRegistered(
            Type viewModelInterfaceType,
            Type typeofView = null)
        {
            var navigationRegistration = NavigationRegistrations.FirstOrDefault(
                registration => registration.ViewModelInterfaceType == viewModelInterfaceType);

            if (navigationRegistration == null)
            {
                return false;
            }

            if (typeofView != null && navigationRegistration.ViewType != typeofView)
            {
                return false;
            }

            return true;
        }
    }
}