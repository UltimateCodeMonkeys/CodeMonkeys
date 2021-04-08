using CodeMonkeys.Configuration;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CodeMonkeys.Dialogs.Xamarin.Forms
{
    public class DialogService : OptionsConsumer<DialogOptions>,
        IDialogService
    {
        /// <inheritdoc />
        public async Task ShowAlertAsync(
            string title,
            string body)
        {
            await ShowAlertAsync(
                title,
                body,
                Options.DefaultCloseLabel);
        }

        /// <inheritdoc />
        public async Task ShowAlertAsync(
            string title,
            string body,
            string closeButtonText)
        {
            if (string.IsNullOrWhiteSpace(closeButtonText))
            {
                throw new ArgumentNullException(
                    nameof(closeButtonText),
                    $"To show a alert dialog the value of parameter '{nameof(closeButtonText)}' can't be null or empty.");
            }

            await Application.Current.MainPage.DisplayAlert(
                title,
                body,
                closeButtonText);
        }

        /// <inheritdoc />
        public async Task ShowErrorAsync(
            string title, 
            string body, 
            Exception exception = null)
        {
            await ShowErrorAsync(
                title,
                body,
                Options.DefaultCloseLabel,
                exception, 
                null);
        }

        /// <inheritdoc />
        public async Task ShowErrorAsync(
            string title, 
            string body, 
            string closeButtonText, 
            Exception exception = null)
        {
            await ShowErrorAsync(
                title,
                body,
                closeButtonText,
                exception,
                null);
        }

        /// <inheritdoc />
        public async Task<bool> ShowConfirmationAsync(
            string title,
            string body)
        {
            return await ShowConfirmationAsync(
                title,
                body,
                Options.DefaultConfirmLabel,
                Options.DefaultDeclineLabel);
        }

        /// <inheritdoc />
        public async Task<bool> ShowConfirmationAsync(
            string title,
            string body,
            string confirmButtonText,
            string declineButtonText)
        {
            if (string.IsNullOrWhiteSpace(confirmButtonText))
            {
                throw new ArgumentNullException(
                    nameof(declineButtonText),
                    $"To show a confirmation dialog the value of parameter '{nameof(confirmButtonText)}' can't be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(declineButtonText))
            {
                throw new ArgumentNullException(
                    nameof(declineButtonText),
                    $"To show a confirmation dialog the value of parameter '{nameof(declineButtonText)}' can't be null or empty.");
            }

            return await Application.Current.MainPage.DisplayAlert(
                title,
                body,
                confirmButtonText,
                declineButtonText);
        }

        private async Task ShowErrorAsync(
            string title,
            string body,
            string closeButtonText,
            Exception exception = null,
            Func<string, Exception, string> formatter = null)
        {
            formatter ??= _defaultErrorFormatter;

            if (string.IsNullOrWhiteSpace(closeButtonText))
            {
                throw new ArgumentNullException(
                    nameof(closeButtonText),
                    $"To show a error dialog the value of parameter '{nameof(closeButtonText)}' can't be null or empty.");
            }

            await Application.Current.MainPage.DisplayAlert(
                title,
                formatter(body, exception),
                closeButtonText);
        }

        private readonly Func<string, Exception, string> _defaultErrorFormatter = (body, exception) =>
        {
            if (exception == null && body != null)
                return body.ToString();

            if (body == null && exception != null)
                return exception.ToString();

            return $"{body}:\n{exception}";
        };
    }
}
