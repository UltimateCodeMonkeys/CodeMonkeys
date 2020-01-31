using System.Threading.Tasks;

namespace CodeMonkeys.Core.Interfaces.Navigation.ViewModels
{
    public interface IListenToChildViewModelClosing
    {
        Task OnChildViewModelClosingAsync();
    }
}