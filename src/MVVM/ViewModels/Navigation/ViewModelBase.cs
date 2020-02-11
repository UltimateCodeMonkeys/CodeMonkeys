using System;
using System.Threading.Tasks;

using CodeMonkeys.Core.Interfaces.MVVM;
using CodeMonkeys.Core.Interfaces.Navigation;
using CodeMonkeys.Core.Interfaces.Navigation.ViewModels;
using CodeMonkeys.MVVM.Factories;

namespace CodeMonkeys.MVVM.ViewModels.Navigation
{
    public abstract class ViewModelBase :
        ViewModels.ViewModelBase,
        IHandleClosing
    {
        internal Lazy<IViewModelNavigationService> NavigationService = new Lazy<IViewModelNavigationService>(
            ViewModelFactory.TryResolveNavigationServiceInstance);


        /// <summary>
        /// Invokes the IViewModelNavigationService (if registered) to show the requested ViewModel
        /// </summary>
        /// <typeparam name="TDestination">Type of the ViewModel interface to show</typeparam>
        public async Task ShowAsync<TDestination>()
            where TDestination : class, IViewModel
        {
            await NavigationService.Value
                .ShowAsync<TDestination>()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Invokes the IViewModelNavigationService (if registered) to show the requested ViewModel
        /// Passes the parameter for the ViewModel initialization
        /// </summary>
        /// <typeparam name="TDestination">Type of the ViewModel interface to show</typeparam>
        /// /// <typeparam name="TParameter">Type of the initialization data</typeparam>
        /// <param name="data">The data to use for ViewModel initialization</param>
        public async Task ShowAsync<TDestination, TParameter>(
            TParameter data)
            where TDestination : class, IViewModel<TParameter>
        {
            await NavigationService.Value
                .ShowAsync<TDestination, TParameter>(
                    data)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Closes this ViewModel using the IViewModelNavigationService (if registered)
        /// </summary>
        public virtual async Task CloseAsync()
        {
            var closeViewModelTask = (Task)this.GetCloseViewModelAsyncMethodInfo(0, 1)
               .MakeGenericMethod(GetType())
               .Invoke(NavigationService.Value, null);

            await closeViewModelTask
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Closes this ViewModel and informs the parent using the IViewModelNavigationService (if registered)
        /// </summary>
        /// <typeparam name="TParentViewModelInterface">Type of the parent ViewModel to inform</typeparam>
        public virtual async Task CloseAsync<TParentViewModelInterface>()
            where TParentViewModelInterface : IListenToChildViewModelClosing
        {
            var closeViewModelTask = (Task)this.GetCloseViewModelAsyncMethodInfo(0, 2)
                .MakeGenericMethod(
                    GetType(),
                    typeof(TParentViewModelInterface))
                .Invoke(NavigationService.Value, null);

            await closeViewModelTask
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Closes this ViewModel and informs the parent using the IViewModelNavigationService (if registered)
        /// </summary>
        /// <typeparam name="TParentViewModelInterface">Type of the parent ViewModel to inform</typeparam>
        /// <typeparam name="TResult">Type of the result data</typeparam>
        /// <param name="resultToPassToParent">The data to pass to the parent ViewModel</param>
        public virtual async Task CloseAsync<TParentViewModelInterface, TResult>(
            TResult resultToPassToParent)
            where TParentViewModelInterface : IListenToChildViewModelClosing<TResult>
        {
            var closeViewModelTask = (Task)this.GetCloseViewModelAsyncMethodInfo(
                    genericArgumentsCount: 3)
                .MakeGenericMethod(
                    GetType(),
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