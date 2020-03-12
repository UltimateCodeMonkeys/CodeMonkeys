using CodeMonkeys.Core.MVVM;
using CodeMonkeys.MVVM.PropertyChanged;

using System.Threading.Tasks;

namespace CodeMonkeys.MVVM.ViewModels
{
    public abstract class ViewModelBase<TModel> :
        BindingBase,
        IViewModel<TModel>
    {
        public bool IsInitialized { get; set; }

        /// <inheritdoc />
        public virtual Task InitializeAsync()
        {
            IsInitialized = true;
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task InitializeAsync(
            TModel model)
        {
            IsInitialized = true;
            
            return Task.CompletedTask;
        }
    }
}
