using CodeMonkeys.MVVM;

using System;
using System.Windows;

namespace CodeMonkeys.Navigation.WPF
{
    public sealed class NavigationStackEntry
    {
        public IViewModel ViewModel { get; private set; }

        public FrameworkElement Content { get; private set; }



        public NavigationStackEntry(
            IViewModel viewModel,
            FrameworkElement content)
        {
            ViewModel = viewModel;
            Content = content;
        }



        public override bool Equals(
            object other)
        {
            if (!(other is NavigationStackEntry entry))
                return false;


            if (entry.Content != Content)
                return false;

            if (entry.ViewModel != ViewModel)
                return false;


            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                ViewModel,
                Content);
        }
    }


    public sealed class WeakNavigationStackEntry
    {
        public WeakReference<IViewModel> ViewModel { get; set; }
        public WeakReference<FrameworkElement> Content { get; set; }


        public WeakNavigationStackEntry(
            IViewModel viewModel,
            FrameworkElement content)
        {
            ViewModel = new WeakReference<IViewModel>(viewModel);
            Content = new WeakReference<FrameworkElement>(content);
        }    
    }
}