﻿using System;
using System.Collections.Generic;

namespace CodeMonkeys.Navigation.WPF
{
    public class Configuration
    {
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