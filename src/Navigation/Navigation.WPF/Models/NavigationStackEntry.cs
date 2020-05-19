using CodeMonkeys.MVVM;

using System.Windows;

namespace CodeMonkeys.Navigation.WPF
{
    public class NavigationStackEntry
    {
        public IViewModel ViewModel { get; set; }
        public FrameworkElement Content { get; set; }
    }
}