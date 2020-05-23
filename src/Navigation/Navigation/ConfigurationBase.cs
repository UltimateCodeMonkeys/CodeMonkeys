using System;
using System.Collections.Generic;

namespace CodeMonkeys.Navigation
{
    public class ConfigurationBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="CodeMonkeys.Navigation.INavigationService" />
        /// should cache built content instances.
        /// This is required if pages should be prebuilt (<see cref="CodeMonkeys.Navigation.INavigationService.Register{TViewModel}(Type, bool)" />
        /// </summary>
        /// <value><c>true</c> if content should be cached; otherwise, <c>false</c>. (default is <c>false</c>)</value>
        public bool CacheContent { get; set; } = false;

        /// <summary>
        /// Add content types that should not be cached
        /// </summary>
        public IList<Type> ContentTypesToExcludeFromCaching { get; set; } = new List<Type>();


        /// <summary>
        /// <para>
        /// <see cref="CodeMonkeys.Navigation.INavigationService"/> uses the <see cref="CodeMonkeys.DependencyInjection.IDependencyContainer"/> instance to create views if set to <value>true</value>.
        /// You need to register your view types in bootstrap using <see cref="CodeMonkeys.DependencyInjection.IDependencyRegister.RegisterType(Type)"/> or a similar method.
        /// </para>
        /// <para>
        /// <see cref="CodeMonkeys.Navigation.INavigationService"/> uses the <see cref="CodeMonkeys.Activator"/> to create views if set to <value>false</value>.
        /// Eventual dependencies in your view won't get resolved.
        /// </para>
        /// <para>
        /// Defaults to <value>false</value>
        /// </para>
        /// </summary>
        public bool UseDependencyInjectionForViews { get; set; } = false;


        /// <summary>
        /// <para>
        /// If <value>true</value>, you can register different view types for the same ViewModel type.
        /// You can define predicates for the registrations to define when to resolve which view type.
        /// </para>
        /// <para>
        /// <see cref="INavigationRegistration.Condition" /> is used to determine wether a registration should be used.
        /// </para>
        /// Default is <value>false</value>
        /// </summary>
        public bool AllowDifferentViewTypeRegistrationForSameViewModel { get; set; } = false;
    }
}