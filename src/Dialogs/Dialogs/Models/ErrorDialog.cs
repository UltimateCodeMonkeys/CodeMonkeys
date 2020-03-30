using System;

namespace CodeMonkeys.Dialogs
{
    public class ErrorDialog : Dialog
    {
        /// <summary>
        /// The associated exception - if existent
        /// </summary>
        public Exception Exception { get; set; }
    }
}
