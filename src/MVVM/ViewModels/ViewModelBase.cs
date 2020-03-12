using CodeMonkeys.Core.MVVM;
using CodeMonkeys.MVVM.PropertyChanged;

using System.Threading.Tasks;

namespace CodeMonkeys.MVVM.ViewModels
{
    public abstract class ViewModelBase :
        BindingBase,
        IViewModel
    {
        public bool IsInitialized { get; protected set; }


        /// <inheritdoc />
        public virtual Task InitializeAsync()
        {
            IsInitialized = true;

            return Task.CompletedTask;
        }
    }
}