using CodeMonkeys.MVVM.Commands;

using System.Collections.ObjectModel;
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
        
        public ICommand LoadItemsCommand { get; }


        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<ItemViewModel>();
            LoadItemsCommand = new Command(GenerateItems);
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

            IsBusy = false;
        }
    }
}