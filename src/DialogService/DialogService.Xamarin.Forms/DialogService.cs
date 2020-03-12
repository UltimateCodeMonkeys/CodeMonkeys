using CodeMonkeys.Core.Configuration;
using CodeMonkeys.Core.DialogService;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CodeMonkeys.DialogService.Xamarin.Forms
{
    public class DialogService :
        OptionsConsumer<DialogOptions>,
        IDialogService
    {
        private string _defaultCloseLabel;
        private string _defaultConfirmLabel;
        private string _defaultDeclineLabel;

        public DialogService(DialogOptions options)
            : base(options)
        {
            OnOptionsChanged(options);
        }

        /// <inheritdoc />
        public async Task DisplayAlertAsync(
            string title,
            string message)
        {
            await DisplayAlertAsync(
                title,
                message,
                _defaultCloseLabel);
        }

        /// <inheritdoc />
        public async Task DisplayAlertAsync(
            string title,
            string message,
            Action onDialogClosed)
        {
            await DisplayAlertAsync(
                title,
                message,
                _defaultCloseLabel,
                onDialogClosed);
        }

        /// <inheritdoc />
        public async Task DisplayAlertAsync(
            string title,
            string message,
            string closeButtonLabel,
            Action onDialogClosed = null)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                closeButtonLabel);

            onDialogClosed?.Invoke();
        }

        /// <inheritdoc />
        public async Task<bool> DisplayConfirmationAsync(
            string title,
            string message)
        {
            return await DisplayConfirmationAsync(
                title,
                message,
                _defaultConfirmLabel,
                _defaultDeclineLabel);
        }

        /// <inheritdoc />
        public async Task<bool> DisplayConfirmationAsync(
            string title,
            string message,
            string confirmButtonLabel,
            string declineButtonLabel)
        {
            return await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                confirmButtonLabel,
                declineButtonLabel);
        }

        protected override void OnOptionsChanged(DialogOptions options)
        {
            _defaultCloseLabel = options.DefaultCloseLabel;
            _defaultConfirmLabel = options.DefaultConfirmLabel;
            _defaultDeclineLabel = options.DefaultDeclineLabel;
        }
    }
}
