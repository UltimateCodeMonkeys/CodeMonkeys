using CodeMonkeys.MVVM.Commands;

using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeMonkeys.Samples.ViewModels
{
    public class ItemViewModel :
        BaseViewModel
    {
        public ICommand SelectedCommand { get; }


        public ItemViewModel(
            string title)
        {
            Title = title;

            SelectedCommand = new AsyncCommand(
                ShowDetails);
        }

        private async Task ShowDetails()
        {
            await ShowAsync<ItemDetailsViewModel, string>(
                Title);
        }
    }
}