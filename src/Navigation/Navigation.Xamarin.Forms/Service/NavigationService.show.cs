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
    public partial class NavigationService :
        INavigationService
    {

        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.ShowAsync{TViewModelInterface}" />
        public virtual async Task ShowAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TViewModel>();

            if (IsRoot<TViewModel>())
            {
                await PopToRootAsync();
                return;
            }

            var viewModelInstance = await InitializeViewModelAsync<TViewModel>();

            var page = CreateView<TViewModel>(
                viewModelInstance);


            var showAsyncFunc = BuildShowAsyncFunc(
                page);

            await showAsyncFunc.Invoke(
                page);
        }

        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.ShowAsync{TViewModelInterface, TModel}(TModel)" />
        public virtual async Task ShowAsync<TViewModel, TModel>(
            TModel model)

            where TViewModel : class, IViewModel<TModel>
        {
            ThrowIfNotRegistered<TViewModel>();

            if (IsRoot<TViewModel>())
            {
                await PopToRootAsync();
                return;
            }

            var viewModelInstance =
                await InitializeViewModelAsync<TViewModel, TModel>(
                    model);

            var page = CreateView<TViewModel>(
                viewModelInstance);


            var showAsyncFunc = BuildShowAsyncFunc(
                page);

            await showAsyncFunc.Invoke(
               page);
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
            else return PushAsync;
        }


        private async Task PushAsync(
            Page page)
        {
            if (Navigation == null)
                return;

            await Device.InvokeOnMainThreadAsync(() =>
            {
                Navigation.PushAsync(
                    page,
                    animated: Configuration.UseAnimations);
            });
        }

        private async Task SetDetailAsync(
            Page page)
        {
            if (!(Application.Current.MainPage is MasterDetailPage masterDetailPage))
                return;


            await Device.InvokeOnMainThreadAsync(() =>
            {
                masterDetailPage.Detail = new NavigationPage(page);
                masterDetailPage.IsPresented = !Configuration.MasterDetailConfig.HideMenuOnPageSwitch;
            });
        }

        private async Task SetTabAsync(
            Page page)
        {
            if (!(Application.Current.MainPage is TabbedPage tabbedPage))
                return;


            await Device.InvokeOnMainThreadAsync(() =>
            {
                if (!tabbedPage.Children.Contains(page))
                    return;


                tabbedPage.CurrentPage = new NavigationPage(page);
            });
        }

        protected async Task<TViewModel> InitializeViewModelAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            return await InitializeViewModelInternal<TViewModel>();
        }

        protected async Task<TViewModel> InitializeViewModelAsync<TViewModel, TModel>(
            TModel model)

            where TViewModel : class, IViewModel<TModel>
        {
            return await InitializeViewModelInternal<TViewModel, TModel>(
                model);
        }


        protected Page CreateView<TViewModel>(
            TViewModel viewModel)

            where TViewModel : class, IViewModel
        {
            return CreateViewInternal<TViewModel, Page>(
                viewModel);
        }


        internal async Task<TViewModelInterface> InitializeViewModelInternal<TViewModelInterface>()

            where TViewModelInterface : class, IViewModel
        {
            var viewModelInstance = dependencyResolver.Resolve<TViewModelInterface>();
            await viewModelInstance.InitializeAsync();

            Log?.Info(
                $"ViewModel viewModel of type {typeof(TViewModelInterface).Name} has been created and initialized!");

            return viewModelInstance;
        }

        internal async Task<TViewModelInterface> InitializeViewModelInternal<TViewModelInterface, TModel>(
            TModel model)

            where TViewModelInterface : class, IViewModel<TModel>
        {
            var viewModelInstance = dependencyResolver.Resolve<TViewModelInterface>();
            await viewModelInstance.InitializeAsync(
                model);

            Log?.Info(
                $"ViewModel viewModel of type {typeof(TViewModelInterface).Name} has been created and initialized!");

            return viewModelInstance;
        }

        internal TPage CreateViewInternal<TViewModel, TPage>(
            TViewModel viewModel)

            where TViewModel : class, IViewModel
            where TPage : Page
        {
            if (!TryGetRegistration(
                typeof(TViewModel),
                out var registrationInfo))
            {
                // todo: set exception message
                throw new InvalidOperationException();
            }


            if (registrationInfo.ViewType.IsAssignableFrom(typeof(TPage)))
            {
                throw new InvalidOperationException(
                    $"Non-assignable view type specifications for ViewModel '{typeof(TViewModel).Name}' - registered: '{registrationInfo.ViewType.Name}', generic parameter: {typeof(TPage).Name}");
            }


            Page view;

            if (Configuration.CachePageInstances)
            {
                if (PageCache.All(cachedPage => cachedPage.Type != registrationInfo.ViewType))
                {
                    CreateCachedPage(registrationInfo.ViewType);
                }

                var reference = PageCache
                    .First(cachedPage => cachedPage.Type == registrationInfo.ViewType)
                    .Reference;

                if (!reference.TryGetTarget(out view))
                {
                    view = GetViewInstance<TPage>(
                        registrationInfo.ViewType);

                    reference.SetTarget(view);
                }
            }
            else
            {
                view = GetViewInstance<TPage>(
                    registrationInfo.ViewType);
            }


            view.BindingContext = viewModel;

            if (viewModel is IHandleClosing)
            {
                view.Disappearing += OnViewClosing;
            }

            Log?.Info(
                $"View of type {view.GetType().Name} has been created!");

            return (TPage)view;
        }

        private TView GetViewInstance<TView>(
            Type contentType)

            where TView : Page
        {
            if (Configuration.UseDependencyInjectionForViews)
            {
                return (TView)dependencyResolver.Resolve(
                    contentType);
            }
            else
            {
                return (TView)Activator.CreateInstance(
                        contentType);
            }
        }
    }
}