using System;

namespace CodeMonkeys.Navigation.WPF
{
    public class RegistrationInfo :
        INavigationRegistration
    {
        public Type ViewModelType { get; set; }

        public Type ViewType { get; set; }


        public bool PreCreateInstance { get; set; } = false;


        /// <inheritdoc cref="INavigationRegistration.ResolveViewUsingDependencyInjection" />
        public bool ResolveViewUsingDependencyInjection { get; set; } = false;


        //public bool OpenInNewWindow { get; set; } = false;
        //public bool OpenAsPopup { get; set; } = false;


        public Func<bool> Condition { get; set; } = () => true;



        public override bool Equals(
            object other)
        {
            if (!(other is RegistrationInfo registrationInfo))
            {
                return false;
            }

            if (registrationInfo.ViewModelType != ViewModelType)
            {
                return false;
            }
            else if (registrationInfo.ViewType != ViewType)
            {
                return false;
            }


            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                ViewModelType,
                ViewType);
        }
    }
}