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
                Options.CloseButtonText);
        }

        /// <inheritdoc />
        public async Task ShowAlertAsync(
            string title,
            string body,
            string closeButtonText)
        {
            Argument.NotEmptyOrWhiteSpace(
                closeButtonText,
                nameof(closeButtonText),
                $"To show a dialog with a custom close button text the parameter '{nameof(closeButtonText)}' can't be null or empty.");

            await Application.Current.MainPage
                .DisplayAlert(
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
                Options.CloseButtonText,
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
                Options.ConfirmButtonText,
                Options.DeclineButtonText);
        }

        /// <inheritdoc />
        public async Task<bool> ShowConfirmationAsync(
            string title,
            string body,
            string confirmButtonText,
            string declineButtonText)
        {
            Argument.NotEmptyOrWhiteSpace(
                confirmButtonText,
                nameof(confirmButtonText),
                $"To show a dialog with a custom confirm button text the parameter '{nameof(confirmButtonText)}' can't be null or empty.");

            Argument.NotEmptyOrWhiteSpace(
                declineButtonText,
                nameof(declineButtonText),
                $"To show a dialog with a custom decline button text the parameter '{nameof(declineButtonText)}' can't be null or empty.");

            return await Application.Current.MainPage
                .DisplayAlert(
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

            Argument.NotEmptyOrWhiteSpace(
                closeButtonText,
                nameof(closeButtonText),
                $"To show a dialog with a custom close button text the parameter '{nameof(closeButtonText)}' can't be null or empty.");

            await Application.Current.MainPage
                .DisplayAlert(
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
