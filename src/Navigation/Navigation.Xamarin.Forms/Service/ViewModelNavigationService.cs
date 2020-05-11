using CodeMonkeys.DependencyInjection;
using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class ViewModelNavigationService :
        IViewModelNavigationService
    {
        public event EventHandler<IViewModel> CurrentViewModelChanged;



        private static readonly SemaphoreSlim _semaphore =
            new SemaphoreSlim(1, 1);

        private static IDependencyResolver dependencyResolver;
        protected static ILogService LogService;              


        public static Configuration Configuration { get; set; } =
            new Configuration();


        private Page rootPage;
        protected Page RootPage
        {
            get
            {
                if (rootPage != null)
                {
                    return rootPage;
                }

                rootPage = Application.Current.MainPage;

                if (Application.Current.MainPage is NavigationPage navigationPage)
                {
                    rootPage = navigationPage.RootPage;
                }

                return rootPage;
            }
        }

        protected INavigation Navigation => RootPage.Navigation;

        protected Page CurrentPage => Navigation?.NavigationStack.Last();


        public IReadOnlyCollection<INavigationRegistration> Registrations =>
            new ReadOnlyCollection<INavigationRegistration>(NavigationRegistrations);

        public ViewModelNavigationService(
            IDependencyResolver resolver)
        {
            SetResolverInstance(
                resolver);
        }

        private void SetResolverInstance(
            IDependencyResolver resolver)
        {
            if (dependencyResolver != null)
            {
                return;
            }

            _semaphore.Wait();

            if (dependencyResolver == null)
            {
                dependencyResolver = resolver;
            }

            _semaphore.Release();
        }


        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.SetRoot{TViewModelInterface}(TViewModelInterface)" />
        public void SetRoot<TViewModel>(
            TViewModel dataContext)

            where TViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TViewModel>();

            var page = CreateViewInstance<TViewModel>(
                dataContext);

            Application.Current.MainPage = new NavigationPage(
                page);
        }        

        private bool IsRoot<TDestinationViewModel>()
            where TDestinationViewModel : class, IViewModel
        {
            if (!IsRegistered(typeof(TDestinationViewModel)))
            {
                return false;
            }

            var registrationInfo = NavigationRegistrations.First(
                registration => registration.ViewModelType == typeof(TDestinationViewModel));

            return registrationInfo?.ViewType == RootPage.GetType();
        }

        private async Task PopToRootAsync()
        {
            await Navigation.PopToRootAsync(
                animated: Configuration.UseAnimations);

            LogService?.Info(
                $"Navigation stack cleared.");
        }
        

        private void RaiseCurrentViewModelChanged(
            IViewModel current)
        {
            var threadSafeCall = CurrentViewModelChanged;

            threadSafeCall?.Invoke(
                this,
                current);
        }


        #region View Disappearing event
        private async void OnViewClosing(
            object sender,
            EventArgs eventArgs)
        {
            if (!(sender is Page page))
            {
                return;
            }

            if (Registrations == null ||
                !Registrations.Any(
                    registration => registration.ViewType == page.GetType()))
            {
                DetachDisappearingEventListener(page);
                return;
            }

            if (!(page.BindingContext is IHandleClosing viewModel))
            {
                DetachDisappearingEventListener(page);
                return;
            }

            await viewModel.OnClosing();

            DetachDisappearingEventListener(page);
        }


        private void DetachDisappearingEventListener(
            Page closedPage)
        {
            if (closedPage == null)
            {
                return;
            }

            closedPage.Disappearing -= OnViewClosing;
        }
        #endregion



        public static void SetupLogging(
            ILogService logService)
        {
            LogService = logService;
        }
    }
}