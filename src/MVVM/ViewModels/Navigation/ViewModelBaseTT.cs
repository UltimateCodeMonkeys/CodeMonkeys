using CodeMonkeys.Core.MVVM;

using System.Threading.Tasks;

namespace CodeMonkeys.MVVM.ViewModels.Navigation
{
    public abstract class ViewModelBase<TInterface, TModel> :
        ViewModelBase<TInterface>,
        IViewModel<TModel>

        where TInterface : IViewModel<TModel>
    {
        /// <inheritdoc />
        public virtual Task InitializeAsync(
            TModel model)
        {
            IsInitialized = true;

            return Task.CompletedTask;
        }
    }
}