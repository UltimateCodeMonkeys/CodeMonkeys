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
        public DialogService(DialogOptions options)
            : base(options)
        {

        }

        protected override void OnOptionsChanged(DialogOptions options)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task DisplayAlertAsync(
            string title,
            string message)
        {
            throw new NotImplementedException();
            //await DisplayAlertAsync(
            //    title,
            //    message,
            //    Configuration.AlertDialog.DefaultCloseLabel);
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
                Configuration.AlertDialog.DefaultCloseLabel,
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

    }
}
