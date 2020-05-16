using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CodeMonkeys.Navigation.WPF
{
    public class NavigationHost :
        ContentControl
    {
        public static readonly DependencyProperty NavigationServiceProperty =
            DependencyProperty.Register(
                nameof(NavigationService),
                typeof(NavigationService),
                typeof(NavigationHost),
                new PropertyMetadata(OnNavigationServiceChanged));

        public NavigationService NavigationService
        {
            get => (NavigationService)GetValue(NavigationServiceProperty);
            set => SetValue(NavigationServiceProperty, value);
        }


        public NavigationHost()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == nameof(ContentProperty))
            {

            }
        }


        private static void OnNavigationServiceChanged(
            DependencyObject @object,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            if (!(@object is NavigationHost host))
                return;

            if (!(eventArgs.NewValue is NavigationService navigationService))
                return;


            navigationService.CurrentViewModelChanged += (sender, eventArgs) => { host.Content = navigationService.CurrentContent; };

            //var contentBinding = new Binding(nameof(navigationService.CurrentContent))
            //{
            //    Source = host.NavigationService,
            //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            //};

            //BindingOperations.SetBinding(
            //    host,
            //    ContentProperty,
            //    contentBinding);
        }
    }
}