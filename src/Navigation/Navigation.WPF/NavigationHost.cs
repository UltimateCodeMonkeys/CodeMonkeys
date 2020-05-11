using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CodeMonkeys.Navigation.WPF
{
    public class NavigationHost :
        Frame
    {
        public static readonly DependencyProperty NavigationServiceProperty =
            DependencyProperty.Register(
                nameof(NavigationService),
                typeof(NavigationService),
                typeof(NavigationHost),
                new PropertyMetadata(OnNavigationServiceChanged));

        public new NavigationService NavigationService
        {
            get => (NavigationService)GetValue(NavigationServiceProperty);
            set => SetValue(NavigationServiceProperty, value);
        }


        public NavigationHost()
        {
            NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }



        private static void OnNavigationServiceChanged(
            DependencyObject @object,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            if (!(@object is NavigationHost host))
                return;

            if (!(eventArgs.NewValue is NavigationService navigationService))
                return;


            var contentBinding = new Binding
            {
                Source = host.NavigationService,
                Path = new PropertyPath(nameof(WPF.NavigationService.CurrentContent))
            };

            host.SetBinding(
                ContentProperty,
                contentBinding);
        }
    }
}