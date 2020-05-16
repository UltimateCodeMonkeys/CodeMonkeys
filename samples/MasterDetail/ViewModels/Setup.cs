using CodeMonkeys.DependencyInjection;
using CodeMonkeys.MVVM.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMonkeys.Samples.ViewModels
{
    public static class Setup
    {
        public static void RegisterTypes(
            IDependencyContainer container)
        {
            container.RegisterType<MainViewModel>();
            container.RegisterType<ItemsViewModel>();
            container.RegisterType<AboutViewModel>();
            container.RegisterType<ItemDetailsViewModel>();

            container.RegisterType<ItemViewModel>();

            ViewModelFactory.Configure(
                container);
        }
    }
}