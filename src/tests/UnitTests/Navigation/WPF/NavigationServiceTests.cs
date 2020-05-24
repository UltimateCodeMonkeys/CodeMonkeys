using CodeMonkeys.DependencyInjection;
using CodeMonkeys.Navigation;
using CodeMonkeys.Navigation.WPF;
using CodeMonkeys.UnitTests.Navigation.ViewModels;
using CodeMonkeys.UnitTests.Navigation.WPF.Views;

using Moq;
using NUnit.Framework;

using System.Threading;
using System.Threading.Tasks;

namespace CodeMonkeys.UnitTests.Navigation.WPF
{
    public partial class NavigationServiceTests
    {
        private readonly Mock<IDependencyContainer> _dependencyContainerMock =
            new Mock<IDependencyContainer>();


        private CodeMonkeys.Navigation.WPF.INavigationService navigationService;



        [SetUp]
        public void Setup()
        {
            navigationService = new NavigationService(
                _dependencyContainerMock.Object);


            _dependencyContainerMock
                .Setup(container => container.Resolve<MainViewModel>())
                .Returns(new MainViewModel());

            _dependencyContainerMock
                .Setup(container => container.Resolve<SecondViewModel>())
                .Returns(new SecondViewModel());

            _dependencyContainerMock
                .Setup(container => container.Resolve<ParameterViewModel>())
                .Returns(new ParameterViewModel());
        }

        [TearDown]
        public void Clean()
        {
            NavigationService.Configuration.AllowDifferentViewTypeRegistrationForSameViewModel = false;

            navigationService.ResetRegistrations();
        }



        [Test, Apartment(ApartmentState.STA)]
        public async Task SetRootAsync_IfViewModelIsRegistered_RootViewModelAndCurrentViewModelAreSet()
        {
            var expectedRootViewModelType = typeof(MainViewModel);
            navigationService.Register<MainViewModel, MainPage>();


            await navigationService.SetRootAsync<MainViewModel>();


            Assert.AreEqual(
                expectedRootViewModelType,
                navigationService.RootViewModel.GetType());

            Assert.AreEqual(
                expectedRootViewModelType,
                navigationService.CurrentViewModel.GetType());
        }


        private void RegisterViewModels()
        {
            navigationService.Register<MainViewModel, MainPage>();
            navigationService.Register<SecondViewModel, SecondPage>();
            navigationService.Register<ParameterViewModel, ParameterPage>();
        }
    }
}