using System.Threading.Tasks;

namespace CodeMonkeys.Core.Interfaces.Navigation.ViewModels
{
    public interface IListenToChildViewModelClosing<TResult>
    {
        Task OnChildViewModelClosingAsync(TResult result);
    }
}