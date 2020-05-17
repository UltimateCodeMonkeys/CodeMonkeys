using CodeMonkeys.MVVM.Attributes;
using CodeMonkeys.MVVM.Commands;
using CodeMonkeys.MVVM.PropertyChanged;

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
                ShowItemCommand = new AsyncCommand(ShowAsync<ItemsViewModel>),
            });

            MenuItems.Add(new MenuItem
            {
                Title = "About",
                ShowItemCommand = new AsyncCommand(ShowAsync<AboutViewModel>),
            });

            await base.InitializeAsync();
        }
    }

    public class MenuItem :
        BindingBase
    {
        public string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ICommand ShowItemCommand
        {
            get => GetValue<ICommand>();
            set => SetValue(value);
        }
    }
}