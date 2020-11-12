using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.Xamarin.Forms;
using CodeMonkeys.Samples.ViewModels;

using TabbedSample.Views;

using Xamarin.Forms;

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

            dependencyContainer.RegisterType<CodeMonkeys.Navigation.INavigationService, NavigationService>();
            dependencyContainer.RegisterType<CodeMonkeys.Navigation.Xamarin.Forms.INavigationService, NavigationService>();


            dependencyContainer.RegisterType<MainViewModel>();
            dependencyContainer.RegisterType<ItemsViewModel>();
            dependencyContainer.RegisterType<AboutViewModel>();

            dependencyContainer.RegisterType<ItemViewModel>();
            dependencyContainer.RegisterType<ItemDetailsViewModel>();


            ViewModelFactory.Configure(dependencyContainer);

            var navigationService = dependencyContainer.Resolve<CodeMonkeys.Navigation.Xamarin.Forms.INavigationService>();


            navigationService.Register<MainViewModel, MainPage>();

            navigationService.Register<ItemsViewModel, ItemsPage>();
            navigationService.Register<AboutViewModel, AboutPage>();

            navigationService.Register<ItemDetailsViewModel, ItemDetailsPage>();

            await navigationService.SetRootAsync<MainViewModel>();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
