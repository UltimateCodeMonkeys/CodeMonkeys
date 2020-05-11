using CodeMonkeys.Navigation;
using CodeMonkeys.MVVM.Factories;
using CodeMonkeys.Navigation.Xamarin.Forms;
using CodeMonkeys.Samples.ViewModels;

using Xamarin.Forms;

using TabbedSample.Views;

namespace TabbedSample
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
        }

        protected override async void OnStart()
        {
            var dependencyContainer = CodeMonkeys.DependencyInjection.DryIoC.DryFactory.CreateInstance();

            dependencyContainer.RegisterType<INavigationService, NavigationService>();


            dependencyContainer.RegisterType<MainViewModel>();
            dependencyContainer.RegisterType<ItemsViewModel>();
            dependencyContainer.RegisterType<AboutViewModel>();

            dependencyContainer.RegisterType<ItemViewModel>();
            dependencyContainer.RegisterType<ItemDetailsViewModel>();

            ViewModelFactory.Configure(dependencyContainer);

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
