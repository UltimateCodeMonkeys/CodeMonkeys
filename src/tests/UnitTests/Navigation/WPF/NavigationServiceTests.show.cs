using CodeMonkeys.Navigation;
using CodeMonkeys.UnitTests.Navigation.ViewModels;
using CodeMonkeys.UnitTests.Navigation.WPF.Views;

using NUnit.Framework;

using System.Threading;
using System.Threading.Tasks;

namespace CodeMonkeys.UnitTests.Navigation.WPF
{
    public partial class NavigationServiceTests
    {
        [Test, Apartment(ApartmentState.STA)]
        public async Task ShowAsync_IfViewModelIsRegistered_ViewIsCreatedAndContentIsSet()
        {
            var expectedCurrentViewModelType = typeof(MainViewModel);
            navigationService.Register<MainViewModel, MainPage>();

            await navigationService.ShowAsync<MainViewModel>();

            Assert.AreEqual(
                expectedCurrentViewModelType,
                navigationService.CurrentViewModel.GetType());
        }
    }
}