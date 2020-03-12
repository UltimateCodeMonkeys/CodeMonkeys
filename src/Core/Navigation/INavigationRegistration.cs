using System;

namespace CodeMonkeys.Core.Navigation
{
    public interface INavigationRegistration
    {
        Type ViewModelInterfaceType { get; }
        Type ViewType { get; }

        bool PreCreateInstance { get; set; }
    }
}