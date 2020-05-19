using System;
using System.Collections.Generic;

using XF = Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    /// <summary>
    /// Settings for the ViewModelNavigationService
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// <para>
        /// <see cref="CodeMonkeys.Navigation.Xamarin.Forms.NavigationService"/> uses the <see cref="CodeMonkeys.DependencyInjection.IDependencyContainer"/> instance to create views if set to <value>true</value>.
        /// You need to register your view types in bootstrap using <see cref="CodeMonkeys.DependencyInjection.IDependencyRegister.RegisterType(Type)"/> or a similar method.
        /// </para>
        /// <para>
        /// <see cref="CodeMonkeys.Navigation.Xamarin.Forms.NavigationService"/> uses the <see cref="CodeMonkeys.Activator"/> to create views if set to <value>false</value>.
        /// Eventual dependencies in your view won't get resolved.
        /// </para>
        /// <para>
        /// Defaults to <value>false</value>
        /// </para>
        /// </summary>
        public bool UseDependencyInjectionForViews { get; set; } = false;


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