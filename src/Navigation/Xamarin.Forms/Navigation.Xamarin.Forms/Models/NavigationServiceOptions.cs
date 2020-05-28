using XF = Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    /// <summary>
    /// Settings for the ViewModelNavigationService
    /// </summary>
    public class NavigationServiceOptions :
        NavigationServiceOptionsBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the
        /// <see cref="CodeMonkeys.Navigation.Xamarin.Forms.NavigationService" /> should use animations.
        /// More information:
        /// <see cref="XF.INavigation.PushAsync(XF.Page, bool)" />
        /// <see cref="XF.INavigation.PopAsync(bool)" />
        /// </summary>
        /// <value><c>true</c> if use animations; otherwise, <c>false</c>. (default is <c>false</c>)</value>
        public bool UseAnimations { get; set; } = true;


        /// <summary>
        /// Possibility to configure the navigation behavior when using the MasterDetail pattern
        /// </summary>
        public MasterDetailConfiguration MasterDetailConfig { get; set; }
            = new MasterDetailConfiguration();
    }


    public class MasterDetailConfiguration
    {
        public bool HideMenuOnPageSwitch { get; set; } = true;
    }
}