using CodeMonkeys.MVVM;
using CodeMonkeys.MVVM.Commands;

using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeMonkeys.Samples.ViewModels
{
    public class ItemDetailsViewModel :
        BaseViewModel,

        IViewModel<string>
    {
        public ICommand GoBackCommand { get; }


        public ItemDetailsViewModel()
        {
            GoBackCommand = new AsyncCommand(
                GoBackAsync);
        }


        public async Task InitializeAsync(
            string item)
        {
            Title = item;

            await base.InitializeAsync();
        }


        private async Task GoBackAsync()
        {
            await CloseAsync();
        }
    }
}