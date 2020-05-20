using System;

namespace CodeMonkeys.Navigation.WPF
{
    public class RegistrationInfo :
        INavigationRegistration
    {
        public Type ViewModelType { get; set; }

        public Type ViewType { get; set; }


        public bool PreCreateInstance { get; set; } = false;

        public bool ResolveViewUsingDependencyInjection { get; set; } = false;


        public bool OpenInNewWindow { get; set; } = false;
        //public bool OpenAsPopup { get; set; } = false;
    }
}