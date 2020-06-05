using System;
using System.Collections.Generic;

namespace CodeMonkeys.Navigation
{
    public class NavigationServiceOptionsBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="CodeMonkeys.Navigation.INavigationService" />
        /// should cache built content instances.
        /// This is required if pages should be prebuilt. />
        /// </summary>
        /// <value>true</value> if content should be cached; otherwise, <value>false</value>
        /// (default is <value>false</value>)
        public bool CacheContent { get; set; }

        /// <summary>
        /// Add content types that should not be cached
        /// </summary>
        public IList<Type> ContentTypesToExcludeFromCaching { get; set; } = new List<Type>();


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
        public bool AllowDifferentViewTypeRegistrationForSameViewModel { get; set; }



        protected NavigationServiceOptionsBase()
        {
        }
    }
}