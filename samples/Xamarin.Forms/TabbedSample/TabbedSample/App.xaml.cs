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
            var dependencyContainer = CodeMonkeys.DependencyInjection.Ninject.NinjectFactory.CreateInstance();

            //dependencyContainer.RegisterType<CodeMonkeys.Navigation.INavigationService, NavigationService>();
            //dependencyContainer.RegisterType<INavigationService, NavigationService>();

            var navigationService = new NavigationService(
                dependencyContainer);

            dependencyContainer.RegisterInstance<INavigationService>(navigationService);
            dependencyContainer.RegisterInstance<CodeMonkeys.Navigation.INavigationService>(navigationService);


            dependencyContainer.RegisterType<MainViewModel>();
            dependencyContainer.RegisterType<ItemsViewModel>();
            dependencyContainer.RegisterType<AboutViewModel>();

            dependencyContainer.RegisterType<ItemViewModel>();
            dependencyContainer.RegisterType<ItemDetailsViewModel>();

            

            try
            {
                navigationService.Register<MainViewModel, MainPage>();

                navigationService.Register<ItemsViewModel, ItemsPage>();
                navigationService.Register<AboutViewModel, AboutPage>();

                navigationService.Register<ItemDetailsViewModel, ItemDetailsPage>();

                CodeMonkeys.MVVM.ViewModelFactory.Configure(
                    dependencyContainer);

                await navigationService.SetRootAsync<MainViewModel>();

            }
            catch (System.Exception ex)
            {

            }


            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
