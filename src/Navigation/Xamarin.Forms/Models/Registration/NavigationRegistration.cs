using System;

using CodeMonkeys.Core.Interfaces.Navigation;

namespace CodeMonkeys.Navigation.Xamarin.Forms.Models
{
    public class NavigationRegistration :
        INavigationRegistration
    {
        public virtual Type ViewModelInterfaceType { get; internal set; }
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

            if (ViewModelInterfaceType != registration.ViewModelInterfaceType)
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