using System;

namespace CodeMonkeys.Navigation
{
    public interface INavigationRegistration
    {
        Type ViewModelInterfaceType { get; }
        Type ViewType { get; }

        bool PreCreateInstance { get; set; }
    }
}