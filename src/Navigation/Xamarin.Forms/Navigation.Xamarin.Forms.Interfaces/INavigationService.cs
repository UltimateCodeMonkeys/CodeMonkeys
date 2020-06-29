using CodeMonkeys.MVVM;

using System.Threading.Tasks;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public interface INavigationService :
        CodeMonkeys.Navigation.INavigationService
    {
        /// <summary>
        /// Sets the given ViewModels as Master and Detail main view
        /// </summary>
        /// <typeparam name="TMasterViewModel">Master ViewModel containing the menu information</typeparam>
        /// <typeparam name="TDetailViewModel">Detail ViewModel to show on start</typeparam>
        /// <returns><see cref="Task" /> to await on</returns>
        Task SetRootAsync<TMasterViewModel, TDetailViewModel>()
            where TMasterViewModel : class, IViewModel
            where TDetailViewModel : class, IViewModel;



        /// <summary>
        /// Displays the requested ViewModel in a modal dialog
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel type to show modally</typeparam>
        /// <returns><see cref="Task" /> to await on</returns>
        Task ShowModalAsync<TViewModel>()
            where TViewModel : class, IViewModel;

        /// <summary>
        /// Displays the requested ViewModel in a modal dialog
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel type to show modally</typeparam>
        /// <returns><see cref="Task" /> to await on</returns>
        Task ShowModalAsync<TViewModel, TData>(
            TData data)
            where TViewModel : class, IViewModel<TData>;


        /// <summary>
        /// Closes the modal dialog of this ViewModel
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel type to hide</typeparam>
        /// <returns><see cref="Task" /> to await on</returns>
        Task CloseModalAsync<TViewModel>();


        /// <summary>
        /// Closes all open pages and goes back to the application's root page
        /// </summary>
        Task CloseAllAsync();
    }
}