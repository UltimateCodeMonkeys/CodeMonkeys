namespace CodeMonkeys.Dialogs
{
    public abstract class Dialog
    {
        /// <summary>
        /// The title of the dialog
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The message text of the dialog
        /// </summary>
        public string Message { get; set; }


        public DialogButton ConfirmButton { get; set; }
    }
}
