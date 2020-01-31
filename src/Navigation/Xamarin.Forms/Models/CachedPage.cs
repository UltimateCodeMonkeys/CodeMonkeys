using System;

namespace CodeMonkeys.Navigation.Xamarin.Forms.Models
{
    internal class CachedPage
    {
        internal Type Type { get; }
        internal WeakReference<Page> Reference { get; }

        internal CachedPage(
            Page instance)
        {
            Type = instance.GetType();
            Reference = new WeakReference<Page>(
                instance);
        }
    }
}«