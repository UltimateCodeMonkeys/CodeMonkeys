using CodeMonkeys.Configuration;

namespace CodeMonkeys.Dialogs
{
    public class DialogOptions : Options
    {
        /// <summary>
        /// <para>Defaults to <c>'OK'</c></para>
        /// <para>Value changes of this property are monitored and applied dynamically at runtime.</para>
        /// </summary>
        public string CloseButtonText
        {
            get => GetValue(defaultValue: "OK");
            set => SetValue(value);
        }

        /// <summary>
        /// <para>Defaults to <c>'OK'</c></para>
        /// <para>Value changes of this property are monitored and applied dynamically at runtime.</para>
        /// </summary>
        public string ConfirmButtonText
        {
            get => GetValue(defaultValue: "OK");
            set => SetValue(value);
        }

        /// <summary>
        /// <para>Defaults to <c>'Cancel'</c></para>
        /// <para>Value changes of this property are monitored and applied dynamically at runtime.</para>
        /// </summary>
        public string DeclineButtonText
        {
            get => GetValue(defaultValue: "Cancel");
            set => SetValue(value);
        }
    }
}
