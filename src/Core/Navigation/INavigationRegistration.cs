using System;

namespace CodeMonkeys.Navigation
{
    public interface INavigationRegistration
    {
        Type ViewModelType { get; }
        Type ViewType { get; }

        bool PreCreateInstance { get; set; }
    }
}