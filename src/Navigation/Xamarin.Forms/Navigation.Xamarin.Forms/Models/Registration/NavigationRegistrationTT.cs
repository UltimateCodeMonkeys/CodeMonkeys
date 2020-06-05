using CodeMonkeys.MVVM;

using System;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public class NavigationRegistration<TViewModelInterface, TView> :
        NavigationRegistration

        where TViewModelInterface : class, IViewModel
        where TView : Page
    {
        public override Type ViewModelType => typeof(TViewModelInterface);
        public override Type ViewType => typeof(TView);
    }
}
