using CodeMonkeys.Logging;

using System;
using System.Collections.Generic;
using System.Windows;

namespace CodeMonkeys.Navigation.WPF
{
    public partial class NavigationService
    {
        private static readonly IList<CachedContent> ContentCache =
            new List<CachedContent>();


        /// <inheritdoc cref="CodeMonkeys.Navigation.INavigationService.ClearCache" />
        public void ClearCache()
        {
            Log?.Info(
                $"{ContentCache.Count} cached pages removed.");

            ContentCache.Clear();
        }

        private void CreateCachedContent(
            Type viewType)
        {
            if (!Configuration.CacheContent)
            {
                return;
            }

            if (Configuration.ContentTypesToExcludeFromCaching
                .Contains(viewType))
            {
                return;
            }

            var content = (FrameworkElement) Activator.CreateInstance(
                viewType);

            var cachedContent = new CachedContent(content);
            ContentCache.Add(cachedContent);
        }
    }
}