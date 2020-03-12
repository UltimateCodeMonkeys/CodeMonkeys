using CodeMonkeys.Core;
using CodeMonkeys.Core.Dialogs;

namespace CodeMonkeys.Dialogs
{
    public static class DialogServiceExtensions
    {
        public static void ShowAlertAsync(
            this IDialogService _this,
            AlertDialog dialog)
        {
            Argument.NotNull(
                dialog, 
                nameof(dialog));

            _this.ShowAlertAsync(
                dialog.Title,
                dialog.Message,
                dialog.ConfirmButton?.Label,
                () => dialog.ConfirmButton?.Action?.Execute(null));
        }

        public static void ShowErrorAsync(
            this IDialogService _this,
            ErrorDialog dialog)
        {
            Argument.NotNull(
                dialog,
                nameof(dialog));

            _this.ShowErrorAsync(
                dialog.Title,
                dialog.Message,
                dialog.ConfirmButton?.Label,
                dialog.Exception,
                () => dialog.ConfirmButton?.Action?.Execute(null));

        }

        public static void ShowConfirmationAsync(
            this IDialogService _this,
            ConfirmationDialog dialog)
        {
            Argument.NotNull(
                dialog,
                nameof(dialog));

            _this.ShowConfirmationAsync(
                dialog.Title,
                dialog.Message,
                dialog.ConfirmButton?.Label,
                dialog.DeclineButton?.Label);
        }
    }
}
