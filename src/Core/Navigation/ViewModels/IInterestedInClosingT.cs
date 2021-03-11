using System.Threading.Tasks;

namespace CodeMonkeys.Navigation.ViewModels
{
    public interface IInterestedInClosing<TResult>
    {
        Task OnInterestedViewModelClosingAsync(TResult result);
    }
}