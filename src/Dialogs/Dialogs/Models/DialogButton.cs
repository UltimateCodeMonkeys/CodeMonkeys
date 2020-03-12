using System.Windows.Input;

namespace CodeMonkeys.Dialogs
{
    public class DialogButton
    {
        /// <summary>
        /// The text of the button
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Action to invoke on button click
        /// </summary>
        public ICommand Action { get; set; }
    }
}
