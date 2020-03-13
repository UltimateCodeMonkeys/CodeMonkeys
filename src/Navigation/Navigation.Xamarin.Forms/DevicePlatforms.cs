using System;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    [Flags]
    public enum DevicePlatforms : short
    {
        All = 1,
        Android = 2,
        iOS = 3,
        macOS = 4,
        UWP = 5,
        WPF = 6,
        GTK = 7
    }
}