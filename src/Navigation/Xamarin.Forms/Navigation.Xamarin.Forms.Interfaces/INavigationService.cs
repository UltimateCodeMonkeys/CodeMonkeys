using CodeMonkeys.MVVM;

using System.Threading.Tasks;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public interface INavigationService :
        CodeMonkeys.Navigation.INavigationService
    {
        Task ShowModalAsync<TViewModel>()
            where TViewModel : class, IViewModel;

        Task ShowModalAsync<TViewModel, TData>(
            TData data)
            where TViewModel : class, IViewModel<TData>;


        Task CloseModalAsync<TViewModel>();
    }
}