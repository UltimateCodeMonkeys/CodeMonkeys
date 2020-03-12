using System.Threading.Tasks;

namespace CodeMonkeys.Core.Navigation.ViewModels
{
    public interface IHandleClosing
    {
        Task OnClosing();
    }
}