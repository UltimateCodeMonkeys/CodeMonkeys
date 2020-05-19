using System;
using System.Collections.Generic;

namespace CodeMonkeys.Navigation.WPF
{
    public class Configuration
    {
        /// <summary>
        /// <para>
        /// <see cref="CodeMonkeys.Navigation.WPF.NavigationService"/> uses the <see cref="CodeMonkeys.DependencyInjection.IDependencyContainer"/> instance to create views if set to <value>true</value>.
        /// You need to register your view types in bootstrap using <see cref="CodeMonkeys.DependencyInjection.IDependencyRegister.RegisterType(Type)"/> or a similar method.
        /// </para>
        /// <para>
        /// <see cref="CodeMonkeys.Navigation.WPF.NavigationService"/> uses the <see cref="CodeMonkeys.Activator"/> to create views if set to <value>false</value>.
        /// Eventual dependencies in your view won't get resolved.
        /// </para>
        /// <para>
        /// Defaults to <value>false</value>
        /// </para>
        /// </summary>
        public bool UseDependencyInjectionForViews { get; set; } = false;


        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="CodeMonkeys.Navigation.WPF.NavigationService" />
        /// should cache built content instances.
        /// This is required if pages should be prebuilt (<see cref="CodeMonkeys.Navigation.WPF.NavigationService.Register{TViewModel}(Type, bool)" />
        /// </summary>
        /// <value><c>true</c> if content should be cached; otherwise, <c>false</c>. (default is <c>false</c>)</value>
        public bool CacheContent { get; set; } = false;

        /// <summary>
        /// Add content types that should not be cached
        /// </summary>
        public IList<Type> ContentTypesToExcludeFromCaching { get; set; } = new List<Type>();
    }
}