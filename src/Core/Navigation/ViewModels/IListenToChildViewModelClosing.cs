using System.Threading.Tasks;

namespace CodeMonkeys.Navigation.ViewModels
{
    public interface IListenToChildViewModelClosing
    {
        Task OnChildViewModelClosingAsync();
    }
}