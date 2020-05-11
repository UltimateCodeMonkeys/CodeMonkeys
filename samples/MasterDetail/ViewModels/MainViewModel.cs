using CodeMonkeys.MVVM.Commands;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeMonkeys.Samples.ViewModels
{
    public class MainViewModel :
        BaseViewModel
    {
        public ObservableCollection<MenuItem> MenuItems { get; }

        public MainViewModel()
        {
            MenuItems = new ObservableCollection<MenuItem>();
        }

        public override async Task InitializeAsync()
        {
            MenuItems.Add(new MenuItem
            {
                Title = "Items",
                ShowAsyncFunc = ShowAsync<ItemsViewModel>,
            });

            MenuItems.Add(new MenuItem
            {
                Title = "About",
                ShowAsyncFunc = ShowAsync<AboutViewModel>,
            });

            await base.InitializeAsync();
        }
    }

    public class MenuItem
    {
        public string Title { get; set; }
        public Func<Task> ShowAsyncFunc { get; set; }

        public ICommand ShowItemCommand => new AsyncCommand(ShowAsyncFunc.Invoke);
    }
}