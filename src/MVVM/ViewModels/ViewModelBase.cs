using System.Threading.Tasks;

using CodeMonkeys.Core.Interfaces.MVVM;
using CodeMonkeys.MVVM.PropertyChanged;

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