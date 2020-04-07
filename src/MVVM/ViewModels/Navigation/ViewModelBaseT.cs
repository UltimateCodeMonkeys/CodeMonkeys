using System.Threading.Tasks;

namespace CodeMonkeys.MVVM.ViewModels.Navigation
{
    public abstract class ViewModelBase<TInterface> :
        ViewModelBase

        where TInterface : IViewModel
    {
        /// <inheritdoc />
        public override async Task CloseAsync()
        {
            var closeViewModelTask = (Task)this.GetCloseViewModelAsyncMethodInfo(0, 1)
               .MakeGenericMethod(typeof(TInterface))
               .Invoke(NavigationService.Value, null);

            await closeViewModelTask
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task CloseAsync<TParentViewModelInterface>()
        {
            var closeViewModelTask = (Task)this.GetCloseViewModelAsyncMethodInfo(0, 2)
                .MakeGenericMethod(
                    typeof(TInterface),
                    typeof(TParentViewModelInterface))
                .Invoke(NavigationService.Value, null);

            await closeViewModelTask
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task CloseAsync<TParentViewModelInterface, TResult>(
            TResult resultToPassToParent)
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
    }
}