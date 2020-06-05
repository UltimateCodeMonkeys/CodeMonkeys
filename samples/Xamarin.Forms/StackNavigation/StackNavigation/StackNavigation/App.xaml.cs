using CodeMonkeys.DependencyInjection.DryIoC;
using CodeMonkeys.Navigation;
using CodeMonkeys.Navigation.Xamarin.Forms;
using CodeMonkeys.Samples.ViewModels;
using CodeMonkeys.Samples.XF.StackNavigation.Views;
using Xamarin.Forms;

namespace CodeMonkeys.Samples.XF.StackNavigation
{
    public partial class App :
        Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override async void OnStart()
        {
            var dependencyContainer = DryFactory.CreateInstance();

            CodeMonkeys.Samples.ViewModels.Setup.RegisterTypes(
                dependencyContainer);


            dependencyContainer.RegisterType<INavigationService, NavigationService>();


            var navigationService = dependencyContainer.Resolve<INavigationService>();

            navigationService.Register<MainViewModel, MainPage>();
            navigationService.Register<ItemsViewModel, ItemsPage>();
            navigationService.Register<AboutViewModel, AboutPage>();
            navigationService.Register<ItemDetailsViewModel, ItemDetailsPage>();


            await navigationService.SetRoot<MainViewModel>();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}