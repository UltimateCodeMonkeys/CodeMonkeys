using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class NavigationService :
        INavigationService
    {
        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.CloseAsync{TViewModelInterface}" />
        public virtual async Task CloseAsync<TViewModelInterface>()

            where TViewModelInterface : class, IViewModel
        {
            if (!TryGetRegistration(
                typeof(TViewModelInterface),
                out var registration))
            {
                throw new InvalidOperationException();
            }

            if (!Navigation.NavigationStack.Any(
                page => page.GetType() == registration.ViewType))
            {
                return;
            }


            var view = Navigation.NavigationStack.First(
                page => page.GetType() == registration.ViewType);

            await CloseCurrentPage();
        }

        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.CloseAsync{TViewModelInterface, TParentViewModelInterface}" />
        public virtual async Task CloseAsync<TViewModelInterface, TParentViewModelInterface>()

            where TViewModelInterface : class, IViewModel
            where TParentViewModelInterface : class, IViewModel, IListenToChildViewModelClosing
        {
            ThrowIfNotRegistered<TViewModelInterface>(
                CurrentPage.GetType());

            if (!TryGetRegistration(
                typeof(TViewModelInterface),
                out var registration))
            {
                throw new InvalidOperationException();
            }

            if (!Navigation.NavigationStack.Any(
                page => page.GetType() == registration.ViewType))
            {
                return;
            }


            await ResolveAndInformParent<TParentViewModelInterface>();

            await CloseCurrentPage();
        }

        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.CloseAsync{TViewModelInterface, TParentViewModelInterface, TResult}(TResult)" />
        public virtual async Task CloseAsync<TViewModelInterface, TParentViewModelInterface, TResult>(
            TResult result)

            where TViewModelInterface : class, IViewModel
            where TParentViewModelInterface : class, IViewModel, IListenToChildViewModelClosing<TResult>
        {
            ThrowIfNotRegistered<TViewModelInterface>(
                CurrentPage.GetType());

            if (!TryGetRegistration(
                typeof(TViewModelInterface),
                out var registration))
            {
                throw new InvalidOperationException();
            }

            if (!Navigation.NavigationStack.Any(
                page => page.GetType() == registration.ViewType))
            {
                return;
            }


            await ResolveAndInformParent<TParentViewModelInterface, TResult>(
                result);

            await CloseCurrentPage();
        }

        public virtual async Task CloseAllAsync()
        {
            await PopToRootAsync();
        }


        private async Task CloseCurrentPage()
        {
            await Navigation.PopAsync(
                animated: Configuration.UseAnimations);

            Log?.Info(
                "Page has been removed from Xamarin navigation stack.");


            var bindingContext = Navigation.NavigationStack.Last()?.BindingContext;

            if (!(bindingContext is IViewModel viewModel))
                return;
        }

        private async Task PopToRootAsync()
        {
            await Navigation.PopToRootAsync(
                animated: Configuration.UseAnimations);

            Log?.Info(
                $"Navigation stack cleared.");
        }


        private async Task ResolveAndInformParent<TParentViewModelInterface>()

            where TParentViewModelInterface : class, IListenToChildViewModelClosing
        {
            var parentViewModel = dependencyResolver.Resolve<TParentViewModelInterface>();

            Log?.Info(
                $"ViewModelInstance for type {typeof(TParentViewModelInterface).Name} has been resolved.");

            await parentViewModel.OnChildViewModelClosingAsync();
        }

        private async Task ResolveAndInformParent<TParentViewModelInterface, TResult>(
            TResult result)

            where TParentViewModelInterface : class, IListenToChildViewModelClosing<TResult>
        {
            var parentViewModel = dependencyResolver.Resolve<TParentViewModelInterface>();

            Log?.Info(
                $"ViewModelInstance for type {typeof(TParentViewModelInterface).Name} has been resolved.");

            await parentViewModel.OnChildViewModelClosingAsync(
                result);
        }
    }
}