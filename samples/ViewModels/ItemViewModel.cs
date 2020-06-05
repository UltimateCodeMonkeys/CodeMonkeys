using CodeMonkeys.MVVM.Attributes;
using CodeMonkeys.MVVM.Commands;

using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeMonkeys.Samples.ViewModels
{
    public class ItemViewModel :
        BaseViewModel
    {
        [IsRelevantForCommand(nameof(SelectedCommand))]
        public bool CanSelect
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ICommand SelectedCommand { get; }


        public ItemViewModel(
            string title)
        {
            Title = title;

            SelectedCommand = new AsyncCommand(
                ShowDetails,
                CanShowDetails);
        }

        private async Task ShowDetails()
        {
            await ShowAsync<ItemDetailsViewModel, string>(
                Title);
        }

        private bool CanShowDetails()
        {
            return CanSelect;
        }
    }
}