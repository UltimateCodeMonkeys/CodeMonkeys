using System.Threading.Tasks;

namespace CodeMonkeys.Navigation.ViewModels
{
    public interface IHandleClosing
    {
        Task OnClosing();
    }
}