using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class NavigationService
    {
        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.CloseAsync{TViewModel}" />
        public virtual async Task CloseAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            if (!TryGetRegistration(
                typeof(TViewModel),
                out var registration))
            {
                throw new InvalidOperationException();
            }

            if (!Navigation.NavigationStack.Any(
                page => page.GetType() == registration.ViewType))
            {
                return;
            }


            await ResolveAndInformListener(
                registration.InterestedType);


            await CloseCurrentPage()
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="INavigationService.CloseAllAsync" />
        public virtual async Task CloseAllAsync()
        {
            await PopToRootAsync()
                .ConfigureAwait(false);
        }


        /// <inheritdoc cref="INavigationService.CloseModalAsync" />
        public virtual async Task CloseModalAsync<TViewModel>()
        {
            if (Navigation == null)
            {
                Log?.Error(
                    $"'{nameof(Navigation)}' is null!");
                return;
            }


            await Navigation.PopModalAsync(
                animated: Configuration.UseAnimations);
        }


        private async Task CloseCurrentPage()
        {
            await Navigation.PopAsync(
                animated: Configuration.UseAnimations);

            Log?.Info(
                "Page has been removed from Xamarin navigation stack.");
        }

        private async Task PopToRootAsync()
        {
            await Navigation.PopToRootAsync(
                animated: Configuration.UseAnimations);

            Log?.Info(
                $"Navigation stack cleared.");
        }


        private async Task ResolveAndInformListener(
            Type listenerType)
        {
            if (listenerType == null)
            {
                return;
            }


            var listener = dependencyResolver.Resolve<IInterestedInClosing>(
                listenerType);


            Log?.Info(
                $"ViewModelInstance for type {listener?.GetType().Name} has been resolved.");


            await listener?.OnInterestedViewModelClosingAsync();
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

            var registrationInfo = Registrations?.FirstOrDefault(
                registration => registration.ViewType == page.GetType());

            if (registrationInfo == null)
            {
                DetachDisappearingEventListener(
                    page);

                return;
            }


            if (page.BindingContext is IHandleClosing viewModel)
            {
                await viewModel.OnClosing();
            }

            if (registrationInfo.InterestedType != null)
            {
                await ResolveAndInformListener(
                    registrationInfo.InterestedType);
            }


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
    }
}