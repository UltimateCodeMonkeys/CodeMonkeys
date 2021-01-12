using System;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public class NavigationRegistration :
        NavigationRegistrationBase
    {
        public DevicePlatforms Platform { get; set; } = DevicePlatforms.All;


        public NavigationRegistration(
            Type viewModelType,
            Type viewType)
        {
            ViewModelType = viewModelType;
            ViewType = viewType;
        }


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
            return HashCode.Combine(
                ViewModelType,
                ViewType,
                Platform);
        }
    }
}