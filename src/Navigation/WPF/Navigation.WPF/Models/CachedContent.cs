using System;
using System.Windows;

namespace CodeMonkeys.Navigation.WPF
{
    internal class CachedContent
    {
        internal Type Type { get; }
        internal WeakReference<FrameworkElement> Reference { get; }


        internal CachedContent(
            FrameworkElement content)
        {
            Type = content.GetType();

            Reference = new WeakReference<FrameworkElement>(
                content);
        }
    }
}