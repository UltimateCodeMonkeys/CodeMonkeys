using System.Threading.Tasks;

namespace CodeMonkeys.Core.Interfaces.MVVM
{
    public interface IViewModel<TModel> :
        IViewModel
    {
        /// <summary>
        /// Method that automatically gets called when the instance was built using the ViewModelFactory or the <see cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService" />
        /// </summary>
        /// <param name="model">Data that should be used for initialization</param>
        Task InitializeAsync(TModel model);
    }
}