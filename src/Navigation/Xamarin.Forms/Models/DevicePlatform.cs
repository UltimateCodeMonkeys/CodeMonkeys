using System;
using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms.Models
{
    [Flags]
    public enum DevicePlatform : short
    {
        All = 1,
        Android = 2,
        iOS = 3,
        macOS = 4,
        UWP = 5,
        WPF = 6,
        GTK = 7
    }


    public static class DevicePlatformMapper
    {
        public static string ToXamarinPlatform(
            this DevicePlatform platform)
        {
            switch (platform)
            {
                case DevicePlatform.All:
                    return Device.RuntimePlatform;
                case DevicePlatform.Android:
                    return Device.Android;
                case DevicePlatform.iOS:
                    return Device.iOS;
                case DevicePlatform.macOS:
                    return Device.macOS;
                case DevicePlatform.UWP:
                    return Device.UWP;
                case DevicePlatform.WPF:
                    return Device.WPF;
                case DevicePlatform.GTK:
                    return Device.GTK;

                default:
                    throw new NotSupportedException();
            }
        }

        public static DevicePlatform ToDevicePlatform(
            this string platform)
        {
            return platform switch
            {
                Device.Android => DevicePlatform.Android,
                Device.iOS => DevicePlatform.iOS,
                Device.macOS => DevicePlatform.macOS,
                Device.UWP => DevicePlatform.UWP,
                Device.WPF => DevicePlatform.WPF,
                Device.GTK => DevicePlatform.GTK,
                _ => throw new NotSupportedException(),
            };
        }
    }
}
