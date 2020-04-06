using CodeMonkeys.Core.MVVM;
using CodeMonkeys.Logging;
using CodeMonkeys.Navigation.ViewModels;

using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class ViewModelNavigationService :
        IViewModelNavigationService
    {
        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.ShowAsync{TViewModelInterface}" />
        public virtual async Task ShowAsync<TDestinationViewModel>()

            where TDestinationViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TDestinationViewModel>();

            if (IsRoot<TDestinationViewModel>())
            {
                await PopToRootAsync();
                return;
            }

            var viewModelInstance = await PrepareToShowViewModel<TDestinationViewModel>();

            var page = CreateViewInstance<TDestinationViewModel>(
                viewModelInstance);

            await ShowAsync<TDestinationViewModel>(
                page);
        }

        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.ShowAsync{TViewModelInterface, TModel}(TModel)" />
        public virtual async Task ShowAsync<TDestinationViewModel, TModel>(
            TModel model)

            where TDestinationViewModel : class, IViewModel<TModel>
        {
            ThrowIfNotRegistered<TDestinationViewModel>();

            if (IsRoot<TDestinationViewModel>())
            {
                await PopToRootAsync();
                return;
            }

            var viewModelInstance =
                await PrepareToShowViewModel<TDestinationViewModel, TModel>(
                    model);

            var page = CreateViewInstance<TDestinationViewModel>(
                viewModelInstance);

            await ShowAsync<TDestinationViewModel>(
                page);
        }


        private Task ShowAsync<TDestinationViewModel>(
            Page page)

            where TDestinationViewModel : class, IViewModel
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(
                    page,
                    animated: Configuration.UseAnimations);
            });

            return Task.CompletedTask;
        }

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
    }
}