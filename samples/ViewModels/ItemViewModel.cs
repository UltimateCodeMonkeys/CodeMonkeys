using CodeMonkeys.MVVM.Attributes;
using CodeMonkeys.MVVM.Commands;
using CodeMonkeys.Navigation;

using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeMonkeys.Samples.ViewModels
{
    public class ItemViewModel :
        BaseViewModel
    {
        private readonly INavigationService _navigationService;


        [IsRelevantForCommand(nameof(SelectedCommand))]
        public bool CanSelect
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ICommand SelectedCommand { get; }


        public ItemViewModel(
            INavigationService navigationService,
            string title)
        {
            _navigationService = navigationService;


            Title = title;

            SelectedCommand = new AsyncCommand(
                ShowDetails,
                CanShowDetails);
        }

        private async Task ShowDetails()
        {
            await _navigationService.ShowAsync<ItemDetailsViewModel, string>(
                Title);
        }

        private bool CanShowDetails()
        {
            return CanSelect;
        }
    }
}