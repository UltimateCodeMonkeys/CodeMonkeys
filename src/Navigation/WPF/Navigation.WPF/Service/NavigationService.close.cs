﻿using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

using System.Threading.Tasks;

namespace CodeMonkeys.Navigation.WPF
{
    public partial class NavigationService
    {
        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.CloseAsync{TViewModelInterface}" />
        public virtual Task CloseAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TViewModel>();


            TryGoBack();


            return Task.CompletedTask;
        }

        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.CloseAsync{TViewModelInterface, TParentViewModelInterface}" />
        public virtual async Task CloseAsync<TViewModel, TParentViewModel>()

            where TViewModel : class, IViewModel
            where TParentViewModel : class, IViewModel, IInterestedInClosing
        {
            ThrowIfNotRegistered<TViewModel>();


            if (!TryGoBack())
                return;


            await ResolveAndInformParent<TParentViewModel>();
        }

        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.CloseAsync{TViewModelInterface, TParentViewModelInterface, TResult}(TResult)" />
        public virtual async Task CloseAsync<TViewModelInterface, TParentViewModelInterface, TResult>(
            TResult result)

            where TViewModelInterface : class, IViewModel
            where TParentViewModelInterface : class, IViewModel, IInterestedInClosing<TResult>
        {
            ThrowIfNotRegistered<TViewModelInterface>();


            if (!TryGoBack())
                return;


            await ResolveAndInformParent<TParentViewModelInterface, TResult>(
                result);
        }


        public virtual Task CloseAllAsync()
        {
            if (Root == Current)
                return Task.CompletedTask;


            ClearStacks();

            SetCurrent(
                Root.ViewModel,
                Root.Content);


            return Task.CompletedTask;
        }



        private async Task ResolveAndInformParent<TParentViewModelInterface>()

            where TParentViewModelInterface : class, IInterestedInClosing
        {
            var parentViewModel = dependencyResolver.Resolve<TParentViewModelInterface>();

            Log?.Info(
                $"ViewModelInstance for type {typeof(TParentViewModelInterface).Name} has been resolved.");

            await parentViewModel.OnInterestedViewModelClosingAsync();
        }

        private async Task ResolveAndInformParent<TParentViewModelInterface, TResult>(
            TResult result)

            where TParentViewModelInterface : class, IInterestedInClosing<TResult>
        {
            var parentViewModel = dependencyResolver.Resolve<TParentViewModelInterface>();

            Log?.Info(
                $"ViewModelInstance for type {typeof(TParentViewModelInterface).Name} has been resolved.");

            await parentViewModel.OnInterestedViewModelClosingAsync(
                result);
        }
    }
}