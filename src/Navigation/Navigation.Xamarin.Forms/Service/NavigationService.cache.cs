using CodeMonkeys.Logging;

using System;
using System.Collections.Generic;
using Xamarin.Forms;


namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class NavigationService :
        INavigationService
    {
        private static readonly IList<CachedPage> PageCache =
            new List<CachedPage>();

        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.ClearCache" />
        public void ClearCache()
        {
            Log?.Info(
                $"{PageCache.Count} cached pages removed.");

            PageCache.Clear();
        }

        private void CreateCachedPage(
            Type pageType)
        {
            if (!Configuration.CacheContent)
            {
                return;
            }

            if (Configuration.ContentTypesToExcludeFromCaching
                .Contains(pageType))
            {
                return;
            }


            var pageInstance = (Page)Activator.CreateInstance(
                pageType);

            var cachedPage = new CachedPage(pageInstance);
            PageCache.Add(cachedPage);
        }
    }
}