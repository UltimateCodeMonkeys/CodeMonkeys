using CodeMonkeys.Configuration;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CodeMonkeys.Dialogs.Xamarin.Forms
{
    public class DialogService :
        OptionsConsumer<DialogOptions>,
        IDialogService
    {
        private readonly Func<string, Exception, string> _defaultErrorFormatter = (message, exception) =>
        {
            if (exception == null && message != null)
                return message.ToString();

            if (message == null && exception != null)
                return exception.ToString();

            return $"{message}:\n{exception}";
        };

        /// <inheritdoc />
        public async Task ShowAlertAsync(
            string title,
            string message)
        {
            await ShowAlertAsync(
                title,
                message,
                Options.DefaultCloseLabel);
        }

        /// <inheritdoc />
        public async Task ShowAlertAsync(
            string title,
            string message,
            Action onDialogClosed)
        {
            await ShowAlertAsync(
                title,
                message,
                Options.DefaultCloseLabel,
                onDialogClosed);
        }

        /// <inheritdoc />
        public async Task ShowAlertAsync(
            string title,
            string message,
            string closeButtonLabel,
            Action onDialogClosed = null)
        {
            if (string.IsNullOrWhiteSpace(closeButtonLabel))
            {
                throw new ArgumentNullException(
                    nameof(closeButtonLabel),
                    $"To show a alert dialog the value of parameter '{nameof(closeButtonLabel)}' can't be null or empty.");
            }

            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                closeButtonLabel);

            onDialogClosed?.Invoke();
        }

        /// <inheritdoc />
        public async Task ShowErrorAsync(
            string title, 
            string message, 
            Exception exception = null)
        {
            await ShowErrorAsync(
                title,
                message,
                Options.DefaultCloseLabel,
                exception, 
                null,
                null);
        }

        /// <inheritdoc />
        public async Task ShowErrorAsync(
            string title, 
            string message, 
            Exception exception = null, 
            Action closeCallback = null)
        {
            await ShowErrorAsync(
                title,
                message,
                Options.DefaultCloseLabel,
                exception,
                null,
                closeCallback);
        }

        /// <inheritdoc />
        public async Task ShowErrorAsync(
            string title, 
            string message, 
            string closeButtonLabel, 
            Exception exception = null, 
            Action closeCallback = null)
        {
            await ShowErrorAsync(
                title,
                message,
                closeButtonLabel,
                exception,
                null,
                closeCallback);
        }

        /// <inheritdoc />
        public async Task<bool> ShowConfirmationAsync(
            string title,
            string message)
        {
            return await ShowConfirmationAsync(
                title,
                message,
                Options.DefaultConfirmLabel,
                Options.DefaultDeclineLabel);
        }

        /// <inheritdoc />
        public async Task<bool> ShowConfirmationAsync(
            string title,
            string message,
            string confirmButtonLabel,
            string declineButtonLabel)
        {
            if (string.IsNullOrWhiteSpace(confirmButtonLabel))
            {
                throw new ArgumentNullException(
                    nameof(declineButtonLabel),
                    $"To show a confirmation dialog the value of parameter '{nameof(confirmButtonLabel)}' can't be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(declineButtonLabel))
            {
                throw new ArgumentNullException(
                    nameof(declineButtonLabel),
                    $"To show a confirmation dialog the value of parameter '{nameof(declineButtonLabel)}' can't be null or empty.");
            }

            return await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                confirmButtonLabel,
                declineButtonLabel);
        }

        private async Task ShowErrorAsync(
            string title,
            string message,
            string closeButtonLabel,
            Exception exception = null,
            Func<string, Exception, string> formatter = null,
            Action closedCallback = null)
        {
            formatter ??= _defaultErrorFormatter;

            if (string.IsNullOrWhiteSpace(closeButtonLabel))
            {
                throw new ArgumentNullException(
                    nameof(closeButtonLabel),
                    $"To show a error dialog the value of parameter '{nameof(closeButtonLabel)}' can't be null or empty.");
            }

            await Application.Current.MainPage.DisplayAlert(
                title,
                formatter(message, exception),
                closeButtonLabel);

            closedCallback?.Invoke();
        }
    }
}
