using CodeMonkeys.Navigation;
using CodeMonkeys.Navigation.WPF;
using CodeMonkeys.UnitTests.Navigation.ViewModels;
using CodeMonkeys.UnitTests.Navigation.WPF.Views;

using NUnit.Framework;

using System.Linq;

namespace CodeMonkeys.UnitTests.Navigation.WPF
{
    public partial class NavigationServiceTests
    {
        [Test]
        public void Register_WhenThereIsNoRegistrationWithSameTypes_AddsRegistration()
        {
            int expectedRegistrationsCount = 3;


            navigationService.Register<MainViewModel, MainPage>();
            navigationService.Register<SecondViewModel, SecondPage>();
            navigationService.Register<ParameterViewModel, ParameterPage>();


            Assert.AreEqual(
                expectedRegistrationsCount,
                navigationService.Registrations.Count);
        }

        [Test]
        public void Register_WhenThereIsARegistrationWithTheSameType_RemovesOldRegistrationAndAddsNew()
        {
            var registeredMainViewModelPageType = typeof(MainPage);


            navigationService.Register<MainViewModel, SecondPage>();
            navigationService.Register<MainViewModel, MainPage>();


            var registrationInfo = navigationService.Registrations
                .FirstOrDefault(registration => registration.ViewModelType == typeof(MainViewModel));

            Assert.AreEqual(
                registeredMainViewModelPageType,
                registrationInfo.ViewType);
        }

        [Test]
        public void Register_WhenThereIsARegistrationWithTheSameTypeButMultipleRegistrationsAreAllowed_KeepsBoth()
        {
            int expectedRegistrationsCount = 2;
            NavigationService.Configuration.AllowDifferentViewTypeRegistrationForSameViewModel = true;


            navigationService.Register<MainViewModel, SecondPage>();
            navigationService.Register<MainViewModel, MainPage>();


            Assert.AreEqual(
                expectedRegistrationsCount,
                navigationService.Registrations.Count);
        }


        [Test]
        public void Unregister_WhenThereIsARegistration_RemovesRegistration()
        {
            int expectedRegistrationsCount = 0;
            navigationService.Register<MainViewModel, MainPage>();


            navigationService.Unregister<MainViewModel>();


            Assert.AreEqual(
                expectedRegistrationsCount,
                navigationService.Registrations.Count);
        }

        [Test]
        public void Unregister_WhenThereIsNoMatchingRegistration_NothingHappens()
        {
            int expectedRegistrationsCount = 1;
            navigationService.Register<MainViewModel, MainPage>();


            navigationService.Unregister<SecondViewModel>();


            Assert.AreEqual(
                expectedRegistrationsCount,
                navigationService.Registrations.Count);
        }
    }
}