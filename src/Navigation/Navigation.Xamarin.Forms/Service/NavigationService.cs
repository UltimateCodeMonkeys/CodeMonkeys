using CodeMonkeys.DependencyInjection;
using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class NavigationService :
        INavigationService
    {
        public event EventHandler<IViewModel> CurrentViewModelChanged;



        private static readonly SemaphoreSlim _semaphore =
            new SemaphoreSlim(1, 1);

        private static IDependencyResolver dependencyResolver;
        protected static ILogService LogService;              


        public static Configuration Configuration { get; set; } =
            new Configuration();



        private Page rootPage;
        protected Page RootPage
        {
            get
            {
                if (rootPage != null)
                {
                    return rootPage;
                }


                switch (Application.Current.MainPage)
                {
                    case NavigationPage navigationPage:
                        rootPage = navigationPage.RootPage;
                        break;

                    case TabbedPage tabbedPage:
                        rootPage = tabbedPage;
                        break;

                    case MasterDetailPage masterDetailPage:
                        rootPage = masterDetailPage;
                        break;

                    default:
                        rootPage = Application.Current.MainPage;
                        break;
                }


                return rootPage;
            }
        }

        protected INavigation Navigation
        {
            get
            {
                switch (RootPage)
                {
                    case MasterDetailPage masterDetail:
                        return masterDetail.Detail.Navigation;

                    case TabbedPage tabbed:
                        return tabbed.Navigation;

                    default:
                        return RootPage.Navigation;
                }
            }
        }

        protected Page CurrentPage => Navigation?.NavigationStack.Last();


        public IReadOnlyCollection<INavigationRegistration> Registrations =>
            new ReadOnlyCollection<INavigationRegistration>(NavigationRegistrations);

        public NavigationService(
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


        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.SetRoot{TViewModelInterface}()" />
        public async Task SetRoot<TViewModel>()

            where TViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TViewModel>();


            var viewModelInstance = await InitializeViewModelAsync<TViewModel>();

            var page = CreateView<TViewModel>(
                viewModelInstance);


            SetRootInternal(page);
        }

        internal void SetRootInternal(
            Page page)
        {
            switch (page)
            {
                case NavigationPage _:
                case MasterDetailPage _:
                    break;

                case TabbedPage tabbedPage:
                    page = new NavigationPage(page);
                    _ = ResolveBindingContextsForTabs(tabbedPage);
                    break;

                default:
                    page = new NavigationPage(page);
                    break;
            }

            Application.Current.MainPage = page;

            // prevent caching eventually old root pages, e.g. ContentPage for login, but afterwards switching to TabbedPage
            rootPage = null;
        }

        private async Task ResolveBindingContextsForTabs(
            TabbedPage tabbedPage)
        {
            foreach (var child in tabbedPage.Children)
            {
                Page page;

                if (child is NavigationPage navigationPage)
                {
                    page = navigationPage.RootPage;
                }
                else page = child;


                if (!TryGetRegisteredViewModelType(
                    page.GetType(),
                    out var registration))
                {
                    continue;
                }


                var viewModel = dependencyResolver.Resolve<IViewModel>(
                    registration.ViewModelType);

                await viewModel.InitializeAsync();


                page.BindingContext = viewModel;
            }
        }

        private bool IsRoot<TDestinationViewModel>()
            where TDestinationViewModel : class, IViewModel
        {
            if (!IsRegistered(typeof(TDestinationViewModel)))
            {
                return false;
            }

            var registrationInfo = NavigationRegistrations.First(
                registration => registration.ViewModelType == typeof(TDestinationViewModel));

            return registrationInfo?.ViewType == RootPage.GetType();
        }

        private async Task PopToRootAsync()
        {
            await Navigation.PopToRootAsync(
                animated: Configuration.UseAnimations);

            LogService?.Info(
                $"Navigation stack cleared.");
        }
        

        private void RaiseCurrentViewModelChanged(
            IViewModel current)
        {
            var threadSafeCall = CurrentViewModelChanged;

            threadSafeCall?.Invoke(
                this,
                current);
        }


        #region View Disappearing event
        private async void OnViewClosing(
            object sender,
            EventArgs eventArgs)
        {
            if (!(sender is Page page))
            {
                return;
            }

            if (Registrations == null ||
                !Registrations.Any(
                    registration => registration.ViewType == page.GetType()))
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
            Log = logService;
        }
    }
}