using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class NavigationService
    {
        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.CloseAsync{TViewModel}" />
        public virtual async Task CloseAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            if (!TryGetRegistration(
                typeof(TViewModel),
                out var registration))
            {
                throw new InvalidOperationException();
            }

            if (!Navigation.NavigationStack.Any(
                page => page.GetType() == registration.ViewType))
            {
                return;
            }


            await CloseCurrentPage()
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.CloseAsync{TViewModel, TInterestedViewModel}" />
        public virtual async Task CloseAsync<TViewModel, TInterestedViewModel>()

            where TViewModel : class, IViewModel
            where TInterestedViewModel : class, IViewModel, IListenToChildViewModelClosing
        {
            if (!TryGetRegistration(
                typeof(TViewModel),
                out var registration))
            {
                throw new InvalidOperationException();
            }

            if (Navigation.NavigationStack.Last().GetType() != registration.ViewType)
            {
                return;
            }


            await ResolveAndInformListener<TInterestedViewModel>();

            await CloseCurrentPage()
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.CloseAsync{TViewModel, TInterestedViewModel, TResult}(TResult)" />
        public virtual async Task CloseAsync<TViewModel, TInterestedViewModel, TData>(
            TData data)

            where TViewModel : class, IViewModel
            where TInterestedViewModel : class, IViewModel, IListenToChildViewModelClosing<TData>
        {
            if (!TryGetRegistration(
                typeof(TViewModel),
                out var registration))
            {
                throw new InvalidOperationException();
            }

            if (Navigation.NavigationStack.Last().GetType() != registration.ViewType)
            {
                return;
            }


            await ResolveAndInformListener<TInterestedViewModel, TData>(
                data);

            await CloseCurrentPage()
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="INavigationService.CloseAllAsync" />
        public virtual async Task CloseAllAsync()
        {
            await PopToRootAsync()
                .ConfigureAwait(false);
        }


        /// <inheritdoc cref="INavigationService.CloseModalAsync" />
        public virtual async Task CloseModalAsync<TViewModel>()
        {
            if (Navigation == null)
            {
                Log?.Error(
                    $"'{nameof(Navigation)}' is null!");
                return;
            }


            await Navigation.PopModalAsync(
                animated: Configuration.UseAnimations);
        }


        private async Task CloseCurrentPage()
        {
            await Navigation.PopAsync(
                animated: Configuration.UseAnimations);

            Log?.Info(
                "Page has been removed from Xamarin navigation stack.");
        }

        private async Task PopToRootAsync()
        {
            await Navigation.PopToRootAsync(
                animated: Configuration.UseAnimations);

            Log?.Info(
                $"Navigation stack cleared.");
        }


        private async Task ResolveAndInformListener<TInterestedViewModel>()

            where TInterestedViewModel : class, IListenToChildViewModelClosing
        {
            var parentViewModel = dependencyResolver.Resolve<TInterestedViewModel>();


            Log?.Info(
                $"ViewModelInstance for type {typeof(TInterestedViewModel).Name} has been resolved.");


            await parentViewModel.OnChildViewModelClosingAsync();
        }

        private async Task ResolveAndInformListener<TInterestedViewModel, TData>(
            TData data)

            where TInterestedViewModel : class, IListenToChildViewModelClosing<TData>
        {
            var parentViewModel = dependencyResolver.Resolve<TInterestedViewModel>();


            Log?.Info(
                $"ViewModelInstance for type {typeof(TInterestedViewModel).Name} has been resolved.");


            await parentViewModel.OnChildViewModelClosingAsync(
                data);
        }
    }
}