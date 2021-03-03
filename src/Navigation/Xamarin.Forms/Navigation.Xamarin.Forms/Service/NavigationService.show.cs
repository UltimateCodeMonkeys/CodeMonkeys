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
            ThrowIfNotRegistered<TViewModel>();

            if (IsRoot<TViewModel>())
            {
                await PopToRootAsync();
                return;
            }

            var viewModelInstance = await InitializeViewModelAsync<TViewModel>()
                .ConfigureAwait(false);

            var page = CreateView<TViewModel>(
                viewModelInstance);


            var showAsyncFunc = BuildShowAsyncFunc(
                page);

            await showAsyncFunc.Invoke(page)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.ShowAsync{TViewModelInterface, TModel}(TModel)" />
        public virtual async Task ShowAsync<TViewModel, TData>(
            TData data)

            where TViewModel : class, IViewModel<TData>
        {
            ThrowIfNotRegistered<TViewModel>();

            if (IsRoot<TViewModel>())
            {
                await PopToRootAsync();
                return;
            }

            var viewModelInstance = 
                await InitializeViewModelAsync<TViewModel, TData>(data)
                .ConfigureAwait(false);

            var page = CreateView<TViewModel>(
                viewModelInstance);


            var showAsyncFunc = BuildShowAsyncFunc(
                page);

            await showAsyncFunc.Invoke(
               page);
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



        private Func<Page, Task> BuildShowAsyncFunc(
            Page page)
        {
            if (RootPage is MasterDetailPage &&
                page is DetailPage)
            {
                return SetDetailAsync;
            }
            else if (RootPage is TabbedPage &&
                page is TabPage)
            {
                return SetTabAsync;
            }
            else
            {
                return PushAsync;
            }
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
            if (!(Application.Current.MainPage is MasterDetailPage masterDetailPage))
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
            Page page)
        {
            var mainPage = Application.Current.MainPage;

            if (mainPage is NavigationPage navigation)
            {
                mainPage = navigation.RootPage;
            }

            if (!(mainPage is TabbedPage tabbedPage))
            {
                return;
            }


            await Device.InvokeOnMainThreadAsync(() =>
            {
                var tab = tabbedPage
                    .Children
                    .FirstOrDefault(c => IsPageOfType(
                        c,
                        page.GetType()));

                if (tab == null)
                {
                    Log?.Error(
                        $"{tabbedPage.GetType().Name} does not contain requested view {page.GetType().Name}!");

                    return;
                }


                tabbedPage.CurrentPage = tab;
            });
        }

        private bool IsPageOfType(
            Page page,
            Type type)
        {
            if (page?.GetType() == type)
            {
                return true;
            }

            if (page is NavigationPage navigationPage)
            {
                return navigationPage?.RootPage?.GetType() == type;
            }

            return false;
        }


        private async Task PushModalAsync(
            Page page)
        {
            if (Navigation == null)
            {
                return;
            }


            await Device.InvokeOnMainThreadAsync(() =>
            {
                Navigation.PushModalAsync(
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