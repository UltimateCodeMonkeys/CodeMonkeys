using CodeMonkeys.MVVM;
using CodeMonkeys.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CodeMonkeys.Navigation.WPF
{
    public partial class NavigationService
    {
        protected static readonly IList<INavigationRegistration> NavigationRegistrations =
            new List<INavigationRegistration>();


        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.Register(INavigationRegistration)" />
        public void Register(INavigationRegistration registrationInfo)
        {
            RegisterInternal(registrationInfo);

            if (registrationInfo.PreCreateInstance)
            {
                Task.Run(() => CreateCachedContent(registrationInfo.ViewType));
            }

            Log?.Info($"Registered ViewModel of type {registrationInfo.ViewModelType.Name} to page {registrationInfo.ViewType.Name}.");
        }

        public INavigationRegistration Register<TViewModel, TView>()
            where TViewModel : IViewModel
            where TView : class
        {
            var registrationInfo = new RegistrationInfo
            {
                ViewModelType = typeof(TViewModel),
                ViewType = typeof(TView)
            };

            RegisterInternal(registrationInfo);

            if (registrationInfo.PreCreateInstance)
            {
                Task.Run(() => CreateCachedContent(registrationInfo.ViewType));
            }

            Log?.Info($"Registered ViewModel of type {registrationInfo.ViewModelType.Name} to page {registrationInfo.ViewType.Name}.");

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
                .First(registration => registration.ViewModelType == typeof(TViewModel));

            var cachedPage = ContentCache.FirstOrDefault(
                cache => cache.Type == registrationInfo.ViewType);

            if (cachedPage != null)
            {
                ContentCache.Remove(cachedPage);
            }

            NavigationRegistrations.Remove(registrationInfo);

            Log?.Info($"Unregistered views from ViewModel of type {typeof(TViewModel).Name}.");
        }
                

        private static void RegisterInternal(INavigationRegistration registration)
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

        internal static bool TryGetRegistration(
            Type viewModelInterfaceType,
            out INavigationRegistration registrationInfo)
        {
            if (!IsRegistered(viewModelInterfaceType))
            {
                registrationInfo = null;
                return false;
            }

            registrationInfo = NavigationRegistrations.OfType<RegistrationInfo>()
                .FirstOrDefault(registration =>
                    registration.ViewModelType == viewModelInterfaceType);

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
                viewType.IsAssignableFrom(registration.ViewType));

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

            Log?.Error(notRegisteredException);

            throw notRegisteredException;
        }


        private static bool IsRegistered(
            Type viewModelType,
            Type typeOfView = null)
        {
            var navigationRegistration = NavigationRegistrations.FirstOrDefault(
                registration => registration.ViewModelType == viewModelType);

            if (navigationRegistration == null)
            {
                return false;
            }

            if (typeOfView != null && navigationRegistration.ViewType
                .IsAssignableFrom(typeOfView))
            {
                return false;
            }

            return true;
        }
    }
}