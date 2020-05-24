using System;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    internal static class DevicePlatformMapper
    {
        internal static string ToXamarinPlatform(
            this DevicePlatforms platform)
        {
            switch (platform)
            {
                case DevicePlatforms.All:
                    return Device.RuntimePlatform;
                case DevicePlatforms.Android:
                    return Device.Android;
                case DevicePlatforms.iOS:
                    return Device.iOS;
                case DevicePlatforms.macOS:
                    return Device.macOS;
                case DevicePlatforms.UWP:
                    return Device.UWP;
                case DevicePlatforms.WPF:
                    return Device.WPF;
                case DevicePlatforms.GTK:
                    return Device.GTK;

                default:
                    throw new NotSupportedException();
            }
        }

        internal static DevicePlatforms ToDevicePlatform(
            this string platform)
        {
            return platform switch
            {
                Device.Android => DevicePlatforms.Android,
                Device.iOS => DevicePlatforms.iOS,
                Device.macOS => DevicePlatforms.macOS,
                Device.UWP => DevicePlatforms.UWP,
                Device.WPF => DevicePlatforms.WPF,
                Device.GTK => DevicePlatforms.GTK,
                _ => throw new NotSupportedException(),
            };
        }
    }
}