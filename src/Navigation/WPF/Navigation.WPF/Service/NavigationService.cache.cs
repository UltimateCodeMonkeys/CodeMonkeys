using CodeMonkeys.Logging;

using System.Collections.Generic;
using System.Linq;
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


        private static void CreateCachedContent(
            INavigationRegistration registration)
        {
            if (!Configuration.CacheContent)
            {
                return;
            }

            if (Configuration.ContentTypesToExcludeFromCaching?
                .Contains(registration.ViewType) == true)
            {
                return;
            }

            var content = GetContentInstance<FrameworkElement>(
                registration);

            var cachedContent = new CachedContent(content);
            ContentCache.Add(cachedContent);
        }


        private static TContent AddOrUpdateContentCache<TContent>(
            INavigationRegistration registration)

            where TContent : FrameworkElement
        {
            if (ContentCache.All(cachedPage => cachedPage.Type != registration.ViewType))
            {
                CreateCachedContent(registration);
            }

            var reference = ContentCache
                .First(cachedPage => cachedPage.Type == registration.ViewType)
                .Reference;

            if (!reference.TryGetTarget(out FrameworkElement view))
            {
                view = GetContentInstance<TContent>(
                    registration);

                reference.SetTarget(view);
            }


            return (TContent)view;
        }
    }
}