using CodeMonkeys.Core.MVVM;

using System;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms.Models
{
    public class NavigationRegistration<TViewModelInterface, TView> :
        NavigationRegistration

        where TViewModelInterface : class, IViewModel
        where TView : Page
    {
        public override Type ViewModelInterfaceType => typeof(TViewModelInterface);
        public override Type ViewType => typeof(TView);
    }
}
