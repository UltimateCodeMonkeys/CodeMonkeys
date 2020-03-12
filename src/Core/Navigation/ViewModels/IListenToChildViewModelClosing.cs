using System.Threading.Tasks;

namespace CodeMonkeys.Core.Navigation.ViewModels
{
    public interface IListenToChildViewModelClosing
    {
        Task OnChildViewModelClosingAsync();
    }
}