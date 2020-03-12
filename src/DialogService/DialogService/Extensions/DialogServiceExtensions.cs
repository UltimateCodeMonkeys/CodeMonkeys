using CodeMonkeys.Core;
using CodeMonkeys.Core.DialogService;

namespace CodeMonkeys.DialogService
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
