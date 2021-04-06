namespace CodeMonkeys.Logging.AppCenter
{
    public class AppCenterLogOptions : LogOptions
    {
        /// <summary>
        /// The app center api key for iOS.
        /// </summary>
        public string IOSApiKey
        {
            get => GetValue<string>(defaultValue: string.Empty);
            set => SetValue(value);
        }

        /// <summary>
        /// The app center api key for Android.
        /// </summary>
        public string AndroidApiKey
        {
            get => GetValue<string>(defaultValue: string.Empty);
            set => SetValue(value);
        }

        /// <summary>
        /// The app center api key for UWP.
        /// </summary>
        public string UWPApiKey
        {
            get => GetValue<string>(defaultValue: string.Empty);
            set => SetValue(value);
        }
    }
}
