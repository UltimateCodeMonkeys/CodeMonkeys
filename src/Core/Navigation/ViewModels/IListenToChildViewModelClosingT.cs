using System.Threading.Tasks;

namespace CodeMonkeys.Navigation.ViewModels
{
    public interface IListenToChildViewModelClosing<TResult>
    {
        Task OnChildViewModelClosingAsync(TResult result);
    }
}