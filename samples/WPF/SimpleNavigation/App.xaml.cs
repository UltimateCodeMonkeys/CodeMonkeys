using CodeMonkeys.DependencyInjection.DryIoC;
using CodeMonkeys.Navigation;
using CodeMonkeys.Navigation.WPF;
using CodeMonkeys.Samples.ViewModels;

using System.Windows;

using SimpleNavigation;

namespace CodeMonkeys.Samples.WPF.SimpleNavigation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void OnAppStartup(
            object sender,
            StartupEventArgs eventArgs)
        {
            var dependencyContainer = DryFactory.CreateInstance();


            ViewModels.Bootstrap.RegisterViewModels(
                dependencyContainer);


            var navigationService = new NavigationService(
                dependencyContainer);

            RegisterNavigationRoutes(
                navigationService);

            dependencyContainer.RegisterInstance<Navigation.INavigationService>(
                navigationService);

            dependencyContainer.RegisterInstance<Navigation.WPF.INavigationService>(
                navigationService);



            await navigationService.SetRootWindowAsync<MainViewModel, ItemsViewModel>();
        }

        private void RegisterNavigationRoutes(
            Navigation.WPF.INavigationService navigationService)
        {
            navigationService.Register<MainViewModel, MainWindow>();
            navigationService.Register<ItemsViewModel, ItemsView>();
            navigationService.Register<ItemDetailsViewModel, ItemDetailsView>();
        }
    }
}