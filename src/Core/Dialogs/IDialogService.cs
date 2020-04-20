using System;
using System.Threading.Tasks;

namespace CodeMonkeys.Dialogs
{
    public interface IDialogService
    {
        /// <summary>
        /// Shows an alert dialog that has only one button to close the dialog
        /// </summary>
        /// <param name="title">Title text of the dialog</param>
        /// <param name="message">The message to display in the dialog</param>
        Task ShowAlertAsync(
            string title,
            string message);

        /// <summary>
        /// Shows an alert dialog that has only one button to close the dialog
        /// </summary>
        /// <param name="title">Title text of the dialog</param>
        /// <param name="message">The message to display in the dialog</param>
        /// <param name="onDialogClosed">Action to invoke after the dialog has been closed</param>
        Task ShowAlertAsync(
            string title,
            string message,
            Action onDialogClosed);

        /// <summary>
        /// Shows an alert dialog that has only one button to close the dialog
        /// Invokes the given action when the dialog has been closed
        /// </summary>
        /// <param name="title">Title text of the dialog</param>
        /// <param name="message">The message to display in the dialog</param>
        /// <param name="closeButtonLabel">The text of the button</param>
        /// <param name="onDialogClosed">Action to invoke after the dialog has been closed</param>
        Task ShowAlertAsync(
            string title,
            string message,
            string closeButtonLabel,
            Action onDialogClosed = null);

        /// <summary>
        /// Shows an error dialog that has only one button to close the dialog
        /// </summary>
        /// <param name="title">Title text of the dialog</param>
        /// <param name="message">The message to display in the dialog</param>
        /// <param name="exception">The occured exception which will be embedded in the message</param>
        Task ShowErrorAsync(
            string title,
            string message,
            Exception exception = null);

        /// <summary>
        /// Shows an error dialog that has only one button to close the dialog
        /// </summary>
        /// <param name="title">Title text of the dialog</param>
        /// <param name="message">The message to display in the dialog</param>
        /// <param name="exception">The occured exception which will be embedded in the message</param>
        /// <param name="closeCallback">Action to invoke after the dialog has been closed</param>
        Task ShowErrorAsync(
            string title,
            string message,
            Exception exception = null,
            Action closeCallback = null);

        /// <summary>
        /// Shows an error dialog that has only one button to close the dialog
        /// Invokes the given action when the dialog has been closed
        /// </summary>
        /// <param name="title">Title text of the dialog</param>
        /// <param name="message">The message to display in the dialog</param>
        /// <param name="closeButtonLabel">The text of the button</param>
        /// <param name="exception">The occured exception which will be embedded in the message</param>
        /// <param name="closeCallback">Action to invoke after the dialog has been closed</param>
        Task ShowErrorAsync(
            string title,
            string message,
            string closeButtonLabel,
            Exception exception = null,
            Action closeCallback = null);

        /// <summary>
        /// Shows a confirmation dialog that asks the user wether some action should proceed or not
        /// </summary>
        /// <param name="title">Title text of the dialog</param>
        /// <param name="message">Message to display in the dialog / question to ask</param>
        /// <returns>A bool indicating wether the request has been confirmed or declined</returns>
        Task<bool> ShowConfirmationAsync(
            string title,
            string message);

        /// <summary>
        /// Shows a confirmation dialog that asks the user wether some action should proceed or not
        /// </summary>
        /// <param name="title">Title text of the dialog</param>
        /// <param name="message">Message to display in the dialog / question to ask</param>
        /// <param name="confirmButtonLabel">Text of the button for positive feedback</param>
        /// <param name="declineButtonLabel">Text of the button for negative feedback</param>
        /// <returns>A bool indicating wether the request has been confirmed or declined</returns>
        Task<bool> ShowConfirmationAsync(
            string title,
            string message,
            string confirmButtonLabel,
            string declineButtonLabel);
    }
}
