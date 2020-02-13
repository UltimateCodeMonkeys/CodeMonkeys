using System.Threading.Tasks;

using CodeMonkeys.Core.Interfaces.MVVM;
using CodeMonkeys.MVVM.PropertyChanged;

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
