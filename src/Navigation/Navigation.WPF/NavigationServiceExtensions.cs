using CodeMonkeys.MVVM;

using System.Diagnostics;

namespace CodeMonkeys.Navigation.WPF
{
    public static class NavigationServiceExtensions
    {
        public static IViewModel GetCurrentViewModel(
            this IViewModelNavigationService navigationService)
        {
            if (!(navigationService is NavigationService service))
            {
                Debug.WriteLine($"This extension method can only be used with {nameof(NavigationService)} of type {typeof(NavigationService).FullName}!");
                return null;
            }


            return service.CurrentViewModel;
        }


        public static bool GoBack(
            this IViewModelNavigationService navigationService)
        {
            if (!(navigationService is NavigationService service))
            {
                Debug.WriteLine($"This extension method can only be used with {nameof(NavigationService)} of type {typeof(NavigationService).FullName}!");
                return false;
            }


            return service.TryGoBack();
        }


        public static bool TryGoForward(
            this IViewModelNavigationService navigationService)
        {
            if (!(navigationService is NavigationService service))
            {
                Debug.WriteLine($"This extension method can only be used with {nameof(NavigationService)} of type {typeof(NavigationService).FullName}!");
                return false;
            }


            return service.TryGoForward();
        }


        public static void ClearStacks(
            this IViewModelNavigationService navigationService)
        {
            if (!(navigationService is NavigationService service))
            {
                Debug.WriteLine($"This extension method can only be used with {nameof(NavigationService)} of type {typeof(NavigationService).FullName}!");
                return;
            }

            service.ClearStacks();
        }

        public static void ClearBackStack(
            this IViewModelNavigationService navigationService)
        {
            if (!(navigationService is NavigationService service))
            {
                Debug.WriteLine($"This extension method can only be used with {nameof(NavigationService)} of type {typeof(NavigationService).FullName}!");
                return;
            }

            service.ClearBackStack();
        }

        public static void ClearForwardStack(
            this IViewModelNavigationService navigationService)
        {
            if (!(navigationService is NavigationService service))
            {
                Debug.WriteLine($"This extension method can only be used with {nameof(NavigationService)} of type {typeof(NavigationService).FullName}!");
                return;
            }

            service.ClearForwardStack();
        }
    }
}