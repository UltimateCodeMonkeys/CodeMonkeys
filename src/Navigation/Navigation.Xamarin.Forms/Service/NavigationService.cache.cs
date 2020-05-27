using CodeMonkeys.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
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


        private static TPage AddOrUpdateContentCache<TPage>(
            INavigationRegistration registration)

            where TPage : Page
        {
            if (PageCache.All(cachedPage => cachedPage.Type != registration.ViewType))
            {
                CreateCachedPage(registration.ViewType);
            }

            var reference = PageCache
                .First(cachedPage => cachedPage.Type == registration.ViewType)
                .Reference;

            if (!reference.TryGetTarget(out Page view))
            {
                view = GetViewInstance<TPage>(
                    registration);

                reference.SetTarget(view);
            }


            return (TPage)view;
        }


        private static void CreateCachedPage(
            Type pageType)
        {
            if (!Configuration.CacheContent)
            {
                return;
            }

            if (Configuration.ContentTypesToExcludeFromCaching?
                .Contains(pageType) == true)
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