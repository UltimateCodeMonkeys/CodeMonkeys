using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using CodeMonkeys.MVVM.Attributes;
using CodeMonkeys.MVVM.Commands;
using CodeMonkeys.Navigation;

namespace CodeMonkeys.Samples.ViewModels
{
    public class ItemsViewModel :
        BaseViewModel
    {
        private readonly INavigationService _navigationService;


        public ObservableCollection<ItemViewModel> Items
        {
            get => GetValue<ObservableCollection<ItemViewModel>>();
            set => SetValue(value);
        }
        
        [IsRelevantForCommand(nameof(ToggleSelectedItemCanSelectCommand))]
        public ItemViewModel SelectedItem
        {
            get => GetValue<ItemViewModel>();
            set => SetValue(value);
        }


        public ICommand LoadItemsCommand { get; }
        public ICommand ToggleSelectedItemCanSelectCommand { get; }


        public ItemsViewModel(
            INavigationService navigationService)
        {
            _navigationService = navigationService;


            Title = "Browse";
            Items = new ObservableCollection<ItemViewModel>();

            LoadItemsCommand = new Command(GenerateItems);
            ToggleSelectedItemCanSelectCommand = new Command(
                ToggleItemCanSelect,
                CanToggleItemCanSelect);
        }


        public override async Task InitializeAsync()
        {
            GenerateItems();

            await base.InitializeAsync();
        }


        private void GenerateItems()
        {
            IsBusy = true;

            Items.Clear();
                
            for(int count = 0; count < 100; count++)
            {
                var viewModel = new ItemViewModel(
                    _navigationService,
                    $"Item {count + 1}");

                Items.Add(viewModel);
            }


            Items.First().CanSelect = true;

            IsBusy = false;
        }

        private void ToggleItemCanSelect()
        {
            SelectedItem.CanSelect = !SelectedItem.CanSelect;
        }

        private bool CanToggleItemCanSelect()
        {
            return SelectedItem != null;
        }


        public override Task OnClosing()
        {
            return base.OnClosing();
        }
    }
}