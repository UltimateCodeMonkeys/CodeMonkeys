using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;
using CodeMonkeys.Navigation.Xamarin.Forms.Pages;

using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class NavigationService
    {

        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.ShowAsync{TViewModelInterface}" />
        public virtual async Task ShowAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            if (!TryGetRegistration(
                typeof(TViewModel),
                out var registration))
            {
                ThrowNotRegisteredException<TViewModel>();
            }


            if (IsRoot<TViewModel>())
            {
                await PopToRootAsync()
                    .ConfigureAwait(false);
            }
            else if (IsTab(
                registration.ViewType))
            {
                await SetTabAsync(registration.ViewType)
                    .ConfigureAwait(false);
            }
            else
            {
                var viewModelInstance = await InitializeViewModelAsync<TViewModel>()
                    .ConfigureAwait(false);

                var page = CreateView<TViewModel>(
                    viewModelInstance);


                if (IsDetail(
                    registration.ViewType))
                {
                    await SetDetailAsync(page)
                        .ConfigureAwait(false);
                }
                else
                {
                    await PushAsync(page)
                        .ConfigureAwait(false);
                }
            }
        }

        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.ShowAsync{TViewModelInterface, TModel}(TModel)" />
        public virtual async Task ShowAsync<TViewModel, TData>(
            TData data)

            where TViewModel : class, IViewModel<TData>
        {
            if (!TryGetRegistration(
                typeof(TViewModel),
                out var registration))
            {
                ThrowNotRegisteredException<TViewModel>();
            }


            if (IsRoot<TViewModel>())
            {
                await PopToRootAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                var viewModelInstance = await InitializeViewModelAsync<TViewModel>()
                    .ConfigureAwait(false);

                var page = CreateView<TViewModel>(
                    viewModelInstance);


                if (IsDetail(
                    registration.ViewType))
                {
                    await SetDetailAsync(page)
                        .ConfigureAwait(false);
                }
                else
                {
                    await PushAsync(page)
                        .ConfigureAwait(false);
                }
            }
        }


        /// <inheritdoc cref="INavigationService.ShowModalAsync{TViewModel}" />
        public virtual async Task ShowModalAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TViewModel>();


            var viewModelInstance =
                await InitializeViewModelAsync<TViewModel>()
                .ConfigureAwait(false);

            var page = CreateView<TViewModel>(
                viewModelInstance);


            await PushModalAsync(page)
                .ConfigureAwait(false);
        }


        /// <inheritdoc cref="INavigationService.ShowModalAsync{TViewModel, TData}(TData)" />
        public virtual async Task ShowModalAsync<TViewModel, TData>(
            TData data)

            where TViewModel : class, IViewModel<TData>
        {
            ThrowIfNotRegistered<TViewModel>();


            var viewModelInstance =
                await InitializeViewModelAsync<TViewModel, TData>(data)
                .ConfigureAwait(false);

            var page = CreateView<TViewModel>(
                viewModelInstance);


            await PushModalAsync(page)
                .ConfigureAwait(false);
        }


        protected async Task<TViewModel> InitializeViewModelAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            return await InitializeViewModelInternal<TViewModel>()
                .ConfigureAwait(false);
        }

        protected async Task<TViewModel> InitializeViewModelAsync<TViewModel, TData>(
            TData data)

            where TViewModel : class, IViewModel<TData>
        {
            return await InitializeViewModelInternal<TViewModel, TData>(data)
                .ConfigureAwait(false);
        }


        internal static async Task<TViewModel> InitializeViewModelInternal<TViewModel>()

            where TViewModel : class, IViewModel
        {
            var viewModelInstance = dependencyResolver.Resolve<TViewModel>();
            await viewModelInstance.InitializeAsync()
                .ConfigureAwait(false);

            Log?.Info(
                $"ViewModel viewModel of type {typeof(TViewModel).Name} has been created and initialized!");

            return viewModelInstance;
        }

        internal static async Task<TViewModel> InitializeViewModelInternal<TViewModel, TData>(
            TData data)

            where TViewModel : class, IViewModel<TData>
        {
            var viewModelInstance = dependencyResolver.Resolve<TViewModel>();
            await viewModelInstance.InitializeAsync(data)
                .ConfigureAwait(false);

            Log?.Info(
                $"ViewModel viewModel of type {typeof(TViewModel).Name} has been created and initialized!");

            return viewModelInstance;
        }



        protected Page CreateView<TViewModel>(
            TViewModel viewModel)

            where TViewModel : class, IViewModel
        {
            return CreateViewInternal<TViewModel, Page>(
                viewModel);
        }


        internal TPage CreateViewInternal<TViewModel, TPage>(
            TViewModel viewModel)

            where TViewModel : class, IViewModel
            where TPage : Page
        {
            if (!TryGetRegistration(
                typeof(TViewModel),
                out var registration))
            {
                // todo: set exception message
                throw new InvalidOperationException();
            }


            if (registration.ViewType.IsAssignableFrom(typeof(TPage)))
            {
                throw new InvalidOperationException(
                    $"Non-assignable view type specifications for ViewModel '{typeof(TViewModel).Name}'" +
                    $"registered: '{registration.ViewType.Name}', generic parameter: {typeof(TPage).Name}");
            }


            Page view = Configuration.CacheContent ?
                AddOrUpdateContentCache<TPage>(registration) :
                GetViewInstance<TPage>(registration);


            view.BindingContext = viewModel;

            if (viewModel is IHandleClosing)
            {
                view.Disappearing += OnViewClosing;
            }

            Log?.Info(
                $"View of type {view.GetType().Name} has been created!");


            return (TPage)view;
        }



        private bool IsTab(
            Type viewType)
        {
            if (!(RootPage is TabbedPage tabbedPage) ||
                !viewType.IsAssignableFrom(typeof(TabPage)))
            {
                return false;
            }


            return tabbedPage.Children.Any(
                tab => tab.GetType() == viewType);
        }


        private bool IsDetail(
            Type viewType)
        {
            return RootPage is MasterDetailPage &&
                viewType.IsAssignableFrom(typeof(DetailPage));
        }


        private async Task PushAsync(
            Page page)
        {
            if (CurrentPage?.Navigation == null)
            {
                return;
            }

            await Device.InvokeOnMainThreadAsync(() =>
            {
                CurrentPage.Navigation.PushAsync(
                    page,
                    animated: Configuration.UseAnimations);
            });
        }

        private async Task SetDetailAsync(
            Page page)
        {
            if (!(Application.Current.MainPage is MasterDetailPage masterDetailPage) ||
                !page.GetType().IsAssignableFrom(typeof(DetailPage)))
            {
                return;
            }


            await Device.InvokeOnMainThreadAsync(() =>
            {
                masterDetailPage.Detail = new NavigationPage(page);
                masterDetailPage.IsPresented = !Configuration.MasterDetailConfig.HideMenuOnPageSwitch;
            });
        }

        private async Task SetTabAsync(
            Type selectedTabType)
        {
            if (!IsTab(selectedTabType))
            {
                return;
            }


            var tabbedPage = RootPage as TabbedPage;

            await Device.InvokeOnMainThreadAsync(() =>
            {
                var selectedTab = tabbedPage.Children
                    .FirstOrDefault(tab => tab.GetType() == selectedTabType);

                if (selectedTab == null)
                {
                    Log?.Error(
                        $"{tabbedPage.GetType().Name} does not contain requested view {selectedTabType.Name}!");

                    return;
                }


                tabbedPage.CurrentPage = selectedTab;
            });
        }


        private async Task PushModalAsync(
            Page page)
        {
            if (CurrentPage?.Navigation == null)
            {
                return;
            }


            await Device.InvokeOnMainThreadAsync(() =>
            {
                CurrentPage.Navigation.PushModalAsync(
                    page,
                    animated: Configuration.UseAnimations);
            });
        }
               



        private static TView GetViewInstance<TView>(
            INavigationRegistration registrationInfo)

            where TView : Page
        {
            return registrationInfo.ResolveViewUsingDependencyInjection
                ?
                (TView)dependencyResolver.Resolve(
                    registrationInfo.ViewType)
                :
                (TView)Activator.CreateInstance(
                    registrationInfo.ViewType);
        }
    }
}