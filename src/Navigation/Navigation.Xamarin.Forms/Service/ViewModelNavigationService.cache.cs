using CodeMonkeys.Logging;

using System;
using System.Collections.Generic;
using Xamarin.Forms;


namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public partial class ViewModelNavigationService :
        IViewModelNavigationService
    {
        private static readonly IList<CachedPage> PageCache =
            new List<CachedPage>();

        /// <inheritdoc cref="CodeMonkeys.Core.Interfaces.Navigation.IViewModelNavigationService.ClearCache" />
        public void ClearCache()
        {
            LogService?.Info(
                $"{PageCache.Count} cached pages removed.");

            PageCache.Clear();
        }

        private void CreateCachedPage(
            Type pageType)
        {
            if (!Configuration.CachePageInstances)
            {
                return;
            }

            if (Configuration.PageTypesToExcludeFromCaching
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