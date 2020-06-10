using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class NavigationService
    {
        protected static readonly IList<INavigationRegistration> NavigationRegistrations =
            new List<INavigationRegistration>();


        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.Register(INavigationRegistration)" />
        public void Register(
            INavigationRegistration registrationInfo)
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

            Log?.Info($"Registered ViewModel of type {registrationInfo.ViewModelType.Name} to page {registrationInfo.ViewType.Name}.");
        }

        public INavigationRegistration Register<TViewModel, TView>()

            where TViewModel : IViewModel
            where TView : class
        {
            var registrationInfo = new NavigationRegistration
            {
                ViewModelType = typeof(TViewModel),
                ViewType = typeof(TView),
                Platform = Device.RuntimePlatform.ToDevicePlatform()
            };

            RegisterInternal(registrationInfo);

            if (registrationInfo.PreCreateInstance)
            {
                Task.Run(() => CreateCachedPage(registrationInfo.ViewType));
            }

            Log?.Info($"Registered ViewModel of type {registrationInfo.ViewModelType.Name} to page {registrationInfo.ViewType.Name}.");

            return registrationInfo;
        }


        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.Unregister{TViewModelInterface}" />
        public void Unregister<TViewModel>()

            where TViewModel : class, IViewModel
        {
            if (!IsRegistered(typeof(TViewModel)))
            {
                return;
            }

            var registrationInfo = NavigationRegistrations
                .First(registration => registration.ViewModelType == typeof(TViewModel));

            var cachedPage = PageCache.FirstOrDefault(
                cache => cache.Type == registrationInfo.ViewType);

            if (cachedPage != null)
            {
                PageCache.Remove(cachedPage);
            }

            NavigationRegistrations.Remove(registrationInfo);

            Log?.Info($"Unregistered views from ViewModel of type {typeof(TViewModel).Name}.");
        }


        public void ResetRegistrations()
        {
            NavigationRegistrations.Clear();
        }


        // todo: do we need this functionality?
        internal static TViewModel RegisterView<TViewModel>(
            Page page)

            where TViewModel : class, IViewModel
        {
            var viewType = page.GetType();

            var registrationInfo = new NavigationRegistration
            {
                ViewModelType = typeof(TViewModel),
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

        private static void RegisterInternal(
            INavigationRegistration registration)
        {
            _semaphore.Wait();

            try
            {
                if (TryGetRegistration(
                    registration.ViewModelType,
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
                    registration.ViewModelType == viewModelInterfaceType &&
                    registration.Platform.ToXamarinPlatform() == Device.RuntimePlatform);


            return registrationInfo != null;
        }
                

        private static bool TryGetRegistration(
            Type viewModelType,
            Type viewType,
            out INavigationRegistration registrationInfo)
        {
            if (!IsRegistered(viewModelType, viewType))
            {
                registrationInfo = null;
                return false;
            }


            registrationInfo = NavigationRegistrations.FirstOrDefault(registration =>
                registration.ViewModelType == viewModelType &&
                viewType.IsAssignableFrom(registration.ViewType) &&
                registration.Condition?.Invoke() != false);

            return registrationInfo != null;
        }


        private static bool TryGetRegisteredViewModelType(
            Type viewType,
            out INavigationRegistration registration)
        {
            registration = NavigationRegistrations.FirstOrDefault(
                registration => registration.ViewType == viewType);


            return registration != null;
        }


        internal static void ThrowIfNotRegistered<TViewModelInterface>(
            Type typeOfView = null)
        {
            if (IsRegistered(typeof(TViewModelInterface), typeOfView))
            {
                return;
            }

            // todo: which exception type fits best?
            var notRegisteredException = new InvalidOperationException(
                $"There is no reference from viewmodel type {typeof(TViewModelInterface).Name} to a page.");

            Log?.Error(notRegisteredException);

            throw notRegisteredException;
        }


        private static bool IsRegistered(
            Type viewModelInterfaceType,
            Type typeofView = null)
        {
            var navigationRegistration = NavigationRegistrations.FirstOrDefault(
                registration => registration.ViewModelType == viewModelInterfaceType);

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