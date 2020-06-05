using CodeMonkeys.UnitTests.Navigation.ViewModels;

using NUnit.Framework;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMonkeys.UnitTests.Navigation.WPF
{
    public partial class NavigationServiceTests
    {
        [Test, Apartment(ApartmentState.STA)]
        public async Task CloseAsync_IfBackwardsNavigationIsPossible_GoesBack()
        {
            var expectedCurrentViewModelType = typeof(MainViewModel);

            RegisterViewModels();
            await navigationService.SetRootAsync<MainViewModel>();
            await navigationService.ShowAsync<SecondViewModel>();


            await navigationService.CloseAsync<SecondViewModel>();


            Assert.AreEqual(
                expectedCurrentViewModelType,
                navigationService.CurrentViewModel.GetType());
        }


        [Test, Apartment(ApartmentState.STA)]
        public async Task CloseAsync_IfBackwardsNavigationIsNotPossible_NothingHappens()
        {
            var expectedCurrentViewModelType = typeof(MainViewModel);

            RegisterViewModels();
            await navigationService.SetRootAsync<MainViewModel>();


            await navigationService.CloseAsync<SecondViewModel>();


            Assert.AreEqual(
                expectedCurrentViewModelType,
                navigationService.CurrentViewModel.GetType());
        }


        [Test, Apartment(ApartmentState.STA)]
        public void CloseAsync_IfViewModelIsNotRegistered_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(
                navigationService.CloseAsync<MainViewModel>);
        }   
    }
}