using System.Threading.Tasks;

using Xamarin.Forms;

using CodeMonkeys.Core.Interfaces.MVVM;
using CodeMonkeys.Core.Interfaces.Navigation;

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
    }
}