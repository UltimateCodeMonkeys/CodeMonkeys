using CodeMonkeys.DependencyInjection;
using CodeMonkeys.MVVM;

namespace CodeMonkeys.Samples.ViewModels
{
    public static class Bootstrap
    {
        public static void RegisterViewModels(
            IDependencyContainer container)
        {
            container.RegisterType<MainViewModel>();
            container.RegisterType<ItemsViewModel>();
            container.RegisterType<AboutViewModel>();
            container.RegisterType<ItemDetailsViewModel>();

            container.RegisterType<ItemViewModel>();


            ViewModelFactory.Configure(container);
        }
    }
}