using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

namespace CodeMonkeys.Navigation.WPF
{
    public partial class NavigationService
    {
        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.CloseAsync{TViewModelInterface}" />
        public virtual Task CloseAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TViewModel>();


            TryGoBack();


            return Task.CompletedTask;
        }


        public virtual Task CloseAsync()
        {
            TryGoBack();

            return Task.CompletedTask;
        }


        public virtual Task CloseAllAsync()
        {
            if (Root == Current)
                return Task.CompletedTask;


            ClearStacks();

            SetCurrent(
                Root.ViewModel,
                Root.Content);


            return Task.CompletedTask;
        }


        private async Task ResolveAndInformListenerAsync(
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

        private async void OnContentUnloaded(
            object sender,
            EventArgs eventArgs)
        {
            if (!(sender is FrameworkElement content))
            {
                return;
            }

            var registrationInfo = Registrations?.FirstOrDefault(
                registration => registration.ViewType == content.GetType());

            if (registrationInfo == null)
            {
                DetachDisappearingEventListener(content);
                return;
            }


            if (content.DataContext is IHandleClosing viewModel)
            {
                await viewModel.OnClosing();
            }

            if (registrationInfo.InterestedType != null)
            {
                await ResolveAndInformListenerAsync(
                    registrationInfo.InterestedType)
                    .ConfigureAwait(false);
            }


            DetachDisappearingEventListener(content);
        }


        private void DetachDisappearingEventListener(
            FrameworkElement content)
        {
            if (content == null)
            {
                return;
            }

            content.Unloaded -= OnContentUnloaded;
        }

        #endregion View Disappearing event
    }
}