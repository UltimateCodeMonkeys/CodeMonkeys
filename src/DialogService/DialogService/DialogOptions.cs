using CodeMonkeys.Core.Configuration;

namespace CodeMonkeys.DialogService
{
    public class DialogOptions : Options
    {
        private string _defaultCloseLabel;
        private string _defaultConfirmLabel;
        private string _defaultDeclineLabel;

        /// <summary>
        /// <para>Defaults to <c>'OK'</c></para>
        /// <para>Value changes of this property are monitored and applied dynamically at runtime.</para>
        /// </summary>
        public string DefaultCloseLabel
        {
            get => _defaultCloseLabel;
            set => SetValue(ref _defaultCloseLabel, value);
        }

        /// <summary>
        /// <para>Defaults to <c>'OK'</c></para>
        /// <para>Value changes of this property are monitored and applied dynamically at runtime.</para>
        /// </summary>
        public string DefaultConfirmLabel
        {
            get => _defaultConfirmLabel;
            set => SetValue(ref _defaultConfirmLabel, value);
        }

        /// <summary>
        /// <para>Defaults to <c>'Cancel'</c></para>
        /// <para>Value changes of this property are monitored and applied dynamically at runtime.</para>
        /// </summary>
        public string DefaultDeclineLabel
        {
            get => _defaultDeclineLabel;
            set => SetValue(ref _defaultDeclineLabel, value);
        }

        public DialogOptions()
        {
            DefaultCloseLabel = "OK";
            DefaultConfirmLabel = "OK";
            DefaultDeclineLabel = "Cancel";
        }
    }
}
