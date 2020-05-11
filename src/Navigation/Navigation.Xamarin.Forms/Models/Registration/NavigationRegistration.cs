using System;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public class NavigationRegistration :
        INavigationRegistration
    {
        public virtual Type ViewModelType { get; internal set; }
        public virtual Type ViewType { get; internal set; }

        public bool PreCreateInstance { get; set; } = true;

        public DevicePlatforms Platform { get; set; } = DevicePlatforms.All;


        public override bool Equals(
            object other)
        {
            if (!(other is NavigationRegistration registration))
            {
                return false;
            }

            if (ViewModelType != registration.ViewModelType)
            {
                return false;
            }

            if (ViewType != registration.ViewType)
            {
                return false;
            }

            if (Platform != registration.Platform)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}