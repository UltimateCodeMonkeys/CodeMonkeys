using System.Threading.Tasks;
using System.Windows.Input;

using CodeMonkeys.MVVM;
using CodeMonkeys.MVVM.Commands;
using CodeMonkeys.Navigation;

namespace CodeMonkeys.Samples.ViewModels
{
    public class ItemDetailsViewModel :
        BaseViewModel,

        IViewModel<string>
    {
        private readonly INavigationService _navigationService;


        public ICommand GoBackCommand { get; }


        public ItemDetailsViewModel(
            INavigationService navigationService)
        {
            _navigationService = navigationService;


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
            await _navigationService.CloseAsync<ItemDetailsViewModel>();
        }

        public override Task OnClosing()
        {
            return base.OnClosing();
        }
    }
}