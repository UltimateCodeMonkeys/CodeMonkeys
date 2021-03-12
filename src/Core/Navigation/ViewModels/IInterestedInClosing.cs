using System.Threading.Tasks;

namespace CodeMonkeys.Navigation.ViewModels
{
    public interface IInterestedInClosing
    {
        Task OnInterestedViewModelClosingAsync();
    }
}