using System;
using System.Collections.Generic;

using XF = Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms.Models
{
    /// <summary>
    /// Settings for the ViewModelNavigationService
    /// </summary>
    public class NavigationConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether the
        /// <see cref="CodeMonkeys.Navigation.Xamarin.Forms.ViewModelNavigationService" /> should use animations.
        /// More information:
        /// <see cref="XF.INavigation.PushAsync(XF.Page, bool)" />
        /// <see cref="XF.INavigation.PopAsync(bool)" />
        /// </summary>
        /// <value><c>true</c> if use animations; otherwise, <c>false</c>. (default is <c>false</c>)</value>
        public bool UseAnimations { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="CodeMonkeys.Navigation.Xamarin.Forms.ViewModelNavigationService" />
        /// should cache built page instances.
        /// This is required if pages should be prebuilt (<see cref="CodeMonkeys.Navigation.Xamarin.Forms.ViewModelNavigationService.Register{TViewModel}(Type, bool)" />
        /// </summary>
        /// <value><c>true</c> if use animations; otherwise, <c>false</c>. (default is <c>false</c>)</value>
        public bool CachePageInstances { get; set; } = true;

        /// <summary>
        /// The page types to exclude from caching.
        /// </summary>
        public IList<Type> PageTypesToExcludeFromCaching = new List<Type>();
    }
}
