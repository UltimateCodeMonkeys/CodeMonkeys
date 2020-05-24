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
        public async Task ShowAsync_IfViewModelIsRegistered_ViewIsCreatedAndContentIsSet()
        {
            var expectedCurrentViewModelType = typeof(MainViewModel);
            RegisterViewModels();


            await navigationService.ShowAsync<MainViewModel>();


            Assert.AreEqual(
                expectedCurrentViewModelType,
                navigationService.CurrentViewModel.GetType());
        }


        [Test, Apartment(ApartmentState.STA)]
        public async Task ShowAsync_IfViewModelIsRegistered_ViewModelIsInitialized()
        {
            var expectedCurrentViewModelType = typeof(MainViewModel);
            RegisterViewModels();


            await navigationService.ShowAsync<MainViewModel>();


            Assert.IsTrue(
                navigationService.CurrentViewModel.IsInitialized);
        }


        [Test, Apartment(ApartmentState.STA)]
        public void ShowAsync_IfViewModelIsNotRegistered_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(
                navigationService.ShowAsync<MainViewModel>);
        }


        [Test, Apartment(ApartmentState.STA)]
        public void TryGoBack_IfBackStackIsEmpty_ReturnsFalse()
        {
            bool navigatedBackwards = navigationService.TryGoBack();

            Assert.IsFalse(navigatedBackwards);
        }


        [Test, Apartment(ApartmentState.STA)]
        public async Task TryGoBack_IfBackStackContainsEntry_SetsCurrentViewModelAndReturnsTrue()
        {
            var expectedCurrentViewModelType = typeof(MainViewModel);

            RegisterViewModels();

            await navigationService.SetRootAsync<MainViewModel>();
            await navigationService.ShowAsync<SecondViewModel>();


            bool navigatedBackwards = navigationService.TryGoBack();


            Assert.IsTrue(navigatedBackwards);
            Assert.AreEqual(
                expectedCurrentViewModelType,
                navigationService.CurrentViewModel.GetType());
        }


        [Test]
        public void TryGoForward_IfForwardStackIsEmpty_ReturnsFalse()
        {
            bool navigatedForward = navigationService.TryGoForward();

            Assert.IsFalse(navigatedForward);
        }


        [Test, Apartment(ApartmentState.STA)]
        public async Task TryGoForward_IfForwardStackContainsEntry_SetsCurrentViewModelAndReturnsTrue()
        {
            var expectedCurrentViewModelType = typeof(SecondViewModel);

            RegisterViewModels();

            await navigationService.SetRootAsync<MainViewModel>();
            await navigationService.ShowAsync<SecondViewModel>();
            navigationService.TryGoBack();


            bool navigatedForward = navigationService.TryGoForward();


            Assert.IsTrue(navigatedForward);
            Assert.AreEqual(
                expectedCurrentViewModelType,
                navigationService.CurrentViewModel.GetType());
        }


        [Test]
        public void CanGoBack_WhenBackStackIsEmpty_ReturnsFalse()
        {
            bool canNavigateBackwards = navigationService.CanGoBack();


            Assert.IsFalse(canNavigateBackwards);
        }


        [Test, Apartment(ApartmentState.STA)]
        public async Task CanGoBack_WhenBackStackIsNotEmpty_ReturnsTrue()
        {
            RegisterViewModels();
            await navigationService.SetRootAsync<MainViewModel>();
            await navigationService.ShowAsync<SecondViewModel>();


            bool canNavigateBackwards = navigationService.CanGoBack();


            Assert.IsFalse(canNavigateBackwards);
        }


        [Test]
        public void CanGoForward_WhenForwardStackIsEmpty_ReturnsFalse()
        {
            bool canNavigateBackwards = navigationService.CanGoForward();


            Assert.IsFalse(canNavigateBackwards);
        }


        [Test, Apartment(ApartmentState.STA)]
        public async Task CanGoForward_WhenForwardStackIsNotEmpty_ReturnsTrue()
        {
            RegisterViewModels();
            await navigationService.SetRootAsync<MainViewModel>();
            await navigationService.ShowAsync<SecondViewModel>();
            navigationService.TryGoBack();


            bool canNavigateBackwards = navigationService.CanGoForward();


            Assert.IsFalse(canNavigateBackwards);
        }
    }
}