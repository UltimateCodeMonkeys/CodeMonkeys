using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CodeMonkeys.Navigation.WPF
{
    public sealed class NavigationHost :
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

        private static void OnNavigationServiceChanged(
            DependencyObject @object,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            if (!(@object is NavigationHost navigationHost))
                return;

            if (!(eventArgs.NewValue is NavigationService navigationService))
                return;
            
            

        }
    }
}