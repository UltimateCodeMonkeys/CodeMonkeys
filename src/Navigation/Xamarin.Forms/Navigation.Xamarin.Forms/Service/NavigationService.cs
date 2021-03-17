﻿using CodeMonkeys.DependencyInjection;
using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;
using CodeMonkeys.Navigation.Xamarin.Forms.Pages;

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
        public void SetRoot<TViewModel>()

            where TViewModel : class, IViewModel
        {
            if (!TryGetRegistration(
                typeof(TViewModel),
                out var registration))
            {
                return;
            }


            var viewModelInstance = InitializeViewModelInternal<TViewModel>();

            var page = CreateView<TViewModel>(
                viewModelInstance);


            SetRootInternal(page);
        }

        public void SetRoot<TMasterViewModel, TDetailViewModel>()

            where TMasterViewModel : class, IViewModel
            where TDetailViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TMasterViewModel>();
            ThrowIfNotRegistered<TDetailViewModel>();


            var masterViewModel = InitializeViewModelInternal<TMasterViewModel>();
            var detailViewModel = dependencyResolver.Resolve<TDetailViewModel>();

            var masterPage = CreateViewInternal<TMasterViewModel, MasterDetailPage>(
                masterViewModel);

            var detailPage = CreateViewInternal<TDetailViewModel, DetailPage>(
                detailViewModel);


            _ = Task.Run(() => InitializeChildViewModel(
                masterViewModel,
                detailViewModel)).ConfigureAwait(false);


            if (typeof(TDetailViewModel).IsAssignableFrom(
                typeof(IHandleClosing)))
            {
                detailPage.Disappearing += OnViewClosing;
            }


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
                    ResolveBindingContextsForTabs(tabbedPage);
                    break;

                default:
                    page = new NavigationPage(page);
                    break;
            }

            Application.Current.MainPage = page;

            // prevent caching eventually old root pages, e.g. ContentPage for login, but afterwards switching to TabbedPage
            rootPage = null;
        }

        private void ResolveBindingContextsForTabs(
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

                page.BindingContext = viewModel;


                if (tabbedPage.BindingContext is IViewModel parentViewModel)
                {
                    _ = Task.Run(() => InitializeChildViewModel(
                        parentViewModel,
                        viewModel)).ConfigureAwait(false);
                }


                page.Disappearing += OnViewClosing;
            }
        }

        private void InitializeChildViewModel(
            IViewModel parentViewModel,
            IViewModel viewModel)
        {
            while (!parentViewModel.IsInitialized)
            {
                Task.Delay(100).ConfigureAwait(false);
            }


            _ = viewModel.InitializeAsync();
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