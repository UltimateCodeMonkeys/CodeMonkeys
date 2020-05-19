using System;
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
                typeof(INavigationService),
                typeof(NavigationHost),
                new PropertyMetadata(OnNavigationServiceChanged));

        public new INavigationService NavigationService
        {
            get => (INavigationService)GetValue(NavigationServiceProperty);
            set => SetValue(NavigationServiceProperty, value);
        }



        public static readonly DependencyProperty DefaultContentProperty =
            DependencyProperty.Register(
                nameof(DefaultContent),
                typeof(object),
                typeof(NavigationHost),
                new PropertyMetadata(null));

        public object DefaultContent
        {
            get => GetValue(DefaultContentProperty);
            set => SetValue(DefaultStyleKeyProperty, value);
        }



        public NavigationHost()
        {
            NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;


            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
        }


        protected override void OnInitialized(
            EventArgs eventArgs)
        {
            base.OnInitialized(eventArgs);


            if (NavigationService == null)
                Content = DefaultContent;
        }


        private static void OnNavigationServiceChanged(
            DependencyObject @object,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            if (!(@object is NavigationHost host))
                return;

            if (!(eventArgs.NewValue is NavigationService navigationService))
                return;


            var binding = new Binding(
                nameof(navigationService.CurrentContent))
            {
                Source = host.NavigationService
            };

            host.SetBinding(ContentProperty, binding);

        }
    }
}