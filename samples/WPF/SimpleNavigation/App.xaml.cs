using CodeMonkeys.DependencyInjection.DryIoC;
using CodeMonkeys.Navigation;
using CodeMonkeys.Navigation.WPF;
using CodeMonkeys.Samples.ViewModels;

using SimpleNavigation;
using System.Windows;

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

            dependencyContainer.RegisterType<INavigationService, CodeMonkeys.Navigation.WPF.NavigationService>();

            ViewModels.Bootstrap.RegisterViewModels(
                dependencyContainer);


            var navigationService = dependencyContainer.Resolve<INavigationService>();
            RegisterNavigationRoutes(
                navigationService);


            await navigationService.SetRootWindow<MainViewModel, ItemsViewModel>();
        }

        private void RegisterNavigationRoutes(
            INavigationService navigationService)
        {
            navigationService.Register<MainViewModel, MainWindow>();
            navigationService.Register<ItemsViewModel, ItemsView>();
        }
    }
}