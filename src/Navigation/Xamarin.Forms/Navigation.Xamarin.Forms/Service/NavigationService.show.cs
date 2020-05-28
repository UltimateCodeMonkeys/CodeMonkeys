using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;
using CodeMonkeys.Navigation.Xamarin.Forms.Pages;

using System;
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

            var viewModelInstance = await InitializeViewModelAsync<TViewModel>();

            var page = CreateView<TViewModel>(
                viewModelInstance);


            var showAsyncFunc = BuildShowAsyncFunc(
                page);

            await showAsyncFunc.Invoke(
                page);
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
                await InitializeViewModelAsync<TViewModel, TData>(
                    data);

            var page = CreateView<TViewModel>(
                viewModelInstance);


            var showAsyncFunc = BuildShowAsyncFunc(
                page);

            await showAsyncFunc.Invoke(
               page);
        }


        public virtual async Task ShowModalAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TViewModel>();


            var viewModelInstance =
                await InitializeViewModelAsync<TViewModel>();

            var page = CreateView<TViewModel>(
                viewModelInstance);


            await PushModalAsync(page);
        }


        public virtual async Task ShowModalAsync<TViewModel, TData>(
            TData data)

            where TViewModel : class, IViewModel<TData>
        {
            ThrowIfNotRegistered<TViewModel>();


            var viewModelInstance =
                await InitializeViewModelAsync<TViewModel, TData>(
                    data);

            var page = CreateView<TViewModel>(
                viewModelInstance);


            await PushModalAsync(page);
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




        protected async Task<TViewModel> InitializeViewModelAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            return await InitializeViewModelInternal<TViewModel>();
        }

        protected async Task<TViewModel> InitializeViewModelAsync<TViewModel, TModel>(
            TModel model)



        internal static async Task<TViewModel> InitializeViewModelInternal<TViewModel>()

            where TViewModel : class, IViewModel
        {
            var viewModelInstance = dependencyResolver.Resolve<TViewModel>();
            await viewModelInstance.InitializeAsync();

            Log?.Info(
                $"ViewModel viewModel of type {typeof(TViewModel).Name} has been created and initialized!");

            return viewModelInstance;
        }

        internal static async Task<TViewModel> InitializeViewModelInternal<TViewModel, TModel>(
            TModel model)

            where TViewModel : class, IViewModel<TModel>
        {
            var viewModelInstance = dependencyResolver.Resolve<TViewModel>();
            await viewModelInstance.InitializeAsync(
                model);

            Log?.Info(
                $"ViewModel viewModel of type {typeof(TViewModel).Name} has been created and initialized!");

            return viewModelInstance;
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