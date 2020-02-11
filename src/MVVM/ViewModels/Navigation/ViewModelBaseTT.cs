using System;
using System.Threading.Tasks;

using CodeMonkeys.Core.Interfaces.MVVM;
using CodeMonkeys.Core.Interfaces.Navigation;
using CodeMonkeys.Core.Interfaces.Navigation.ViewModels;
using CodeMonkeys.MVVM.Factories;

namespace CodeMonkeys.MVVM.ViewModels.Navigation
{
    public abstract class ViewModelBase<TInterface, TModel> :
        ViewModels.ViewModelBase<TModel>,
        IHandleClosing

        where TInterface : IViewModel<TModel>
    {
        internal Lazy<IViewModelNavigationService> NavigationService = new Lazy<IViewModelNavigationService>(
            ViewModelFactory.TryResolveNavigationServiceInstance);


        /// <summary>
        /// Invokes the IViewModelNavigationService (if registered) to show the requested ViewModel
        /// </summary>
        /// <typeparam name="TViewModelInterface">Type of the ViewModel interface to show</typeparam>
        public async Task ShowAsync<TViewModelInterface>()
            where TViewModelInterface : class, IViewModel
        {
            await NavigationService.Value
                .ShowAsync<TViewModelInterface>()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Invokes the IViewModelNavigationService (if registered) to show the requested ViewModel
        /// Passes the parameter for the ViewModel initialization
        /// </summary>
        /// <typeparam name="TViewModelInterface">Type of the ViewModel interface to show</typeparam>
        /// /// <typeparam name="TParameter">Type of the initialization data</typeparam>
        /// <param name="data">The data to use for ViewModel initialization</param>
        public async Task ShowAsync<TViewModelInterface, TParameter>(
            TParameter data)
            where TViewModelInterface : class, IViewModel<TParameter>
        {
            await NavigationService.Value
                .ShowAsync<TViewModelInterface, TParameter>(
                    data)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Closes this ViewModel using the IViewModelNavigationService (if registered)
        /// </summary>
        public async Task CloseAsync()
        {
            var closeViewModelTask = (Task)this.GetCloseViewModelAsyncMethodInfo(0, 1)
               .MakeGenericMethod(typeof(TInterface))
               .Invoke(NavigationService, null);

            await closeViewModelTask
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Closes this ViewModel and informs the parent using the IViewModelNavigationService (if registered)
        /// </summary>
        /// <typeparam name="TParentViewModelInterface">Type of the parent ViewModel to inform</typeparam>
        public async Task CloseAsync<TParentViewModelInterface>()
            where TParentViewModelInterface : IListenToChildViewModelClosing
        {
            var closeViewModelTask = (Task)this.GetCloseViewModelAsyncMethodInfo(0, 2)
                .MakeGenericMethod(
                    typeof(TInterface),
                    typeof(TParentViewModelInterface))
                .Invoke(NavigationService, null);

            await closeViewModelTask
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Closes this ViewModel and informs the parent using the IViewModelNavigationService (if registered)
        /// </summary>
        /// <typeparam name="TParentViewModelInterface">Type of the parent ViewModel to inform</typeparam>
        /// <typeparam name="TResult">Type of the result data</typeparam>
        /// <param name="resultToPassToParent">The data to pass to the parent ViewModel</param>
        public async Task CloseAsync<TParentViewModelInterface, TResult>(
            TResult resultToPassToParent)
            where TParentViewModelInterface : IListenToChildViewModelClosing<TResult>
        {
            var closeViewModelTask = (Task)this.GetCloseViewModelAsyncMethodInfo(
                genericArgumentsCount: 3)
                .MakeGenericMethod(
                    typeof(TInterface),
                    typeof(TParentViewModelInterface),
                    typeof(TResult))
                .Invoke(
                    NavigationService.Value,
                    new object[] { resultToPassToParent });

            await closeViewModelTask
                .ConfigureAwait(false);
        }


        public virtual Task OnClosing()
        {
            return Task.CompletedTask;
        }
    }
}