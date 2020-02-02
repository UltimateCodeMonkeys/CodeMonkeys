using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;

using CodeMonkeys.Core.Helpers;
using CodeMonkeys.Core.Interfaces.DependencyInjection;
using CodeMonkeys.Core.Interfaces.MVVM;
using CodeMonkeys.Core.Interfaces.Logging;
using CodeMonkeys.Core.Interfaces.Navigation;
using CodeMonkeys.Core.Interfaces.Navigation.ViewModels;
using CodeMonkeys.Navigation.Xamarin.Forms.Models;

using Activator = CodeMonkeys.Core.Helpers.Activator;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class ViewModelNavigationService :
        IViewModelNavigationService
    {
        private static readonly SemaphoreSlim _semaphore =
            new SemaphoreSlim(1, 1);

        private static IDependencyResolver dependencyResolver;
        protected static ILogService LogService;


        protected static readonly IList<INavigationRegistration> NavigationRegistrations =
            new List<INavigationRegistration>();

        protected static readonly ConcurrentDictionary<Type, Type> ViewModelToViewMap =
            new ConcurrentDictionary<Type, Type>();

        private static readonly IList<CachedPage> PageCache =
            new List<CachedPage>();


        public static NavigationConfiguration Configuration { get; set; } =
            new NavigationConfiguration();


        private Page rootPage;
        protected Page RootPage
        {
            get
            {
                if (rootPage != null)
                {
                    return rootPage;
                }

                rootPage = Application.Current.MainPage;

                if (Application.Current.MainPage is NavigationPage navigationPage)
                {
                    rootPage = navigationPage.RootPage;
                }

                return rootPage;
            }
        }

        protected INavigation Navigation => RootPage.Navigation;

        protected Page CurrentPage => Navigation?.NavigationStack.Last();


        public IReadOnlyCollection<INavigationRegistration> Registrations =>
            new ReadOnlyCollection<INavigationRegistration>(NavigationRegistrations);

        public ViewModelNavigationService(
            IDependencyResolver resolver)
        {
            SetResolverInstance(
                resolver);
        }


        private void SetResolverInstance(
            IDependencyResolver resolver)
        {
            if (dependencyResolver != null)
            {
                return;
            }

            _semaphore.Wait();

            if (dependencyResolver == null)
            {
                dependencyResolver = resolver;
            }

            _semaphore.Release();
        }


        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.SetRoot{TViewModelInterface}(TViewModelInterface)" />
        public void SetRoot<TViewModel>(
            TViewModel dataContext)

            where TViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TViewModel>();

            var page = CreateViewInstance<TViewModel>(
                dataContext);

            Application.Current.MainPage = new NavigationPage(
                page);
        }        

        private bool IsRoot<TDestinationViewModel>()
            where TDestinationViewModel : class, IViewModel
        {
            if (!IsRegistered(typeof(TDestinationViewModel)))
            {
                return false;
            }

            var registrationInfo = NavigationRegistrations.First(
                registration => registration.ViewModelInterfaceType == typeof(TDestinationViewModel));

            return registrationInfo?.ViewType == RootPage.GetType();
        }

        private async Task PopToRootAsync()
        {
            await Navigation.PopToRootAsync(
                animated: Configuration.UseAnimations);

            LogService?.Info(
                $"Navigation stack cleared.");
        }

        #region Helper methods
        protected async Task<TViewModelInterface> PrepareToShowViewModel<TViewModelInterface>()

            where TViewModelInterface : class, IViewModel
        {
            //CheckIfViewModelTypeIsRegisteredToView<TViewModelInterface>();

            var viewModelInstance = dependencyResolver.Resolve<TViewModelInterface>();
            await viewModelInstance.InitializeAsync();

            LogService?.Info(
                $"ViewModel viewModel of type {typeof(TViewModelInterface).Name} has been created and initialized!");

            return viewModelInstance;
        }

        protected async Task<TViewModelInterface> PrepareToShowViewModel<TViewModelInterface, TModel>(
            TModel model)

            where TViewModelInterface : class, IViewModel<TModel>
        {
            //CheckIfViewModelTypeIsRegisteredToView<TViewModelInterface>();

            var viewModelInstance = dependencyResolver.Resolve<TViewModelInterface>();
            await viewModelInstance.InitializeAsync(
                model);

            LogService?.Info(
                $"ViewModel viewModel of type {typeof(TViewModelInterface).Name} has been created and initialized with parameters!");

            return viewModelInstance;
        }


        protected Page CreateViewInstance<TViewModelInterface>(
            TViewModelInterface viewModel)

            where TViewModelInterface : class, IViewModel
        {
            if (!TryGetRegistration(
                typeof(TViewModelInterface),
                out var registrationInfo))
            {
                // todo: set exception message
                throw new InvalidOperationException();
            }

            if (PageCache.All(cachedPage => cachedPage.Type != registrationInfo.ViewType))
            {
                CreateCachedPage(registrationInfo.ViewType);
            }

            var reference = PageCache
                .First(cachedPage => cachedPage.Type == registrationInfo.ViewType)
                .Reference;

            if (!reference.TryGetTarget(out var viewInstance))
            {
                var pageInstance = (Page)Activator.CreateInstance(
                    registrationInfo.ViewType);

                viewInstance = pageInstance;
                reference.SetTarget(pageInstance);
            }

            viewInstance.BindingContext = viewModel;

            if (viewModel is IHandleClosing)
            {
                viewInstance.Disappearing += OnViewClosing;
            }

            LogService?.Info(
                $"View of type {viewInstance.GetType().Name} has been created!");

            return viewInstance;
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

        private async Task ResolveAndInformParent<TParentViewModelInterface>()

            where TParentViewModelInterface : class, IListenToChildViewModelClosing
        {
            var parentViewModel = dependencyResolver.Resolve<TParentViewModelInterface>();

            LogService?.Info(
                $"ViewModelInstance for type {typeof(TParentViewModelInterface).Name} has been resolved.");

            await parentViewModel.OnChildViewModelClosingAsync();
        }

        private async Task ResolveAndInformParent<TParentViewModelInterface, TResult>(
            TResult result)

            where TParentViewModelInterface : class, IListenToChildViewModelClosing<TResult>
        {
            var parentViewModel = dependencyResolver.Resolve<TParentViewModelInterface>();

            LogService?.Info(
                $"ViewModelInstance for type {typeof(TParentViewModelInterface).Name} has been resolved.");

            await parentViewModel.OnChildViewModelClosingAsync(
                result);
        }
        #endregion


        #region Cache
        /// <inheritdoc cref="IViewModelNavigationService.ClearCache" />
        public void ClearCache()
        {
            LogService?.Info(
                $"{PageCache.Count} cached pages removed.");

            PageCache.Clear();
        }

        private void CreateCachedPage(
            Type pageType)
        {
            if (!Configuration.CachePageInstances)
            {
                return;
            }

            if (Configuration.PageTypesToExcludeFromCaching
                .Contains(pageType))
            {
                return;
            }

            var pageInstance = (Page)Activator.CreateInstance(
                pageType);

            var cachedPage = new CachedPage(pageInstance);
            PageCache.Add(cachedPage);
        }
        #endregion


        #region Registration
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

        public void Register(INavigationRegistration registrationInfo)
        {
            if (registrationInfo is NavigationRegistration xamarinRegistration)
            {

                if (xamarinRegistration.Platform != DevicePlatform.All &&
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

        /// <inheritdoc cref="IViewModelNavigationService" />
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
            DevicePlatform platform,
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
        #endregion


        #region View Disappearing event
        private async void OnViewClosing(
            object sender,
            EventArgs eventArgs)
        {
            if (!(sender is Page page))
            {
                return;
            }

            if (ViewModelToViewMap == null ||
                !ViewModelToViewMap.Any())
            {
                DetachDisappearingEventListener(page);
                return;
            }

            if (!ViewModelToViewMap.Values.Any())
            {
                DetachDisappearingEventListener(page);
                return;
            }

            if (!ViewModelToViewMap.Values.Contains(
                page.GetType()))
            {
                DetachDisappearingEventListener(page);
                return;
            }

            if (!(page.BindingContext is IHandleClosing viewModel))
            {
                DetachDisappearingEventListener(page);
                return;
            }

            await viewModel.OnClosing();

            DetachDisappearingEventListener(page);
        }


        private void DetachDisappearingEventListener(
            Page closedPage)
        {
            if (closedPage == null)
            {
                return;
            }

            closedPage.Disappearing -= OnViewClosing;
        }
        #endregion



        public static void SetupLogging(
            ILogService logService)
        {
            LogService = logService;
        }
    }
}