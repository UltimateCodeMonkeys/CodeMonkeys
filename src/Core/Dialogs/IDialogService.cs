using System;
using System.Threading.Tasks;

namespace CodeMonkeys.Dialogs
{
    public interface IDialogService
    {
        /// <summary>
        /// Shows an alert dialog with a button to close it.
        /// </summary>
        /// <param name="title">Title of the dialog</param>
        /// <param name="body">Body of the dialog</param>
        Task ShowAlertAsync(
            string title,
            string body);

        /// <summary>
        /// Shows an alert dialog with a button to close it.
        /// </summary>
        /// <param name="title">Title of the dialog</param>
        /// <param name="body">Body of the dialog</param>
        /// <param name="closeButtonText">The close button text</param>
        Task ShowAlertAsync(
            string title,
            string body,
            string closeButtonText);

        /// <summary>
        /// Shows an error dialog with a button to close it.
        /// </summary>
        /// <param name="title">Title of the dialog</param>
        /// <param name="body">Body of the dialog</param>
        /// <param name="exception">A exception which should be presented in the dialog</param>
        Task ShowErrorAsync(
            string title,
            string body,
            Exception exception = null);

        /// <summary>
        /// Shows an error dialog with a button to close it.
        /// </summary>
        /// <param name="title">Title of the dialog</param>
        /// <param name="body">Body of the dialog</param>
        /// <param name="closeButtonText">The close button text</param>
        /// <param name="exception">A exception which should be presented in the dialog</param>
        Task ShowErrorAsync(
            string title,
            string body,
            string closeButtonText,
            Exception exception = null);

        /// <summary>
        /// Shows an dialog which asks the user for his confirmation
        /// </summary>
        /// <param name="title">Title of the dialog</param>
        /// <param name="body">Body of the dialog</param>
        /// <returns>A bool indicator that tells whether the user has agreed or disagreed</returns>
        Task<bool> ShowConfirmationAsync(
            string title,
            string body);

        /// <summary>
        /// Shows an dialog which asks the user for his confirmation
        /// </summary>
        /// <param name="title">Title of the dialog</param>
        /// <param name="body">Body of the dialog</param>
        /// <param name="confirmButtonText">The confirm button text</param>
        /// <param name="declineButtonText">The decline button text</param>
        /// <returns>A bool indicator that tells whether the user has agreed or disagreed</returns>
        Task<bool> ShowConfirmationAsync(
            string title,
            string body,
            string confirmButtonText,
            string declineButtonText);
    }
}
