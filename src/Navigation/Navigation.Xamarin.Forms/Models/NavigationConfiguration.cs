using System;
using System.Collections.Generic;

using XF = Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    /// <summary>
    /// Settings for the ViewModelNavigationService
    /// </summary>
    public class NavigationConfiguration
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
        /// Gets or sets a value indicating whether the <see cref="CodeMonkeys.Navigation.Xamarin.Forms.NavigationService" />
        /// should cache built page instances.
        /// This is required if pages should be prebuilt (<see cref="CodeMonkeys.Navigation.Xamarin.Forms.NavigationService.Register{TViewModel}(Type, bool)" />
        /// </summary>
        /// <value><c>true</c> if use animations; otherwise, <c>false</c>. (default is <c>false</c>)</value>
        public bool CachePageInstances { get; set; } = true;

        /// <summary>
        /// The page types to exclude from caching.
        /// </summary>
        public IList<Type> PageTypesToExcludeFromCaching = new List<Type>();


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