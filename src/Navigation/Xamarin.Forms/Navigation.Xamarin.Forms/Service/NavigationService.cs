using CodeMonkeys.DependencyInjection;
using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;
using CodeMonkeys.Navigation.Xamarin.Forms.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class NavigationService :
        Navigation.Xamarin.Forms.INavigationService
    {
        private static readonly SemaphoreSlim _semaphore =
            new SemaphoreSlim(1, 1);

        private static IDependencyContainer dependencyResolver;
        protected static ILogService Log;              


        public static NavigationServiceOptions Configuration { get; set; } =
            new NavigationServiceOptions();



        private Page rootPage;
        protected Page RootPage
        {
            get
            {
                if (rootPage != null)
                {
                    return rootPage;
                }


                rootPage = Application.Current.MainPage switch
                {
                    NavigationPage navigationPage => navigationPage.RootPage,
                    TabbedPage tabbedPage => tabbedPage,
                    MasterDetailPage masterDetailPage => masterDetailPage,
                    _ => Application.Current.MainPage,
                };


                return rootPage;
            }
        }

        protected INavigation Navigation
        {
            get
            {
                return CurrentPage switch
                {
                    MasterDetailPage masterDetail => masterDetail.Detail.Navigation,
                    TabbedPage tabbed => tabbed.Navigation,
                    _ => CurrentPage.Navigation,
                };
            }
        }

        protected Page CurrentPage
        {
            get
            {
                //var current = Navigation?.NavigationStack?.LastOrDefault() ?? RootPage;
                var current = RootPage?.Navigation?.NavigationStack?.LastOrDefault() ?? RootPage;

                return current switch
                {
                    MasterDetailPage masterDetail => masterDetail.Detail,
                    TabbedPage tabbed => tabbed.CurrentPage,
                    _ => current,
                };
            }
        }


        public IReadOnlyCollection<INavigationRegistration> Registrations =>
            new ReadOnlyCollection<INavigationRegistration>(NavigationRegistrations);

        public NavigationService(
            IDependencyContainer resolver)
        {
            SetResolverInstance(
                resolver);
        }

        private void SetResolverInstance(
            IDependencyContainer resolver)
        {
            try
            {
                _semaphore.Wait();

                dependencyResolver ??= resolver;
            }
            finally
            {
                _semaphore.Release();
            }
        }


        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.SetRoot{TViewModelInterface}()" />
        public async Task SetRootAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TViewModel>();


            var viewModelInstance = await InitializeViewModelAsync<TViewModel>();

            var page = CreateView<TViewModel>(
                viewModelInstance);


            SetRootInternal(page);
        }

        public async Task SetRootAsync<TMasterViewModel, TDetailViewModel>()

            where TMasterViewModel : class, IViewModel
            where TDetailViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TMasterViewModel>();
            ThrowIfNotRegistered<TDetailViewModel>();


            var masterViewModel = await InitializeViewModelInternal<TMasterViewModel>();
            var detailViewModel = await InitializeViewModelInternal<TDetailViewModel>();


            var masterPage = CreateViewInternal<TMasterViewModel, MasterDetailPage>(
                masterViewModel);

            var detailPage = CreateViewInternal<TDetailViewModel, DetailPage>(
                detailViewModel);


            masterPage.Detail = new NavigationPage(detailPage);
            SetRootInternal(masterPage);
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
                else
                {
                    page = child;
                }


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
                page.Disappearing += OnViewClosing;
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


        public static void SetupLogging(
            ILogService logService)
        {
            _semaphore.Wait();

            Log = logService;

            _semaphore.Release();
        }
    }
}