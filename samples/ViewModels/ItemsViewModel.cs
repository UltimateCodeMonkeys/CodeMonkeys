using CodeMonkeys.MVVM.Attributes;
using CodeMonkeys.MVVM.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeMonkeys.Samples.ViewModels
{
    public class ItemsViewModel :
        BaseViewModel
    {
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


        public ItemsViewModel()
        {
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
                var viewModel = new ItemViewModel($"Item {count + 1}");

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